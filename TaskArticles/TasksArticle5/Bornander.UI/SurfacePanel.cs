using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Bornander.UI.Extensions;

namespace Bornander.UI
{
    public enum EdgeType
    {
        None, 
        Sticky, 
        Bouncy
    }

    public enum AspectRatioStrategy
    {
        Ignore,
        Maintain,
        ResizeNotAllowed
    }

    public class SurfacePanel : Panel
    {
        private const double MinSize = 4.0;
        private const double VelocityAnimationThresholdSquared = 0.001;

        public static readonly DependencyProperty PositionProperty = DependencyProperty.RegisterAttached("Position", typeof(Point), typeof(SurfacePanel), new PropertyMetadata(new Point()));
        public static readonly DependencyProperty AngleProperty = DependencyProperty.RegisterAttached("Angle", typeof(double), typeof(SurfacePanel), new PropertyMetadata(0.0));
        public static readonly DependencyProperty SizeProperty = DependencyProperty.RegisterAttached("Size", typeof(Size), typeof(SurfacePanel), new PropertyMetadata(new Size(120, 80)));
        public static readonly DependencyProperty AspectRatioStrategyProperty = DependencyProperty.RegisterAttached("AspectRatioStrategy", typeof(AspectRatioStrategy), typeof(SurfacePanel), new PropertyMetadata(AspectRatioStrategy.Ignore));

        public static readonly DependencyProperty VelocityProperty = DependencyProperty.RegisterAttached("Velocity", typeof(Vector), typeof(SurfacePanel), new PropertyMetadata(new Vector(0, 0)));
        public static readonly DependencyProperty AngularVelocityProperty = DependencyProperty.RegisterAttached("AngularVelocity", typeof(double), typeof(SurfacePanel), new PropertyMetadata(0.0));

        public static readonly DependencyProperty FrictionProperty = DependencyProperty.Register("Friction", typeof(double), typeof(SurfacePanel), new PropertyMetadata(0.05));
        public static readonly DependencyProperty EdgeTypeProperty = DependencyProperty.Register("EdgeType", typeof(EdgeType), typeof(SurfacePanel), new PropertyMetadata(EdgeType.Bouncy));
        public static readonly DependencyProperty AnimationEnabledProperty = DependencyProperty.Register("AnimationEnabled", typeof(bool), typeof(SurfacePanel), new PropertyMetadata(true));
        public static readonly DependencyProperty SorterProperty = DependencyProperty.Register("Sorter", typeof(ISurfaceSorter), typeof(SurfacePanel), new PropertyMetadata(null));

        #region Dependency property getters and setters

        public static Point GetPosition(UIElement element)
        {
            return (Point)element.GetValue(SurfacePanel.PositionProperty);
        }

        public static void SetPosition(UIElement element, Point value)
        {
            element.SetValue(SurfacePanel.PositionProperty, value);
        }

        public static double GetAngle(UIElement element)
        {
            return (double)element.GetValue(SurfacePanel.AngleProperty);
        }

        public static void SetAngle(UIElement element, double value)
        {
            element.SetValue(SurfacePanel.AngleProperty, value);
        }

        public static Size GetSize(UIElement element)
        {
            return (Size)element.GetValue(SurfacePanel.SizeProperty);
        }

        public static void SetSize(UIElement element, Size value)
        {
            element.SetValue(SurfacePanel.SizeProperty, value);
        }

        public static AspectRatioStrategy GetAspectRatioStrategy(UIElement element)
        {
            return (AspectRatioStrategy)element.GetValue(SurfacePanel.AspectRatioStrategyProperty);
        }

        public static void SetAspectRatioStrategy(UIElement element, AspectRatioStrategy value)
        {
            element.SetValue(SurfacePanel.AspectRatioStrategyProperty, value);
        }

        public static Vector GetVelocity(UIElement element)
        {
            return (Vector)element.GetValue(SurfacePanel.VelocityProperty);
        }

        public static void SetVelocity(UIElement element, Vector value)
        {
            element.SetValue(SurfacePanel.VelocityProperty, value);
        }

        public static double GetAngularVelocity(UIElement element)
        {
            return (double)element.GetValue(SurfacePanel.AngularVelocityProperty);
        }

        public static void SetAngularVelocity(UIElement element, double value)
        {
            element.SetValue(SurfacePanel.AngularVelocityProperty, value);
        }

        public double Friction
        {
            get { return (double)GetValue(SurfacePanel.FrictionProperty); }
            set { SetValue(SurfacePanel.FrictionProperty, value); }
        }

        public EdgeType EdgeType
        {
            get { return (EdgeType)GetValue(SurfacePanel.EdgeTypeProperty); }
            set { SetValue(SurfacePanel.EdgeTypeProperty, value); }
        }

        public bool AnimationEnabled
        {
            get { return (bool)GetValue(SurfacePanel.AnimationEnabledProperty); }
            set { SetValue(SurfacePanel.AnimationEnabledProperty, value); }
        }

        public ISurfaceSorter Sorter
        {
            get { return (ISurfaceSorter)GetValue(SurfacePanel.SorterProperty); }
            set { SetValue(SurfacePanel.SorterProperty, value); }
        }
        #endregion

        private UIElement selectedChild;
        private Vector selectedPosition;
        private Vector currentMousePosition;
        private Vector previousMousePosition;

        private DispatcherTimer animationTimer;

        public SurfacePanel()
        {
            PreviewMouseDown += SurfacePanel_PreviewMouseDown;
            PreviewMouseMove += SurfacePanel_PreviewMouseMove;
            PreviewMouseUp += (s, e) => selectedChild = null;
            SizeChanged += SurfacePanel_SizeChanged;
            animationTimer = new DispatcherTimer(TimeSpan.FromSeconds(0.025), DispatcherPriority.Render, Animate, Dispatcher);
        }

        private void Animate(object sender, EventArgs args)
        {
            if (!AnimationEnabled)
                return;

            double frictionCoefficient = 1.0 - Friction;
            foreach (UIElement child in Children)
            {
                if (child != selectedChild && child.RenderTransform is TransformGroup)
                {
                    TransformGroup transform = (TransformGroup)child.RenderTransform;

                    if (Sorter != null)
                    {
                        Sorter.CalculateSorted(this, selectedChild, animationTimer.Interval.TotalSeconds);
                        InvalidateVisual();
                    }
                    else
                    {
                        Vector velocity = GetVelocity(child);
                        double angularVelocity = GetAngularVelocity(child);

                        if (velocity.LengthSquared > SurfacePanel.VelocityAnimationThresholdSquared)
                        {
                            Vector position = (Vector)GetPosition(child);
                            position += velocity;

                            ClampToEdge(ref position, ref velocity, false);

                            SetPositionTransform(child, position, transform);
                            SetVelocity(child, velocity * frictionCoefficient);
                        }

                        if (Math.Abs(angularVelocity) > SurfacePanel.VelocityAnimationThresholdSquared)
                        {
                            double angle = GetAngle(child);
                            angle += angularVelocity;

                            SetRotationTransform(child, angle, transform);
                            SetAngularVelocity(child, angularVelocity * frictionCoefficient);
                        }
                    }
                }
            }
        }

        private void ClampToEdge(ref Vector position, ref Vector velocity, bool handleResize)
        {
            if (EdgeType == EdgeType.None)
                return;

            if (EdgeType == EdgeType.Bouncy && !handleResize)
            {
                if (position.X < 0 || position.X > ActualWidth)
                    velocity.X = -velocity.X;
                if (position.Y < 0 || position.Y > ActualHeight)
                    velocity.Y = -velocity.Y;
            }

            position.X = Math.Max(0, Math.Min(ActualWidth, position.X));
            position.Y = Math.Max(0, Math.Min(ActualHeight, position.Y));
        }

        private UIElement HitTestChildren(Point position)
        {
            IInputElement inputElement = InputHitTest(position);
            if (inputElement is DependencyObject)
            {
                DependencyObject current = (DependencyObject)inputElement;
                while (current != null)
                {
                    if (current is UIElement && Children.Contains((UIElement)current))
                        return (UIElement)current;
                    else
                        current = VisualTreeHelper.GetParent(current);
                }
            }
            return null;
        }

        private static void SetRotationTransform(UIElement element, double angle, TransformGroup transform)
        {
            ((RotateTransform)transform.Children[1]).Angle = angle;
            SetAngle(element, angle);
        }

        private static void SetPositionTransform(UIElement element, Vector position, TransformGroup transform)
        {
            TranslateTransform translation = ((TranslateTransform)transform.Children[2]);
            translation.X = position.X;
            translation.Y = position.Y;

            SetPosition(element, (Point)position);
        }

        private void SurfacePanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Vector dummy = new Vector();
            foreach (UIElement child in Children)
            {
                if (child.RenderTransform is TransformGroup)
                {
                    TransformGroup transform = (TransformGroup)child.RenderTransform;

                    Vector position = (Vector)GetPosition(child);
                    ClampToEdge(ref position, ref dummy, true);

                    SetPositionTransform(child, position, transform);
                }
            }
        }

        private void SurfacePanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(this);
            UIElement child = HitTestChildren(position);
            if (child != null)
            {
                foreach(UIElement el in this.Children)
                    child.SetValue(Panel.ZIndexProperty, -1);

                int maxZ = (int)(from UIElement el in this.Children.Cast<UIElement>() select el.GetValue(Panel.ZIndexProperty)).Max();
                child.SetValue(Panel.ZIndexProperty, ++maxZ);

                selectedChild = child;
                selectedPosition = (Vector)GetPosition(selectedChild);
                SetVelocity(selectedChild, new Vector());
                SetAngularVelocity(selectedChild, 0);
                previousMousePosition = (Vector)position;
                this.InvalidateVisual();
            }
        }

        private void SurfacePanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            currentMousePosition = (Vector)e.GetPosition(this);
            if (selectedChild != null && (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed))
            {
                Vector centerToPrevious = previousMousePosition - selectedPosition;
                Vector centerToCurrent = currentMousePosition - selectedPosition;

                double angle = Vector.AngleBetween(centerToPrevious, centerToCurrent);
                TransformGroup transform = (TransformGroup)selectedChild.RenderTransform;
                double newAngle = ((RotateTransform)transform.Children[1]).Angle + angle;

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (!KeyboardHelper.IsAnyShiftDown)
                    {
                        SetRotationTransform(selectedChild, newAngle, transform);
                        SetAngularVelocity(selectedChild, angle);
                    }

                    if (!KeyboardHelper.IsAnyCtrlDown)
                    {
                        if (KeyboardHelper.IsAnyShiftDown)
                            selectedPosition += currentMousePosition - previousMousePosition;
                        else
                            selectedPosition += VectorExtensions.Normalize(centerToCurrent) * (centerToCurrent.Length - centerToPrevious.Length);
                        
                        SetPositionTransform(selectedChild, selectedPosition, transform);
                        SetVelocity(selectedChild, currentMousePosition - previousMousePosition);
                    }
                }

                if (GetAspectRatioStrategy(selectedChild) != AspectRatioStrategy.ResizeNotAllowed && e.RightButton == MouseButtonState.Pressed)
                    CalculateSize(centerToPrevious, centerToCurrent, newAngle.ToRadians());
            }
            previousMousePosition = currentMousePosition;
        }

        private void CalculateSize(Vector centerToPrevious, Vector centerToCurrent, double angle)
        {
            Vector vertical = new Vector(Math.Sin(angle), Math.Cos(angle));
            Vector horizontal = new Vector(-vertical.Y, vertical.X);

            double deltaY = centerToCurrent.Project(vertical).Length - centerToPrevious.Project(vertical).Length;
            double deltaX = centerToCurrent.Project(horizontal).Length - centerToPrevious.Project(horizontal).Length;

            Size size = GetSize(selectedChild);
            double ratio = size.Height / size.Width;

            size.Width = Math.Max(GetMinWidth(selectedChild), size.Width + deltaX * 2.0);
            size.Height = Math.Max(GetMinHeight(selectedChild), size.Height + deltaY * 2.0);

            if (GetAspectRatioStrategy(selectedChild) == AspectRatioStrategy.Maintain || KeyboardHelper.IsAnyCtrlDown)
            {
                if (Math.Abs(deltaY) > Math.Abs(deltaX))
                    size.Width = size.Height / ratio;
                else
                    size.Height = size.Width * ratio;
            }
            SetSize(selectedChild, size);

            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in Children)
            {
                Point position = GetPosition(child);
                Size size = GetSize(child);

                child.Arrange(new Rect(size));

                TransformGroup transform = new TransformGroup();
                transform.Children.Add(new TranslateTransform(-size.Width / 2.0, -size.Height / 2.0));
                transform.Children.Add(new RotateTransform(GetAngle(child)));
                transform.Children.Add(new TranslateTransform(position.X, position.Y));
                child.RenderTransform = transform;
            }

            return finalSize;
        }

        private static double GetMinWidth(UIElement element)
        {
            return element is FrameworkElement ? ((FrameworkElement)element).MinWidth : SurfacePanel.MinSize;
        }

        private static double GetMinHeight(UIElement element)
        {
            return element is FrameworkElement ? ((FrameworkElement)element).MinHeight: SurfacePanel.MinSize;
        }

        public Vector MouseDelta
        {
            get { return currentMousePosition - previousMousePosition; }
        }

        public Vector MousePosition
        {
            get { return currentMousePosition; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

//My work collegue Fredrik Bornanders, Surface Panel for WPF
//See codeproject article : http://www.codeproject.com/KB/static/WPFSurfacePanel.aspx
using Bornander.UI; 

namespace WPFImagePipeline.Controls
{
    public class ScatterPanel : SurfacePanel
    {
        private static readonly Random random = new Random();

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (visualAdded is UIElement)
            {
                UIElement element = (UIElement)visualAdded;

                SurfacePanel.SetPosition(element, new Point(ActualWidth / 2.0, ActualHeight / 2.0));
                SurfacePanel.SetVelocity(element, new Vector(random.NextDouble() * 50 - 25, random.NextDouble() * 50 - 25));
                SurfacePanel.SetAngularVelocity(element, random.NextDouble() * 90 - 45);
                SurfacePanel.SetAspectRatioStrategy(element, AspectRatioStrategy.Maintain);

                Size minSize = new Size((double)random.Next(20, 100), (double)random.Next(20, 100));
                SurfacePanel.SetSize(element, minSize);

                if (element is FrameworkElement)
                {
                    FrameworkElement frameworkElement = (FrameworkElement)element;
                    frameworkElement.MinWidth = minSize.Width;
                    frameworkElement.MinHeight = minSize.Height;
                }
                
            }
        }
    }
}

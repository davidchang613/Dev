using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bornander.UI.DropHandlers
{
    public class TextDropHandler
    {
        private static readonly Random random = new Random();
        
        private SurfacePanel panel;
        private double maxVelocity;
        private double maxAngularVelocity;

        private bool originalAllowDrop;

        public TextDropHandler(SurfacePanel panel, double maxVelocity, double maxAngularVelocity)
        {
            this.panel = panel;
            this.maxVelocity = maxVelocity;
            this.maxAngularVelocity = maxAngularVelocity;
            originalAllowDrop = panel.AllowDrop;
        }
        
        public void Attach()
        {
            panel.AllowDrop = true;
            panel.Drop += HandleDrop;
        }

        public void Detatch()
        {
            panel.AllowDrop = originalAllowDrop;
            panel.Drop -= HandleDrop;
        }

        private void HandleDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetFormats().Contains(DataFormats.Text) && e.Data.GetDataPresent(DataFormats.Text))
            {
                foreach (string text in ((string)e.Data.GetData(DataFormats.Text)).Split(' '))
                {
                    Label label = new Label { Content = text, Background = Brushes.White };

                    label.Measure(new Size(panel.ActualWidth, panel.ActualHeight));
                    SurfacePanel.SetPosition(label, (Point)panel.MousePosition);
                    SurfacePanel.SetSize(label, label.DesiredSize);
                    SurfacePanel.SetAngularVelocity(label, random.NextDouble() * maxAngularVelocity - maxAngularVelocity / 2.0);
                    SurfacePanel.SetVelocity(label, new Vector(random.NextDouble() * maxVelocity - maxVelocity / 2.0, random.NextDouble() * maxVelocity - maxVelocity / 2.0));
                    SurfacePanel.SetAspectRatioStrategy(label, AspectRatioStrategy.ResizeNotAllowed);
                    panel.Children.Add(label);
                }
            }
        }
    }
}

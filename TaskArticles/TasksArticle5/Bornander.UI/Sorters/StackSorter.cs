using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Bornander.UI.Sorters
{
    public class StackSorter : ISurfaceSorter
    {
        private readonly IComparer<UIElement> comparer;

        public StackSorter()
        {
        }

        public StackSorter(IComparer<UIElement> comparer)
        {
            this.comparer = comparer;
        }

        public bool CalculateSorted(SurfacePanel panel, UIElement selectedChild, double elapsedTime)
        {
            if (panel.Children.Count == 0)
                return true;

            IEnumerable<Size> sizes = from e in panel.Children.Cast<UIElement>() select SurfacePanel.GetSize(e);

            double maxHeight = (from s in sizes select s.Height).Max();

            double x = 0;
            double y = 0;
            
            foreach (UIElement element in panel.Children.Cast<UIElement>().Where(i => i != selectedChild).OrderBy(i => i, comparer ?? new ChildIndexComparer(panel)))
            {
                Size size = SurfacePanel.GetSize(element);
                Vector position = (Vector)SurfacePanel.GetPosition(element);

                if (x + size.Width > panel.ActualWidth)
                {
                    x = 0;
                    y += maxHeight;
                }

                Vector targetPosition = new Vector(x + size.Width / 2.0, y + size.Height / 2);
                x += size.Width;

                Vector direction = (targetPosition - position) * 0.2 * elapsedTime;

                double angle = SurfacePanel.GetAngle(element);
                SurfacePanel.SetAngle(element, angle - angle * elapsedTime);
                SurfacePanel.SetPosition(element, (Point)(position + direction));
            }

            return false;
        }
    }
}

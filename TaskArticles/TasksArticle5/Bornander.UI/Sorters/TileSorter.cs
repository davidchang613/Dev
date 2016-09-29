using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bornander.UI.Extensions;
using System;

namespace Bornander.UI.Sorters
{
    public class TileSorter : ISurfaceSorter
    {

        private readonly IComparer<UIElement> comparer;

        public TileSorter()
        {
        }

        public TileSorter(IComparer<UIElement> comparer)
        {
            this.comparer = comparer;
        }

        public bool CalculateSorted(SurfacePanel panel, UIElement selectedChild, double elapsedTime)
        {
            if (panel.Children.Count == 0)
                return true;

            IEnumerable<Size> sizes = from e in panel.Children.Cast<UIElement>() select SurfacePanel.GetSize(e);

            double width = (from s in sizes select s.Width).Max();
            double height = (from s in sizes select s.Height).Max();

            int columns = Math.Max((int)(panel.ActualWidth / width), 1);

            int index = 0;
            foreach (UIElement element in panel.Children.Cast<UIElement>().Where(x => x != selectedChild).OrderBy(x => x, comparer ?? new ChildIndexComparer(panel)))
            {
                int column = index % columns;
                int row = index / columns;
                Vector position = (Vector)SurfacePanel.GetPosition(element);
                Vector targetPosition = new Vector(column * width + width / 2.0, row * height + height / 2.0);
                Vector direction = (targetPosition - position) * 0.2 * elapsedTime;

                double angle = SurfacePanel.GetAngle(element);
                SurfacePanel.SetAngle(element, angle - angle * elapsedTime);
                SurfacePanel.SetPosition(element, (Point)(position + direction));
                ++index;
            }

            return false;
        }
    }
}

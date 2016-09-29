using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Bornander.UI.Sorters
{
    internal class ChildIndexComparer : IComparer<UIElement>
    {
        private readonly Panel panel;

        public ChildIndexComparer(Panel panel)
        {
            this.panel = panel;
        }

        public int Compare(UIElement x, UIElement y)
        {
            return panel.Children.IndexOf(x) - panel.Children.IndexOf(y);
        }
    }
}

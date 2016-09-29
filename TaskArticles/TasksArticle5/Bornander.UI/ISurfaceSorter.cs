using System.Windows;

namespace Bornander.UI
{
    public interface ISurfaceSorter
    {
        bool CalculateSorted(SurfacePanel panel, UIElement selectedElement, double elapsedTime);
    }
}

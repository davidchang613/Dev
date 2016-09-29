using System.Linq;
using System.Windows.Input;

namespace Bornander.UI.Extensions
{
    public static class KeyboardHelper
    {
        public static bool IsKeyDown(params Key[] keys)
        {
            return (from key in keys where Keyboard.IsKeyDown(key) select key).Count() > 0; 
        }

        public static bool IsAnyCtrlDown
        {
            get { return IsKeyDown(Key.LeftCtrl, Key.RightCtrl); }
        }

        public static bool IsAnyShiftDown
        {
            get { return IsKeyDown(Key.LeftShift, Key.RightShift); }
        }
    }
}

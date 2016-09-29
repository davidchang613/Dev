using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SyncContextControlAwaiter
{

    public static class ControlExtensionMethods
    {
        public static ControlAwaiter GetAwaiter(this Control control)
        {
            return new ControlAwaiter(control);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SyncContextControlAwaiter
{
    public class ControlAwaiter
    {
        private readonly Control control;

        public ControlAwaiter(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            this.control = control;
            IsCompleted = false;
        }


        public void GetResult()
        {
        }


        public void OnCompleted(Action continuation)
        {
            control.BeginInvoke(continuation);
            IsCompleted = true;
        }

        public bool IsCompleted { get; set; }
    }


}

using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneSVA
{
	partial class FormViewController : UIViewController
	{
		public FormViewController (IntPtr handle) : base (handle)
		{
		}

        partial void Close_TouchUpInside(UIButton sender)
        {
            this.DismissViewController(true, null);
        }
    }
}

using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneApp
{
	partial class Tab2ViewController : UIViewController
	{
		public Tab2ViewController (IntPtr handle) : base (handle)
		{
		}

        partial void CloseMe_TouchUpInside(UIButton sender)
        {
            this.DismissViewController(true, null);
        }
    }
}

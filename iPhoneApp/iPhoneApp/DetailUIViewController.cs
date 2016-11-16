using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneApp
{
	partial class DetailUIViewController : UIViewController
	{
		public DetailUIViewController (IntPtr handle) : base (handle)
		{
		}

        partial void UIButton731_TouchUpInside(UIButton sender)
        {
            this.DismissViewController(true, null);
        }
    }
}

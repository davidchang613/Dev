using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneSVA
{
	partial class F1TableViewController : UITableViewController
	{
		public F1TableViewController (IntPtr handle) : base (handle)
		{
		}

        partial void Close_TouchUpInside(UIButton sender)
        {
            this.DismissViewController(true, null);
        }
    }
}

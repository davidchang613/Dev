using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneApp
{
	partial class MyTableViewController : UITableViewController
	{
		public MyTableViewController (IntPtr handle) : base (handle)
		{
		}

        //partial void CloseButton_TouchUpInside(UIButton sender)
        //{
        //    this.DismissViewController(true, null);
        //}


        partial void ButtonONe_TouchUpInside(UIButton sender)
        {
            this.DismissViewController(true, null);
        }
    }
}

using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneApp
{
	partial class ColorViewController : UIViewController
	{
		public ColorViewController (IntPtr handle) : base (handle)
		{
		}        

      

        partial void UIButton271_TouchUpInside(UIButton sender)
        {
            this.DismissViewController(true, null);
        }

        private void OpenNew()
        {
            UIStoryboard Storyboard = UIStoryboard.FromName("Main", null); // Assume the name of your file is "MainStoryboard.storyboard"
            var cvc = Storyboard.InstantiateViewController("MyTableViewController") as MyTableViewController;
            // ... set any data in webController, etc.
            //this.NavigationController.PushViewController(cvc, true);

            // Make it the root controller for example (the first line adds a paramater to it)
            //cvs.CaseID = int.Parse(notification.UserInfo["ID"].ToString());
            //this.Window.RootViewController = webController;
            //this.DismissViewController(true, null);
            //UIViewController vc = this.PresentingViewController;
            
            this.PresentViewController(cvc, true, null);
            //this.ShowViewController(cvc, null);
            //vc.DismissViewController(false, null);

        }

        partial void OpenNewButton_TouchUpInside(UIButton sender)
        {

            
            this.DismissViewController(false, OpenNew);

        }



    }
}

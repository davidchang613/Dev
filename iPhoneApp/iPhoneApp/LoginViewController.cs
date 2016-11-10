using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneApp
{
	partial class LoginViewController : UIViewController
	{
		public LoginViewController (IntPtr handle) : base (handle)
		{
            
		}

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            this.View.EndEditing(true);
        }



        partial void UIButton215_TouchUpInside(UIButton sender)
        {
            UIStoryboard Storyboard = UIStoryboard.FromName("Main", null); // Assume the name of your file is "MainStoryboard.storyboard"

            var cvc = Storyboard.InstantiateViewController("ColorViewController") as ColorViewController;
            // ... set any data in webController, etc.
            //this.NavigationController.PushViewController(cvc, true);

            // Make it the root controller for example (the first line adds a paramater to it)
            //cvs.CaseID = int.Parse(notification.UserInfo["ID"].ToString());
            //this.Window.RootViewController = webController;
            this.PresentViewController(cvc, true, null);

            //this.DismissViewController(true, null);
        }



        partial void LoginButton_TouchUpInside(UIButton sender)
        {

            OpenTableViewController();
            //OpenTableViewCotroller();

            return;

            UIStoryboard Storyboard = UIStoryboard.FromName("Main", null); // Assume the name of your file is "MainStoryboard.storyboard"

            var cvc = Storyboard.InstantiateViewController("MyTabBarController") as MyTabBarController;
            // ... set any data in webController, etc.
            //this.NavigationController.PushViewController(cvc, true);

            // Make it the root controller for example (the first line adds a paramater to it)
            //cvs.CaseID = int.Parse(notification.UserInfo["ID"].ToString());
            //this.Window.RootViewController = webController;

            //this.PresentViewController(cvc, true, null);
            
        }

        private void OpenTableViewCotroller()
        {
            UIStoryboard Storyboard = UIStoryboard.FromName("Main", null); // Assume the name of your file is "MainStoryboard.storyboard"

            var tvc = Storyboard.InstantiateViewController("MyTableViewController") as MyTableViewController;
            // ... set any data in webController, etc.
            //this.NavigationController.PushViewController(cvc, true);

            // Make it the root controller for example (the first line adds a paramater to it)
            //cvs.CaseID = int.Parse(notification.UserInfo["ID"].ToString());
            //this.Window.RootViewController = webController;
            this.PresentViewController(tvc, true, null);
        }

        private void OpenTableViewController()
        {
            UIStoryboard Storyboard = UIStoryboard.FromName("Main", null); // Assume the name of your file is "MainStoryboard.storyboard"

            var tvc = Storyboard.InstantiateViewController("DynamicUITableViewController") as DynamicUITableViewController;
            // ... set any data in webController, etc.
            //this.NavigationController.PushViewController(cvc, true);

            // Make it the root controller for example (the first line adds a paramater to it)
            //cvs.CaseID = int.Parse(notification.UserInfo["ID"].ToString());
            //this.Window.RootViewController = webController;
            this.PresentViewController(tvc, true, null);
        }
    }
}

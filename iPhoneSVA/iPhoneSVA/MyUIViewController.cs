using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneSVA
{
	partial class MyUIViewController : UIViewController
	{
		public MyUIViewController (IntPtr handle) : base (handle)
		{
		}

        UITableView tableView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            tableView = new UITableView(this.View.Frame);
            //this.View.Add(tableView);
            //this.Add(tableView);    // Xamarin one collection
            this.View.AddSubview(tableView);  // ios one
        }

      
    }
}

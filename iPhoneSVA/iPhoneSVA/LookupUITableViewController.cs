using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneSVA
{
    public delegate void LookupSelected(string key, string value);
   
	partial class LookupUITableViewController : UITableViewController
	{
		public LookupUITableViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UITableView  table = new UITableView(View.Bounds); // defaults to Plain style
            string[] tableItems = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            table.Source = new TableSource(tableItems, this);
            Add(table);

        }

        public string Action
        {
            get; set;
        }

        public string SelectedKey
        {
            get; set;
        }

        public string SelectedValue
        {
            get; set;
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            LookupItemSelectedEvent(SelectedKey, SelectedValue);
        }

        public event LookupSelected LookupItemSelectedEvent;

        //public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        //{
        //    // base.RowSelected(tableView, indexPath);
        //    UIAlertController okAlertController = UIAlertController.Create("Row Selected", indexPath.Row.ToString(), UIAlertControllerStyle.Alert);
        //    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

        //   //     tableView.DeselectRow(indexPath, true);
        //     PresentViewController(okAlertController, true, null);

        //}

        public override void FooterViewDisplayingEnded(UITableView tableView, UIView footerView, nint section)
        {
            base.FooterViewDisplayingEnded(tableView, footerView, section);
        }
    }
}

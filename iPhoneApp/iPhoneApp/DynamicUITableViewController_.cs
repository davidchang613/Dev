using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneApp
{
	partial class DynamicUITableViewController : UITableViewController
	{
		public DynamicUITableViewController (IntPtr handle) : base (handle)
		{
            for (int i = 0; i < 10000; i++)
                tableItems[i] = "Hello " + i.ToString();
		}

        string[] tableItems = new string[10000]; // { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            tableView = new UITableView(View.Bounds); // defaults to Plain style
            
            tableView.Source = new TableSource(tableItems, this);
            Add(tableView);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);
            UIAlertController okAlertController = UIAlertController.Create("Row Selected", tableItems[indexPath.Row], UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
    
            tableView.DeselectRow(indexPath, true);
        }
    }

    public class TableSource : UITableViewSource
    {
        UITableViewController owner;
        string[] TableItems;
        string CellIdentifier = "TableCell";

        public TableSource(string[] items)
        {
            TableItems = items;
        }

        public TableSource(string[] items, UITableViewController owner)
        {
            TableItems = items;
            this.owner = owner;

        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Length;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            DynamicUITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier) as DynamicUITableViewCell;
            string item = TableItems[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new DynamicUITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            cell.SetValues(item);
            //cell.TextLabel.Text = item;

            return cell;
        }
    }
}

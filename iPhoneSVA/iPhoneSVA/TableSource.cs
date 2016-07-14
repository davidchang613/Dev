using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Foundation;

namespace iPhoneSVA
{
    public class TableSource : UITableViewSource
    {

        string[] TableItems;
        string CellIdentifier = "TableCell";
        UIViewController ctrl;

        public TableSource(string[] items, UIViewController ctr)
        {
            TableItems = items;
            ctrl = ctr;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return TableItems.Length;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            //base.RowSelected(tableView, indexPath);
            UIAlertController okAlertController = UIAlertController.Create("Row Selected", TableItems[indexPath.Row], UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            
            tableView.DeselectRow(indexPath, true);
            LookupUITableViewController lookupCtrl = ctrl as LookupUITableViewController;
            if (lookupCtrl != null)
            {
                lookupCtrl.SelectedKey = TableItems[indexPath.Row];
                lookupCtrl.SelectedValue = TableItems[indexPath.Row];
            }
            //ctrl.PresentModalViewController(okAlertController, true);
            //ctrl.PresentViewController(okAlertController, true, null);
            //() => { ctrl.DismissViewController(true, null); });
            ctrl.DismissViewController(true, null);

        }

        
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            string item = TableItems[indexPath.Row];

            //---- if there are no cells to reuse, create a new one
            if (cell == null)
            { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

            cell.Accessory = UITableViewCellAccessory.Checkmark;
            // if selected, use checkmark
            
            cell.TextLabel.Text = item;

            return cell;
        }
    }
}

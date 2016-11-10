using CoreGraphics;
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneApp
{
	partial class DynamicUITableViewCell : UITableViewCell
	{        
        public DynamicUITableViewCell (IntPtr handle) : base (handle)
		{
		}

        public DynamicUITableViewCell(UITableViewCellStyle style, string reuseIdentifier):base(style, reuseIdentifier)
        {
            CellButton = new UIButton(UIButtonType.ContactAdd);
            ContentView.AddSubviews(new UIView[] { CellButton });
        }

        public void SetValues(string btnLabel)
        {
            CellButton.SetTitle(btnLabel, UIControlState.Normal);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CellButton.Frame = new CGRect(0, 0, ContentView.Bounds.Width/4, 25);
        }

    }
}

// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iPhoneApp
{
	[Register ("MyTableViewController")]
	partial class MyTableViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton buttonONe { get; set; }

		[Action ("ButtonONe_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonONe_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (buttonONe != null) {
				buttonONe.Dispose ();
				buttonONe = null;
			}
		}
	}
}

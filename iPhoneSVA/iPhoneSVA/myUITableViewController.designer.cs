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

namespace iPhoneSVA
{
	[Register ("myUITableViewController")]
	partial class myUITableViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton open1 { get; set; }

		[Action ("Open1_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Open1_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (open1 != null) {
				open1.Dispose ();
				open1 = null;
			}
		}
	}
}

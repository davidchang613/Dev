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
	[Register ("ColorViewController")]
	partial class ColorViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton OpenNewButton { get; set; }

		[Action ("OpenNewButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OpenNewButton_TouchUpInside (UIButton sender);

		[Action ("UIButton271_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton271_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (OpenNewButton != null) {
				OpenNewButton.Dispose ();
				OpenNewButton = null;
			}
		}
	}
}

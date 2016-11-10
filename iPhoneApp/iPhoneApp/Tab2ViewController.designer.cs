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
	[Register ("Tab2ViewController")]
	partial class Tab2ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton CloseMe { get; set; }

		[Action ("CloseMe_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void CloseMe_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (CloseMe != null) {
				CloseMe.Dispose ();
				CloseMe = null;
			}
		}
	}
}

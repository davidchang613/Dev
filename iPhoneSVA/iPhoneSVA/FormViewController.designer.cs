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
	[Register ("FormViewController")]
	partial class FormViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Close { get; set; }

		[Action ("Close_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Close_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (Close != null) {
				Close.Dispose ();
				Close = null;
			}
		}
	}
}

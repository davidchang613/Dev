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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnSendCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnVerifyCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblHello { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton login2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton openF1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textboxCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textboxPassword { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textboxUserName { get; set; }

		[Action ("BtnSendCode_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnSendCode_TouchUpInside (UIButton sender);

		[Action ("BtnVerifyCode_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnVerifyCode_TouchUpInside (UIButton sender);

		[Action ("Login2_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Login2_TouchUpInside (UIButton sender);

		[Action ("UIButton3_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton3_TouchUpInside (UIButton sender);

		[Action ("UIButton370_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton370_TouchUpInside (UIButton sender);

		[Action ("UIButton640_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton640_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (btnSendCode != null) {
				btnSendCode.Dispose ();
				btnSendCode = null;
			}
			if (btnVerifyCode != null) {
				btnVerifyCode.Dispose ();
				btnVerifyCode = null;
			}
			if (lblHello != null) {
				lblHello.Dispose ();
				lblHello = null;
			}
			if (login2 != null) {
				login2.Dispose ();
				login2 = null;
			}
			if (openF1 != null) {
				openF1.Dispose ();
				openF1 = null;
			}
			if (textboxCode != null) {
				textboxCode.Dispose ();
				textboxCode = null;
			}
			if (textboxPassword != null) {
				textboxPassword.Dispose ();
				textboxPassword = null;
			}
			if (textboxUserName != null) {
				textboxUserName.Dispose ();
				textboxUserName = null;
			}
		}
	}
}

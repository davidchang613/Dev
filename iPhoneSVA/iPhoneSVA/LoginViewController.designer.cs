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
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnConfirmCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnLogin { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnSendCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblStatus { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIPickerView pickerProviderCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton TestButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtPassword { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtUsername { get; set; }

		[Action ("BtnConfirmCode_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnConfirmCode_TouchUpInside (UIButton sender);

		[Action ("BtnLogin_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnLogin_TouchUpInside (UIButton sender);

		[Action ("btnSendCode_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnSendCode_TouchUpInside (UIButton sender);

		[Action ("TestButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void TestButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (btnConfirmCode != null) {
				btnConfirmCode.Dispose ();
				btnConfirmCode = null;
			}
			if (btnLogin != null) {
				btnLogin.Dispose ();
				btnLogin = null;
			}
			if (btnSendCode != null) {
				btnSendCode.Dispose ();
				btnSendCode = null;
			}
			if (lblStatus != null) {
				lblStatus.Dispose ();
				lblStatus = null;
			}
			if (pickerProviderCode != null) {
				pickerProviderCode.Dispose ();
				pickerProviderCode = null;
			}
			if (TestButton != null) {
				TestButton.Dispose ();
				TestButton = null;
			}
			if (txtCode != null) {
				txtCode.Dispose ();
				txtCode = null;
			}
			if (txtPassword != null) {
				txtPassword.Dispose ();
				txtPassword = null;
			}
			if (txtUsername != null) {
				txtUsername.Dispose ();
				txtUsername = null;
			}
		}
	}
}

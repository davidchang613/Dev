using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using pclWebAPIClient;
using pclWebAPIClient.App;
using System.Collections.Generic;

namespace iPhoneSVA
{
	partial class LoginViewController : UIViewController
	{
        private ReferralAPICaller caller = null;

        private ReferralAPICaller GetReferralAPICaller
        {
            get
            {
                if (caller == null)
                {
                    string httpServer = "https://someserver.com";
                    caller = new ReferralAPICaller(httpServer);
                    caller.BeginCallHandler += Caller_BeginCallHandler;
                    caller.FailedCallHandler += Caller_FailedCallHandler;
                }
                    
                return caller;
            }
        }

        private void Caller_FailedCallHandler(string message)
        {
            this.lblStatus.Text = message;
        }

        private void Caller_BeginCallHandler(string message)
        {
            this.lblStatus.Text = message;
        }

        public LoginViewController (IntPtr handle) : base (handle)
		{
		}

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            this.View.EndEditing(true);
        }

        partial void BtnLogin_TouchUpInside(UIButton sender)
        {
            string username = txtUsername.Text; 
            string password = txtPassword.Text;  
            this.lblStatus.Text = "Login Clicked";
            ReferralAPICaller caller = GetReferralAPICaller;
            
            caller.GetTokenAndLogin(username, password, false);
            // Console.WriteLine(caller.LastError);
            // this.lblStatus.Text = "Login " + (caller.LastError == "" ? "OK" : caller.LastError);
            if (caller.IsLastCallSuccess)
            {
                caller.GetSendCodeProviders(username);
                if (caller.IsLastCallSuccess)
                {
                    ProviderCodePickerModel model = new ProviderCodePickerModel(caller.CodeProviders);
                    pickerProviderCode.Model = model;
                }
            }        
        }

        

        partial void btnSendCode_TouchUpInside(UIButton sender)
        {
            string username = txtUsername.Text;
            ReferralAPICaller caller = GetReferralAPICaller;
            caller.SendCode(username, "Phone Code");
            //this.lblStatus.Text = "Send PHone Code";
         //   pickerProviderCode

        } 

        partial void BtnConfirmCode_TouchUpInside(UIButton sender)
        {
            string username = txtUsername.Text;
            string code = txtCode.Text;
            ReferralAPICaller caller = GetReferralAPICaller;
            caller.VerifyCode(username, "Phone Code", code);
            if (!caller.IsLastCallSuccess)
            {
                // done 
                UIAlertController uiAC = new UIAlertController();
                UIAlertAction uiAA = UIAlertAction.Create("Invalid Code", UIAlertActionStyle.Cancel, null);
                
                uiAC.Message = "Invalid Code";
                uiAC.ModalPresentationStyle = UIModalPresentationStyle.Popover;
                uiAC.AddAction(uiAA);

                this.PresentViewController(uiAC, true, null);
            }
            else
            {
                // open another view
                UIViewController referralMainVC = Storyboard.InstantiateViewController("ReferralUITabBarController");
                this.PresentViewController(referralMainVC, true, null);

            }
        }

         public class ProviderCodePickerModel : UIPickerViewModel
        {
            private Dictionary<string, string> dataList = new Dictionary<string, string>();
            private Dictionary<nint, string> dataListIndex = new Dictionary<nint, string>();


            public ProviderCodePickerModel(Dictionary<string, string> source)
            {
                dataList = source;
                nint index = 0;
                foreach(KeyValuePair<string, string> keyVal in dataList)
                {
                    dataListIndex.Add(index++, keyVal.Value);
                }
                dataListIndex.Add(index++, "Other Code");
            }

            public override nint GetComponentCount(UIPickerView pickerView)
            {
                return 1;
            }

            public override string GetTitle(UIPickerView pickerView, nint row, nint component)
            {
                return dataListIndex[row].ToString();
                //return base.GetTitle(pickerView, row, component);
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return dataListIndex.Count;
            }

            //public override void Selected(UIPickerView pickerView, nint row, nint component)
            //{
            //    base.Selected(pickerView, row, component);
            //}
            //public override string GetTitle(UIPickerView picker, int row, int component)
            //{

            //    return "Component " + row.ToString();
            //}
        }



        partial void TestButton_TouchUpInside(UIButton sender)
        {
            //UIViewController lookupVC = Storyboard.InstantiateViewController("LookupUITableViewController");

            LookupUITableViewController lookupVC = Storyboard.InstantiateViewController("LookupUITableViewController") as LookupUITableViewController;
            lookupVC.LookupItemSelectedEvent += LookupVC_LookupItemSelectedEvent;
            this.PresentViewController(lookupVC, true, completionHandler: () =>
            {
                // ...
                if (lookupVC != null)
                    Console.WriteLine("HELLO, I'm done with the lookup");
            });

        }

        private void LookupVC_LookupItemSelectedEvent(string key, string value)
        {
            TestButton.SetTitle(value, UIControlState.Normal);  
            Console.WriteLine("selected key: " + key);
            Console.WriteLine("selected value: " + value);
        }
    }
}

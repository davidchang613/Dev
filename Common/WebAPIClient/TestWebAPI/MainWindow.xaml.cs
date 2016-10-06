using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APIDHMSApplicationPortable;
using WebAPIModel;

namespace TestWebAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        APIDHMSApplication caller = null;
        public APIDHMSApplication GetDHMSApplication
        {
            get
            {
                if (caller == null)
                {
                    string serverName = textBoxServerName.Text;
                    caller = new APIDHMSApplication(serverName);
                    caller.FailedCallHandler += Caller_FailedCallHandler;
                    caller.SuccessCallHandler += Caller_SuccessCallHandler;
                    caller.BeginCallHandler += Caller_BeginCallHandler;
                }

                return caller;
            }
        }

        private void Caller_BeginCallHandler(string message)
        {
            labelCallStatus.Content = message;
            listBoxEvents.Items.Add(string.Format("{0:yyyyMMdd hh:mm:ss} - {1}", DateTime.Now, message));
        }

        private void Caller_SuccessCallHandler(string message)
        {
            labelCallStatus.Content = message;
            listBoxEvents.Items.Add(string.Format("{0:yyyyMMdd hh:mm:ss} - {1}", DateTime.Now, message));
        }

        private void Caller_FailedCallHandler(string message)
        {
            labelCallStatus.Content = message;
            listBoxEvents.Items.Add(string.Format("{0:yyyyMMdd hh:mm:ss} - {1}", DateTime.Now, message));
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLoginWithObj_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;

            string username = textBoxUserName.Text;
            string password = textBoxPassword.Password;

            if (caller.GetTokenAndLogin(username, password, false))
            {
                textBlockToken.Text = caller.Token;
                labelCallStatus.Content = "Login Success with token returned.";
                Console.WriteLine("success login " + caller.Token);
            }
            else
            {
                textBlockToken.Text = caller.LastError;
                labelCallStatus.Content = "Login Failed : " + caller.LastError;
                Console.WriteLine(caller.LastError);
            }
        }

        private void buttonSendCode_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;
            string username = textBoxUserName.Text;
            caller.SendCodeAsync(username, "Email Code");
            if (caller.IsLastCallSuccess)
                labelCallStatus.Content = "Login Success with token returned.";
            Console.WriteLine("Send code");
        }

        private void buttonVerifyCode_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;
            string username = textBoxUserName.Text;
            string code = textBoxCode.Text;
            caller.VerifyCode(username, "Email Code", code);
        }

        private void buttonGetReferrals_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;
            string username = textBoxUserName.Text;
            //var list = caller.GetReferrals(username);
            Console.WriteLine("GetReferrals");
        }

        private void buttonGetCountries_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;
            var list = caller.GetCountries();
            if (caller.IsLastCallSuccess)
            {

            }
        }

        private void buttonGetStates_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;
            var list = caller.GetStates();
        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;
            caller.Logout();

        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;
            RegisterBindingModel registerModel = new RegisterBindingModel();
            registerModel.BaseUrl = this.textBoxServerName.Text;
            registerModel.Email = textBoxRegisterEmail.Text;
            registerModel.Password = textBoxRegisterPassword.Text;
            registerModel.ConfirmPassword = textBoxRegisterConfirmPassword.Text;
            registerModel.Number = textBoxPhoneNumber.Text;
            caller.Register(registerModel);
        }

        private void btnGetCodeProviders_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;
            caller.GetSendCodeProviders(textBoxUserName.Text);
        }

        private void buttonGetNumber_Click(object sender, RoutedEventArgs e)
        {

            APIDHMSApplication caller = GetDHMSApplication;

            Dictionary<string, List<Reference>> refLists = caller.GetReferences(new List<string> { "STATE", "STATUS", "MONTH" });

            int number = caller.GetNumber();
            if (caller.IsLastCallSuccess)
            {

            }
        }

        private void buttonGetMember_Click(object sender, RoutedEventArgs e)
        {
            APIDHMSApplication caller = GetDHMSApplication;

           // Member member = caller.Get("id");

        }
    }
}

﻿using System;
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
using ReferralApplicationPortable;
using WebAPIModel;

namespace TestWebAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ReferralApplication caller = null;
        public ReferralApplication GetReferralApplication
        {
            get
            {
                if (caller == null)
                {
                    string serverName = textBoxServerName.Text;
                    caller = new ReferralApplication(serverName);
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
            ReferralApplication caller = GetReferralApplication;

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
            ReferralApplication caller = GetReferralApplication;
            string username = textBoxUserName.Text;
            caller.SendCodeAsync(username, "Phone Code");
            if (caller.IsLastCallSuccess)
                labelCallStatus.Content = "Login Success with token returned.";
            Console.WriteLine("Send code");
        }

        private void buttonVerifyCode_Click(object sender, RoutedEventArgs e)
        {
            ReferralApplication caller = GetReferralApplication;
            string username = textBoxUserName.Text;
            string code = textBoxCode.Text;
            caller.VerifyCode(username, "Phone Code", code);
        }

        private void buttonGetReferrals_Click(object sender, RoutedEventArgs e)
        {
            ReferralApplication caller = GetReferralApplication;
            string username = textBoxUserName.Text;
            var list = caller.GetReferrals(username);
            Console.WriteLine("GetReferrals");
        }

        private void buttonGetCountries_Click(object sender, RoutedEventArgs e)
        {
            ReferralApplication caller = GetReferralApplication;
            var list = caller.GetCountries();
            if (caller.IsLastCallSuccess)
            {

            }
        }

        private void buttonGetStates_Click(object sender, RoutedEventArgs e)
        {
            ReferralApplication caller = GetReferralApplication;
            var list = caller.GetStates();
        }

        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            ReferralApplication caller = GetReferralApplication;
            caller.Logout();

        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            ReferralApplication caller = GetReferralApplication;
            RegisterBindingModel registerModel = new RegisterBindingModel();
            registerModel.Email = textBoxRegisterEmail.Text;
            registerModel.Password = textBoxRegisterPassword.Text;
            registerModel.ConfirmPassword = textBoxRegisterConfirmPassword.Text;
            registerModel.Number = textBoxPhoneNumber.Text;
            caller.Register(registerModel);
        }
    }
}
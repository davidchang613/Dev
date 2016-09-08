using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIClientBasePortable
{    
    public static class Constants
    {

    }

    public static class ClientAPIPath
    {
        public const string GetToken = "GetToken";
        public const string Login = "Login";
        public const string Logout = "Logout";
        public const string Register = "Register";
        public const string SetPassword = "SetPassword";
        public const string ForgotPassword = "ForgotPassword";
        public const string ConfirmEmail = "ConfirmEmail";
        public const string VerifyCode = "VerifyCode";
        public const string GetSendCodeProviders = "GetSendCodeProviders";
        public const string SendCode = "SendCode";

    }

}

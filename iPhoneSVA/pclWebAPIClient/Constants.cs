using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pclWebAPIClient
{
    public static class Constants
    {

    }

    public static class ClientAPIPath
    {
        public const string GetToken = "GetToken";
        public const string Login = "Login";
        public const string Register = "Register";
        public const string SetPassword = "SetPassword";
        public const string ForgotPassword = "ForgotPassword";
        public const string ConfirmEmail = "ConfirmEmail";
        public const string VerifyCode = "VerifyCode";
        public const string GetSendCodeProviders = "GetSendCodeProviders";
        public const string SendCode = "SendCode";        

    }

    public static class ReferenceAPIPath
    {
        public const string GetCountries = "GetCountries";
        public const string GetStates = "GetStates";
        public const string GetReferencesByName = "GetReferencesByName";
    }
    public static class ReferralAPIPath
    {                
        public const string GetAccounts = "GetAccounts";
        public const string GetAllAccountInfo = "GetAllAccountInfo";
        public const string GetClients = "GetClients";
        public const string GetExistingReferrals = "GetExistingReferrals";
        public const string GetProgramTypes = "GetProgramTypes";
        public const string GetReferrals = "GetReferrals";
        public const string GetReferralsForIntakeprocess = "GetReferralsForIntakeprocess";
        public const string SaveReferral = "SaveReferral";
        public const string UpdateReferralStatus = "UpdateReferralStatus";        
    }
}

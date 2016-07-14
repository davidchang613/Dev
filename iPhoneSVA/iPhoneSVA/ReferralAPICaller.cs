using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestDovetailAPIUI
{
    public class APIClientBase
    {
        public APIClientBase(string serverName)
        {
            _serverName = serverName;
        }

        protected bool IsInitialized
        {
            get
            {
                if (!string.IsNullOrEmpty(_serverName))
                    return true;
                else return false;
            }
        }

        public string LastError
        {
            get { return _lastErrorDescription; }
        }

        private HttpClientHandler Handler
        {
            get
            {
                if (_handler == null)
                {
                    _cookieContainer = new CookieContainer();
                    _handler = new HttpClientHandler { CookieContainer = _cookieContainer, UseCookies = true, AllowAutoRedirect = false };
                }
                return _handler;
            }
        }

        // This particular Login, gets a bearer token and login
        public bool Login(string userName, string password, bool rememberMe)
        {           
            _userName = userName;
            _password = password;

            using (var client = GetClient())
            {
                var data = new { Email = userName, Password = password, RememberMe = rememberMe };

                List<KeyValuePair<string, string>> loginInfo = new List<KeyValuePair<string, string>>();
                loginInfo.Add(new KeyValuePair<string, string>("grant_type", "password"));
                loginInfo.Add(new KeyValuePair<string, string>("Email", _userName));
                loginInfo.Add(new KeyValuePair<string, string>("password", _password));

                var loginContent = new FormUrlEncodedContent(loginInfo);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);

                HttpResponseMessage response = client.PostAsync(_serverName + @"/api/Account/Login", loginContent).Result;

                //string cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

                string apiReturn = response.Content.ReadAsStringAsync().Result;

                var returnContent = JObject.Parse(apiReturn);
                
                _lastCallSuccess = response.IsSuccessStatusCode;
                
                if (!_lastCallSuccess)
                {                    
                    string failedReason = returnContent["Message"].ToString();
                }
            }

            return _lastCallSuccess;

        }

        //public bool LoginAsync(string userName, string password)
        //{
        //    _userName = userName;
        //    _password = password;

        //    GetTokenAsyncInternal();

        //    return this._lastCallSuccess;

        //}

        protected void SetLastCallResult(bool success, string lastCall, string lastCallError)
        {
            // save the list???
            _lastCallSuccess = success;
            _lastCallMethod = lastCall;
            _lastErrorDescription = lastCallError;
        }

        protected HttpClient GetClient()
        {
            // the second param prevents the Handler from being disposed with the client
            var client = new HttpClient(Handler, false);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private void SetHttpClientHeaders(HttpClient client)
        {
            //        client.DefaultRequestHeaders.Connection.Add("keep-Alive");
            //        client.DefaultRequestHeaders.Add("Host", "localhost");
            //        client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            //        client.DefaultRequestHeaders.Add("User-Agent", "WIN");

        }

        public bool GetTokenAndLogin(string userName, string password, bool rememberMe)
        {
            bool success = false;

            _userName = userName;
            _password = password;

            this.GetTokenInternal();
            
            if (this._lastCallSuccess)
            {
                // do the Login
                if (this.Login(userName, password, rememberMe))                
                {
                    success = true;
                }
            }

            return success;
        }

        public string Token
        {
            get { return _token; }
        }

        private void GetTokenInternal()
        {
            bool success = false;
            string errorDescription = string.Empty;

            string token = string.Empty;

            using (var client = GetClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Referrer = new Uri(_serverName);


                List<KeyValuePair<string, string>> loginInfo = new List<KeyValuePair<string, string>>();
                loginInfo.Add(new KeyValuePair<string, string>("grant_type", "password"));
                loginInfo.Add(new KeyValuePair<string, string>("username", _userName));
                loginInfo.Add(new KeyValuePair<string, string>("password", _password));

                var loginContent = new FormUrlEncodedContent(loginInfo);
                
                HttpResponseMessage response = client.PostAsync(_serverName + @"/Token", loginContent).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;
                
                //string apiReturn = response.Content.
                //HttpResponseMessage response = await client.PostAsJsonAsync(@"http://localhost:61866/Token", data);

                var responseJSON = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    errorDescription = "";
                    if (responseJSON["access_token"] != null)
                        _token = responseJSON["access_token"].ToString();                    
                }
                else
                {                                     
                    if (responseJSON["error_description"] != null)
                        errorDescription = responseJSON["error_description"].ToString();                    
                }
            }

            SetLastCallResult(success, "GetToken", errorDescription);            
        }

        private async void GetTokenAsyncInternal()
        {
            bool success = false;
            string errorDescription = string.Empty;

            string token = string.Empty;

            using (var client = GetClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Referrer = new Uri(_serverName);


                List<KeyValuePair<string, string>> loginInfo = new List<KeyValuePair<string, string>>();
                loginInfo.Add(new KeyValuePair<string, string>("grant_type", "password"));
                loginInfo.Add(new KeyValuePair<string, string>("username", _userName));
                loginInfo.Add(new KeyValuePair<string, string>("password", _password));

                var loginContent = new FormUrlEncodedContent(loginInfo);

                HttpResponseMessage response = await client.PostAsync(_serverName + @"/Token", loginContent);
                string apiReturn = await response.Content.ReadAsStringAsync();
                //HttpResponseMessage response = await client.PostAsJsonAsync(@"http://localhost:61866/Token", data);

                var responseJSON = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    if (responseJSON["access_token"] != null)
                        _token = responseJSON["access_token"].ToString();
                }
                else
                {
                    if (responseJSON["error_description"] != null)
                        errorDescription = responseJSON["error_description"].ToString();
                }
            }

            SetLastCallResult(success, "GetToken", errorDescription);
        }

        
        public async void LoginAsync(string userName, string password, bool rememberMe)
        {
            using (var client = GetClient())
            {
                var data = new { Email = userName, Password = password, RememberMe = rememberMe };
                
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);

                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_serverName + @"/api/Account/Login", content);
                //HttpResponseMessage response = await client.PostAsJsonAsync(_serverName + @"/api/Account/Login", data);
                
                //string cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();

                string apiReturn = await response.Content.ReadAsStringAsync();

                var returnContent = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                   
                }
                else
                {
                    string failedReason = returnContent["Message"].ToString();
                }
            }
        }

        // Just another way to get the token with HttpWebRequest
        private string GetOAuthToken(string userName, string password)
        {            
            string serverName = _serverName;
            System.Net.HttpWebRequest myRequest = (HttpWebRequest)System.Net.WebRequest.Create("http://" + serverName + "//token");
            
            string postData = "grant_type=password&password=" + password + "&username=" + userName;

            byte[] data = System.Text.Encoding.ASCII.GetBytes(postData);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            System.IO.Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myRequest.GetResponse();

            System.IO.Stream responseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, System.Text.Encoding.Default);

            CookieCollection co = myHttpWebResponse.Cookies;
            string pageContent = myStreamReader.ReadToEnd();
            var tokenContent = JObject.Parse(pageContent);
            string token = "";
            if (tokenContent["access_token"] != null)
                token = tokenContent["access_token"].ToString();
            //string token = tokenContent["access_token"] ?? "";

            myStreamReader.Close();
            responseStream.Close();

            myHttpWebResponse.Close();

            return token;
        }

        public void GetSendCodeProviders(string userName)
        {
            using (var client = GetClient())
            {
                var data = new { Email = userName };

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(_serverName + @"/api/Account/GetSendCodeProviders", content).Result;
                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + @"/api/Account/GetSendCodeProviders", data).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {                    
                    JArray codeProv = JArray.Parse(apiReturn);
                    var list = (from t in codeProv select new { Text = t["Text"], Value = t["Value"] }).ToList();
                    SetLastCallResult(true, "GetSendCodeProviders", "");
                }
                else
                {
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, "GetSendCodeProviders", failedReason);
                }
            }            
        }

        public async void SendCodeAsync(string userName, string selectedProvider)
        {
            using (var client = GetClient())
            {
                var data = new { Email = userName, SelectedProvider = selectedProvider };

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_serverName + @"/api/Account/SendCode", content);
                //HttpResponseMessage response = await client.PostAsJsonAsync(_serverName + @"/api/Account/SendCode", data);
                string apiReturn = await response.Content.ReadAsStringAsync();
                var returnContent = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {                    
                    SetLastCallResult(true, "SendCode", "");
                }
                else
                {                    
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, "SendCode", failedReason);
                }
            }
        }

        public void SendCode(string userName, string selectedProvider)
        {
            using (var client = GetClient())
            {
                var data = new { Email = userName, SelectedProvider = selectedProvider };

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(_serverName + @"/api/Account/SendCode", content).Result;
                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + @"/api/Account/SendCode", data).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    JObject codeProv = JObject.Parse(apiReturn);
                    //var list = (from t in codeProv select new { Text = t["Text"], Value = t["Value"] }).ToList();
                    SetLastCallResult(true, "SendCode", "");
                }
                else
                {
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, "SendCode", failedReason);
                }
            }
        }

        public void VerifyCode(string userName, string provider, string code)
        {
            using (var client = GetClient())
            {
                var data = new { Email = userName, Provider = provider, Code = code };

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(_serverName + @"/api/Account/VerifyCoded", content).Result;
                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + @"/api/Account/VerifyCode", data).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;
                var returnContent = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, "SendCode", "");
                }
                else
                {                
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, "SendCode", failedReason);
                }
            }
        }

        public void Register(RegisterBindingModel model)
        {
            using (var client = GetClient())
            {
                var data = new { Email = model.Email, Password = model.Password, ConfirmPassword = model.ConfirmPassword, Number = model.Number };
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + @"/api/Account/Register", data).Result;
                HttpResponseMessage response = client.PostAsync(_serverName + @"/api/Account/Register", content).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;
                

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, "Register", "");
                }
                else
                {
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, "Register", failedReason);
                }
            }
        }

        public async void RegisterAsync(RegisterBindingModel model)
        {
            using (var client = GetClient())
            {
                var data = new { Email = model.Email, Password = model.Password, ConfirmPassword = model.ConfirmPassword, Number = model.Number };
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_serverName + @"/api/Account/Register", content);
                //HttpResponseMessage response = await client.PostAsJsonAsync(_serverName + @"/api/Account/Register", data);
                string apiReturn = await response.Content.ReadAsStringAsync();
                var returnContent = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, "Register", "");
                }
                else
                {
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, "Register", failedReason);
                }
            }
        }
        protected string _serverName = string.Empty;
        private string _userName = string.Empty;
        private string _password = string.Empty;

        private CookieContainer _cookieContainer;
        private HttpClientHandler _handler;

        // keep a list of calls and performance
        protected string _token = string.Empty;

        protected bool _lastCallSuccess = false;
        protected string _lastErrorDescription = string.Empty;
        protected string _lastCallMethod = string.Empty;
        // keep an object of last call status???


    }

    public class ReferralAPICaller : APIClientBase
    {
        public ReferralAPICaller(string serverName) : base(serverName)
        {            
        }

        public List<IReferral> GetReferrals(string userName)
        {
            return GetReferralsAsync(userName);
        }
        
        public List<IReferral> GetReferralsAsync(string userName)
        {
            List<IReferral> returnList = new List<IReferral>();

            using (var client = GetClient())
            {
                string data = userName;

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);

                HttpResponseMessage response = client.GetAsync(_serverName + @"/api/Referral/GetReferrals?username=" + userName).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    JArray codeProv = JArray.Parse(apiReturn);
                    foreach(var c in codeProv)
                    {
                        Referral referral = new Referral();
                        referral.FirstName = c["FirstName"].ToString();
                        referral.LastName = c["LastName"].ToString();
                        if (null != c["DOB"]) referral.DOB = (DateTime)c["DOB"];
                        if (null != c["Id"]) referral.Id = (int)c["Id"];
                        referral.PatientId = c["PatientId"].ToString();
                        referral.Notes = c["Notes"].ToString();
                        referral.CreatedBy = c["created_by"].ToString();
                        referral.ModifiedBy = c["modified_by"].ToString();
                        if (null != c["AccountId"]) referral.AccountId = (Guid)c["AccountId"];
                        if (null != c["AccountGroupId"]) referral.AccountGroupId = (Guid)c["AccountGroupId"];
                        if (null != c["ProgramTypeId"]) referral.ProgramTypeId = (Guid)c["ProgramTypeId"];
                        if (null != c["ReferralStatusId"]) referral.ReferralStatusId = (Guid)c["ReferralStatusId"];
                        referral.ReferralStatus = c["ReferralStatus"].ToString();                        

                        returnList.Add(referral);
                    }
                    
                    SetLastCallResult(true, "GetReferralsAsync", "");
                }
                else
                {
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, "GetReferralsAsync", failedReason);
                }
            }
        
            return returnList;
        }




    }

    public interface IReferral
    {
        DateTime? DOB { get; set; }
        string FirstName { get; set; }
        int Id { get; set; }
        string LastName { get; set; }
        string PatientId { get; set; }
        string PhoneNumber { get; set; }
        string Notes { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string PostalCode { get; set; }
        Guid? StateTypeId { get; set; }
        string State { get; set; }
        Guid? CountryTypeId { get; set; }
        string Country { get; set; }
    }

    public class Referral : IReferral
    {
        public int Id { get; set; }

        public string PatientId
        { get; set; }
        
        public string FirstName
        { get; set; }
        
        public string LastName
        { get; set; }
        
        public DateTime? DOB
        { get; set; }

        public string PhoneNumber
        { get; set; }

        public string Notes
        { get; set; }

        public string CreatedBy
        { get; set; }

        public string ModifiedBy
        { get; set; }

        public Guid? AccountId
        {
            get; set;
        }

        public Guid? AccountGroupId
        { get; set; }

        public Guid? ProgramTypeId
        { get; set; }

        public Guid? ReferralStatusId
        { get; set; }

        public string ReferralStatus
        { get; set; }

        public string AdditionalData
        { get; set; }

        public string Address1
        { get; set; }

        public string Address2
        { get; set; }

        public string City
        { get; set; }

        public string PostalCode
        { get; set; }

        public Guid? StateTypeId
        { get; set; }

        public Guid? CountryTypeId
        { get; set; }

        public string State
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }
    }

    public class RegisterBindingModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Number { get; set; }

        public string BaseUrl { get; set; }

    }
}

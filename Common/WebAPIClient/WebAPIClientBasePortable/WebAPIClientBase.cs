using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAPIModel;
//using WebAPIClientBasePortable.Models;


namespace WebAPIClientBasePortable
{
    //public interface IClientBase
    //{
    //    bool IsLastCallSuccess { get; }
    //}

    public abstract class APIClientBase //: IClientBase
    {
        public delegate void APICallHandler(string message);

        protected Dictionary<string, string> apiPath = new Dictionary<string, string>();

        public event APICallHandler BeginCallHandler;
        public event APICallHandler FailedCallHandler;
        public event APICallHandler SuccessCallHandler;

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

        Dictionary<string, string> codeProvider = new Dictionary<string, string>();

        public string ServerName
        {
            get { return _serverName; }
        }

        public APIClientBase(string serverName)
        {
            _serverName = serverName;
            InitAPIPaths();
        }
        string loginUrl = "";

        protected virtual void InitAPIPaths()
        {
            apiPath = new Dictionary<string, string>();
            apiPath.Add(ClientAPIPath.GetToken, @"/Token");
            apiPath.Add(ClientAPIPath.Login, @"/api/Account/Login");
            apiPath.Add(ClientAPIPath.Logout, @"/api/Account/Logout");
            apiPath.Add(ClientAPIPath.Register, @"/api/Account/Register");
            apiPath.Add(ClientAPIPath.SetPassword, @"/api/Account/SetPassword");
            apiPath.Add(ClientAPIPath.ConfirmEmail, @"/AccountConfirm/ConfirmEmail?userId={0}&code={1}");
            apiPath.Add(ClientAPIPath.VerifyCode, @"/api/Account/VerifyCode");
            apiPath.Add(ClientAPIPath.GetSendCodeProviders, @"/api/Account/GetSendCodeProviders");
            apiPath.Add(ClientAPIPath.SendCode, @"/api/Account/SendCode");

        }

        public string GetServerAPIPath(string referencePath)
        {            
            return _serverName + GetAPIPath(referencePath);
        }

        public string GetAPIPath(string referencePath)
        {
            string path = string.Empty;
            if (apiPath.ContainsKey(referencePath))
                path = apiPath[referencePath];
            return path;
        }

        public void AddAPIPath(string key, string path)
        {
            if (!apiPath.ContainsKey(key))
                apiPath.Add(key, path);
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

        public bool IsLastCallSuccess
        {
            get { return _lastCallSuccess; }
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

        public Dictionary<string, string> CodeProviders
        {
            get { return codeProvider; }
        }

        public void SetDefaultHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
        }

        // This particular Login, gets a bearer token and login
        public bool Login(string userName, string password, bool rememberMe)
        {
            _userName = userName;
            _password = password;

            using (HttpClient client = GetClient(apiPath[ClientAPIPath.Login]))
            {
                var data = new { Email = userName, Password = password, RememberMe = rememberMe };

                List<KeyValuePair<string, string>> loginInfo = new List<KeyValuePair<string, string>>();
                loginInfo.Add(new KeyValuePair<string, string>("grant_type", "password"));
                loginInfo.Add(new KeyValuePair<string, string>("Email", _userName));
                loginInfo.Add(new KeyValuePair<string, string>("password", _password));

                var loginContent = new FormUrlEncodedContent(loginInfo);

                SetDefaultHeaders(client);

                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.Login], loginContent).Result;

                string apiReturn = response.Content.ReadAsStringAsync().Result;

                var returnContent = JObject.Parse(apiReturn);

                _lastCallSuccess = response.IsSuccessStatusCode;

                string failedReason = string.Empty;
                if (!_lastCallSuccess)
                {
                    failedReason = returnContent["Message"].ToString();
                }
                SetLastCallResult(_lastCallSuccess, apiPath[ClientAPIPath.Login], failedReason);
            }

            return _lastCallSuccess;

        }

        public void SetLastCallResult(bool success, string lastCall, string lastCallError)
        {
            // save the list???
            _lastCallSuccess = success;
            _lastCallMethod = lastCall;
            _lastErrorDescription = lastCallError;
            if (success)
            {
                if (SuccessCallHandler != null)
                    SuccessCallHandler(string.Format("{0} success", lastCall));
            }
            else
            {
                if (FailedCallHandler != null)
                    FailedCallHandler(string.Format("{0} failed: {1}", lastCall, lastCallError));
            }

        }

        public void SetBeginCallEvent(string lastCall)
        {
            if (BeginCallHandler != null)
            {
                BeginCallHandler(lastCall);
            }
        }

        public HttpClient GetClient(string lastCall, bool setDefaultHeaders)
        {
            SetBeginCallEvent(lastCall);
            HttpClient client = GetClient();
            if (setDefaultHeaders)
                SetDefaultHeaders(client);
            return client;
        }

        public HttpClient GetClient(string lastCall)
        {
            SetBeginCallEvent(lastCall);
            return GetClient();
        }

        public HttpClient GetClient()
        {
            // the second param prevents the Handler from being disposed with the client
            HttpClient client = new HttpClient(Handler, false);
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

            using (HttpClient client = GetClient(apiPath[ClientAPIPath.GetToken]))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Referrer = new Uri(_serverName);

                List<KeyValuePair<string, string>> loginInfo = new List<KeyValuePair<string, string>>();
                loginInfo.Add(new KeyValuePair<string, string>("grant_type", "password"));
                loginInfo.Add(new KeyValuePair<string, string>("username", _userName));
                loginInfo.Add(new KeyValuePair<string, string>("password", _password));

                var loginContent = new FormUrlEncodedContent(loginInfo);

                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.GetToken], loginContent).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;

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

            SetLastCallResult(success, ClientAPIPath.GetToken, errorDescription);
        }

        private async void GetTokenAsyncInternal()
        {
            bool success = false;
            string errorDescription = string.Empty;

            string token = string.Empty;

            using (HttpClient client = GetClient())
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

                var responseJSON = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    if (responseJSON["access_token"] != null)
                        _token = responseJSON["access_token"].ToString();
                    success = true;
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
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.Login]))
            {
                var data = new { Email = userName, Password = password, RememberMe = rememberMe };

                SetDefaultHeaders(client);

                // This PostAsJsonAsync is available to the .net framework,
                //HttpResponseMessage response = await client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.Login], data);

                // replace this for portable library
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_serverName + apiPath[ClientAPIPath.Login], content);

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

        public void Logout()
        {
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.Logout], true))
            {
                var data = new { };
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.Logout], content).Result;

                string apiReturn = response.Content.ReadAsStringAsync().Result;

                var returnContent = JObject.Parse(apiReturn);

                _lastCallSuccess = response.IsSuccessStatusCode;

                string failedReason = string.Empty;
                if (!_lastCallSuccess)
                {
                    failedReason = returnContent["Message"].ToString();
                }
                SetLastCallResult(_lastCallSuccess, apiPath[ClientAPIPath.Login], failedReason);
            }
        }

        public void GetSendCodeProviders(string userName)
        {
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.GetSendCodeProviders]))
            {
                var data = new { Email = userName };

                SetDefaultHeaders(client);

                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.GetSendCodeProviders], data).Result;

                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.GetSendCodeProviders], content).Result;

                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    JArray codeProv = JArray.Parse(apiReturn);
                    var list = (from t in codeProv select new { Text = t["Text"].ToString(), Value = t["Value"].ToString() }).ToList();
                    this.codeProvider = list.ToDictionary(x => x.Text, x => x.Value);
                    SetLastCallResult(true, ClientAPIPath.GetSendCodeProviders, "");
                }
                else
                {
                    //dynamic returnContent = JObject.Parse(apiReturn);
                    //string failedReason = returnContent.Message;
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, ClientAPIPath.GetSendCodeProviders, failedReason);
                }
            }
        }

        public async void SendCodeAsync(string userName, string selectedProvider)
        {
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.SendCode]))
            {
                var data = new { Email = userName, SelectedProvider = selectedProvider };

                SetDefaultHeaders(client);

                //HttpResponseMessage response = await client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.SendCode], data);

                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_serverName + apiPath[ClientAPIPath.SendCode], content);

                string apiReturn = await response.Content.ReadAsStringAsync();
                var returnContent = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, ClientAPIPath.SendCode, "");
                }
                else
                {
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, ClientAPIPath.SendCode, failedReason);
                }
            }
        }

        public void SendCode(string userName, string selectedProvider)
        {
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.SendCode]))
            {
                var data = new { Email = userName, SelectedProvider = selectedProvider };

                SetDefaultHeaders(client);

                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.SendCode], data).Result;
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.SendCode], content).Result;

                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    JObject jo = JObject.Parse(apiReturn);

                    SetLastCallResult(true, ClientAPIPath.SendCode, "");
                }
                else
                {
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, ClientAPIPath.SendCode, failedReason);
                }
            }
        }


        public virtual void VerifyCode(string userName, string provider, string code)
        {
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.VerifyCode]))
            {
                var data = new { Email = userName, Provider = provider, Code = code };

                SetDefaultHeaders(client);

                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.VerifyCode], data).Result;
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.VerifyCode], content).Result;

                string apiReturn = response.Content.ReadAsStringAsync().Result;
                var returnContent = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, ClientAPIPath.VerifyCode, "");
                }
                else
                {
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, ClientAPIPath.VerifyCode, failedReason);
                }
            }
        }

        protected virtual RegisterBindingModel getRegisterBindingModel(RegisterBindingModel model)
        {
            return new RegisterBindingModel { Email = model.Email, Password = model.Password, ConfirmPassword = model.ConfirmPassword, Number = model.Number };
        }

        public virtual void Register(RegisterBindingModel model)
        {
            SetBeginCallEvent(ClientAPIPath.Register);

            using (HttpClient client = GetClient(apiPath[ClientAPIPath.Register]))
            {
                var data = getRegisterBindingModel(model);

                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.Register], data).Result;

                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.Register], content).Result;

                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, ClientAPIPath.Register, "");
                }
                else
                {
                    string failedReason = ParseReturnError(apiReturn);                    
                    SetLastCallResult(false, ClientAPIPath.Register, failedReason);
                }
            }
        }

        public virtual async void RegisterAsync(RegisterBindingModel model)
        {
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.Register]))
            {
                var data = getRegisterBindingModel(model); // new { Email = model.Email, Password = model.Password, ConfirmPassword = model.ConfirmPassword, Number = model.Number };

                //HttpResponseMessage response = await client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.Register], data);
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(_serverName + apiPath[ClientAPIPath.Register], content);

                string apiReturn = await response.Content.ReadAsStringAsync();
                var returnContent = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, ClientAPIPath.Register, "");
                }
                else
                {
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, ClientAPIPath.Register, failedReason);
                }
            }
        }

        public virtual void SetPassword(SetPasswordBindingModel model)
        {
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.SetPassword]))
            {
                var data = new { NewPassword = model.NewPassword, ConfirmPassword = model.ConfirmPassword };

                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.SetPassword], data).Result;
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.SetPassword], content).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, ClientAPIPath.SetPassword, "");
                }
                else
                {
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, ClientAPIPath.SetPassword, failedReason);
                }
            }
        }

        public virtual void ForgotPassword(ForgotPasswordViewModel model)
        {
            using (HttpClient client = GetClient(apiPath[ClientAPIPath.ForgotPassword]))
            {
                var data = new { Email = model.Email, BaseUrl = model.BaseUrl };

                //HttpResponseMessage response = client.PostAsJsonAsync(_serverName + apiPath[ClientAPIPath.ForgotPassword], data).Result;
                JObject o = JObject.FromObject(data);
                HttpContent content = new StringContent(o.ToString(), UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(_serverName + apiPath[ClientAPIPath.ForgotPassword], content).Result;
                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    SetLastCallResult(true, ClientAPIPath.ForgotPassword, "");
                }
                else
                {
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, ClientAPIPath.ForgotPassword, failedReason);
                }
            }
        }

        private string ParseReturnError(string apiReturn)
        {
            var returnContent = JObject.Parse(apiReturn);
            string failedReason = returnContent["Message"].ToString();
            if (returnContent["ModelState"] != null)
            {
                failedReason += ":";
                foreach(string error in returnContent["ModelState"][""])
                {
                    failedReason += "\r\n" + error;
                }                
            }

            return failedReason;
        }
            
    }
}

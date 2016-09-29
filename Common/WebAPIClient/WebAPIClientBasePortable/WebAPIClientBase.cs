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

        
        // This particular Login, bearer token is already there.
        public bool Login(string userName, string password, bool rememberMe)
        {
            _userName = userName;
            _password = password;

            string apiFunction = ClientAPIPath.Login;            

            var data = new { Email = userName, Password = password, RememberMe = rememberMe };

            JObject joData = JObject.FromObject(data);

            CallWebAPI(apiFunction, joData);            

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
            _userName = userName;
            _password = password;

            string apiFunction = ClientAPIPath.Login;
            
            var data = new { Email = userName, Password = password, RememberMe = rememberMe };
            JObject joData = JObject.FromObject(data);

            await CallWebAPIAsync(apiFunction, joData);
            
        }

        public void Logout()
        {
            string apiFunction = ClientAPIPath.Logout;            

            var data = new {};
            JObject joData = JObject.FromObject(data);

            CallWebAPI(apiFunction, joData);
        }

        public void GetSendCodeProviders(string userName)
        {
            Action<string> assignProviders = (apiReturn) =>
            {                
                JArray codeProv = JArray.Parse(apiReturn);
                var list = (from t in codeProv select new { Text = t["Text"].ToString(), Value = t["Value"].ToString() }).ToList();
                this.codeProvider = list.ToDictionary(x => x.Text, x => x.Value);
            };

            string apiFunction = ClientAPIPath.GetSendCodeProviders;

            var data = new { Email = userName };
            JObject joData = JObject.FromObject(data);

            CallWebAPI(apiFunction, joData, assignProviders);
           
        }

        public async void SendCodeAsync(string userName, string selectedProvider)
        {
            string apiFunction = ClientAPIPath.SendCode;
            
            var data = new { Email = userName, SelectedProvider = selectedProvider };
            JObject joData = JObject.FromObject(data);

            await CallWebAPIAsync(apiFunction, joData);
        }
        
        public void SendCode(string userName, string selectedProvider)
        {
            string apiFunction = ClientAPIPath.SendCode;
                    
            var data = new { Email = userName, SelectedProvider = selectedProvider };
            JObject joData = JObject.FromObject(data);

            CallWebAPI(apiFunction, joData);
        }
        
        public virtual void VerifyCode(string userName, string provider, string code)
        {
            string apiFunction = ClientAPIPath.VerifyCode;
            
            var data = new { Email = userName, Provider = provider, Code = code };
            JObject joData = JObject.FromObject(data);

            CallWebAPI(apiFunction, joData);  
        }

        protected virtual RegisterBindingModel getRegisterBindingModel(RegisterBindingModel model)
        {
            return new RegisterBindingModel { BaseUrl = model.BaseUrl, Email = model.Email, Password = model.Password, ConfirmPassword = model.ConfirmPassword, Number = model.Number };
        }

        public virtual void Register(RegisterBindingModel model)
        {
            string apiFunction = ClientAPIPath.Register;
            
            var data = getRegisterBindingModel(model);            
            JObject joData = JObject.FromObject(data);

            CallWebAPI(apiFunction, joData);
        }

        public virtual async void RegisterAsync(RegisterBindingModel model)
        {
            string apiFunction = ClientAPIPath.Register;
            
            var data = getRegisterBindingModel(model);
            JObject joData = JObject.FromObject(data);

            await CallWebAPIAsync(apiFunction, joData);
           
        }

        public virtual void SetPassword(SetPasswordBindingModel model)
        {
            string apiFunction = ClientAPIPath.SetPassword;
            
            var data = new { NewPassword = model.NewPassword, ConfirmPassword = model.ConfirmPassword };
            JObject joData = JObject.FromObject(data);

            CallWebAPI(apiFunction, joData);
            
        }

        public virtual void ForgotPassword(ForgotPasswordViewModel model)
        {
            string apiFunction = ClientAPIPath.ForgotPassword;
            
            var data = new { Email = model.Email, BaseUrl = model.BaseUrl };
            JObject joData = JObject.FromObject(data);

            CallWebAPI(apiFunction, joData);           
        }

        public void CallWebAPI(string apiFunction, JObject jObjData)
        {
            CallWebAPI(apiFunction, jObjData, null);
        }

        public void CallWebAPI(string apiFunction, JObject jObjData, Action<string> processAPIReturn)
        {            
            string apiFunctionPath = apiPath[apiFunction];

            try
            {
                using (HttpClient client = GetClient(apiFunctionPath, true))
                {
                    HttpContent content = new StringContent(jObjData.ToString(), UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(_serverName + apiFunctionPath, content).Result;

                    string apiReturn = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        SetLastCallResult(true, apiFunction, "");
                        if (processAPIReturn != null)
                            processAPIReturn(apiReturn);
                    }
                    else
                    {
                        string failedReason = ParseReturnError(apiReturn);
                        SetLastCallResult(false, apiFunction, failedReason);
                    }
                }
            }
            catch(Exception ex)
            {
                SetLastCallResult(false, apiFunction, ex.Message);
            }
        }

        public void CallWebAPIGet(string apiFunction, Action<string> processAPIReturn)
        {
            string apiFunctionPath = apiPath[apiFunction];

            try
            {                
                using (HttpClient client = GetClient(apiFunctionPath, true))
                {
                    HttpResponseMessage response = client.GetAsync(GetServerAPIPath(apiFunction)).Result;

                    string apiReturn = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        //number = int.Parse(apiReturn);
                        if (processAPIReturn != null)
                            processAPIReturn(apiReturn);
                        SetLastCallResult(true, apiFunction, "");
                    }
                    else
                    {
                        string failedReason = ParseReturnError(apiReturn);
                        SetLastCallResult(false, apiReturn, failedReason);
                    }
                }
            }
            catch (Exception ex)
            {
                SetLastCallResult(false, apiFunction, ex.Message);
            }
        }


        private async Task CallWebAPIAsync(string apiFunction, JObject jObjData)
        {
            string apiFunctionPath = apiPath[apiFunction];

            try
            {
                using (HttpClient client = GetClient(apiFunctionPath, true))
                {
                    HttpContent content = new StringContent(jObjData.ToString(), UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(_serverName + apiFunctionPath, content);

                    string apiReturn = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        JObject jo = JObject.Parse(apiReturn);
                        SetLastCallResult(true, apiFunction, "");
                    }
                    else
                    {
                        string failedReason = ParseReturnError(apiReturn);
                        SetLastCallResult(false, apiFunction, failedReason);
                    }
                }
            }
            catch (Exception ex)
            {
                SetLastCallResult(false, apiFunction, ex.Message);
            }
        }
        
        private string ParseReturnError(string apiReturn)
        {
            string failedReason = apiReturn;
            
            if (IsJson(apiReturn))
            {
                var returnContent = JObject.Parse(apiReturn);
                failedReason = returnContent["Message"].ToString();
                if (returnContent["ModelState"] != null)
                {
                    failedReason += ":";
                    foreach (string error in returnContent["ModelState"][""])
                    {
                        failedReason += "\r\n" + error;
                    }
                }

                if (returnContent["ExceptionMessage"] != null)
                {
                    failedReason += "\r\n" + returnContent["ExceptionMessage"].ToString();
                }
            }
            

            return failedReason;
        }
        
        private bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }
    }
}

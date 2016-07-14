using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using UIKit;
using TestDovetailAPIUI;


namespace iPhoneSVA
{
    public partial class ViewController : UIViewController
    {
        private ReferralAPICaller caller = null;

        private ReferralAPICaller GetReferralAPICaller
        {
            get { if (caller == null)
                    caller = new ReferralAPICaller("https://api.dovetailhealth.com");
                        return caller; }
        }

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void UIButton3_TouchUpInside(UIButton sender)
        {
            this.lblHello.Text = "Button Clicked!";
            GetOAuthToken2(); 
            //this.lblHello.Text = GetOAuthToken2();
        }

        private CookieContainer _cookieContainer;
        private HttpClientHandler _handler;

        private HttpClientHandler Handler
        {
            get
            {
                if (_handler == null)
                {
                    _cookieContainer = new CookieContainer();
                    _handler = new HttpClientHandler
                    {
                        CookieContainer = _cookieContainer,
                        UseCookies = true,
                        AllowAutoRedirect = false
                    };
                }
                return _handler;
            }
        }
        private HttpClient GetClient()
        {
            // second param prevents the Handler from being disposed with the client
            var client = new HttpClient(Handler, false);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;

        }

        private async void GetOAuthToken2()
        {
            string username = textboxUserName.Text; // "dchang@dovetailhealth.com";
            string password = textboxPassword.Text;  //"741746Dc!";
            using (var client = GetClient())
            {
                string serverName = "https://api.dovetailhealth.com";
                //string serverName = "http://192.168.56.101:61866";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                client.DefaultRequestHeaders.Referrer = new Uri(serverName);

                //
                client.DefaultRequestHeaders.Connection.Add("keep-Alive");
                //        client.DefaultRequestHeaders.Add("Host", "localhost");
                //
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                //        client.DefaultRequestHeaders.Add("User-Agent", "WIN");

                List<KeyValuePair<string, string>> loginInfo = new List<KeyValuePair<string, string>>();
                loginInfo.Add(new KeyValuePair<string, string>("grant_type", "password"));
                loginInfo.Add(new KeyValuePair<string, string>("username", username));
                loginInfo.Add(new KeyValuePair<string, string>("password", password));

                var loginContent = new FormUrlEncodedContent(loginInfo);
                //var data = new { Email = textBoxUserName.Text, Password = textBoxPassword.Text, RememberMe = false };
            //HttpStringContent.HttpContent stringContent = new HttpStringContent();
            //string postData = "grant_type=password&password=" +textBoxPassword.Text + "&username=" + textBoxUserName.Text;
            //        //var data = new { Email = textBoxUserName.Text, SelectedProvider = "Phone Code", ReturnUrl = "", RememberMe = false };
        //        //var data = new { }
        //HttpContent content = new HttpStringContent();

        HttpResponseMessage response = await client.PostAsync(serverName + @"/Token", loginContent);
        string apiReturn = await response.Content.ReadAsStringAsync();

        //HttpResponseMessage response =       client.PostAsync(@"http://localhost:61866/Token", loginContent).Result;


                //HttpResponseMessage response = await client.PostAsJsonAsync(@"http://localhost:61866/Token", data);
                //
                var responseJSON = JObject.Parse(apiReturn);
                //var address = new JavaScriptSerializer().Deserialize<dynamic>(apiReturn);

                //Dictionary<string, string> dict =  address as Dictionary<string, string>;
                //        dynamic returnContent = JObject.Parse(apiReturn);

                if (response.IsSuccessStatusCode)
                {
                    this.lblHello.Text = "success with token: " + responseJSON["access_token"].ToString();
                    //string saveTheToken = returnContent.Token;
                    //textBlockToken.Text = returnContent.Token;
                    //            //dynamic jsonObject = c.Deserialize(new StringReader(ooo.ToString()), );
                    //            //buttonSendCode.IsEnabled = true;
                    // Send the code
                }
                else
                {
                    string failedReason = ""; // returnContent.Message;
                    if (responseJSON["error_description"] != null)
                        failedReason = responseJSON["error_description"].ToString();
                    this.lblHello.Text = failedReason;
                    //MessageBox.Show(string.Format("Failed to login: { 0}", failedReason));
                }
            }
           // return "";
        }


        private string GetOAuthToken()
        {
            //var data = new { Email = textBoxUserName.Text, Password= textBoxPassword.Text, RememberMe = false };
        string serverName = "192.168.56.101:61866";
        System.Net.HttpWebRequest myRequest = (HttpWebRequest)System.Net.WebRequest.Create("http://" + serverName + "//token");
            string username = "dchang@hello.com"; // textBoxUserName.Text;
            string password = "password"; // textBoxPassword.Text;
        string postData = "grant_type=password&password=" + password + "&username=" + username;

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
        dynamic tokenContent = JObject.Parse(pageContent);
            string token = ""; // tokenContent.access_token ?? "";

        myStreamReader.Close();
            responseStream.Close();

            myHttpWebResponse.Close();

            return token;
        }

        partial void Login2_TouchUpInside(UIButton sender)
        {
            string username = textboxUserName.Text; // "dchang@dovetailhealth.com";
            string password = textboxPassword.Text;  //"741746Dc!";
            this.lblHello.Text = "Login Clicked";
            ReferralAPICaller caller = GetReferralAPICaller;

            caller.GetTokenAndLogin(username, password, false);
            Console.WriteLine(caller.LastError);
            this.lblHello.Text = "Login " + (caller.LastError == "" ? "OK" : caller.LastError);
        }

       

        partial void BtnSendCode_TouchUpInside(UIButton sender)
        {
            string username = textboxUserName.Text;
            ReferralAPICaller caller = GetReferralAPICaller;
            caller.SendCode(username, "Phone Code");
            this.lblHello.Text = "Send PHone Code";
        }

        partial void BtnVerifyCode_TouchUpInside(UIButton sender)
        {
            string username = textboxUserName.Text;
            string code = textboxCode.Text;
            this.lblHello.Text = "Sending Verify Code";
            ReferralAPICaller caller = GetReferralAPICaller;
            caller.VerifyCode(username, "Phone Code", code);
            if (caller.LastError == "")
                this.lblHello.Text = "Code verified";
            else
                this.lblHello.Text = caller.LastError;
        }



     

       


        partial void UIButton370_TouchUpInside(UIButton sender)
        {
            DismissViewController(true, null);
        }



       


    }
}
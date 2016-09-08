using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPIClientBasePortable;
using APIReferralAppPortable.Models;

using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace APIReferralAppPortable
{
    public class APIReferralApp : IAPIReferralApp
    {
        APIClientBase apiClientBase;

        public APIReferralApp(APIClientBase clientBase)
        {
            apiClientBase = clientBase;
            clientBase.AddAPIPath(ReferralAPIPath.GetReferrals, @"/api/Referral/GetReferrals/");
            
        }

        public List<IReferral> GetReferrals(string userName)
        {
            List<IReferral> referrals = new List<IReferral>();

            using (HttpClient client = apiClientBase.GetClient(ReferralAPIPath.GetReferrals, true))
            {
                //var data = new { Email = userName };
            
                HttpResponseMessage response = response = client.GetAsync(apiClientBase.GetServerAPIPath(ReferralAPIPath.GetReferrals) + "?username=" + userName).Result;
                //client.PostAsJsonAsync(_serverName + "api/Referral/GetReferrals", data).Result;

                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    JArray refs = JArray.Parse(apiReturn);
                    foreach (JToken refc in refs)
                    {
                        //var list = (from t in codeProv select new { Text = t["Text"].ToString(), Value = t["Value"].ToString() }).ToList();
                        Referral newRef = new Referral();
                        newRef.AccountId = new Guid(refc["AccountId"].ToString());
                        referrals.Add(newRef);
                    }
                    apiClientBase.SetLastCallResult(true, ReferralAPIPath.GetReferrals, "");
                }
                else
                {
                    //dynamic returnContent = JObject.Parse(apiReturn);
                    //string failedReason = returnContent.Message;
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    apiClientBase.SetLastCallResult(false, ReferralAPIPath.GetReferrals, failedReason);
                }

            }
            return referrals;
        }
    }
}

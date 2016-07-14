using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using WebAPIClient.Models;
using pclWebAPIClient;
//using Dovetail.Referral.BAL;
//using Dovetail.Referral.BAL.BusinessLayer;

namespace pclWebAPIClient.App
{
    public class ReferralAPICaller : APIClientBase
    {               
        public ReferralAPICaller(string serverName) : base(serverName)
        {

        }

        protected override void InitAPIPaths()
        {
            base.InitAPIPaths();

            // should this be a different class for references...?
            apiPath.Add(ReferenceAPIPath.GetCountries, @"/api/Reference/GetCountries");
            apiPath.Add(ReferenceAPIPath.GetStates, @"/api/Reference/GetStates");
            apiPath.Add(ReferenceAPIPath.GetReferencesByName, @"/api/Reference/GetReferencesByName");

            apiPath.Add(ReferralAPIPath.GetAccounts, @"/api/Referral/GetAccounts");
            apiPath.Add(ReferralAPIPath.GetAllAccountInfo, @"/api/Referral/GetAllAccountInfo");
            apiPath.Add(ReferralAPIPath.GetClients, @"/api/Referral/GetClients");
            apiPath.Add(ReferralAPIPath.GetExistingReferrals, @"/api/Referral/GetExistingReferrals");
            apiPath.Add(ReferralAPIPath.GetProgramTypes, @"/api/Referral/GetProgramTypes");
            apiPath.Add(ReferralAPIPath.GetReferrals, @"/api/ReferralGetReferrals/");
            apiPath.Add(ReferralAPIPath.GetReferralsForIntakeprocess, @"/api/Referral/GetReferralsForIntakeprocess");
            apiPath.Add(ReferralAPIPath.SaveReferral, @"/api/Referral/SaveReferral");           

        }

        //public List<Reference> GetCountries()
        //{
        //    List<Reference> countries = new List<Reference>();

        //    using (HttpClient client = GetClient())
        //    {
        //        //var data = new { Email = userName };
        //        SetDefaultHeaders(client);

        //        HttpResponseMessage response = response = client.GetAsync(_serverName + apiPath[ReferenceAPIPath.GetCountries]).Result;

        //        string apiReturn = response.Content.ReadAsStringAsync().Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            JArray codeProv = JArray.Parse(apiReturn);
        //            foreach (JToken refc in codeProv)
        //            {
        //                if (!(refc["id"] == null || string.IsNullOrEmpty(refc["id"].ToString())))
        //                {
        //                    Reference newRef = new Reference();
        //                    newRef.id = new Guid(refc["id"].ToString());
        //                    newRef.default_value = refc["default_value"].ToString();
        //                    newRef.short_value = refc["short_value"].ToString();
        //                    newRef.order = refc["order"].Value<int>();
        //                    countries.Add(newRef);
        //                }
        //            }
                   
        //            SetLastCallResult(true, apiPath[ReferenceAPIPath.GetCountries], "");
        //        }
        //        else
        //        {
        //            //dynamic returnContent = JObject.Parse(apiReturn);
        //            //string failedReason = returnContent.Message;
        //            var returnContent = JObject.Parse(apiReturn);
        //            string failedReason = returnContent["Message"].ToString();
        //            SetLastCallResult(false, apiPath[ReferenceAPIPath.GetCountries], failedReason);
        //        }

        //    }
        //    return countries;
        //}
        public List<Reference> GetCountries()
        {
            return GetReferences(ReferenceAPIPath.GetCountries);
        }

        public List<Reference> GetStates()
        {
            return GetReferences(ReferenceAPIPath.GetStates);
        }


        public List<Reference> GetReferences(string referencePath)
        {
            List<Reference> references = new List<Reference>();

            using (HttpClient client = GetClient(referencePath))
            {
                //var data = new { Email = userName };
                SetDefaultHeaders(client);

                HttpResponseMessage response = response = client.GetAsync(_serverName + apiPath[referencePath]).Result;

                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    JArray codeProv = JArray.Parse(apiReturn);
                    foreach (JToken refc in codeProv)
                    {
                        if (!(refc["id"] == null || string.IsNullOrEmpty(refc["id"].ToString())))
                        {
                            Reference newRef = new Reference();
                            newRef.id = new Guid(refc["id"].ToString());
                            newRef.default_value = refc["default_value"].ToString();
                            newRef.short_value = refc["short_value"].ToString();
                            newRef.order = refc["order"].Value<int>();
                            // add other columns
                            references.Add(newRef);
                        }
                    }

                    SetLastCallResult(true, apiPath[referencePath], "");
                }
                else
                {
                    //dynamic returnContent = JObject.Parse(apiReturn);
                    //string failedReason = returnContent.Message;
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, apiPath[referencePath], failedReason);
                }
            }
            return references;
        }

        //public List<Account> GetAccounts()
        //{
        //    List<Account> accounts = new List<Account>();

        //}


        public List<IReferral> GetReferrals(string userName)
        {
            List<IReferral> referrals = new List<IReferral>();

            using (HttpClient client = GetClient(ReferralAPIPath.GetReferrals))
            {
                //var data = new { Email = userName };
                SetDefaultHeaders(client);

                HttpResponseMessage response = response = client.GetAsync(_serverName + apiPath[ReferralAPIPath.GetReferrals] + "?username=" + userName).Result;
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
                    SetLastCallResult(true, ReferralAPIPath.GetReferrals, "");
                }
                else
                {
                    //dynamic returnContent = JObject.Parse(apiReturn);
                    //string failedReason = returnContent.Message;
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    SetLastCallResult(false, ReferralAPIPath.GetReferrals, failedReason);
                }

            }
             return referrals;
        }
    }

    //public interface IReferral
    //{
    //    DateTime? DOB { get; set; }
    //    string FirstName { get; set; }
    //    int Id { get; set; }
    //    string LastName { get; set; }
    //    string PatientId { get; set; }
    //    string PhoneNumber { get; set; }
    //    string Notes { get; set; }
    //    string CreatedBy { get; set; }
    //    string ModifiedBy { get; set; }
    //    string Address1 { get; set; }
    //    string Address2 { get; set; }
    //    string City { get; set; }
    //    string PostalCode { get; set; }
    //    Guid? StateTypeId { get; set; }
    //    string State { get; set; }
    //    Guid? CountryTypeId { get; set; }
    //    string Country { get; set; }
    //}

}

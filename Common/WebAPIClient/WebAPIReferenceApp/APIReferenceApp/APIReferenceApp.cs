using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIClientBasePortable;
using APIReferenceAppPortable;
using APIReferenceAppPortable.Models;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APIReferenceAppPortable
{
    public class APIReferenceApp : IAPIReferenceApp
    {
        APIClientBase apiClientBase;

        public APIReferenceApp(APIClientBase clientBase)
        {
            apiClientBase = clientBase;
            clientBase.AddAPIPath(ReferenceAPIPath.GetCountries, @"/api/Reference/GetCountries");
            clientBase.AddAPIPath(ReferenceAPIPath.GetStates, @"/api/Reference/GetStates");
            clientBase.AddAPIPath(ReferenceAPIPath.GetReferencesByName, @"/api/Reference/GetReferencesByName");
            clientBase.AddAPIPath(ReferenceAPIPath.GetNumber, @"/api/Reference/GetNumber");

        }

        public int GetNumber()
        {
            string ftn = "GetNumber";
            int number = 0;
            using (HttpClient client = apiClientBase.GetClient(ftn, true))
            {
                HttpResponseMessage response = client.GetAsync(apiClientBase.GetServerAPIPath(ftn)).Result;
                
                string apiReturn = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    //JArray codeProv = JArray.Parse(apiReturn);
                    //    foreach (JToken refc in codeProv)
                    //    {
                    //         if (!(refc["id"] == null || string.IsNullOrEmpty(refc["id"].ToString())))
                    //         {

                    //        }
                    //    }
                    number = int.Parse(apiReturn);

                    apiClientBase.SetLastCallResult(true, apiClientBase.GetAPIPath(ftn), "");
                }
                else
                {
                    //dynamic returnContent = JObject.Parse(apiReturn);
                    //string failedReason = returnContent.Message;
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    apiClientBase.SetLastCallResult(false, apiClientBase.GetAPIPath(ftn), failedReason);
                }
            }
            return number;
        }
    

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

            using (HttpClient client = apiClientBase.GetClient(referencePath))
            {
                //var data = new { Email = userName };
                apiClientBase.SetDefaultHeaders(client);

                HttpResponseMessage response = client.GetAsync(apiClientBase.GetServerAPIPath(referencePath)).Result;

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

                    apiClientBase.SetLastCallResult(true, apiClientBase.GetAPIPath(referencePath), "");
                }
                else
                {
                    //dynamic returnContent = JObject.Parse(apiReturn);
                    //string failedReason = returnContent.Message;
                    var returnContent = JObject.Parse(apiReturn);
                    string failedReason = returnContent["Message"].ToString();
                    apiClientBase.SetLastCallResult(false, apiClientBase.GetAPIPath(referencePath), failedReason);
                }
            }
            return references;
        }
    }
}

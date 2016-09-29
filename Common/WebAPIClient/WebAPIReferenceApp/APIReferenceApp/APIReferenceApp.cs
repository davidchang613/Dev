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
using WebAPIModel;

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
            clientBase.AddAPIPath(ReferenceAPIPath.GetReferences, @"/api/Reference/GetReferences");
        }

        public int GetNumber()
        {
            int num = 0;
            Action<string> processNumber = (apiReturn) =>
            {
                num = int.Parse(apiReturn);
            };

            string apiFunction = ReferenceAPIPath.GetNumber;

            //var data = new { };
            //JObject joData = JObject.FromObject(data);

            apiClientBase.CallWebAPIGet(apiFunction, processNumber);

            return num;
            
        }
    

        public List<Reference> GetCountries()
        {
            return GetReferences(ReferenceAPIPath.GetCountries);
        }

        public List<Reference> GetStates()
        {
            return GetReferences(ReferenceAPIPath.GetStates);
        }

        public List<Reference> GetReferences(string apiFunction)
        {
            List<Reference> references = new List<Reference>();

            Action<string> processReference = (apiReturn) =>
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
            };

            //var data = new { };
            //JObject joData = JObject.FromObject(data);

            apiClientBase.CallWebAPIGet(apiFunction, processReference);
            
            return references;
        }


        public Dictionary<string, List<Reference>> GetReferences(List<string> referenceNames)
        {
            string apiFunction = ReferenceAPIPath.GetReferences;

            Dictionary<string, List<Reference>> references = new Dictionary<string, List<Reference>>();

            Action<string> processReferences = (apiReturn) =>
            {
                JObject jObjData = JObject.Parse(apiReturn);

                foreach(string refName in referenceNames)
                {
                    if (jObjData[refName] != null)
                    {
                        JArray stateList = JArray.Parse(jObjData["STATE"].ToString());
                        var list = from state in stateList
                                   select new Reference()
                                   {
                                       id = new Guid(state["id"].ToString()),
                                       default_value = state["default_value"].ToString()

                                   };

                        references.Add(refName, list.ToList());
                    }
                }                                

            };

            var data = new { referenceNames };
            JObject joData = JObject.FromObject(data);

            apiClientBase.CallWebAPI(apiFunction, joData, processReferences);

            return references;

        }
        //public List<Reference> GetReferences(string referencePath)
        //{
        //    List<Reference> references = new List<Reference>();

        //    using (HttpClient client = apiClientBase.GetClient(referencePath, true))
        //    {
        //        //var data = new { Email = userName };
        //        //apiClientBase.SetDefaultHeaders(client);

        //        HttpResponseMessage response = client.GetAsync(apiClientBase.GetServerAPIPath(referencePath)).Result;

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
        //                    // add other columns
        //                    references.Add(newRef);
        //                }
        //            }

        //            apiClientBase.SetLastCallResult(true, apiClientBase.GetAPIPath(referencePath), "");
        //        }
        //        else
        //        {
        //            //dynamic returnContent = JObject.Parse(apiReturn);
        //            //string failedReason = returnContent.Message;
        //            var returnContent = JObject.Parse(apiReturn);
        //            string failedReason = returnContent["Message"].ToString();
        //            apiClientBase.SetLastCallResult(false, apiClientBase.GetAPIPath(referencePath), failedReason);
        //        }
        //    }
        //    return references;
        //}
    }
}

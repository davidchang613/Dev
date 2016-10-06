using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPIClientBasePortable;
using WebAPIModel;

namespace APIReferenceModulePortable
{
    public class APIReferenceModule : IAPIReferenceModule
    {
        APIClientBase apiClientBase;

        public APIReferenceModule(APIClientBase clientBase)
        {
            apiClientBase = clientBase;

            //clientBase.AddAPIPath(TemplateAPIKey.GetKey, TemplateAPIPath.GetPath);
            //clientBase.AddAPIPath(TemplateAPIKey.PostKey, TemplateAPIPath.PostPath);
            clientBase.AddAPIPath(ReferenceAPIKey.GetCountries, ReferenceAPIPath.GetCountriesPath);
            clientBase.AddAPIPath(ReferenceAPIKey.GetStates, ReferenceAPIPath.GetStatePaths);
            clientBase.AddAPIPath(ReferenceAPIKey.GetReferencesByName, ReferenceAPIPath.GetReferencesPath);
            clientBase.AddAPIPath(ReferenceAPIKey.GetNumber, ReferenceAPIPath.GetNumberPath);
            clientBase.AddAPIPath(ReferenceAPIKey.GetReferences, ReferenceAPIPath.GetReferencesPath);

        }


        public int GetNumber()
        {
            int num = 0;
            Action<string> processNumber = (apiReturn) =>
            {
                num = int.Parse(apiReturn);
            };

            string apiFunction = ReferenceAPIKey.GetNumber;

            //var data = new { };
            //JObject joData = JObject.FromObject(data);

            apiClientBase.CallWebAPIGet(apiFunction, processNumber);

            return num;

        }


        public List<Reference> GetCountries()
        {
            return GetReferences(ReferenceAPIKey.GetCountries);
        }

        public List<Reference> GetStates()
        {
            return GetReferences(ReferenceAPIKey.GetStates);
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
            string apiFunction = ReferenceAPIKey.GetReferences;

            Dictionary<string, List<Reference>> references = new Dictionary<string, List<Reference>>();

            Action<string> processReferences = (apiReturn) =>
            {
                JObject jObjData = JObject.Parse(apiReturn);

                foreach (string refName in referenceNames)
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
        public void TemplateGetCall()
        {
            //Template template = null;

            Action<string> processReturn = (apiReturn) =>
            {
                JObject jObjData = JObject.Parse(apiReturn);

                //template = new Template();
                //template.FieldName = jObjData["FieldName"].ToString();

            };

            //var data = new { };
            //JObject joData = JObject.FromObject(data);

            //apiClientBase.CallWebAPIGet(TemplateAPIPath.GetPath, parameter, processReturn);
            //apiClientBase.CallWebAPIGet(TemplateAPIPath.GetPath, processReturn);

            // return template; 
        }

        public void TemplatePostCall()
        {
            Action<string> processReturn = (apiReturn) =>
            {
                JObject jObjData = JObject.Parse(apiReturn);

                //template = new Template();

                //template.FieldName = jObjData["FieldName"].ToString();

            };

            //var data = new { };
            //JObject joData = JObject.FromObject(data);

            //apiClientBase.CallWebAPI(TemplateAPIPath.PostPath, joData, processReturn);
            // return template;
        }
    }
}

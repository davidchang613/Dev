using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPIClientBasePortable;

namespace APITemplateModulePortable
{
    public class APITemplateModule : IAPITemplateModule
    {        
        APIClientBase apiClientBase;

        public APITemplateModule(APIClientBase clientBase)
        {
            apiClientBase = clientBase;

            clientBase.AddAPIPath(TemplateAPIKey.GetKey, TemplateAPIPath.GetPath);
            clientBase.AddAPIPath(TemplateAPIKey.PostKey, TemplateAPIPath.PostPath);

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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPIClientBasePortable;
using WebAPIModel;

namespace APIMemberModulePortable
{
    public class APIMemberModule : IAPIMemberModule
    {
        APIClientBase apiClientBase;

        public APIMemberModule(APIClientBase clientBase)
        {
            apiClientBase = clientBase;

            clientBase.AddAPIPath(MemberAPIKey.GetKey, MemberAPIPath.GetPath);
            clientBase.AddAPIPath(MemberAPIKey.PostKey, MemberAPIPath.PostPath);
        }

        public Member Get(string Id)
        {
            Member member = null;

            Action<string> processMember = (apiReturn) =>
            {
                JObject jObjData = JObject.Parse(apiReturn);

                member = new Member();

                member.FirstName = jObjData["FirstName"].ToString();

            };

            //var data = new { };
            //JObject joData = JObject.FromObject(data);

            apiClientBase.CallWebAPIGet(MemberAPIKey.GetKey, Id, processMember);

            return member;
        }

        public void Post(Member mem)
        {

        }
    }
}

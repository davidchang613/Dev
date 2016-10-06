using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPISite.Attributes;
using WebAPIModel;

namespace WebAPISite.Controllers
{
    //[Authorize(Roles = "TwoFactorVerified")]
   // [Authorize]
    //[AuthorizeUserRole(]
    [RoutePrefix("api/Reference")]
    public class ReferenceController : ApiController
    {

        public IEnumerable<Reference> GetList()
        {
            List<Reference> list = new List<Reference>();
            
            return list;
        }

        public async Task<IEnumerable<Reference>> GetListAsync()
        {
            return await Task.FromResult(GetList());
        }

        [System.Web.Http.Route("GetNumber")]
        public async Task<IHttpActionResult> GetNumber()
        {
            int number = await getSomeNumber();
            return Ok(number);
        }

        [System.Web.Http.Route("GetStates")]
        public async Task<IHttpActionResult> GetStates()
        {
            List<Reference> refList = new List<Reference>();
            await Task.Run(() => {
                refList.Add(new Reference() { default_value = "defValue" });
                refList.Add(new Reference() { default_value = "defValue" });
            });
            
            return Ok(refList);
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("GetReferences")]
        public async Task<IHttpActionResult> GetReferences(List<string> referenceNames)
        {                        
            Dictionary<string, List<Reference>> referenceDictionary = new Dictionary<string, List<Reference>>();
            List<Reference> refList = new List<Reference>();
            await Task.Run(() => {
                refList.Add(new Reference() { default_value = "MA" });
                refList.Add(new Reference() { default_value = "NY" });
                referenceDictionary.Add("STATE", refList);

                refList = new List<Reference>();
                refList.Add(new Reference() { default_value = "STOPPED" });
                refList.Add(new Reference() { default_value = "RUNNING" });
                referenceDictionary.Add("STATUS", refList);

                refList = new List<Reference>();
                refList.Add(new Reference() { default_value = "JANUARY" });
                refList.Add(new Reference() { default_value = "FEBRUARY" });
                referenceDictionary.Add("MONTH", refList);

            });

            return Ok(referenceDictionary);
        }

        private Task<int> getSomeNumber()
        {
            return Task.Run(async () =>
            {
                await Task.Delay(10);
                return 123;
            });
        }
    }
}

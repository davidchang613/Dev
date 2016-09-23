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
    [Authorize]
    //[AuthorizeUserRole(]
    [RoutePrefix("api/Reference")]
    public class ReferenceController : ApiController
    {
        [System.Web.Http.Route("GetNumber")]
        public async Task<IHttpActionResult> GetNumber()
        {
            int number = await getSomeNumber();
            return Ok(number);
        }

        [System.Web.Http.Route("GetStates")]
        public async Task<IHttpActionResult> GetStates()
        {
            await Task.Delay(0);
            List<Reference> refList = new List<Reference>();
            refList.Add(new Reference() { default_value = "defValue" });
            refList.Add(new Reference() { default_value = "defValue" });
            return Ok(refList);
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

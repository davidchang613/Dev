using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPISite.Controllers
{
    [Authorize]
    [RoutePrefix("api/Reference")]
    public class ReferenceController : ApiController
    {
        public async Task<IHttpActionResult> GetNumber()
        {
            int number = await getSomeNumber();
            return Ok(number);
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

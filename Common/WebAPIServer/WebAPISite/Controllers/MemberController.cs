using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPIModel;

namespace WebAPISite.Controllers
{
    [RoutePrefix("api/Member")]
    public class MemberController : ApiController
    {

        public MemberController()
        {

        }

        //public async Task<IHttpActionResult> Get(Guid id)
        //{
        //    await Task.Factory.StartNew(() => { });
        //    return Ok("GET Id");
        //}

        public async Task<IHttpActionResult> Get(string id)
        {            
            Guid result;
            Member member = null;
            if (Guid.TryParse(id, out result))
            {
                await Task.Factory.StartNew(() => {
                    member = new Member() { FirstName = "David" };

                });
                
                return Ok(member);
            }
            else
                return BadRequest("ID is not valid");
        }

        public async Task<IHttpActionResult> Post(Member member)
        {
            await Task.Factory.StartNew(() => { });
            return Ok("Save");
        }
        public async Task<IHttpActionResult> Put(Member member)
        {
            await Task.Factory.StartNew(() => { });
            return Ok("Put");
        }
        
       
        [Route("All")]
        public async Task<IHttpActionResult> Members(string id)
        {
            await Task.Factory.StartNew(() => { });

            return Ok("");
        }

        [Route("Tasks")]
        public async Task<IHttpActionResult> Tasks(string id)
        {
            await Task.Factory.StartNew(() => { });

            return Ok("");
        }


    }
}

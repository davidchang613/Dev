using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DevExtremeMvcApp1.Controllers
{
    public class TagAPIController : ApiController
    {
        // GET api/<controller>
    //    public IEnumerable<string> Get()
       // {
    //        return new string[] { "value1", "value2" };
      //  }

        // GET api/<controller>/5

        

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
            
        }

        private void AddTagValue(string tagName, DateTime time, double val )
        {

        }

        public IEnumerable<TagValue> Get()
        {
            TagList listT = new TagList();
            DateTime rptTime = DateTime.Now.AddHours(-10);
            Random rnd = new Random();
            for(int i=1; i<60*24; i++)
            {
                listT.Add("CO2", rptTime.AddMinutes(i), rnd.NextDouble() * 50 );
                listT.Add("O2", rptTime.AddMinutes(i), rnd.NextDouble() * 80);
            }
             
            return listT.ToList();
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
            
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            
        }
    }

    public class TagValue
    {
        public string tagName;
        public DateTime time;
        public double val;

        public TagValue(string tagName, DateTime time, double val)
        {
            this.tagName = tagName;
            this.time = time;
            this.val = val;
        }
    }

    public class TagList
    {
        private List<TagValue> listTagValue = new List<TagValue>();
        public void Add(string tagName, DateTime time, double val)
        {
            TagValue tag = new TagValue(tagName, time, val);
            listTagValue.Add(tag);
        }

        public IEnumerable<TagValue> ToList()
        {
            return listTagValue;
        }
    }

}
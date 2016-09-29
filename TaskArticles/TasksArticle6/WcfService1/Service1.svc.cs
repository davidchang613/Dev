using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    public class Service1 : IService1
    {
        public List<String> GetData(int numberOf)
        {
            List<String> data = new List<string>();
            for (int i = 0; i < numberOf; i++)
            {
                data.Add(String.Format("String_{0}",i));
            }
            return data;
        }
    }
}

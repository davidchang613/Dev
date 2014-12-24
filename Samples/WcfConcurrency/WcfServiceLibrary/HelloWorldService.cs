using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using DataLibrary;
using System.ServiceModel.Web;

namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class HelloWorldService : IHelloWorldService
    {
        private int i;
        public void Call(string ClientName)
        {
            i++;
            Console.WriteLine("Client name :" + ClientName + " Instance:" + i.ToString() + " Thread:" + Thread.CurrentThread.ManagedThreadId.ToString() + " Time:" + DateTime.Now.ToString() + "\n\n");
            Thread.Sleep(5000);
        }
        
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        
        public Person GetPerson(int Id)
        {
            return new Person() { Id = 1, LastName = "Chang", FirstName = "David" };
        }
    }
}

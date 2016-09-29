using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AsyncFromBeginEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();

            Task<List<String>> task = 
                Task<List<String>>.Factory.FromAsync(
                    client.BeginGetData(10, null, null),
                    ar => client.EndGetData(ar));

            List<String> result = task.Result;
            Console.WriteLine("Successfully read all bytes using a Task");
            foreach (string s in result)
            {
                Console.WriteLine(s);
            }
            Console.ReadLine();
        }
    }
}

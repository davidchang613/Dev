using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace ConsoleWCFClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Client name");
            string str = Console.ReadLine();
            ServiceReference.HelloWorldServiceClient obj = new ServiceReference.HelloWorldServiceClient();
            for (int i = 0; i < 5; i++)
            {
                obj.Call(str);
                Thread.Sleep(2000);
            }
        }
    }
}

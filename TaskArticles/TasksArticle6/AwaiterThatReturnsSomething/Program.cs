using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AwaiterThatReturnsSomething
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(async delegate
            {
                double x = 10;
                double x2 = await x.GetAwaiter(3);
                Console.WriteLine(string.Format("x was : {0}, x2 is {1}",x,x2));
            });

            Console.ReadLine();
        }
    }
}

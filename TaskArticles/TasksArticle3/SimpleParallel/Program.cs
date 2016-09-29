using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleParallel
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> data = new List<string>() 
                { "There","were","many","animals","at","the","zoo"};

            //parallel for
            Parallel.For(0, 10, (x) =>
            {
                Console.WriteLine(x);
            });


            //parallel for each
            Parallel.ForEach(data, (x) =>
                {
                    Console.WriteLine(x);
                });


            Console.ReadLine();
        }
    }
}

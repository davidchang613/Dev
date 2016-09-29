using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelRange
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> results = (from i in ParallelEnumerable.Range(0, 100).AsOrdered() select i);

            foreach (int item in results)
            {
                Console.WriteLine("Result is {0}", item);
            }

            Console.ReadLine();

        }
    }
}

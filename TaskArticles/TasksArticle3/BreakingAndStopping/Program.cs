using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakingAndStopping
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> data = new List<string>() 
                { "There", "were", "many", "animals", "at", "the", "zoo" };

            //parallel for stop
            ParallelLoopResult res1 = Parallel.For(0, 10, (x, state) =>
            {
                if (x < 5)
                    Console.WriteLine(x);
                else
                    state.Stop();
            });

            Console.WriteLine("For loop LowestBreak Iteration : {0}", res1.LowestBreakIteration);
            Console.WriteLine("For loop Completed : {0}", res1.IsCompleted);
            Console.WriteLine("\r\n");



            //parallel foreach stop
            ParallelLoopResult res2 = Parallel.ForEach(data, (x, state) =>
            {
                if (!x.Equals("zoo"))
                    Console.WriteLine(x);
                else
                    state.Stop();
            });
            Console.WriteLine("Foreach loop LowestBreak Iteration : {0}", res2.LowestBreakIteration);
            Console.WriteLine("Foreach loop Completed : {0}", res2.IsCompleted);
            Console.WriteLine("\r\n");


            //parallel for each that actuaally breaks, rather than stops
            ParallelLoopResult res3 = Parallel.ForEach(data, (x, state) =>
            {
                if (x.Equals("zoo"))
                {
                    Console.WriteLine(x);
                    state.Break();
                }
            });
            Console.WriteLine("Foreach loop LowestBreak Iteration : {0}", res3.LowestBreakIteration);
            Console.WriteLine("Foreach loop Completed : {0}", res3.IsCompleted);
Console.WriteLine("\r\n");

            Console.ReadLine();
        }
    }
}

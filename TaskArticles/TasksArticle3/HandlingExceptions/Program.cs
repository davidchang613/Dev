using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlingExceptions
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> data = new List<string>() { "There", "were", "many", "animals", "at", "the", "zoo" };


            //parallel for Exception handling
            try
            {
                ParallelLoopResult res1 = Parallel.For(0, 10, (x, state) =>
                {
                    if (!state.IsExceptional)
                    {
                        if (x < 5)
                            Console.WriteLine(x);
                        else
                            throw new InvalidOperationException("Don't like nums > 5");
                    }
                });
                Console.WriteLine("For loop Completed : {0}", res1.IsCompleted);
                Console.WriteLine("\r\n");
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine(string.Format("Caught exception '{0}'",
                        ex.Message));
                }
            }






            //parallel foreach Exception handling
            try
            {
                ParallelLoopResult res2 = Parallel.ForEach(data, (x, state) =>
                {
                    if (!x.Equals("zoo"))
                        Console.WriteLine(x);
                    else
                        throw new InvalidOperationException("Found Zoo throwing Exception");
                });
                Console.WriteLine("Foreach loop Completed : {0}", res2.IsCompleted);
                Console.WriteLine("\r\n");
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine(string.Format("Caught exception '{0}'",
                        ex.Message));
                }
            }

         

            Console.ReadLine();
        }
    }
}

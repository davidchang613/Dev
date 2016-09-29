using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CancellingLoops
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> data = new List<string>() { "There", "were", "many", "animals", "at", "the", "zoo" };

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            Task cancelTask = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(500);
                    tokenSource.Cancel();
                });


            ParallelOptions options = new ParallelOptions()
            {
                CancellationToken = tokenSource.Token
            };


            //parallel for cancellation
            try
            {
                ParallelLoopResult res1 = Parallel.For(0, 1000, options, (x, state) =>
                {
                    if (x % 10 == 0)
                        Console.WriteLine(x);

                    Thread.Sleep(100);
                });

                Console.WriteLine("For loop LowestBreak Iteration : {0}", res1.LowestBreakIteration);
                Console.WriteLine("For loop Completed : {0}", res1.IsCompleted);
                Console.WriteLine("\r\n");
            }
            catch (OperationCanceledException opCanEx)
            {
                Console.WriteLine("Operation Cancelled");
            }
            catch (AggregateException aggEx)
            {
                Console.WriteLine("Operation Cancelled");
            }


            //parallel foreach cancellation
            try
            {
                ParallelLoopResult res2 = Parallel.ForEach(data,options, (x, state) =>
                {
                    Console.WriteLine(x);
                    Thread.Sleep(100);
                });
                Console.WriteLine("Foreach loop LowestBreak Iteration : {0}", res2.LowestBreakIteration);
                Console.WriteLine("Foreach loop Completed : {0}", res2.IsCompleted);
                Console.WriteLine("\r\n");
            }
            catch (OperationCanceledException opCanEx)
            {
                Console.WriteLine("Operation Cancelled");
            }
            catch (AggregateException aggEx)
            {
                Console.WriteLine("Operation Cancelled");
            }


            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParallelLINQ.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation
{
    class Program
    {
        static void Main(string[] args)
        {

            // create a cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            IEnumerable<double> results = 
                StaticData.DummyRandomHugeIntValues.Value
                .AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithCancellation(tokenSource.Token)
                .Select(x => Math.Pow(x,2));


            // create a task that will wait for 500 ms and then cancel the token
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                tokenSource.Cancel();
                Console.WriteLine("Cancelling");
            });

            //now try and use the results, and make sure we catch Exceptions
            try
            {
                foreach (int item in results)
                {
                    Console.WriteLine("Result is {0}", item);
                }
            }
            catch (OperationCanceledException opcnclEx)
            {
                Console.WriteLine("Operation was cancelled");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CancellingContinuations
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource tokenSource
                = new CancellationTokenSource();

            CancellationToken token = tokenSource.Token;


            try
            {
                // create the task
                Task<List<int>> taskWithFactoryAndState =
                    Task.Factory.StartNew<List<int>>((stateObj) =>
                    {
                        Console.WriteLine("In TaskWithFactoryAndState");
                        List<int> ints = new List<int>();
                        for (int i = 0; i < (int)stateObj; i++)
                        {
                            tokenSource.Token.ThrowIfCancellationRequested();
                            ints.Add(i);
                            Console.WriteLine("taskWithFactoryAndState, creating Item: {0}", i);
                            Thread.Sleep(1000); // simulate some work
                        }
                        return ints;
                    }, 10000, tokenSource.Token);


                Thread.Sleep(5000); //wait 5 seconds then cancel the runnning Task

                tokenSource.Cancel();


                //Setup a continuation which will not run
                taskWithFactoryAndState.ContinueWith<List<int>>((ant) =>
                {
                    Console.WriteLine("In Continuation");

                    List<int> parentResult = ant.Result;
                    List<int> result = new List<int>();
                    foreach (int resultValue in parentResult)
                    {

                        Console.WriteLine("Parent Task produced {0}, which will be squared by continuation",
                            resultValue);
                        result.Add(resultValue * resultValue);
                    }
                    return result;
                }, tokenSource.Token);


                taskWithFactoryAndState.Wait();

            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine(string.Format("Caught exception '{0}'", ex.Message));
                }
            }
            finally
            {
                tokenSource.Dispose();
            }


            Console.WriteLine("Finished");
            Console.ReadLine();

        }
    }
}

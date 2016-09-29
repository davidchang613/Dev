using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingContinuationForExceptionHandling
{
    class Program
    {
            static void Main(string[] args)
            {
            // create the task
            Task<List<int>> taskWithFactoryAndState =
                Task.Factory.StartNew<List<int>>((stateObj) =>
                {
                    List<int> ints = new List<int>();
                    for (int i = 0; i < (int)stateObj; i++)
                    {
                        ints.Add(i);
                        if (i > 100)
                        {
                            InvalidOperationException ex =
                                new InvalidOperationException("oh no its > 100");
                            ex.Source = "taskWithFactoryAndState";
                            throw ex;
                        }
                    }
                    return ints;
                }, 2000);


            //and setup a continuation for it only on when faulted
            taskWithFactoryAndState.ContinueWith((ant) =>
                {
                    AggregateException aggEx = ant.Exception;
                    Console.WriteLine("OOOOPS : The Task exited with Exception(s)");

                    foreach (Exception ex in aggEx.InnerExceptions)
                    {
                        Console.WriteLine(string.Format("Caught exception '{0}'",
                            ex.Message));
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);


            //and setup a continuation for it only on ran to completion
            taskWithFactoryAndState.ContinueWith((ant) =>
            {
                List<int> result = ant.Result;
                foreach (int resultValue in result)
                {
                    Console.WriteLine("Task produced {0}", resultValue);
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Console.ReadLine();

        }
    }
}

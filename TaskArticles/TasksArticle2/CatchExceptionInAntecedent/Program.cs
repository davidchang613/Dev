using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchExceptionInAntecedent
{
    class Program
    {
        static void Main(string[] args)
        {
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
                                Console.WriteLine("taskWithFactoryAndState, creating Item: {0}", i);
                                ints.Add(i);
                                if (i == 5)
                                    throw new InvalidOperationException("Don't like 5 its vulgar and dirty");
  
                            }
                            return ints;
                        }, 100);


                    //Setup a continuation which will not run
                    taskWithFactoryAndState.ContinueWith<List<int>>((ant) =>
                    {
                        if (ant.Status == TaskStatus.Faulted)
                            throw ant.Exception.InnerException;


                        Console.WriteLine("In Continuation, no problems in Antecedent");

                        List<int> parentResult = ant.Result;
                        List<int> result = new List<int>();
                        foreach (int resultValue in parentResult)
                        {

                            Console.WriteLine("Parent Task produced {0}, which will be squared by continuation",
                                resultValue);
                            result.Add(resultValue * resultValue);
                        }
                        return result;
                    });


                    //wait for the task to complete
                    taskWithFactoryAndState.Wait();
                }
                catch (AggregateException aggEx)
                {
                    foreach (Exception ex in aggEx.InnerExceptions)
                    {
                        Console.WriteLine(string.Format("Caught exception '{0}'", ex.Message));
                    }
                }


                Console.WriteLine("Finished");
                Console.ReadLine();
        }
    }
}

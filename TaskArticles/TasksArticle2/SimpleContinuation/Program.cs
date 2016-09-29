using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleContinuation
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
                    }
                    return ints;
                }, 20);


            try
            {
                //setup a continuation for task
                taskWithFactoryAndState.ContinueWith((ant) =>
                {
                    List<int> result = ant.Result;
                    foreach (int resultValue in result)
                    {
                        Console.WriteLine("Task produced {0}", resultValue);
                    }
                });
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

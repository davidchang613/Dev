using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingContinuationsAsPipelines
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
                }, 10);


            //and setup a continuation for it only on ran to completion, where this continuation
            //returns a result too, which will be used by yet another continuation
            taskWithFactoryAndState.ContinueWith<List<int>>((ant) =>
            {
                List<int> parentResult = ant.Result;
                List<int> result = new List<int>();
                foreach (int resultValue in parentResult)
                {

                    Console.WriteLine("Parent Task produced {0}, which will be squared by continuation", 
                        resultValue);
                    result.Add(resultValue * resultValue);
                }
                return result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion)
            //Another continution
            .ContinueWith((ant) =>
                {
                    List<int> parentResult = ant.Result;
                    foreach (int resultValue in parentResult)
                    {
                        Console.WriteLine("Parent Continuation Task produced Square of {0}", 
                            resultValue);
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Console.ReadLine();

        }
    }
}

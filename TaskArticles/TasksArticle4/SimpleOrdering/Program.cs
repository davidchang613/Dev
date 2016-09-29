using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using ParallelLINQ.Common;



namespace SimpleOrdering
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ManualResetEventSlim mre = new ManualResetEventSlim();
 

            //***********************************************************************************
            //
            //   SCENARIO 1 : Sequential (which will maintain order)
            //
            //***********************************************************************************
            IEnumerable<int> results1 = StaticData.DummyOrderedIntValues.Value
                                        .Select(x => x);
            foreach (int item in results1)
            {
                Console.WriteLine("Sequential Result is {0}", item);
            }
            mre.Set();


            //***********************************************************************************
            //
            //   SCENARIO 2 : No Ordering At All
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();
            IEnumerable<int> results2 = StaticData.DummyOrderedIntValues.Value.AsParallel()
                                        .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                        .Select(x => x);
            foreach (int item in results2)
            {
                Console.WriteLine("PLINQ Result is {0}", item);
            }
            mre.Set();


            //***********************************************************************************
            //
            //   SCENARIO 3 : No Ordered
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();

            IEnumerable<int> results3 = StaticData.DummyOrderedIntValues.Value.AsParallel().AsOrdered()
                                        .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                        .Select(x => x);
            foreach (int item in results3)
            {
                Console.WriteLine("PLINQ AsOrdered() Result is {0}", item);
            }
            
            
            
            Console.ReadLine();



        }
    }
}

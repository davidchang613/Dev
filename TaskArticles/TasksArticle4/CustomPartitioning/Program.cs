using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParallelLINQ.Common;
using System.Diagnostics;
using System.Threading;

namespace CustomPartitioning
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] sourceData = StaticData.DummyOrderedLotsOfIntValues.Value;
            ManualResetEventSlim mre = new ManualResetEventSlim();

            List<string> overallResults = new List<string>();

            //***********************************************************************************
            //
            //   SCENARIO 1 : Sequential LINQ
            //
            //***********************************************************************************
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            IEnumerable<double> results1 =
                sourceData.Select(item => Math.Pow(item, 2));

            // enumerate results
            int visited1 = 0;

            foreach (double item in results1)
            {
                Console.WriteLine("Result is {0}", item);
                visited1++;
            }
            watch1.Stop();

            

            overallResults.Add(string.Format("Visited {0} elements in {1} ms",
                visited1.ToString(), watch1.ElapsedMilliseconds));
            mre.Set();

            //***********************************************************************************
            //
            //   SCENARIO 2 : Use PLINQ
            //
            //***********************************************************************************

            mre.Wait();
            mre.Reset();

            Stopwatch watch2 = new Stopwatch();
            watch2.Start();
            IEnumerable<double> results2 =
                sourceData.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(item => Math.Pow(item, 2));

            // enumerate results
            int visited2 = 0;

            foreach (double item in results2)
            {
                Console.WriteLine("Result is {0}", item);
                visited2++;
            }
            watch2.Stop();
            overallResults.Add(string.Format("PLINQ No Partioner Visited {0} elements in {1} ms",
                visited2.ToString(), watch2.ElapsedMilliseconds));
            mre.Set();


            //***********************************************************************************
            //
            //   SCENARIO 3 : Use PLINQ and custom partitioner
            //
            //***********************************************************************************

            mre.Wait();
            mre.Reset();

            // create the partitioner
            SimpleCustomPartitioner<int> partitioner =
                new SimpleCustomPartitioner<int>(sourceData);

            Stopwatch watch3 = new Stopwatch();
            watch3.Start();
            IEnumerable<double> results3 =
                partitioner.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(item => Math.Pow(item, 2));

            // enumerate results
            int visited3 = 0;

            foreach (double item in results3)
            {
                Console.WriteLine("Result is {0}", item);
                visited3++;
            }
            watch3.Stop();
            overallResults.Add(string.Format("PLINQ With Custom Partioner Visited {0} elements in {1} ms",
                visited3.ToString(), watch3.ElapsedMilliseconds));




            //print results of 3 different variations
            foreach (string overallResult in overallResults)
            {
                Console.WriteLine(overallResult);
            }


            Console.ReadLine();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParallelLINQ.Common;
using System.Threading;
using System.Diagnostics;


namespace SimpleParrallelLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualResetEventSlim mre = new ManualResetEventSlim();

            //***********************************************************************************
            //
            //   SCENARIO 1 : Sequential LINQ
            //
            //***********************************************************************************
            Stopwatch watch = new Stopwatch();
            watch.Start();
            IEnumerable<double> results = StaticData.DummyRandomIntValues.Value
                                        .Select(x => Math.Pow(x, 2));
            foreach (int item in results)
            {
                Console.WriteLine("Result is {0}", item);
            }
            watch.Stop();
            Console.WriteLine("time ellapsed for Sequential version {0}ms", watch.ElapsedMilliseconds);
            mre.Set();


            //***********************************************************************************
            //
            //   SCENARIO 2 : Possibly Parallel LINQ, possibly extra overhead is inccurred as TPL
            //                must analyze the query to decide if it would better run Sequentially 
            //                or using Task(s)
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();
            Stopwatch watch2 = new Stopwatch();
            watch2.Start();
            var results2 = StaticData.DummyRandomIntValues.Value.AsParallel()
                .Select(x => Math.Pow(x, 2));

            foreach (int item in results2)
            {
                Console.WriteLine("Result is {0}", item);
            }
            watch2.Stop();
            Console.WriteLine("time ellapsed for Possibly Parrallel LINQ version {0}ms", watch2.ElapsedMilliseconds);
            mre.Set();



            //***********************************************************************************
            //
            //   SCENARIO 3 : Parallel LINQ
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();
            Stopwatch watch3 = new Stopwatch();
            watch3.Start();
            var results3 = StaticData.DummyRandomIntValues.Value
                .AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(x => Math.Pow(x, 2));

            foreach (int item in results3)
            {
                Console.WriteLine("Result is {0}", item);
            }
            watch3.Stop();
            Console.WriteLine("time ellapsed for Parrallel LINQ version {0}ms", watch3.ElapsedMilliseconds);


            Console.ReadLine();

        }
    }
}

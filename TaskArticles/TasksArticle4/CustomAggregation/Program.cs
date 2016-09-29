using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ParallelLINQ.Common;

namespace CustomAggregation
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualResetEventSlim mre = new ManualResetEventSlim();
            List<Person> peopleData = StaticData.DummyRandomPeople.Value;

            //***********************************************************************************
            //
            //   SCENARIO 1 : Sequential LINQ
            //
            //***********************************************************************************
            int sequentialResult = (from x in peopleData where x.Age > 50 select x).Count();
            Console.WriteLine("Sequential LINQ found {0} people with Age > 50", sequentialResult);
            mre.Set();

            //***********************************************************************************
            //
            //   SCENARIO 2 : Cusom PLINQ Aggregation
            //
            //***********************************************************************************
            int plinqResult =
                peopleData.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(2) //cores on my laptop
                .Aggregate(
                    // 1st function - initialize the result
                    0,
                    // 2nd function - process each person and the per-Task subtotal
                    (subtotal, person) => subtotal += (person.Age > 50) ? 1 : 0,
                    // 3rd function - process the overall total and the per-Task total
                    (total, subtotal) => total + subtotal,
                    // 4th function - perform final processing
                    total => total);

            Console.WriteLine("PLINQ custom Aggregate found {0} people with Age > 50", plinqResult);



            Console.ReadLine();


        }
    }
}

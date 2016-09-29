using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParallelLINQ.Common;
using System.Threading;
using System.Diagnostics;

namespace CatchingExceptions
{
    class Program
    {
        static void Main(string[] args)
        {

            ManualResetEventSlim mre = new ManualResetEventSlim();

            //***********************************************************************************
            //
            //   SCENARIO 1 : Using a List source, but we are not doing anything in an
            //                ordered manner, so we get Exception when we get it
            //
            //***********************************************************************************

            IEnumerable<Person> results1 =
                StaticData.DummyRandomPeople.Value.AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(2)
                .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                .Select(x =>
                    {
                        if (x.Age >= 100)
                            throw new InvalidOperationException(
                                "Can only accept items < 100");
                        else
                            return x;
                    });


            //Put try catch around the enumerating over the results of the PLINQ query
            try
            {
                foreach (Person item in results1)
                {
                    Console.WriteLine("Result is {0}", item);
                }
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine(string.Format("PLinq over List<T> caught exception '{0}'",
                        ex.Message));
                }
            }
            
            mre.Set();



            //***********************************************************************************
            //
            //   SCENARIO 2 : Using a IEnumerable, so we would not neccesarily expect any partitioning
            //                to occur as PLinq does not know the Count of the data source, so can
            //                not use partitions, so must enumerate over everything
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();

            //Using a IEnumerable, so we would not neccesarily expect any partitioning
            //so would expect the entire IEnumerable<Person> to be enumerated until we see
            //an Exception case
            IEnumerable<Person> results2 =
                StaticData.DummyRandomPeopleEnumerable().AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithDegreeOfParallelism(2)
                .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                .Select(x =>
                {
                    if (x.Age >= 100)
                        throw new InvalidOperationException(
                            "Can only accept items < 100");
                    else
                        return x;
                });


            //Put try catch around the enumerating over the results of the PLINQ query
            try
            {
                foreach (Person item in results2)
                {
                    Console.WriteLine("Result is {0}", item);
                }
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine(string.Format("PLinq over IEnumerable<T> caught exception '{0}'",
                        ex.Message));
                }
            }

            mre.Set();


            //***********************************************************************************
            //
            //   SCENARIO 3 : Using a IEnumerable, so we would not neccesarily expect any partitioning
            //                to occur as PLinq does not know the Count of the data source, so can
            //                not use partitions, so must enumerate over everything, but this time
            //                we are using AsOrdered() which behing the scenes will have to use Tasks
            //                where we use Task.Result, which is one of the special trigger methods
            //                which will cause AggregateException to become observed, so we should
            //                get Exceptions straight away
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();

            //Using a IEnumerable, but use AsOrdered() which should catch Exceptions straight away
            IEnumerable<Person> results3 =
                StaticData.DummyRandomPeopleEnumerable().AsParallel()
                .AsOrdered()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithMergeOptions(ParallelMergeOptions.Default)
                .WithDegreeOfParallelism(2)
                .Select(x =>
                {
                    if (x.Age >= 100)
                        throw new InvalidOperationException(
                            "Can only accept items < 100");
                    else
                        return x;
                });


            //Put try catch around the enumerating over the results of the PLINQ query
            try
            {
                foreach (Person item in results3)
                {
                    Console.WriteLine("Result is {0}", item);
                }
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Console.WriteLine(string.Format("PLinq over IEnumerable<T> using AsOrdered() caught exception '{0}'",
                        ex.Message));
                }
            }



            


            
            Console.ReadLine();
        }
    }
}

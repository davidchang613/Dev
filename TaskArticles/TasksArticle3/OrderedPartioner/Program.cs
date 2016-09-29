using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace OrderedPartitioner
{
    class Program
    {
        static void Main(string[] args)
        {
            // create the results array
            int[] inputData = new int[50000000];
            int[] resultData = new int[50000000];
            ManualResetEventSlim mre = new ManualResetEventSlim(false);
            Random rand = new Random();
            Object locker = new Object();

            //create some dummy data
            Task setupTask = Task.Factory.StartNew(() =>
            {
                Parallel.For(0, inputData.Length, (i) =>
                {
                    lock (locker)
                    {
                        inputData[i] = rand.Next(2, 10);
                    }
                });
                mre.Set();
            });



            //***********************************************************************************
            //
            //   SCENARIO 1 : No partitioning at all
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();

            Task timerTask1 = Task.Factory.StartNew(() =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                Parallel.ForEach(inputData, (int item, ParallelLoopState loopState, long index) =>
                {
                    resultData[index] = item * item; 
                });
                watch.Stop();
                Console.WriteLine("time ellapsed for No partitioning at all version {0}ms", watch.ElapsedMilliseconds);
                Console.WriteLine("Number of results with 0 as square : {0}, which proves each element was hit\r\n",
                    (from x in resultData where x == 0 select x).Count()); mre.Set();
                mre.Set();
            });


            //***********************************************************************************
            //
            //   SCENARIO 2 : Use the default TPL partitioning algorithm (affected by your PCs # of cores)
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();

            //clear results
            Parallel.For(0, inputData.Length, (i) =>
            {
                resultData[i] = 0;
            });

            Task timerTask2 = Task.Factory.StartNew(() =>
            {
                // create an orderable partitioner
                Stopwatch watch = new Stopwatch();
                OrderablePartitioner<int> op = Partitioner.Create(inputData);
                watch.Reset();
                watch.Start();
                Parallel.ForEach(op, (int item, ParallelLoopState loopState, long index) =>
                {
                    resultData[index] = item * item;
                });
                watch.Stop();
                Console.WriteLine("time ellapsed for default TPL partitioning algorithm version {0}ms", watch.ElapsedMilliseconds);
                Console.WriteLine("Number of results with 0 as square : {0}, which proves each element was hit\r\n",
                    (from x in resultData where x == 0 select x).Count()); mre.Set();
                mre.Set();
            });



            //***********************************************************************************
            //
            //   SCENARIO 3 : Use our own custom partitioning logic
            //
            //***********************************************************************************
            mre.Wait();
            mre.Reset();

            //clear results
            Parallel.For(0, inputData.Length, (i) =>
            {
                resultData[i] = 0;
            });

            Task timerTask3 = Task.Factory.StartNew(() =>
            {
                // create an chunking orderable partitioner
                Stopwatch watch = new Stopwatch();
                watch.Reset();
                watch.Start();
                OrderablePartitioner<Tuple<int, int>> chunkPart = Partitioner.Create(0, inputData.Length, 5000);
                Parallel.ForEach(chunkPart, chunkRange =>
                {
                    for (int i = chunkRange.Item1; i < chunkRange.Item2; i++)
                    {
                        resultData[i] = inputData[i] * inputData[i];
                    }
                });
                watch.Stop();
                Console.WriteLine("time ellapsed for custom Partitioner version {0}ms", watch.ElapsedMilliseconds);
                Console.WriteLine("Number of results with 0 as square : {0}, which proves each element was hit\r\n",
                    (from x in resultData where x == 0 select x).Count()); mre.Set();
                mre.Set();
            });



            mre.Wait();
            mre.Reset();


            Console.ReadLine();



        }
    }
}

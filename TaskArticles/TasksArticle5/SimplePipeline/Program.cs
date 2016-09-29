using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SimplePipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            var buffer1 = new BlockingCollection<int>(10);
            var buffer2 = new BlockingCollection<int>(10);

            var f = new TaskFactory(TaskCreationOptions.LongRunning,
                                    TaskContinuationOptions.None);

            //Start the phases of the pipeline
            var stage1 = f.StartNew(() => CreateInitialRange(buffer1));
            var stage2 = f.StartNew(() => DoubleTheRange(buffer1, buffer2));
            var stage3 = f.StartNew(() => WriteResults(buffer2));
            //wait for the phases to complete
            Task.WaitAll(stage1, stage2, stage3);

            Console.ReadLine();
        }



        static void CreateInitialRange(BlockingCollection<int> output)
        {
            try
            {
                for (int i = 1; i < 10; i++)
                {
                    output.Add(i);
                    Console.WriteLine("CreateInitialRange {0}", i);
                }
            }
            finally
            {
                output.CompleteAdding();
            }
        }


        static void DoubleTheRange(BlockingCollection<int> input, BlockingCollection<int> output)
        {
            try
            {
                foreach (var number in input.GetConsumingEnumerable())
                {
                    output.Add((int)(number * number));
                }
            }
            finally
            {
                output.CompleteAdding();
            }
        }


        static void WriteResults(BlockingCollection<int> input)
        {
            foreach (var squaredNumber in input.GetConsumingEnumerable())
            {
                Console.WriteLine("Result is {0}", squaredNumber);
            }
        }

    }
}

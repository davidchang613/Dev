using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCompletionSource1
{
    /// <summary>
    /// This example is adapted from Microsoft MSDN code freely available at
    /// http://msdn.microsoft.com/en-us/library/dd449174.aspx
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            TaskCompletionSource<String> tcs1 = new TaskCompletionSource<String>();
            Task<String> task1 = tcs1.Task;

            // Complete tcs1 in background Task
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                tcs1.SetResult("Task1 Completed");
            });

            // Waits 1 second for the result of the task
            Stopwatch sw = Stopwatch.StartNew();
            String result = task1.Result;
            sw.Stop();

            Console.WriteLine("(ElapsedTime={0}): t1.Result={1} (expected \"Task1 Completed\") ", 
                sw.ElapsedMilliseconds, result);





            TaskCompletionSource<String> tcs2 = new TaskCompletionSource<String>();
            Task<String> task2 = tcs2.Task;

            // Raise Exception tcs2 in background Task
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                tcs2.SetException(new InvalidProgramException("Oh no...Something is wrong"));
            });

            sw = Stopwatch.StartNew();
            try
            {
                result = task2.Result;

                Console.WriteLine("t2.Result succeeded. THIS WAS NOT EXPECTED.");
            }
            catch (AggregateException e)
            {
                Console.Write("(ElapsedTime={0}): ", sw.ElapsedMilliseconds);
                Console.WriteLine("The following exceptions have been thrown by t2.Result: (THIS WAS EXPECTED)");
                for (int j = 0; j < e.InnerExceptions.Count; j++)
                {
                    Console.WriteLine("\n-------------------------------------------------\n{0}", 
                        e.InnerExceptions[j].ToString());
                }
            }


            Console.ReadLine();

        }
    }
}

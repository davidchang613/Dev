using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;



namespace TaskCompletionSource2
{
    /// <summary>
    /// This example is baased on a example in the "The Task-based Asynchronous Pattern" white paper
    /// by Stephen Toub, Microsoft, April 2011
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Task<DateTimeOffset> delayTask = Delay(5000);
            Console.WriteLine(String.Format("Ellapsed Time 1 : {0}", watch.Elapsed));
            delayTask.ContinueWith((x) =>
                {
                    Console.WriteLine(String.Format("Ellapsed Time 2 : {0}", watch.Elapsed));
                });
            Console.ReadLine();
        }



        public static Task<DateTimeOffset> Delay(int millisecondsTimeout)
        {
            var tcs = new TaskCompletionSource<DateTimeOffset>();
            new Timer(self =>
                {
                    ((IDisposable)self).Dispose();
                    tcs.TrySetResult(DateTime.Now);
                }).Change(millisecondsTimeout, - 1);
            return tcs.Task;
        }
    }
}

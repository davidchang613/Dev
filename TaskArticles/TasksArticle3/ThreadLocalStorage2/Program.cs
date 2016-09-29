using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadLocalStorage2
{
    class Program
    {
        static void Main(string[] args)
        {
            double total = 0;
            object syncLock = new object();

            double[] values = new double[100];
            Random rand = new Random();
            Object locker = new Object();
            ManualResetEventSlim mre = new ManualResetEventSlim();

            //initialise some random values, and signal when done
            Task initialiserTask = Task.Factory.StartNew(() =>
                {
                    Parallel.For(0, values.Length, (idx) =>
                        {
                            lock (locker)
                            {
                                values[idx] = rand.NextDouble();
                            }
                        });
                    mre.Set();
                });

            //wait until all initialised
            mre.Wait();

            //now use Thread Local Storage and Sum them together
            Parallel.For<double>(
                0,
                values.Length,
                //local init
                () => 0,
                //body
                (int index, ParallelLoopState loopState, double tlsValue) =>
                {
                    tlsValue += values[index];
                    return tlsValue;
                },
                //local finally
                tlsValue =>
                {
                    lock (syncLock)
                    {
                        total += tlsValue;
                    }
                });

            Console.WriteLine("Total: {0}", total);

            // wait for input before exiting
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }
    }
}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsVersusTasks
{
    /// <summary>
    /// Simple example showing Threads Versus Tasks
    /// where we spin up a number of Threads and run a simple
    /// loop in it, and we also do the same with Tasks
    /// 
    /// One thing to examine is the Windows Task Manager
    /// when these things are happening.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            //64 is upper limit for WaitHandle.WaitAll() method
            int maxWaitHandleWaitAllAllowed = 64;
            ManualResetEventSlim[] mres = new ManualResetEventSlim[maxWaitHandleWaitAllAllowed]; 

            for (int i = 0; i < mres.Length; i++)
            {
                mres[i] = new ManualResetEventSlim(false);
            }

            
            long threadTime = 0;
            long taskTime = 0;
            watch.Start();

            //start a new classic Thread and signal the ManualResetEvent when its done
            //so that we can snapshot time taken, and 

            for (int i = 0; i < mres.Length; i++)
            {
                int idx = i;
                Thread t = new Thread((state) =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Console.WriteLine(string.Format("Thread : {0}, outputing {1}",
                            state.ToString(), j.ToString()));
                    }
                    mres[idx].Set();
                });
                t.Start(string.Format("Thread{0}", i.ToString()));
            }

            Console.WriteLine("Before WaitAll");
           
            WaitHandle.WaitAll( (from x in mres select x.WaitHandle).ToArray());

            Console.WriteLine("After WaitAll");
    
            threadTime = watch.ElapsedMilliseconds;
            watch.Reset();
   
            for (int i = 0; i < mres.Length; i++)
            {
                mres[i].Reset();
            }

            watch.Start();

            for (int i = 0; i < mres.Length; i++)
            {
                int idx = i;
                Task task = Task.Factory.StartNew((state) =>
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            Console.WriteLine(string.Format("Task : {0}, outputing {1}",
                                state.ToString(), j.ToString()));
                        }
                        mres[idx].Set();
                    }, string.Format("Task{0}", i.ToString()));
            }

            WaitHandle.WaitAll((from x in mres select x.WaitHandle).ToArray());
            taskTime = watch.ElapsedMilliseconds;
            Console.WriteLine("Thread Time waited : {0}ms", threadTime);
            Console.WriteLine("Task Time waited : {0}ms", taskTime);

            for (int i = 0; i < mres.Length; i++)
            {
                mres[i].Reset();
            }
            Console.WriteLine("All done, press Enter to Quit");

            Console.ReadLine();

            MRES_SetWaitReset();
            MRES_SpinCountWaitHandle();

            Console.WriteLine("All done, press Enter to Quit");

            Console.ReadLine();
        }

        static void MRES_SetWaitReset()
        {
            ManualResetEventSlim mres1 = new ManualResetEventSlim(false); // initialize as unsignaled
            ManualResetEventSlim mres2 = new ManualResetEventSlim(false); // initialize as unsignaled
            ManualResetEventSlim mres3 = new ManualResetEventSlim(true);  // initialize as signaled

            // Start an asynchronous Task that manipulates mres3 and mres2
            var observer = Task.Factory.StartNew(() =>
            {
                mres1.Wait();
                Console.WriteLine("observer sees signaled mres1!");
                Console.WriteLine("observer resetting mres3...");
                mres3.Reset(); // should switch to unsignaled
                Console.WriteLine("observer signalling mres2");
                mres2.Set();
            });

            Console.WriteLine("main thread: mres3.IsSet = {0} (should be true)", mres3.IsSet);
            Console.WriteLine("main thread signalling mres1");
            mres1.Set(); // This will "kick off" the observer Task
            mres2.Wait(); // This won't return until observer Task has finished resetting mres3
            Console.WriteLine("main thread sees signaled mres2!");
            Console.WriteLine("main thread: mres3.IsSet = {0} (should be false)", mres3.IsSet);

            // It's good form to Dispose() a ManualResetEventSlim when you're done with it
            observer.Wait(); // make sure that this has fully completed
            mres1.Dispose();
            mres2.Dispose();
            mres3.Dispose();
        }

        // Demonstrates:
        //      ManualResetEventSlim construction w/ SpinCount
        //      ManualResetEventSlim.WaitHandle
        static void MRES_SpinCountWaitHandle()
        {
            // Construct a ManualResetEventSlim with a SpinCount of 1000
            // Higher spincount => longer time the MRES will spin-wait before taking lock
            ManualResetEventSlim mres1 = new ManualResetEventSlim(false, 1000);
            ManualResetEventSlim mres2 = new ManualResetEventSlim(false, 1000);

            Task bgTask = Task.Factory.StartNew(() =>
            {
                // Just wait a little
                Thread.Sleep(100);

                // Now signal both MRESes
                Console.WriteLine("Task signalling both MRESes");
                mres1.Set();
                mres2.Set();
            });

            // A common use of MRES.WaitHandle is to use MRES as a participant in 
            // WaitHandle.WaitAll/WaitAny.  Note that accessing MRES.WaitHandle will
            // result in the unconditional inflation of the underlying ManualResetEvent.
            WaitHandle.WaitAll(new WaitHandle[] { mres1.WaitHandle, mres2.WaitHandle });
            Console.WriteLine("WaitHandle.WaitAll(mres1.WaitHandle, mres2.WaitHandle) completed.");

            // Clean up
            bgTask.Wait();
            mres1.Dispose();
            mres2.Dispose();
        }

    }
}

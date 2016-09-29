using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Moq;
using System.Threading.Tasks;

namespace MoreRealisticAwaiterWithCancellation
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualResetEvent mre1 = new ManualResetEvent(true);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            ManualResetEvent mre3 = new ManualResetEvent(false);

            DoItForReal(false, mre1, mre2);
            DoItForReal(true, mre2, mre3);

            DoItUsingMoq_YouKnowForUnitTestsLike(mre3);
            
            
            Console.ReadLine();
        }


        /// <summary>
        /// Shows how you can await on a Task based service
        /// </summary>
        private static async void DoItForReal(
            bool shouldCancel, ManualResetEvent mreIn, ManualResetEvent mreOut)
        {

            mreIn.WaitOne();

            CancellationTokenSource cts = new CancellationTokenSource();

            int upperLimit = 50;
            int cancelAfter = (int)upperLimit / 5;
            // which is what is used in WorkProvider class
            int waitDelayForEachDataItem = 10; 

            //allows some items to be processed before cancelling
            if (shouldCancel)
                cts.CancelAfter(cancelAfter * waitDelayForEachDataItem); 

            Console.WriteLine();
            Console.WriteLine("=========================================");
            Console.WriteLine("Started DoItForReal()");


            try
            {
                List<String> data = await new Worker(new WorkProvider(), 
                    upperLimit, cts.Token).GetData();

                foreach (String item in data)
                {
                    Console.WriteLine(item);
                }
                //allow those waiting on this WaitHandle to continue
                if (mreOut != null) { mreOut.Set(); } 
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Processing canceled.");
                //allow those waiting on this WaitHandle to continue
                if (mreOut != null) { mreOut.Set(); }
            }
            catch (AggregateException aggEx)
            {
                Console.WriteLine("AggEx caught");
                //allow those waiting on this WaitHandle to continue
                if (mreOut != null) { mreOut.Set(); }
            }
            finally
            {
                Console.WriteLine("Finished DoItForReal()");
                Console.WriteLine("=========================================");
                Console.WriteLine();
            }
        }


        /// <summary>
        /// Shows how you might mock a task based service using "Moq" and TaskCompletionSource
        /// </summary>
        private static async void DoItUsingMoq_YouKnowForUnitTestsLike(ManualResetEvent mreIn)
        {

            mreIn.WaitOne();
            CancellationTokenSource cts = new CancellationTokenSource();

            int upperLimit = 50;
            
            List<String> dummyResults = new List<string>();
            for (int i = 0; i < upperLimit; i++)
		    {
                dummyResults.Add(String.Format("Dummy Result {0}", i.ToString()));     
		    }
            
            //Allows this test method to simulate a Task with a result without actually creating a Task
            TaskCompletionSource<List<String>> tcs = 
                new TaskCompletionSource<List<String>>();
            tcs.SetResult(dummyResults);


            Console.WriteLine();
            Console.WriteLine("=========================================");
            Console.WriteLine("Started DoItUsingMoq_YouKnowForUnitTestsLike()");

            try
            {
                Mock<IWorkProvider> mockWorkProvider = new Mock<IWorkProvider>();
                mockWorkProvider
                    .Setup(x => x.GetData(
                        It.IsAny<Int32>(), 
                        It.IsAny<CancellationToken>()))
                    .Returns(tcs.Task);

                List<String> data = await new Worker(
                    mockWorkProvider.Object, upperLimit, cts.Token).GetData();

                foreach (String item in data)
                {
                    Console.WriteLine(item);
                }
            }
            finally
            {
                Console.WriteLine("Finished DoItUsingMoq_YouKnowForUnitTestsLike()");
                Console.WriteLine("=========================================");
                Console.WriteLine();
            }
        }
    }
}

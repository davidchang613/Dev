using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MoreRealisticAwaiterWithCancellation
{
    public class WorkProvider : IWorkProvider
    {
        public Task<List<string>> GetData(int upperLimit, System.Threading.CancellationToken token)
        {
            //will not be TaskEx when CTP is in .NET 5.0 Framework
            return TaskEx.Run(() =>
            {
                List<string> results = new List<string>();

                for (int i = 0; i < upperLimit; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(10);  
                    results.Add(string.Format("Added runtime string {0}",i.ToString()));
                }
                return results;
            });
        }
    }
}

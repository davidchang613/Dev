using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoreRealisticAwaiterWithCancellation
{
    public class Worker
    {
        private IWorkProvider workprovider;
        private int upperLimit;
        private CancellationToken token;

        public Worker(IWorkProvider workprovider, int upperLimit, CancellationToken token)
        {
            this.workprovider = workprovider;
            this.upperLimit = upperLimit;
            this.token = token;
        }

        public Task<List<String>> GetData()
        {
            return workprovider.GetData(upperLimit, token);
        }
    }
}

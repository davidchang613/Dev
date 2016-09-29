using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoreRealisticAwaiterWithCancellation
{
    public interface IWorkProvider
    {
        Task<List<String>> GetData(int upperLimit, CancellationToken token);
    }
}

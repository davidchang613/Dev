using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIModel;

namespace APIMemberModulePortable
{
    public interface IAPIMemberModule
    {
        Member Get(string Id);
        void Post(Member mem);

    }
}

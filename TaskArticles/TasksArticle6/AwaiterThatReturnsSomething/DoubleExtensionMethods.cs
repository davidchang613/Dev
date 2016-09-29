using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AwaiterThatReturnsSomething
{

    public static class DoubleExtensionMethods
    {
        public static DoubleAwaiter GetAwaiter(this double demoDouble, int power)
        {
            return new DoubleAwaiter(demoDouble, power);
        }
    }

}

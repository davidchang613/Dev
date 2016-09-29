using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AwaiterThatReturnsSomething
{
    public class DoubleAwaiter
    {
        private double theValue;
        private int power;

        public DoubleAwaiter(double theValue, int power)
        {
            this.theValue = theValue;
            this.power = power;
            IsCompleted = false;
        }

        public DoubleAwaiter GetAwaiter()
        {
            return this;
        }

        public double GetResult()
        {
            return theValue;
        }


        public void OnCompleted(Action continuation)
        {
            this.theValue = Math.Pow(theValue, power);
            IsCompleted = true;
            continuation();
            
        }

        public bool IsCompleted { get; set; }
    }


}

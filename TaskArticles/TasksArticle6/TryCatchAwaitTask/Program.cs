using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryCatchAwaitTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.DoIt();
        }


        public async void DoIt()
        {
            //No problems with this chap
            try
            {
                List<int> results = await 
                    GetSomeNumbers(10,20);
                Console.WriteLine("==========START OF GOOD CASE=========");
                Parallel.For(0, results.Count, (x) =>
                {
                    Console.WriteLine(x);
                });
                Console.WriteLine("==========END OF GOOD CASE=========");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            //Make something go wrong
            try
            {
                //simulate a failure by erroring at 5
                List<int> results = await GetSomeNumbers(10,5);
                Parallel.ForEach(results, (x) =>
                {
                    Console.WriteLine(x);
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

        }


        /// <summary>
        /// Throws InvalidOperationException when index > shouldFailAt value
        /// </summary>
        public async Task<List<int>> GetSomeNumbers(int upperLimit, int shouldFailAt)
        {
            
            List<int> ints = new List<int>();
            for (int i = 0; i < upperLimit; i++)
            {
                if (i > shouldFailAt)
                    throw new InvalidOperationException(
                        String.Format("Oh no its > {0}",shouldFailAt));

                ints.Add(i);
            }
            return ints;
            
                
        }
    }
}

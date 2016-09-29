using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressReporting
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
            Progress<int> progress = new Progress<int>();
            progress.ProgressChanged += (sender, e) =>
            {
                Console.WriteLine(String.Format("Progress has seen {0} item", e));
            };

            List<int> results = await GetSomeNumbers(10, progress);
            Console.WriteLine("Task results are");
            Parallel.For(0, results.Count, (x) =>
            {
                Console.WriteLine(x);
            });

            Console.ReadLine();
        }



        public async Task<List<int>> GetSomeNumbers(int upperLimit, IProgress<int> progress)
        {
            List<int> ints = new List<int>();
            for (int i = 0; i < upperLimit; i++)
            {
                ints.Add(i);
                progress.Report(i + 1);
            }
            return ints;
        }

    }
}

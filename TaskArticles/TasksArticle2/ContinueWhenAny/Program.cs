using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContinueWhen.Common;
using System.Diagnostics;

namespace ContinueWhenAny
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a list of random numbers to sort
            Random rand = new Random();
            List<int> unsortedList = new List<int>();
            int numberOfItemsToSort = 5000;

            for (int i = 0; i < numberOfItemsToSort; i++)
            {
                unsortedList.Add(rand.Next(numberOfItemsToSort));
            }

            //create 3 tasks to run 3 different sorting algorithms
            Task<SortingTaskResult>[] tasks = new Task<SortingTaskResult>[3];

            //Bubble Sort Task
            tasks[0] = Task.Factory.StartNew((state) =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                List<int> source = (List<int>)state;
                List<int> localWorkList = new List<int>();

                //copy
                for (int i = 0; i < source.Count; i++)
                {
                    localWorkList.Add(source[i]);
                }
                //run algorithm
                List<int> result = SortingAlgorithms.BubbleSort(localWorkList);
                watch.Stop();
                return new SortingTaskResult(
                    watch.ElapsedMilliseconds, result, "Bubble Sort");
            }, unsortedList);


            //Selection Sort Task
            tasks[1] = Task.Factory.StartNew((state) =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                List<int> source = (List<int>)state;
                List<int> localWorkList = new List<int>();

                //copy
                for (int i = 0; i < source.Count; i++)
                {
                    localWorkList.Add(source[i]);
                }
                //run algorithm
                List<int> result = SortingAlgorithms.SelectionSort(localWorkList);
                watch.Stop();
                return new SortingTaskResult(
                    watch.ElapsedMilliseconds, result, "Selection Sort");
            }, unsortedList);



            //Counting Sort Task
            tasks[2] = Task.Factory.StartNew((state) =>
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                List<int> source = (List<int>)state;
                List<int> localWorkList = new List<int>();

                //copy
                for (int i = 0; i < source.Count; i++)
                {
                    localWorkList.Add(source[i]);
                }
                //run algorithm
                List<int> result = SortingAlgorithms.CountingSort(localWorkList);
                watch.Stop();
                return new SortingTaskResult(
                    watch.ElapsedMilliseconds, result, "Counting Sort");
            }, unsortedList);


            //Wait for any of them (assuming nothing goes wrong)
            Task.Factory.ContinueWhenAny(
                tasks,
                (Task<SortingTaskResult> antecedent) =>
                {
                    Console.WriteLine(antecedent.Result.ToString());
                });


            Console.ReadLine();
        }
    }
}
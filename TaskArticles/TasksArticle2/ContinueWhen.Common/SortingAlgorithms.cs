using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContinueWhen.Common
{
    public static class SortingAlgorithms
    {
        /// <summary>
        /// Counting sort algorithm
        /// </summary>
        public static List<int> CountingSort(List<int> arrayToSort)
        {
            object min;
            object max;

            min = max = arrayToSort[0];

            for (int i = 0; i < arrayToSort.Count; i++)
            {
                if (((IComparable)arrayToSort[i]).CompareTo(min) < 0)
                {
                    min = arrayToSort[i];
                }
                else if (((IComparable)arrayToSort[i]).CompareTo(max) > 0)
                {
                    max = arrayToSort[i];
                }
            }

            int range = (int)max - (int)min + 1;

            int[] count = new int[range * sizeof(int)];

            for (int i = 0; i < range; i++)
            {
                count[i] = 0;
            }

            for (int i = 0; i < arrayToSort.Count; i++)
            {
                count[(int)arrayToSort[i] - (int)min]++;
            }
            int z = 0;
            for (int i = (int)min; i < arrayToSort.Count; i++)
            {
                for (int j = 0; j < count[i - (int)min]; j++)
                {
                    arrayToSort[z++] = i;
                }
            }

            return arrayToSort;
        }


        /// <summary>
        /// Selection sort algorithm
        /// </summary>
        public static List<int> SelectionSort(List<int> arrayToSort)
        {
            int min;
            for (int i = 0; i < arrayToSort.Count; i++)
            {
                min = i;
                for (int j = i + 1; j < arrayToSort.Count; j++)
                {
                    if (((IComparable)arrayToSort[j]).CompareTo(arrayToSort[min]) < 0)
                    {
                        min = j;
                    }
                }
                int temp = arrayToSort[i];
                arrayToSort[i] = arrayToSort[min];
                arrayToSort[min] = temp;
            }

            return arrayToSort;
        }




        /// <summary>
        /// Bubble sort algorithm
        /// </summary>
        public static List<int> BubbleSort(List<int> arrayToSort)
        {
            int n = arrayToSort.Count - 1;
            for (int i = 0; i < n; i++)
            {
                for (int j = n; j > i; j--)
                {
                    if (((IComparable)arrayToSort[j - 1]).CompareTo(arrayToSort[j]) > 0)
                    {
                        int temp = arrayToSort[j - 1];
                        arrayToSort[j - 1] = arrayToSort[j];
                        arrayToSort[j] = temp;

                    }
                }
            }
            return arrayToSort;
        }
    }
}

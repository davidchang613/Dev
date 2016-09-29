using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CustomPartitioning
{
    /// <summary>
    /// Good source for TPL partitioning can be found at 
    /// http://msdn.microsoft.com/en-us/library/dd997411.aspx
    /// </summary>
    public class SimpleCustomPartitioner<T> : Partitioner<T> 
    {
        private T[] sourceData;

        public SimpleCustomPartitioner(T[] sourceData)
        {
            this.sourceData = sourceData;
        }

        public override bool SupportsDynamicPartitions 
        {
            get 
            {
                return false;
            }
        }

        public override IList<IEnumerator<T>> GetPartitions(int partitionCount)
        {
            IList<IEnumerator<T>> partitioned = new List<IEnumerator<T>>();
            //work out how many items will go into a single partition
            int itemsPerPartition = sourceData.Length / partitionCount;
            //now create the partititions, all but the last one, which we treat as special case
            for (int i = 0; i < partitionCount - 1; i++)
            {
                partitioned.Add(GetItemsForPartition(i * itemsPerPartition, (i + 1) * itemsPerPartition));
            }
            //now create the lasst partition
            partitioned.Add(GetItemsForPartition((partitionCount - 1) * itemsPerPartition, sourceData.Length));
            return partitioned;
        }


        private IEnumerator<T> GetItemsForPartition(int start, int end)
        {
            for (int i = start; i < end; i++)
                yield return sourceData[i];
        }
    }
}

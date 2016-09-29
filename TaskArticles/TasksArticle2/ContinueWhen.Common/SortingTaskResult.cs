using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContinueWhen.Common
{
    public class SortingTaskResult
    {
        public long TimeTaken { get; private set; }
        public List<int> SortedList { get; private set; }
        public string TaskName { get; private set; }

        public SortingTaskResult(long timeTaken, List<int> sortedList, string taskName)
        {
            this.TimeTaken = timeTaken;
            this.SortedList = sortedList;
            this.TaskName = taskName;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("TaskName : {0}, Took {1} ms with the 1st 50 sorted results of\r\n [", TaskName, TimeTaken);
            for (int i = 0; i < 50; i++)
            {
                sb.AppendFormat("{0},", SortedList[i]);
            }
            sb.Replace(",", "\r\n", sb.Length - 1, 1);
            return sb.ToString();
        }

    }
}

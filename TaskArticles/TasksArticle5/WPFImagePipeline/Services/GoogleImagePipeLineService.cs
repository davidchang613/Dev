using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFImagePipeline.Model;
using System.Threading;
using System.Threading.Tasks;
using Gapi.Search;
using System.Collections.Concurrent;

namespace WPFImagePipeline.Services
{
    #region IGoogleSearchProvider
    public interface IGoogleImagePipeLineService
    {
        void StartPipeline();
        event EventHandler<ImagePipelineCompletedArgs> PipeLineCompleted;
    }
    #endregion

    #region GoogleSearchProvider
    public class GoogleImagePipeLineService : IGoogleImagePipeLineService
    {

        private List<string> randomKeyWords = new List<string>() 
            {   "pitbull", "shark", "dog", "parrot", "robot", 
                "cheerleader", "gun", "skull", "plane", "manga", 
                "bikini","model","snake","spider" 
            };
        private Random rand = new Random();
        private List<string> urls = new List<string>();
        private Object locker = new Object();



        private void CreateImageUrls(BlockingCollection<string> urls)
        {
            try
            {
                int added = 0;

                do
                {
                    string keyword = randomKeyWords[rand.Next(0, randomKeyWords.Count)];
                    SearchResults searchResults = Searcher.Search(SearchType.Image, keyword);

                    if (searchResults.Items.Count() > 0)
                    {
                        foreach (var searchResult in searchResults.Items)
                        {
                            urls.Add(searchResult.Url);
                            ++added;
                        }
                    }
                } while (added < 100);


            }
            finally
            {
                urls.CompleteAdding();
            }
        }

        private void CreateImageInfos(BlockingCollection<string> urls,
            BlockingCollection<ImageInfo> initialImageInfos)
        {
            try
            {
                foreach (string url in urls.GetConsumingEnumerable())
                {
                    int idx = url.LastIndexOf(@"\") + 1;
                    initialImageInfos.Add(new ImageInfo(url, url.Substring(idx, url.Length - idx)));
                }
            }
            finally
            {
                initialImageInfos.CompleteAdding();
            }
        }


        private void AlertViewModel(BlockingCollection<ImageInfo> initialImageInfos)
        {
            List<ImageInfo> localInfos = new List<ImageInfo>();

            try
            {
                foreach (ImageInfo imageInfo in initialImageInfos.GetConsumingEnumerable())
                {
                    lock (locker)
                    {
                        localInfos.Add(imageInfo);
                    }
                }
            }
            finally
            {
                OnPipeLineCompleted(new ImagePipelineCompletedArgs(localInfos));
            }
        }




        #region IImagePipelineService Members

        public void StartPipeline()
        {
            BlockingCollection<string> buffer1 = new BlockingCollection<string>(100);
            BlockingCollection<ImageInfo> buffer2 = new BlockingCollection<ImageInfo>(100);

            TaskFactory factory = new TaskFactory(TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None);

            Task stage1 = factory.StartNew(() => CreateImageUrls(buffer1));
            Task stage2 = factory.StartNew(() => CreateImageInfos(buffer1, buffer2));
            Task stage3 = factory.StartNew(() => AlertViewModel(buffer2));

        }


        public event EventHandler<ImagePipelineCompletedArgs> PipeLineCompleted;

        protected virtual void OnPipeLineCompleted(ImagePipelineCompletedArgs e)
        {
            if (PipeLineCompleted != null)
            {
                PipeLineCompleted(this, e);
            }
        }
        #endregion
    }
    #endregion

}

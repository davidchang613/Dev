using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using WPFImagePipeline.Model;
using System.Threading.Tasks;
using System.IO;

namespace WPFImagePipeline.Services
{

    public class ImagePipelineCompletedArgs : EventArgs
    {
        public List<ImageInfo> GatheredImages { get; private set; }

        public ImagePipelineCompletedArgs(List<ImageInfo> gatheredImages)
        {
            this.GatheredImages = gatheredImages;
        }
    }


    public interface ILocalImagePipelineService
    {
        void StartPipeline(string yourImageFolder);
        event EventHandler<ImagePipelineCompletedArgs> PipeLineCompleted;

    }



    public class LocalImagePipelineService : ILocalImagePipelineService
    {
        private Object locker = new Object();


        private FileInfo[] GetAllMatchingImageFiles(string yourImageFolder)
        {
            string lookfor = "*.png;*.jpg;*.gif;*.tif";
            string[] extensions = lookfor.Split(new char[] { ';' });

            List<FileInfo> myfileinfos = new List<FileInfo>();
            DirectoryInfo di = new DirectoryInfo(yourImageFolder);

            foreach (string ext in extensions)
            {
                myfileinfos.AddRange(di.GetFiles(ext));
            }

            return myfileinfos.ToArray();
        }


        private void CreateImageUrls(string yourImageFolder, BlockingCollection<string> urls)
        {
            try
            {
                FileInfo[] files = GetAllMatchingImageFiles(yourImageFolder);
                Random rand = new Random();
                int added = 0;
                do
                {
                    int idx = rand.Next(0, files.Count());
                    urls.Add(files[idx].FullName);
                    ++added;
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
                    initialImageInfos.Add(new ImageInfo(url, url.Substring(idx,url.Length-idx)));
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

        public void StartPipeline(string yourImageFolder)
        {
            BlockingCollection<string> buffer1 = new BlockingCollection<string>(100);
            BlockingCollection<ImageInfo> buffer2 = new BlockingCollection<ImageInfo>(100);

            TaskFactory factory = new TaskFactory(TaskCreationOptions.LongRunning, 
                TaskContinuationOptions.None);

            Task stage1 = factory.StartNew(() => CreateImageUrls(yourImageFolder,buffer1));
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
}

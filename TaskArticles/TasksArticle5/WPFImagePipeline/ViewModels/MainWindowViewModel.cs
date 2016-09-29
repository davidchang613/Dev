using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFImagePipeline.Common;
using WPFImagePipeline.Services;
using WPFImagePipeline.Model;
using System.Configuration;
using System.IO;
using WPFImagePipeline.Controls;

namespace WPFImagePipeline.ViewModels
{
    public class MainWindowViewModel : INPCBase
    {
        private ILocalImagePipelineService localImagePipelineService;
        private IGoogleImagePipeLineService googleImagePipelineService;
        private IMessageBoxService messageBoxService;
        private List<ImageInfo> processedImages = new List<ImageInfo>();
        private bool useWebBasedImages = false;
        private string localImageFolder = "";
        private string defaultImagePath = @"C:\Users\Public\Pictures\Sample Pictures";


        private string waitText;
        private string errorMessage;
        private AsyncType asyncState = AsyncType.Content;


        public MainWindowViewModel(
            ILocalImagePipelineService imagePipelineService, 
            IGoogleImagePipeLineService googleSearchProvider,
            IMessageBoxService messageBoxService)
        {
            this.localImagePipelineService = imagePipelineService;
            this.googleImagePipelineService = googleSearchProvider;
            this.messageBoxService = messageBoxService;

            imagePipelineService.PipeLineCompleted += ImagePipelineService_PipeLineCompleted;
            googleSearchProvider.PipeLineCompleted += GooglePipelineService_PipeLineCompleted;

            AsyncState = AsyncType.Content;
            WaitText ="Fetching images";

        }




        public void DoIt()
        {
            AsyncState = AsyncType.Busy;
            bool result=false;
            if (Boolean.TryParse(
                ConfigurationManager.AppSettings["UseWebBasedImages"].ToString(), 
                out useWebBasedImages))
            {
                if (useWebBasedImages)
                {
                    googleImagePipelineService.StartPipeline();
                }
                else
                {
                    ShowUsingLocalImages();
                }
            }
            else
            {
                ShowUsingLocalImages();   
            }
        }


        public List<ImageInfo> ProcessedImages
        {
            get { return processedImages; }
            set
            {
                if (processedImages != value)
                {
                    processedImages = value;
                    NotifyPropertyChanged("ProcessedImages");
                }
            }
        }


        public AsyncType AsyncState
        {
            get { return asyncState; }
            set
            {
                if (asyncState != value)
                {
                    asyncState = value;
                    NotifyPropertyChanged("AsyncState");
                }
            }
        }



        public string WaitText
        {
            get { return waitText; }
            set
            {
                if (waitText != value)
                {
                    waitText = value;
                    NotifyPropertyChanged("WaitText");
                }
            }
        }


        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }




        private void ShowUsingLocalImages()
        {
            localImageFolder = ConfigurationManager.AppSettings["LocalImageFolder"].ToString();
            if (!String.IsNullOrEmpty(localImageFolder))
            {
                if (Directory.Exists(localImageFolder))
                {
                    localImagePipelineService.StartPipeline(localImageFolder);
                }
                else
                {
                    messageBoxService.ShowMessage("The LocalImageFolder folder you specified does not exist");
                }
            }
            else
            {
                localImagePipelineService.StartPipeline(@"C:\Users\Public\Pictures\Sample Pictures");
            }
        }


        private void ImagePipelineService_PipeLineCompleted(object sender, ImagePipelineCompletedArgs e)
        {
            ProcessedImages = e.GatheredImages;
            AsyncState = AsyncType.Content;
        }

        private void GooglePipelineService_PipeLineCompleted(object sender, ImagePipelineCompletedArgs e)
        {
            ProcessedImages = e.GatheredImages;
            AsyncState = AsyncType.Content;
        }
       

    }
}

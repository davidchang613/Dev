using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFImagePipeline.Common;

namespace WPFImagePipeline.Model
{
    public class ImageInfo : INPCBase
    {
        private string url;
        private string name;

        public ImageInfo(string url, string name)
        {
            this.Url = url;
            this.Name = name;
        }

        public string Url
        {
            get { return url; }
            set
            {
                if (url != value)
                {
                    url = value;
                    NotifyPropertyChanged("Url");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }


    }
}

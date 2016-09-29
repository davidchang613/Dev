using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Bornander.UI.DropHandlers
{
    using ParameterType = Tuple<IEnumerable<FileInfo>, Queue<Stream>, AutoResetEvent>;

    public class ImageDropHandler
    {
        private static readonly Random random = new Random();

        private SurfacePanel panel;
        private double scaleFactor;
        private double maxVelocity;
        private double maxAngularVelocity;

        private bool originalAllowDrop;
        private bool loadingDone;

        public ImageDropHandler(SurfacePanel panel, double scaleFactor, double maxVelocity, double maxAngularVelocity)
        {
            this.panel = panel;
            this.scaleFactor = scaleFactor;
            this.maxVelocity = maxVelocity;
            this.maxAngularVelocity = maxAngularVelocity;
            originalAllowDrop = panel.AllowDrop;
        }
        
        public void Attach()
        {
            panel.AllowDrop = true;
            panel.Drop += HandleDrop;
        }

        public void Detatch()
        {
            panel.AllowDrop = originalAllowDrop;
            panel.Drop -= HandleDrop;
        }

        private void HandleDrop(object sender, DragEventArgs e)
        {
            IEnumerable<FileInfo> files = ExtractFileList(e);

            Queue<Stream> streams = new Queue<Stream>();
            AutoResetEvent resetEvent = new AutoResetEvent(false);

            BackgroundWorker loader = new BackgroundWorker();
            BackgroundWorker adder = new BackgroundWorker();

            loader.DoWork += LoadImages;
            adder.DoWork += AddImages;

            ParameterType parameters = Tuple.Create(files, streams, resetEvent);
            loadingDone = false;
            loader.RunWorkerAsync(parameters);
            adder.RunWorkerAsync(parameters);
        }

        private void LoadImages(object sender, DoWorkEventArgs args)
        {
            
            ParameterType parameters = (ParameterType)args.Argument;
            foreach (FileInfo file in parameters.Item1)
            {
                FileStream stream = file.OpenRead();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                parameters.Item2.Enqueue(new MemoryStream(buffer));
                parameters.Item3.Set();
            }
            loadingDone = true;
            parameters.Item3.Set();
        }

        private void AddImages(object sender, DoWorkEventArgs args)
        {
            ParameterType parameters = (ParameterType)args.Argument;
            while (!loadingDone || parameters.Item2.Count > 0)
            {
                if (parameters.Item2.Count == 0)
                    continue;
                Stream stream = parameters.Item2.Dequeue();
                panel.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    Image image = new Image();
                    image.Source = bitmapImage;

                    SurfacePanel.SetPosition(image, (Point)panel.MousePosition);
                    SurfacePanel.SetSize(image, new Size(bitmapImage.Width * scaleFactor, bitmapImage.Height * scaleFactor));
                    SurfacePanel.SetAngularVelocity(image, random.NextDouble() * maxAngularVelocity - maxAngularVelocity / 2.0);
                    SurfacePanel.SetAspectRatioStrategy(image, AspectRatioStrategy.Maintain);
                    SurfacePanel.SetVelocity(image, new Vector(random.NextDouble() * maxVelocity - maxVelocity / 2.0, random.NextDouble() * maxVelocity - maxVelocity / 2.0));
                    panel.Children.Add(image);
                });
            }
       }

        private static IList<FileInfo> ExtractFileList(DragEventArgs e)
        {
            IList<FileInfo> files = new List<FileInfo>();

            if (e.Data.GetFormats().Contains(DataFormats.FileDrop) && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                foreach (string fileName in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    DirectoryInfo directory = new DirectoryInfo(fileName);
                    if (directory.Exists)
                    {
                        foreach (FileInfo file in directory.GetFiles("*.jpg"))
                            files.Add(file);
                    }
                    else
                    {
                        FileInfo file = new FileInfo(fileName);
                        if (file.Name.ToLower().EndsWith(".jpg"))
                            files.Add(file);
                    }
                }
            }
            return files;
        }
    }
}

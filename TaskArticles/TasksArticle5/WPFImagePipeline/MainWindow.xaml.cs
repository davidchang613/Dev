using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFImagePipeline.ViewModels;
using WPFImagePipeline.Services;

namespace WPFImagePipeline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowViewModel(
                new LocalImagePipelineService(), 
                new GoogleImagePipeLineService(), 
                new MessageBoxService());
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        #region Private  Methods

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)this.DataContext).DoIt();
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch
            {
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            e.Handled = true;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            spButtons.Margin = new Thickness(0, 3, 5, 0);
        }

        private void Info_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("This is a simple demo to demonstrate a slightly more sophisticated");
            sb.AppendLine("pipeline example. This pipelines gathers Url, and then create some");
            sb.AppendLine("ImageInfo objects which are then added to a ViewModel property");
            sb.AppendLine("and shown to the UI\r\n");
            sb.AppendLine("This demo makes use of my work collegues Surface Panel for WPF\r\n");
            sb.AppendLine("Which can be found at : http://www.codeproject.com/KB/static/WPFSurfacePanel.aspx\r\n\r\n");
            sb.AppendLine("To drag and rotate use LeftMouseButton, to resize use RightMouseButton");
            MessageBox.Show(sb.ToString());

        }
        #endregion

    }
}

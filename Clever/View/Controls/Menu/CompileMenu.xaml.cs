using Clever.CommonData;
using Clever.View.Controls.Helps;
using Clever.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clever.View.Controls.Menu
{
    /// <summary>
    /// Логика взаимодействия для CompileMenu.xaml
    /// </summary>
    public partial class CompileMenu : UserControl
    {
        public CompileMenu()
        {
            InitializeComponent();
            connectCB.IsChecked = Configurations.Get.Brick_Connect;
            compileCB.IsChecked = Configurations.Get.Brick_Compile;
            compileAndRunCB.IsChecked = Configurations.Get.Brick_CompileAndRun;
            stopCB.IsChecked = Configurations.Get.Brick_Stop;
            downloadCB.IsChecked = Configurations.Get.Brick_Download;
        }

        private void connectCB_Click(object sender, RoutedEventArgs e)
        {
            if (connectCB.IsChecked == true)
            {
                MainWindowVM.ConnectV = Visibility.Visible;
                Configurations.Get.Brick_Connect = true;
            }
            else
            {
                MainWindowVM.ConnectV = Visibility.Collapsed;
                Configurations.Get.Brick_Connect = false;
            }
        }

        private void compileCB_Click(object sender, RoutedEventArgs e)
        {
            if (compileCB.IsChecked == true)
            {
                MainWindowVM.CompileV = Visibility.Visible;
                Configurations.Get.Brick_Compile = true;
            }
            else
            {
                MainWindowVM.CompileV = Visibility.Collapsed;
                Configurations.Get.Brick_Compile = false;
            }
        }

        private void compileAndRunCB_Click(object sender, RoutedEventArgs e)
        {
            if (compileAndRunCB.IsChecked == true)
            {
                MainWindowVM.CompileAndRunV = Visibility.Visible;
                Configurations.Get.Brick_CompileAndRun = true;
            }
            else
            {
                MainWindowVM.CompileAndRunV = Visibility.Collapsed;
                Configurations.Get.Brick_CompileAndRun = false;
            }
        }

        private void stopCB_Click(object sender, RoutedEventArgs e)
        {
            if (stopCB.IsChecked == true)
            {
                MainWindowVM.StopV = Visibility.Visible;
                Configurations.Get.Brick_Stop = true;
            }
            else
            {
                MainWindowVM.StopV = Visibility.Collapsed;
                Configurations.Get.Brick_Stop = false;
            }
        }

        private void downloadCB_Click(object sender, RoutedEventArgs e)
        {
            if (downloadCB.IsChecked == true)
            {
                MainWindowVM.DownloadV = Visibility.Visible;
                Configurations.Get.Brick_Download = true;
            }
            else
            {
                MainWindowVM.DownloadV = Visibility.Collapsed;
                Configurations.Get.Brick_Download = false;
            }
        }

        private void Connect_Click(object sender, MouseButtonEventArgs e)
        {
            EV3Brick.GetEV3Brick.EV3SwitchDevice_PreviewMouseLeftButtonDown(sender, e);
        }

        private void Compile_Click(object sender, MouseButtonEventArgs e)
        {
            EV3Brick.GetEV3Brick.Compile_PreviewMouseLeftButtonDown(sender, e);
        }

        private void CompileAndRun_Click(object sender, MouseButtonEventArgs e)
        {
            EV3Brick.GetEV3Brick.CompileAndRun_PreviewMouseLeftButtonDown(sender, e);
        }

        private void Stop_Click(object sender, MouseButtonEventArgs e)
        {
            EV3Brick.GetEV3Brick.BrickStop_PreviewMouseLeftButtonDown(sender, e);
        }

        private void Download_Click(object sender, MouseButtonEventArgs e)
        {
            EV3Brick.GetEV3Brick.Download_PreviewMouseLeftButtonDown(sender, e);
        }
    }
}

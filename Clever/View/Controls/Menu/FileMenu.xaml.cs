using Clever.CommonData;
using Clever.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Clever.View.Controls.Menu
{
    /// <summary>
    /// Логика взаимодействия для FileMenu.xaml
    /// </summary>
    public partial class FileMenu : UserControl
    {
        public FileMenu()
        {
            InitializeComponent();

            fileNewCB.IsChecked = Configurations.Get.File_New;
            fileOpenCB.IsChecked = Configurations.Get.File_Open;
            fileCloseAllCB.IsChecked = Configurations.Get.File_CloseAll;
            fileSaveCB.IsChecked = Configurations.Get.File_Save;
            fileSaveAllCB.IsChecked = Configurations.Get.File_SaveAll;
            fileSaveAsCB.IsChecked = Configurations.Get.File_SaveAs;
        }

        private void fileNewCB_Click(object sender, RoutedEventArgs e)
        {
            if (fileNewCB.IsChecked == true)
            {
                MainWindowVM.FileNewV = Visibility.Visible;
                Configurations.Get.File_New = true;
            }
            else
            {
                MainWindowVM.FileNewV = Visibility.Collapsed;
                Configurations.Get.File_New = false;
            }
        }

        private void fileOpenCB_Click(object sender, RoutedEventArgs e)
        {
            if (fileOpenCB.IsChecked == true)
            {
                MainWindowVM.FileOpenV = Visibility.Visible;
                Configurations.Get.File_Open = true;
            }
            else
            {
                MainWindowVM.FileOpenV = Visibility.Collapsed;
                Configurations.Get.File_Open = false;
            }
        }

        private void fileCloseAllCB_Click(object sender, RoutedEventArgs e)
        {
            if (fileCloseAllCB.IsChecked == true)
            {
                MainWindowVM.FileCloseAllV = Visibility.Visible;
                Configurations.Get.File_CloseAll = true;
            }
            else
            {
                MainWindowVM.FileCloseAllV = Visibility.Collapsed;
                Configurations.Get.File_CloseAll = false;
            }
        }

        private void fileSaveCB_Click(object sender, RoutedEventArgs e)
        {
            if (fileSaveCB.IsChecked == true)
            {
                MainWindowVM.FileSaveV = Visibility.Visible;
                Configurations.Get.File_Save = true;
            }
            else
            {
                MainWindowVM.FileSaveV = Visibility.Collapsed;
                Configurations.Get.File_Save = false;
            }
        }

        private void fileSaveAllCB_Click(object sender, RoutedEventArgs e)
        {
            if (fileSaveAllCB.IsChecked == true)
            {
                MainWindowVM.FileSaveAllV = Visibility.Visible;
                Configurations.Get.File_SaveAll = true;
            }
            else
            {
                MainWindowVM.FileSaveAllV = Visibility.Collapsed;
                Configurations.Get.File_SaveAll = false;
            }
        }

        private void fileSaveAsCB_Click(object sender, RoutedEventArgs e)
        {
            if (fileSaveAsCB.IsChecked == true)
            {
                MainWindowVM.FileSaveAsV = Visibility.Visible;
                Configurations.Get.File_SaveAs = true;
            }
            else
            {
                MainWindowVM.FileSaveAsV = Visibility.Collapsed;
                Configurations.Get.File_SaveAs = false;
            }
        }
    }
}

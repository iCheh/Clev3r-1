using Clever.CommonData;
using Clever.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Clever.View.Controls.Menu
{
    /// <summary>
    /// Логика взаимодействия для EditMenu.xaml
    /// </summary>
    public partial class EditMenu : UserControl
    {
        public EditMenu()
        {
            InitializeComponent();

            editCopyCB.IsChecked = Configurations.Get.Edit_Copy;
            editPasteCB.IsChecked = Configurations.Get.Edit_Paste;
            editCutCB.IsChecked = Configurations.Get.Edit_Cut;
            editUndoCB.IsChecked = Configurations.Get.Edit_Undo;
            editRedoCB.IsChecked = Configurations.Get.Edit_Redo;
            editSelectAllCB.IsChecked = Configurations.Get.Edit_SelectAll;
            editFormatCB.IsChecked = Configurations.Get.Edit_Format;
            editShowLineCB.IsChecked = Configurations.Get.Edit_ShowLine;
            editShowNumberCB.IsChecked = Configurations.Get.Edit_ShowNumber;
            editShowSpaceCB.IsChecked = Configurations.Get.Edit_ShowSpace;
            editWrapCB.IsChecked = Configurations.Get.Edit_Wrap;
            editFindCB.IsChecked = Configurations.Get.Edit_Find;
        }

        private void editCopyCB_Click(object sender, RoutedEventArgs e)
        {
            if (editCopyCB.IsChecked == true)
            {
                MainWindowVM.EditCopyV = Visibility.Visible;
                Configurations.Get.Edit_Copy = true;
            }
            else
            {
                MainWindowVM.EditCopyV = Visibility.Collapsed;
                Configurations.Get.Edit_Copy = false;
            }
        }

        private void editCutCB_Click(object sender, RoutedEventArgs e)
        {
            if (editCutCB.IsChecked == true)
            {
                MainWindowVM.EditCutV = Visibility.Visible;
                Configurations.Get.Edit_Cut = true;
            }
            else
            {
                MainWindowVM.EditCutV = Visibility.Collapsed;
                Configurations.Get.Edit_Cut = false;
            }
        }

        private void editPasteCB_Click(object sender, RoutedEventArgs e)
        {
            if (editPasteCB.IsChecked == true)
            {
                MainWindowVM.EditPasteV = Visibility.Visible;
                Configurations.Get.Edit_Paste = true;
            }
            else
            {
                MainWindowVM.EditPasteV = Visibility.Collapsed;
                Configurations.Get.Edit_Paste = false;
            }
        }

        private void editUndoCB_Click(object sender, RoutedEventArgs e)
        {
            if (editUndoCB.IsChecked == true)
            {
                MainWindowVM.EditUndoV = Visibility.Visible;
                Configurations.Get.Edit_Undo = true;
            }
            else
            {
                MainWindowVM.EditUndoV = Visibility.Collapsed;
                Configurations.Get.Edit_Undo = false;
            }
        }

        private void editSelectAllCB_Click(object sender, RoutedEventArgs e)
        {
            if (editSelectAllCB.IsChecked == true)
            {
                MainWindowVM.EditSelectAllV = Visibility.Visible;
                Configurations.Get.Edit_SelectAll = true;
            }
            else
            {
                MainWindowVM.EditSelectAllV = Visibility.Collapsed;
                Configurations.Get.Edit_SelectAll = false;
            }
        }

        private void editFormatCB_Click(object sender, RoutedEventArgs e)
        {
            if (editFormatCB.IsChecked == true)
            {
                MainWindowVM.EditFormatV = Visibility.Visible;
                Configurations.Get.Edit_Format = true;
            }
            else
            {
                MainWindowVM.EditFormatV = Visibility.Collapsed;
                Configurations.Get.Edit_Format = false;
            }
        }

        private void editRedoCB_Click(object sender, RoutedEventArgs e)
        {
            if (editRedoCB.IsChecked == true)
            {
                MainWindowVM.EditRedoV = Visibility.Visible;
                Configurations.Get.Edit_Redo = true;
            }
            else
            {
                MainWindowVM.EditRedoV = Visibility.Collapsed;
                Configurations.Get.Edit_Redo = false;
            }
        }

        private void editShowLineCB_Click(object sender, RoutedEventArgs e)
        {
            if (editShowLineCB.IsChecked == true)
            {
                MainWindowVM.EditShowLineV = Visibility.Visible;
                Configurations.Get.Edit_ShowLine = true;
            }
            else
            {
                MainWindowVM.EditShowLineV = Visibility.Collapsed;
                Configurations.Get.Edit_ShowLine = false;
            }
        }

        private void editShowNumberCB_Click(object sender, RoutedEventArgs e)
        {
            if (editShowNumberCB.IsChecked == true)
            {
                MainWindowVM.EditShowNumberV = Visibility.Visible;
                Configurations.Get.Edit_ShowNumber = true;
            }
            else
            {
                MainWindowVM.EditShowNumberV = Visibility.Collapsed;
                Configurations.Get.Edit_ShowNumber = false;
            }
        }

        private void editShowSpaceCB_Click(object sender, RoutedEventArgs e)
        {
            if (editShowSpaceCB.IsChecked == true)
            {
                MainWindowVM.EditShowSpaceV = Visibility.Visible;
                Configurations.Get.Edit_ShowSpace = true;
            }
            else
            {
                MainWindowVM.EditShowSpaceV = Visibility.Collapsed;
                Configurations.Get.Edit_ShowSpace = false;
            }
        }

        private void editWrapCB_Click(object sender, RoutedEventArgs e)
        {
            if (editWrapCB.IsChecked == true)
            {
                MainWindowVM.EditWrapV = Visibility.Visible;
                Configurations.Get.Edit_Wrap = true;
            }
            else
            {
                MainWindowVM.EditWrapV = Visibility.Collapsed;
                Configurations.Get.Edit_Wrap = false;
            }
        }

        private void editFindCB_Click(object sender, RoutedEventArgs e)
        {
            if (editFindCB.IsChecked == true)
            {
                MainWindowVM.EditFindV = Visibility.Visible;
                Configurations.Get.Edit_Find = true;
            }
            else
            {
                MainWindowVM.EditFindV = Visibility.Collapsed;
                Configurations.Get.Edit_Find = false;
            }
        }
    }
}

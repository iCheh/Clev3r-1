using Clever.View.Controls.Editors;
using Clever.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clever.Model.Utils
{
    internal static class TextEditorEvents
    {
        //static string text = "";
        internal static void Editor_Change(object sender, RoutedEventArgs e)
        {
            //EditorPanel.SelectedItemHeaderChange = "●";
            var name = MainWindowVM.Project.SelectedItemName();

            if (MainWindowVM.Project.ContainsKey(name))
            {
                MainWindowVM.Project.GetDictionary()[name].TextChange = true;
            }
        }

        internal static void EditorName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var ti = sender as TabItem;
            var ed = ti.Content as Editor;
            ed.GetTextEditor.TextArea.Focus();
        }

        internal static void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            var ti = sender as TabItem;
            var ed = ti.Content as Editor;
            ed.GetTextEditor.TextArea.Focus();
            var header = ti.Header as EditorHeader;
            var data = MainWindowVM.Project.GetProgramData(ti);
            data.ToolTip = data.FullPath;
            header.MyToolTip = data.ToolTip;
        }
    }
}

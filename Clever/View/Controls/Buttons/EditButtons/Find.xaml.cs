using Clever.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clever.View.Controls.Buttons.EditButtons
{
    /// <summary>
    /// Логика взаимодействия для Find.xaml
    /// </summary>
    public partial class Find : UserControl
    {
        public Find()
        {
            InitializeComponent();
        }

        private void FindFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (MainWindowVM.Project.Count == 0)
                {
                    return;
                }
                var name = MainWindowVM.Project.SelectedItemName();
                var ti = MainWindowVM.Project.GetDictionary()[name];
                var data = MainWindowVM.Project.GetProgramData(ti.Item);
                if (data != null && FindFile.Text != "")
                    data.Editor.searchManager.Find(true, FindFile.Text);
            }
        }
    }
}

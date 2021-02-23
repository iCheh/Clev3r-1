using Clever.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clever.View.Controls.Buttons.HomeButtons
{
    /// <summary>
    /// Логика взаимодействия для hFind.xaml
    /// </summary>
    public partial class hFind : UserControl
    {
        public hFind()
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

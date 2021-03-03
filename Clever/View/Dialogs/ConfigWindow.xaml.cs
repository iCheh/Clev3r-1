using Clever.CommonData;
using Clever.Model.Bplus;
using Clever.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
        }

        private void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            Configurations.SetDefaultColor();
            var dc = this.DataContext as ConfigWindowVM;
            dc.ChangeColor = false;
            dc.SetSettingColor();
            dc.ChangeColor = true;
        }
    }
}

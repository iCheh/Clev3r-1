using Clever.ViewModel.PanelsVM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clever.View.Panels
{
    /// <summary>
    /// Логика взаимодействия для StatusPanel.xaml
    /// </summary>
    public partial class StatusPanel : UserControl
    {
        public StatusPanel()
        {
            InitializeComponent();
            this.DataContext = new StatusPanelVM();
        }

        private void StatusMessage_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dc = this.DataContext as StatusPanelVM;
            dc.StatusMessage_PreviewMouseDoubleClick(sender, e);
        }

        private void StatusMessage_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var dc = this.DataContext as StatusPanelVM;
            dc.StatusMessage_PreviewMouseRightButtonUp(sender, e);
        }
    }
}

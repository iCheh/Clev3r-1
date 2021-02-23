using Clever.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Clever.View.Controls
{
    /// <summary>
    /// Логика взаимодействия для HelpPanel.xaml
    /// </summary>
    public partial class HelpPanel : UserControl
    {
        private static UserControl _control;
        private static TabControl _tcHelps;
        private static TabItem _tiView;
        public HelpPanel()
        {
            InitializeComponent();
            _control = this;
            _tcHelps = TcHelps;
            _tiView = TiView;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = e.NewSize.Width;
            MainWindowVM.SpWidth = width - 55;
            MainWindowVM.SpWidthCl = width - 50;

            var height = e.NewSize.Height;
            MainWindowVM.WidthHelpPanel = width;
            MainWindowVM.HeightHelpPanel = height;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //var s = this.DesiredSize;
            //var h = s.Height;
            //var w = s.Width;
            MainWindowVM.HeightHelpPanel = this.DesiredSize.Height;
            MainWindowVM.WidthHelpPanel = this.DesiredSize.Width;

            //MainWindowVM.Status.Clear();
            //MainWindowVM.Status.Add(this.Width.ToString() + "    " + this.Height.ToString());
            //MainWindowVM.Status.Add(w.ToString() + "    " + h.ToString());
        }

        public static TabControl GetTabControlHelps()
        {
            return _tcHelps;
        }

        public static TabItem GetTabItemView()
        {
            return _tiView;
        }
    }
}

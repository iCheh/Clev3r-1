using Clever.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Clever.View.Controls.Editors
{

    /// <summary>
    /// Логика взаимодействия для EditorHeader.xaml
    /// </summary>
    public partial class EditorHeader : UserControl
    {
        private static TextBlock _header;
        public static string CloseName = "";
        public EditorHeader()
        {
            InitializeComponent();
            _header = hdrChanges;
            MyToolTip = MainWindowVM.GetLocalization["tHeaderToolTip"];
        }

        public static string Header
        {
            get { return _header.Text; }
            set { _header.Text = value; }
        }

        public string HeaderName
        {
            get { return hdrText.Text; }
            set { hdrText.Text = value; }
        }

        public string HeaderChange
        {
            get { return hdrChanges.Text; }
            set { hdrChanges.Text = value; }
        }

        public Brush HeaderColor
        {
            get { return hdrChanges.Foreground; }
            set { hdrChanges.Foreground = value; }
        }

        public string MyToolTip
        {
            get { return HeaderToolTip.Text; }
            set { HeaderToolTip.Text = value; }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            CloseName = HeaderName;
        }

        public Thickness LabelMargin
        {
            get { return lClose.Margin; }
            set { lClose.Margin = value; }
        }

        public double GetWidth
        {
            get { return HeaderGrid.DesiredSize.Width; }
        }
    }
}

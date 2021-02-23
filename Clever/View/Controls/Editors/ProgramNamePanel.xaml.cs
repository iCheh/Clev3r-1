using System.Windows;
using System.Windows.Controls;

namespace Clever.View.Controls.Editors
{
    /// <summary>
    /// Логика взаимодействия для ProgramNamePanel.xaml
    /// </summary>
    public partial class ProgramNamePanel : UserControl
    {
        public static string CloseName = "";

        public ProgramNamePanel()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            CloseName = HeaderName;
            EditorHeader.CloseName = HeaderName;
        }

        public string HeaderName
        {
            get { return hdrText.Text; }
            set { hdrText.Text = value; }
        }

        public Label GetLabel
        {
            get { return nameLabel; }
        }

        public Border GetBorder
        {
            get { return xBorder; }
        }

        public TextBlock GetTextBlock
        {
            get { return hdrText; }
        }
    }
}

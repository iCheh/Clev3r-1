using System.Windows;
using System.Windows.Controls;

namespace Clever.View.Controls.Menu
{
    /// <summary>
    /// Логика взаимодействия для PreviewMenu.xaml
    /// </summary>
    public partial class PreviewMenu : UserControl
    {
        public PreviewMenu()
        {
            InitializeComponent();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            View.Controls.Helps.View.GetEditFileContents().Copy();
            View.Controls.Helps.View.GetPreviewPopup.IsOpen = false;
        }
    }
}

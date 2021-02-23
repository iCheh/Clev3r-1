using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Clever.View.Controls
{
    /// <summary>
    /// Логика взаимодействия для ButtonsPanel.xaml
    /// </summary>
    public partial class ButtonsPanel : UserControl
    {
        private static Popup _filePopup;
        private static Popup _editPopup;
        private static Popup _homePopup;
        private static Popup _compilePopup;

        public ButtonsPanel()
        {
            InitializeComponent();
            _filePopup = filePopup;
            _editPopup = editPopup;
            _homePopup = homePopup;
            _compilePopup = compilePopup;
        }

        public static Popup GetFilePopup
        {
            get { return _filePopup; }
        }

        public static Popup GetEditPopup
        {
            get { return _editPopup; }
        }

        public static Popup GetHomePopup
        {
            get { return _homePopup; }
        }

        public static Popup GetBrickPopup
        {
            get { return _compilePopup; }
        }

        private void File_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            filePopup.IsOpen = true;
        }

        private void Edit_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            editPopup.IsOpen = true;
        }

        private void Home_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            homePopup.IsOpen = true;
        }

        private void Brick_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            compilePopup.IsOpen = true;
        }
    }
}

using Clever.View.Controls.Helps;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clever.View.Controls.Buttons.CompileButtons
{
    /// <summary>
    /// Логика взаимодействия для ButtonDownload.xaml
    /// </summary>
    public partial class ButtonDownload : UserControl
    {
        private static Button _button;
        public ButtonDownload()
        {
            InitializeComponent();
            _button = button;
        }
        public static Button GetButton
        {
            get { return _button; }
        }

        private void button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EV3Brick.GetEV3Brick.Download_PreviewMouseLeftButtonDown(sender, e);
        }
    }
}

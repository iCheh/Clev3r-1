using Clever.View.Controls.Helps;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clever.View.Controls.Buttons.CompileButtons
{
    /// <summary>
    /// Логика взаимодействия для ButtonStop.xaml
    /// </summary>
    public partial class ButtonStop : UserControl
    {
        private static Button _button;
        public ButtonStop()
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
            EV3Brick.GetEV3Brick.BrickStop_PreviewMouseLeftButtonDown(sender, e);
        }
    }
}

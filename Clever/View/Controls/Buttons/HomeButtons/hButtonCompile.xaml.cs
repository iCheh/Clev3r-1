using Clever.View.Controls.Helps;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clever.View.Controls.Buttons.HomeButtons
{
    /// <summary>
    /// Логика взаимодействия для hButtonCompile.xaml
    /// </summary>
    public partial class hButtonCompile : UserControl
    {
        private static Button _button;
        public hButtonCompile()
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
            EV3Brick.GetEV3Brick.Compile_PreviewMouseLeftButtonDown(sender, e);
        }
    }
}

using Clever.View.Controls.Helps;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Clever.View.Controls.Buttons.HomeButtons
{
    /// <summary>
    /// Логика взаимодействия для hButtonConnect.xaml
    /// </summary>
    public partial class hButtonConnect : UserControl
    {
        private static ToggleButton _toggleButton;

        public hButtonConnect()
        {
            InitializeComponent();
            _toggleButton = toggleButton;
        }

        public static ToggleButton GetButton
        {
            get { return _toggleButton; }
        }

        private void toggleButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EV3Brick.GetEV3Brick.EV3SwitchDevice_PreviewMouseLeftButtonDown(sender, e);
        }
    }
}

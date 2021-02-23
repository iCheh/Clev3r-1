using Clever.View.Controls.Helps;
using System.Windows;
using System.Windows.Controls;

namespace Clever.View.Controls.Menu
{
    /// <summary>
    /// Логика взаимодействия для LanguageMenu.xaml
    /// </summary>
    public partial class EV3BrickMenu : UserControl
    {
        public EV3BrickMenu()
        {
            InitializeComponent();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            EV3Brick.GetEV3Brick.EV3RefreshList();
        }
    }
}

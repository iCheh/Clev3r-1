using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Atom_Click(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            Process.Start(link.NavigateUri.ToString());
            this.DialogResult = true;
        }

        private void Optimum_Click(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            Process.Start(link.NavigateUri.ToString());
            this.DialogResult = true;
        }

        private void Robomaks_Click(object sender, RoutedEventArgs e)
        {
            var link = (Hyperlink)sender;
            Process.Start(link.NavigateUri.ToString());
            this.DialogResult = true;
        }
    }
}

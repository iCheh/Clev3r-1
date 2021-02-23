using Clever.ViewModel;
using System.Windows;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow(string defaultAnswer = "")
        {
            InitializeComponent();
            txtException.Text = MainWindowVM.GetLocalization["dException"];
            txtAnswer.Text = defaultAnswer;
            butText.Text = MainWindowVM.GetLocalization["dButClose"];
        }

        private void btnDialogClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

using Clever.ViewModel;
using System.Windows;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для NameValidator.xaml
    /// </summary>
    public partial class NameValidator : Window
    {
        public NameValidator(string defaultAnswer = "")
        {
            InitializeComponent();
            txtAnswer.Text = defaultAnswer;
            butText.Text = MainWindowVM.GetLocalization["dButClose"];
        }

        private void btnDialogClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

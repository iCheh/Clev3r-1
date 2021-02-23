using Clever.ViewModel;
using System.Windows;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SaveAllPrograms.xaml
    /// </summary>
    public partial class SaveAllPrograms : Window
    {

        public SaveAllPrograms(string defaultAnswer = "")
        {
            InitializeComponent();
            txtAnswer.Text = defaultAnswer;
            butTextYes.Text = MainWindowVM.GetLocalization["dButYes"];
            butTextNo.Text = MainWindowVM.GetLocalization["dButNo"];
            butTextCancel.Text = MainWindowVM.GetLocalization["dButCancel"];
        }

        private void btnDialogYes_Click(object sender, RoutedEventArgs e)
        {
            //this.Visibility = Visibility.Hidden;
            MainWindowVM.SaveAllAndClose();
            this.DialogResult = true;
        }

        private void btnDialogNo_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = true;
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

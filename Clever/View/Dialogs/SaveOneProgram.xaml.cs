using Clever.ViewModel;
using System.Windows;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SaveOneProgram.xaml
    /// </summary>
    public partial class SaveOneProgram : Window
    {
        int result;
        public SaveOneProgram(string defaultAnswer = "")
        {
            InitializeComponent();
            txtAnswer.Text = defaultAnswer;
            butTextYes.Text = MainWindowVM.GetLocalization["dButYes"];
            butTextNo.Text = MainWindowVM.GetLocalization["dButNo"];
            butTextCancel.Text = MainWindowVM.GetLocalization["dButCancel"];
            result = 0;
        }

        private void btnDialogYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnDialogNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnDialogCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            result = -1;
        }

        public int GetResult
        {
            get { return result; }
        }
    }
}

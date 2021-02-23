using Clever.ViewModel;
using System.Windows;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для UnAssociationWindow.xaml
    /// </summary>
    public partial class UnAssociationWindow : Window
    {
        public UnAssociationWindow()
        {
            InitializeComponent();
            txtAnswer.Text = MainWindowVM.GetLocalization["prgUnAssociation"];
            butTextYes.Text = MainWindowVM.GetLocalization["prgYes"];
            butTextNo.Text = MainWindowVM.GetLocalization["prgNo"];
            butTextCancel.Text = MainWindowVM.GetLocalization["dButCancel"];
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
        }
    }
}

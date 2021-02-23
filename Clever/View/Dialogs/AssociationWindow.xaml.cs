using Clever.ViewModel;
using System.Windows;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AssociationWindow.xaml
    /// </summary>
    public partial class AssociationWindow : Window
    {
        public AssociationWindow()
        {
            InitializeComponent();
            txtAnswer.Text = MainWindowVM.GetLocalization["prgAssociation"];
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

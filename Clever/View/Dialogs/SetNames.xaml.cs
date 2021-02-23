using Clever.Model.Utils;
using Clever.ViewModel;
using System.Windows;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SetNames.xaml
    /// </summary>
    public partial class SetNames : Window
    {
        public SetNames(string prgN, string modN, string incN)
        {
            InitializeComponent();
            programName.Text = prgN.Replace(".bp", "");
            moduleName.Text = modN.Replace(".bpm", "");
            includeName.Text = incN.Replace(".bpi", "");
            //MessageBox.Show("1");
            butTextPrg.Text = MainWindowVM.GetLocalization["winCreate"];
            butTextMod.Text = MainWindowVM.GetLocalization["winCreate"];
            butTextInc.Text = MainWindowVM.GetLocalization["winCreate"];
            butTextClose.Text = MainWindowVM.GetLocalization["winCancel"];
            txtToolTip.Text = MainWindowVM.GetLocalization["winToolTip"];
            txtProgramName.Text = MainWindowVM.GetLocalization["winProgName"] + ":";
            txtModuleName.Text = MainWindowVM.GetLocalization["winModName"] + ":";
            txtIncludeName.Text = MainWindowVM.GetLocalization["winIncName"] + ":";
            //MessageBox.Show("2");
        }

        private void butCreatePrg_Click(object sender, RoutedEventArgs e)
        {
            GetName = programName.Text + ".bp";
            if (ValidName(".bp"))
                this.DialogResult = true;
        }

        private void butCreateMod_Click(object sender, RoutedEventArgs e)
        {
            GetName = moduleName.Text + ".bpm";
            if (ValidName(".bpm"))
                this.DialogResult = true;
        }

        private void butCreateInc_Click(object sender, RoutedEventArgs e)
        {
            GetName = includeName.Text + ".bpi";
            if (ValidName(".bpi"))
                this.DialogResult = true;
        }

        private void butClose_Click(object sender, RoutedEventArgs e)
        {
            GetName = "";
            this.DialogResult = false;
        }

        private bool ValidName(string ext)
        {
            return new ValidName().Valid(GetName.Replace(ext, ""));
        }

        public string GetName { get; set; }
    }
}

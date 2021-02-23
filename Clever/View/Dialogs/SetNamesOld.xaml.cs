using Clever.Model.Utils;
using Clever.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SetNamesOld.xaml
    /// </summary>
    public partial class SetNamesOld : Window
    {
        public SetNamesOld(string prgN, string modN)
        {
            InitializeComponent();
            programName.Text = prgN.Replace(".bp", "");
            includeName.Text = modN.Replace(".bpi", "");
            butTextPrg.Text = MainWindowVM.GetLocalization["winCreate"];
            butTextMod.Text = MainWindowVM.GetLocalization["winCreate"];
            butTextClose.Text = MainWindowVM.GetLocalization["winCancel"];
            txtToolTip.Text = MainWindowVM.GetLocalization["winToolTip"];
            txtProgramName.Text = MainWindowVM.GetLocalization["winProgName"] + ":";
            txtIncludeName.Text = MainWindowVM.GetLocalization["winIncName"] + ":";
        }

        private void butCreatePrg_Click(object sender, RoutedEventArgs e)
        {
            GetName = programName.Text + ".bp";
            if (ValidName(".bp"))
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

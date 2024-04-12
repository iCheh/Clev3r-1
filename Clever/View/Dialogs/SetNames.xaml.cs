using Clever.Model.Utils;
using Clever.ViewModel;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Input;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SetNames.xaml
    /// </summary>
    public partial class SetNames : Window
    {
        private int curFocus;
        public SetNames(string prgN, string modN, string incN)
        {
            curFocus = 0;
            InitializeComponent();
            programName.Text = prgN.Replace(".bp", "");
            moduleName.Text = modN.Replace(".bpm", "");
            includeName.Text = incN.Replace(".bpi", "");

            butTextPrg.Text = MainWindowVM.GetLocalization["winCreate"];
            butTextMod.Text = MainWindowVM.GetLocalization["winCreate"];
            butTextInc.Text = MainWindowVM.GetLocalization["winCreate"];
            butTextClose.Text = MainWindowVM.GetLocalization["winCancel"];
            txtToolTip.Text = MainWindowVM.GetLocalization["winToolTip"];
            txtProgramName.Text = MainWindowVM.GetLocalization["winProgName"] + ":";
            txtModuleName.Text = MainWindowVM.GetLocalization["winModName"] + ":";
            txtIncludeName.Text = MainWindowVM.GetLocalization["winIncName"] + ":";

            SetNewFocus();
        }

        private void SetNewFocus()
        {
            if (curFocus == 0)
            {
                programName.Focus();
            }
            else if (curFocus == 1)
            {
                includeName.Focus();
            }
            else if (curFocus == 2)
            {
                moduleName.Focus();
            }
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

        private void programName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetName = programName.Text + ".bp";
                if (ValidName(".bp"))
                    this.DialogResult = true;
            }
        }

        private void includeName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetName = includeName.Text + ".bpi";
                if (ValidName(".bpi"))
                    this.DialogResult = true;
            }
        }

        private void moduleName_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetName = moduleName.Text + ".bpm";
                if (ValidName(".bpm"))
                    this.DialogResult = true;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                GetName = "";
                this.DialogResult = false;
            }
            else if (e.Key == Key.Down)
            {
                if (curFocus == 2)
                {
                    curFocus = 0;
                }
                else
                {
                    curFocus += 1;
                }
                SetNewFocus();
            }
            else if (e.Key == Key.Up)
            {
                if (curFocus == 0)
                {
                    curFocus = 2;
                }
                else
                {
                    curFocus -= 1;
                }
                SetNewFocus();
            }
        }
    }
}

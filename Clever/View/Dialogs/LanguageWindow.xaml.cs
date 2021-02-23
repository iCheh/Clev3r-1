using Clever.CommonData;
using Clever.Model.Utils;
using Clever.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Clever.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для LanguageWindow.xaml
    /// </summary>
    public partial class LanguageWindow : Window
    {
        List<string> Items = new List<string>();
        Dictionary<string, string> Lang = new Dictionary<string, string>();
        public LanguageWindow()
        {
            InitializeComponent();

            string path = Application.ResourceAssembly.Location.Replace("Clever.exe", "Localization\\");
            var lines = File.ReadAllLines(path + "setting.txt");

            foreach (var line in lines)
            {
                var tmp = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                Items.Add(tmp[0].Trim());
                Lang.Add(tmp[0].Trim(), tmp[1].Trim());
            }

            txtAnsver.Text = MainWindowVM.GetLocalization["lAnsver"];
            butText.Text = MainWindowVM.GetLocalization["dButClose"];
            Combo.ItemsSource = Items;
            Combo.SelectedIndex = 0;
            txtAnsver.Visibility = Visibility.Hidden;
        }

        private void butClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lang.Count > 0 && Lang.ContainsKey(Items[Combo.SelectedIndex]))
            {
                Configurations.Get.Language = Lang[Items[Combo.SelectedIndex]];
            }

            txtAnsver.Visibility = Visibility.Visible;
        }
    }
}

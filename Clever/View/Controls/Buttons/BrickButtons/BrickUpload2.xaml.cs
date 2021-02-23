using Clever.CommonData;
using Clever.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Clever.View.Controls.Buttons.BrickButtons
{
    /// <summary>
    /// Логика взаимодействия для BrickUpload2.xaml
    /// </summary>
    public partial class BrickUpload2 : UserControl
    {
        public static byte[] ev3fileB = null;
        public static string ev3fileName = "";
        string ev3filePath = "";

        public BrickUpload2()
        {
            InitializeComponent();
        }

        private void but_Click(object sender, RoutedEventArgs e)
        {
            if (ev3fileB == null)
            {
                return;
            }
            WriteEv3File();
        }

        private void WriteEv3File()
        {
            try
            {
                if (ev3filePath == "")
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = ev3fileName;
                    if (sfd.ShowDialog() == true)
                    {
                        ev3filePath = sfd.FileName;
                    }
                }
                FileStream fs = new FileStream(ev3filePath, FileMode.Create, FileAccess.Write);
                fs.Write(ev3fileB, 0, ev3fileB.Length);
                fs.Close();
            }
            catch (Exception ex)
            {
                Status.Clear();
                Status.Add("Exception: " + ex.Message);
            }
        }
    }
}

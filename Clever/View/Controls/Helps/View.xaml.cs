using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Clever.View.Controls.Helps
{

    /// <summary>
    /// Логика взаимодействия для View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        private static TextBox _edtFileContents;
        //public static byte[] ev3fileB = null;
        //public static string ev3fileName = "";
        //string ev3filePath = "";
        private static Popup _previewPopup;

        public View()
        {
            InitializeComponent();
            //edtFileContents.ContextMenu = new ContextMenu();
            _edtFileContents = edtFileContents;
            _previewPopup = previewPopup;
        }

        public static TextBox GetEditFileContents()
        {
            return _edtFileContents;
        }

        public static Popup GetPreviewPopup
        {
            get { return _previewPopup; }
        }

        private void edtFileContents_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            previewPopup.IsOpen = true;
        }

        /*
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
                MainWindowVM.Status.Clear();
                MainWindowVM.Status.Add("Exception: " + ex.Message);
            }
        }
        */
    }
}

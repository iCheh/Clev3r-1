using Clever.Model.Bplus;
using Clever.Model.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Clever.View.Controls.Editors
{
    /// <summary>
    /// Логика взаимодействия для Editor.xaml
    /// </summary>
    public partial class Editor : System.Windows.Controls.UserControl
    {
        TextEditor te;
        public event RoutedEventHandler EditorTextChange;
        //CompletionWindow completionWindow;

        public Editor()
        {
            InitializeComponent();
            var color = BpColors.Back_Margin_Color;
            edit.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            te = new TextEditor();
            te.TextArea.BorderStyle = BorderStyle.None;
            wfh.Child = te.TextArea;
        }

        public TextEditor GetTextEditor
        {
            get { return te; }
            //set { aEditor = value; }
        }

        private void edit_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            editorPopup.IsOpen = true;
        }

        public Popup GetTextEditorPopup
        {
            get { return editorPopup; }
        }

        public Popup GetTextEditorTools
        {
            get { return editorTools; }
        }

        public string SetTextEditorToolsText
        {
            get
            {
                return editorToolsText.Text;
            }
            set
            {
                editorToolsText.Text = value;
            }
        }

        public System.Windows.Forms.Integration.WindowsFormsHost GetHost
        {
            get { return wfh; }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.EditorTextChange -= TextEditorEvents.Editor_Change;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.EditorTextChange += TextEditorEvents.Editor_Change;
        }
    }
}

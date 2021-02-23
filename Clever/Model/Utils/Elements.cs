using Clever.Model.Bplus;
using Clever.View.Controls.Editors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Clever.Model.Utils
{
    class Elements
    {
        public string GetTabItemHeaderName(TabItem item)
        {
            var header = (EditorHeader)item.Header;
            return header.HeaderName;
        }

        public void SetTabItemHeaderName(TabItem item, string text)
        {
            var header = (EditorHeader)item.Header;
            header.HeaderName = text;
        }

        public string GetTabItemHeaderChange(TabItem item)
        {
            var header = (EditorHeader)item.Header;
            return header.HeaderChange;
        }

        public void SetTabItemHeaderChange(TabItem item, string text)
        {
            var header = (EditorHeader)item.Header;
            header.HeaderChange = text;
        }

        public TextEditor GetTextEditor(TabItem item)
        {
            var editor = (Editor)item.Content;
            return editor.GetTextEditor;
        }

        public void SetTextEditor(TabItem item, TextEditor textEditor)
        {
            var editor = (Editor)item.Content;
            //editor.GetTextEditor = textEditor;
        }

        public string GetTextEditorText(TabItem item)
        {
            var editor = (Editor)item.Content;
            return editor.GetTextEditor.TextArea.Text;
        }

        public void SetTextEditorText(TabItem item, string text)
        {
            var editor = (Editor)item.Content;
            editor.GetTextEditor.TextArea.Text = text;
        }

        public Thickness GetCloseButtonlMargin(TabItem item)
        {
            var header = (EditorHeader)item.Header;
            return header.LabelMargin;
        }

        public void SetCloseButtonlMargin(TabItem item, double right)
        {
            var header = (EditorHeader)item.Header;
            header.LabelMargin = new Thickness(0, 0, right, 0);
        }

        public Brush GetColor(TabItem item)
        {
            var header = (EditorHeader)item.Header;
            return header.HeaderColor;
        }

        public void SetColor(TabItem item, Brush color)
        {
            var header = (EditorHeader)item.Header;
            header.HeaderColor = color;
        }
    }
}

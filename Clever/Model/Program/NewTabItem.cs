using Clever.Model.Bplus;
using Clever.Model.Utils;
using Clever.View.Controls.Editors;
using Clever.View.Dialogs;
using Clever.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Clever.Model.Program
{
    class NewTabItem
    {
        public TabItem Create(Dictionary<string, ProgramData> items, out string name, out string path)
        {
            var prgName = "Program" + GetProgramNumber(items).ToString() + MainWindowVM.ext;
            var modName = "Module" + GetModuleNumber(items).ToString() + ".bpm";
            var incName = "Include" + GetIncludeNumber(items).ToString() + ".bpi";
            string newName = "";

            var win = new SetNames(prgName, modName, incName);
            if (win.ShowDialog() == true)
            {
                newName = win.GetName;
            }
            else
            {
                name = "";
                path = "";
                return null;
            }

            var tmpExt = newName.Substring(newName.Length - 3);
            var ext = "";

            if (tmpExt == ".bp")
            {
                ext = ".bp";
            }
            else
            {
                ext = "." + tmpExt;
            }

            var saveName = new SaveFile().Save("", newName, ext);
            
            if (saveName == "")
            {
                name = "";
                path = "";
                return null;
            }

            var ind = saveName.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            newName = saveName.Substring(ind);
            path = saveName.Substring(0, saveName.Length - newName.Length - 1);

            TabItem tabItem = new TabItem();
            tabItem.IsSelected = true;
            tabItem.Loaded += TextEditorEvents.Editor_Loaded;
            tabItem.MouseLeftButtonUp += TextEditorEvents.EditorName_MouseLeftButtonUp;
            EditorHeader header = new EditorHeader();
            Editor editor = new Editor();
            editor.GetTextEditor.EventsAdd();
            editor.GetTextEditor.TextArea.Text = "";
            editor.GetTextEditor.Menu = editor.GetTextEditorPopup;
            editor.GetTextEditor.ToolsWindow = editor.GetTextEditorTools;
            header.HeaderName = newName;
            name = header.HeaderName;
            header.HeaderChange = "";
            tabItem.Header = header;
            tabItem.Content = editor;
            tabItem.Style = (Style)Application.Current.Resources["FirstTabItem"];
            if (items.Count == 0)
            {
                var element = new Elements();
                element.SetCloseButtonlMargin(tabItem, 1);
            }

            return tabItem;
        }

        public TabItem Create(Dictionary<string, ProgramData> items, string text, string itemName)
        {
            TabItem tabItem = new TabItem();
            tabItem.IsSelected = true;
            tabItem.Loaded += TextEditorEvents.Editor_Loaded;
            tabItem.MouseLeftButtonUp += TextEditorEvents.EditorName_MouseLeftButtonUp;
            EditorHeader header = new EditorHeader();
            Editor editor = new Editor();
            editor.GetTextEditor.TextArea.Text = text;
            editor.GetTextEditor.EventsAdd();          
            editor.GetTextEditor.Menu = editor.GetTextEditorPopup;
            editor.GetTextEditor.ToolsWindow = editor.GetTextEditorTools;
            editor.GetTextEditor.Lexer.SetFolding();
            header.HeaderName = itemName;
            header.HeaderChange = "";
            tabItem.Header = header;
            tabItem.Content = editor;
            tabItem.Style = (Style)Application.Current.Resources["FirstTabItem"];
            if (items.Count == 0)
            {
                var element = new Elements();
                element.SetCloseButtonlMargin(tabItem, 1);
            }

            return tabItem;
        }

        public TabItem Create(string text, string itemName)
        {
            TabItem tabItem = new TabItem();
            tabItem.IsSelected = true;
            EditorHeader header = new EditorHeader();
            Editor editor = new Editor();
            editor.GetTextEditor.TextArea.Text = text;
            header.HeaderName = itemName;
            header.HeaderChange = "";
            tabItem.Header = header;
            tabItem.Content = editor;

            return tabItem;
        }

        private int GetProgramNumber(Dictionary<string, ProgramData> items)
        {
            int num = 1;
            string tmpName = "Program" + num.ToString() + MainWindowVM.ext;

            if (items.Count > 0)
            {
                while (items.ContainsKey(tmpName))
                {
                    num++;
                    tmpName = "Program" + num.ToString() + MainWindowVM.ext;
                }
            }

            return num;
        }
        private int GetModuleNumber(Dictionary<string, ProgramData> items)
        {
            int num = 1;
            string tmpName = "Module" + num.ToString() + ".bpm";

            if (items.Count > 0)
            {
                while (items.ContainsKey(tmpName))
                {
                    num++;
                    tmpName = "Module" + num.ToString() + ".bpm";
                }
            }

            return num;
        }
        private int GetIncludeNumber(Dictionary<string, ProgramData> items)
        {
            int num = 1;
            string tmpName = "Include" + num.ToString() + ".bpi";

            if (items.Count > 0)
            {
                while (items.ContainsKey(tmpName))
                {
                    num++;
                    tmpName = "Include" + num.ToString() + ".bpi";
                }
            }

            return num;
        }
    }
}

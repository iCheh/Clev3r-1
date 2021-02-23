using Clever.Model.Utils;
using Clever.View.Controls.Editors;
using Clever.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Clever.Model.Program
{
    class CloseProgram
    {
        int result;

        public ObservableCollection<TabItem> Close(ObservableCollection<TabItem> programs, string name, out int res)
        {
            ObservableCollection<TabItem> items = new ObservableCollection<TabItem>();
            Elements elem = new Elements();
            result = 0;

            foreach (var i in programs)
            {
                if (elem.GetTabItemHeaderName(i) != name)
                {
                    items.Add(i);
                }
                else
                {
                    var pData = MainWindowVM.Project.GetProgramData(i);
                    if (pData.TextChange)
                    {
                        if (DoYouWantToDelete(name))
                        {
                            if (pData.FullPath != "")
                            {
                                new SaveFile().Save(pData);
                            }
                            else
                            {
                                string tmpExt = "";
                                for (int a = name.IndexOf('.'); a < name.Length; a++)
                                {
                                    tmpExt += name[a];
                                }
                                new SaveFile().Save(pData.Editor.TextArea.Text, name, tmpExt);
                            }
                        }
                    }

                    if (result == -1)
                    {
                        items.Add(i);
                    }
                    else
                    {
                        i.Loaded -= TextEditorEvents.Editor_Loaded;
                        i.MouseLeftButtonUp -= TextEditorEvents.EditorName_MouseLeftButtonUp;
                        var ed = (Editor)i.Content;
                        ed.GetTextEditor.Dispose();
                    }
                }
            }

            res = result;
            return items;
        }

        private bool DoYouWantToDelete(string name)
        {
            var win = new View.Dialogs.SaveOneProgram(MainWindowVM.GetLocalization["dSaveOne"] + " " + name + "?");
            if (win.ShowDialog() == true)
            {
                return true;
            }
            else
            {
                result = win.GetResult;
                return false;
            }
        }
    }
}

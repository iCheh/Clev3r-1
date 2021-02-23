using Clever.Model.Utils;
using Microsoft.Win32;
using System.IO;

namespace Clever.Model.Program
{
    class SaveFile
    {
        public string Save(string text, string name, string ext)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            if (ext == "")
            {
                string tmpExt = "";
                for (int a = name.IndexOf('.'); a < name.Length; a++)
                {
                    tmpExt += name[a];
                }
                if (tmpExt == ".bp")
                {
                    sfd.Filter = "Basic plus import files (*.bp)|*.bp" + "|Basic plus import files (*.bpm)|*.bpm" + "|Basic plus import files (*.bpi)|*.bpi";
                }
                else if (tmpExt == ".bpm")
                {
                    sfd.Filter = "Basic plus import files (*.bpm)|*.bpm" + "|Basic plus import files (*.bp)|*.bp" + "|Basic plus import files (*.bpi)|*.bpi";
                }
                else if (tmpExt == ".bpi")
                {
                    sfd.Filter = "Basic plus import files (*.bpi)|*.bpi" + "|Basic plus import files (*.bp)|*.bp" + "|Basic plus import files (*.bpm)|*.bpm";
                }
                //sfd.Filter = "Basic plus file (*" + MainWindowVM.ext + ")|*" + MainWindowVM.ext + "|Basic plus import files (*.bpm)|*.bpm";
            }
            else
            {
                sfd.Filter = "Basic plus file (*" + ext + ")|*" + ext;
            }

            string tmpName = "";
            bool notValid = true;
            do
            {
                sfd.FileName = name;
                if (sfd.ShowDialog() == true)
                {

                    if (new ValidName().Valid(ReplaceName(sfd.SafeFileName)))
                    {
                        tmpName = sfd.FileName;
                        notValid = false;
                        File.WriteAllText(tmpName, text);
                    }
                }
                else
                {
                    notValid = false;
                }
            } while (notValid);

            return tmpName;
        }

        public void Save(ProgramData data)
        {
            File.WriteAllText(data.FullPath, data.Editor.TextArea.Text);
        }

        private string ReplaceName(string name)
        {
            string tmp = "";

            foreach (var c in name)
            {
                if (c != '.')
                {
                    tmp += c;
                }
                else
                {
                    break;
                }
            }

            return tmp;
        }
    }
}

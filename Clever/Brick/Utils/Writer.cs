using System.Collections.Generic;
using System.IO;

namespace Clever.Brick.Utils
{
    class Writer
    {
        public void WriteFile(string path, string name, List<string> text, string suf)
        {
            string tmpName = "~";
            int index = name.IndexOf('.');
            for (int i = 0; i < index; i++)
            {
                tmpName += name[i];
            }
            string fName = tmpName;
            tmpName += suf;
            DirectoryInfo df = new DirectoryInfo(path + fName);
            if (!df.Exists)
            {
                df.Create();
            }

            string tmpText = "";
            for (int i = 0; i < text.Count; i++)
            {
                tmpText += text[i];
                if (i < text.Count - 1)
                {
                    tmpText += '\n';
                }
            }
            File.WriteAllText(df.FullName + "\\" + tmpName, tmpText);
            //File.WriteAllLines(df.FullName + "\\" + tmpName, text);
        }
    }
}

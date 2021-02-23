using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clever.Model.Bplus.BPInterpreter
{
    internal class Module
    {
        private string separator = System.IO.Path.DirectorySeparatorChar.ToString();

        internal Module(string name, string mainPath)
        {
            var tmpPath = Application.ResourceAssembly.Location.Replace("Clever.exe", "");

            name = name.Replace("\"", "").Trim().Replace("\\", separator).Replace("/", separator);
            mainPath = mainPath.Replace("\"", "").Trim().Replace("\\", separator).Replace("/", separator);

            if (name.IndexOf("Clev3r:" + separator + separator) == 0)
            {
                //var pth = tmpPath + name.Replace("Clev3r:" + separator + separator, "Lib" + separator + "Modules" + separator) + ".bpm";
                //Path = tmpPath + name.Replace("Clev3r:" + separator + separator, "Lib" + separator + "Modules" + separator) + ".bpm";
                Path = tmpPath + "Lib" + separator + "Modules" + separator;
            }
            else if (name.IndexOf("Clever:" + separator + separator) == 0)
            {
                //var pth = tmpPath + name.Replace("Clev3r:" + separator + separator, "Lib" + separator + "Modules" + separator) + ".bpm";
                //Path = tmpPath + name.Replace("Clever:" + separator + separator, "Lib" + separator + "Modules" + separator) + ".bpm";
                Path = tmpPath + "Lib" + separator + "Modules" + separator;
            }
            else
            {
                Path = GetPath(name, mainPath);
            }

            OriginName = GetName(name);
            Name = OriginName.ToLower();
            Summary = new List<string>();
        }
        internal string OriginName { get; private set; }
        internal string Name { get; private set; }
        internal string Path { get; private set; }
        internal List<string> Summary { get; private set; }

        private string GetName(string name)
        {
            var tmpName = name + ".bpm";
            var tmpStart = name.LastIndexOf(separator);
            if (tmpStart != -1 && tmpStart + 1 < name.Length)
            {
                tmpName = name.Substring(tmpStart + 1) + ".bpm";
            }

            return tmpName;
        }

        private string GetPath(string name, string mainPath)
        {
            var tmpName = "";

            var mainWords = mainPath.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            var nameWords = name.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            if (nameWords.Length > 1)
            {
                var down = 0;
                var nextPath = "";
                for (int j = 0; j < nameWords.Length - 1; j++)
                {
                    var w = nameWords[j];
                    if (w == "..")
                    {
                        down++;
                    }
                    else
                    {
                        nextPath += w + separator;
                    }
                }
                if (down > 0)
                {
                    for (int j = 0; j < mainWords.Length - down; j++)
                    {
                        tmpName += mainWords[j] + separator;
                    }
                    tmpName += nextPath;
                }
                else
                {
                    tmpName = mainPath + separator + nextPath;
                }
            }
            else
            {
                tmpName = mainPath + separator;
            }

            return tmpName;
        }
    }
}

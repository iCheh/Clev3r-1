using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using Interpreter.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class IncludeErrorParser
    {
        private static string separator = Path.DirectorySeparatorChar.ToString();

        internal static void Start(Line line)
        {
            IncludeName = "";
            if (line.Count == 1)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1102, ""));
                return;
            }
            else if (line.Count > 2 || line.Words.Count <= 0)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1103, ""));
                return;
            }
            else if (line.Words[1].Token != Enums.Tokens.STRING)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1104, ""));
                return;
            }
            else
            {
                //var includeFullPath = Data.Project.Path + line.Words[1].Text.Replace("\"", "").Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
                var includeFullPath = CreateFullPath(line.Words[1].Text, Data.Project.Path);
                //Console.WriteLine("=>" + includeFullPath + "<=");

                string tmpPath = GetIncludePath(includeFullPath) + GetIncludeName(includeFullPath) + Extension.BPInclude;

                FileInfo fi = new FileInfo(tmpPath);
                if (!fi.Exists)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1101, includeFullPath + Extension.BPInclude));
                }
                else
                {
                    IncludeName = GetIncludeName(tmpPath.Replace(Extension.BPInclude, ""));
                    IncludePath = GetIncludePath(tmpPath.Replace(Extension.BPInclude, ""));
                    Lines = new List<Line>();
                    OldText = new List<string>();
                    GetAllLines(tmpPath, IncludePath);
                }
            }
        }

        private static string GetIncludePath(string word)
        {
            var fi = new FileInfo(word + Extension.BPInclude);
            if (fi.Exists)
            {
                return fi.DirectoryName + separator;
            }

            return "";
        }

        private static string GetIncludeName(string word)
        {
            var fi = new FileInfo(word + Extension.BPInclude);

            if (fi.Exists)
            {
                return fi.Name.Replace(Extension.BPInclude, "");
            }
            return "";
        }

        private static string CreateFullPath(string name, string mainPath)
        {
            var fullPath = "";

            name = name.Replace("\"", "").Trim().Replace("\\", separator).Replace("/", separator);

            var fileName = "";
            if (name.LastIndexOf(separator) != -1)
            {
                fileName = name.Substring(name.LastIndexOf(separator));
            }
            else
            {
                fileName = name;
            }

            mainPath = mainPath.Replace("\"", "").Trim().Replace("\\", separator).Replace("/", separator);

            if (name.IndexOf("Clev3r:" + separator + separator) == 0)
            {
                fullPath = Data.Project.ModuleLibPath + name.Replace("Clev3r:" + separator + separator, "Lib" + separator + "Includes" + separator);
                return (fullPath).Replace(separator + separator, separator);
            }
            else if (name.IndexOf("Clever:" + separator + separator) == 0)
            {
                fullPath = Data.Project.ModuleLibPath + name.Replace("Clever:" + separator + separator, "Lib" + separator + "Includes" + separator);
                return (fullPath).Replace(separator + separator, separator);
            }
            else
            {
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
                            fullPath += mainWords[j] + separator;
                        }
                        fullPath += nextPath;
                    }
                    else
                    {
                        fullPath = mainPath + separator + nextPath;
                    }
                }
                else
                {
                    fullPath = mainPath + separator;
                }
            }

            while (fullPath.LastIndexOf(separator) == fullPath.Length - 1)
            {
                fullPath = fullPath.Substring(0, fullPath.Length - 1);
            }

            return (fullPath + separator + fileName).Replace(separator + separator, separator);
        }

        private static void GetAllLines(string fullPath, string path)
        {
            var tmpText = File.ReadAllLines(fullPath);
            //var fi = new FileInfo(path);
            for (int i = 0; i < tmpText.Length; i++)
            {
                var tmpLine = new Line(LineBuilder.GetWords(tmpText[i]), tmpText[i]);
                tmpLine.Number = i + 1;
                //tmpLine.FileName = IncludeName + Extension.BPInclude;
                tmpLine.FileName = fullPath;
                //tmpLine.FileName = fi.Name;
                tmpLine.Type = LineBuilder.GetType(tmpLine);

                if (tmpLine.Type == LineType.INCLUDE)
                {
                    Data.Errors.Add(new Errore(tmpLine.Number, IncludeName + Extension.BPInclude, 1105, ""));
                    //Data.Errors.Add(new Errore(tmpLine.Number, fi.Name, 1105, ""));
                    return;
                }
                else if (tmpLine.Type == LineType.FOLDER)
                {
                    Data.Errors.Add(new Errore(tmpLine.Number, IncludeName + Extension.BPInclude, 1106, ""));
                    //Data.Errors.Add(new Errore(tmpLine.Number, fi.Name, 1106, ""));
                    return;
                }
                else if (tmpLine.Type == LineType.IMPORT)
                {
                    ImportErrorParser.Start(tmpLine, path);
                    if (Data.Errors.Count > 0)
                        return;
                }

                // Парсим ошибки на количество скобок в строке
                BracketErrorParser.Start(tmpLine);
                if (Data.Errors.Count > 0)
                    return;

                Lines.Add(tmpLine);
                OldText.Add(tmpText[i]);
            }
        }

        internal static string IncludeName { get; private set; } // Без расширения
        internal static string IncludePath { get; private set; }
        internal static List<Line> Lines { get; private set; }
        internal static List<string> OldText { get; private set; }
    }
}

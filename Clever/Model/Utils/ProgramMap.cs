using Clever.Model.Bplus;
using Clever.Model.Bplus.BPInterpreter;
using Clever.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Clever.Model.Intellisense;

namespace Clever.Model.Utils
{
    internal class ProgramMap
    {
        private string separator = Path.DirectorySeparatorChar.ToString();
        internal ProgramMap(BPType type)
        {
            Type = type;
            Variables = new List<Variable>();
            Subroutines = new List<Function>();
            Labels = new List<Label>();
            Includes = new Dictionary<string, Include>();
            Imports = new Dictionary<string, Module>();
            Map = new Dictionary<int, bool>();
            Summary = new List<string>();
            MainName = "";
            MainPath = "";
        }

        internal void Clear()
        {
            Variables.Clear();
            Subroutines.Clear();
            Labels.Clear();
            Includes.Clear();
            Imports.Clear();
            Map.Clear();
            Summary.Clear();
            MainName = "";
            MainPath = "";
        }

        internal BPType Type { get; set; }
        internal List<Variable> Variables { get; private set; }
        internal List<Function> Subroutines { get; private set; }
        internal List<Label> Labels { get; private set; }
        internal Dictionary<string, Include> Includes { get; set; }
        internal Dictionary<string, Module> Imports { get; set; }
        internal Dictionary<int, bool> Map { get; private set; }
        internal List<string> Summary { get; private set; }
        internal string MainName { get; set; }
        internal string MainPath { get; set; }

        internal void SetMainFullPath(string name, string path)
        {
            name = name.Replace("\"", "").Trim().Replace("\\", separator).Replace("/", separator);
            path = path.Replace("\"", "").Trim().Replace("\\", separator).Replace("/", separator);

            MainName = GetName(name);
            MainPath = GetPath(name, path);
            //MessageBox.Show(MainPath + "   " + MainName);
        }

        private string GetName(string name)
        {
            var tmpName = name;
            var tmpStart = name.LastIndexOf(separator);
            if (tmpStart != -1 && tmpStart + 1 < name.Length)
            {
                tmpName = name.Substring(tmpStart + 1);
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

        internal List<string> GetVariables(int lineNumber, string name)
        {
            var variables = new List<string>();

            if (Map.ContainsKey(lineNumber))
            {
                if (!Map[lineNumber])
                {
                    foreach (var v in Variables)
                    {
                        if (!v.FromFunction)
                        {
                            variables.Add(v.Name);
                        }
                    }
                    // Ищем переменные по инклюдам
                    var obj = IntellisenseParser.Data;
                    GetIncludeVars(variables, Includes, obj);
                    //CommonData.Status.Clear();
                    //CommonData.Status.Add(variables.Count.ToString());
                }
                else
                {
                    foreach (var v in Variables)
                    {
                        if (v.FromFunction)
                        {
                            var start = v.Interval.Item1;
                            var end = v.Interval.Item2;

                            if (lineNumber >= start && lineNumber < end)
                            {
                                variables.Add(v.Name);
                            }
                        }
                        else
                        {
                            Type = BPType.PROGRAM;

                            if (name.IndexOf(".bpi") != -1)
                            {
                                Type = BPType.INCLUDE;
                            }
                            else if (name.IndexOf(".bpm") != -1)
                            {
                                Type = BPType.MODULE;
                            }

                            if (Type == BPType.MODULE)
                            {
                                variables.Add(v.Name);
                            }
                        }
                    }
                }
            }

            variables = variables.Distinct().ToList();
            variables.Sort();
            return variables;
        }

        internal List<string> GetSubroutines(string name)
        {
            var subs = new List<string>();

            foreach (var s in Subroutines)
            {
                subs.Add(s.Name);
            }

            // Ищем переменные по инклюдам
            var obj = IntellisenseParser.Data;
            GetIncludeSubs(subs, Includes, obj);

            subs = subs.Distinct().ToList();
            subs.Sort();
            return subs;
        }

        internal List<string> GetLabels(int lineNumber)
        {
            var labels = new List<string>();

            if (Map.ContainsKey(lineNumber))
            {
                if (!Map[lineNumber])
                {
                    foreach (var l in Labels)
                    {
                        if (!l.FromFunction)
                        {
                            labels.Add(l.Name);
                        }
                    }
                }
                else
                {
                    foreach (var l in Labels)
                    {
                        if (l.FromFunction)
                        {
                            var start = l.Interval.Item1;
                            var end = l.Interval.Item2;

                            if (lineNumber >= start && lineNumber < end)
                            {
                                labels.Add(l.Name);
                            }
                        }
                    }
                }
            }

            labels = labels.Distinct().ToList();
            labels.Sort();
            return labels;
        }

        internal List<string> GetModuleVariables (string moduleName, string fileName)
        {
            // moduleName - без расширения
            // fileName - с расширением
            moduleName += ".bpm";

            var data = new List<string>();

            if (IntellisenseParser.Data.ContainsKey(moduleName))
            {
                var map = IntellisenseParser.Data[moduleName].Map;

                //CommonData.Status.Clear();
                //CommonData.Status.Add(map.Variables.Count.ToString());
                
                if (moduleName == fileName)
                {
                    var vars = map.Variables;

                    foreach (var v in vars)
                    {
                        if (!v.FromFunction)
                        {
                            data.Add(v.Name);
                        }
                    }
                }
                else if (Imports.ContainsKey(moduleName.ToLower()))
                {
                    var vars = map.Variables;

                    foreach (var v in vars)
                    {
                        if (!v.Private && !v.FromFunction)
                        {
                            data.Add(v.Name);
                        }
                    }
                }
            }

            data = data.Distinct().ToList();
            data.Sort();
            return data;
        }

        internal List<string> GetModuleMethods(string moduleName, string fileName)
        {
            // moduleName - без расширения
            // fileName - с расширением
            moduleName += ".bpm";

            var data = new List<string>();

            if (IntellisenseParser.Data.ContainsKey(moduleName))
            {
                var map = IntellisenseParser.Data[moduleName].Map;
                if (moduleName == fileName)
                {
                    var func = map.Subroutines;

                    foreach (var f in func)
                    {
                        data.Add(f.Name);

                    }
                }
                else if (Imports.ContainsKey(moduleName.ToLower()))
                {
                    var func = map.Subroutines;

                    foreach (var f in func)
                    {
                        if (!f.Private)
                        {
                            data.Add(f.Name);
                        }
                    }
                }
            }

            data = data.Distinct().ToList();
            data.Sort();
            return data;
        }

        internal bool IsContains (Function func)
        {
            var res = false;
            
            foreach (var s in Subroutines)
            {
                if (s.Name == func.Name && s.ParamCount == func.ParamCount)
                {
                    res = true;
                }
            }

            return res;
        }

        internal void GetIncludeVars(List<string> data, Dictionary<string, Include> includes, Dictionary<string, ProgramData> obj)
        {
            var names = new List<string>();
            foreach (var inc in includes)
            {
                var tmpName = inc.Value.OriginName;

                // Чтение файла
                /*
                if (!obj.ContainsKey(tmpName))
                {
                    var path = inc.Value.Path;
                    FileInfo fi = new FileInfo(path + tmpName);
                    if (!fi.Exists)
                    {
                        return;
                    }
                    else
                    {
                        var text = File.ReadAllText(path + tmpName);
                    }
                }
                */

                if (!names.Contains(tmpName) && obj.ContainsKey(tmpName))
                {
                    names.Add(tmpName);
                    var vars = obj[tmpName].Map.Variables;

                    foreach (var v in vars)
                    {
                        if (!v.FromFunction)
                        {
                            data.Add(v.Name);
                        }
                    }
                }
            }
        }

        internal void GetIncludeSubs(List<string> data, Dictionary<string, Include> includes, Dictionary<string, ProgramData> obj)
        {
            var names = new List<string>();
            foreach (var inc in includes)
            {
                var tmpName = inc.Value.OriginName;

                if (!names.Contains(tmpName) && obj.ContainsKey(tmpName))
                {
                    names.Add(tmpName);
                    var subs = obj[tmpName].Map.Subroutines;

                    foreach (var s in subs)
                    {
                        data.Add(s.Name);
                    }
                }
            }
        }
    }
}

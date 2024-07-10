using Clever.Model.Bplus;
using Clever.Model.Bplus.BPInterpreter;
using Clever.Model.Enums;
using Clever.Model.Program;
using Clever.Model.Utils;
using Clever.ViewModel;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clever.Model.Intellisense
{
    internal static class IntellisenseParser
    {
        private static Task taskP;
        private static bool newActionP = false;
        private static bool dataAdded = false;
        internal static LineBuilder Builder;
        internal static HashSet<string> BPObjects;
        internal static HashSet<string> BPKeywords;
        internal static Dictionary<string, ProgramData> Data { get; set; }

        private static bool firstOpen = true;
        //private static int start = 0;
        //private static Stopwatch stopWatch = new Stopwatch();

        internal static void Install()
        {
            //Get = new IntellisenseParser();
            SetHashSets();
            Builder = new LineBuilder(BPObjects, BPKeywords);
            Data = new Dictionary<string, ProgramData>();
        }

        #region AsyncParser

        internal static void UpdateMap(string name)
        {
            //start++;
            //CommonData.Status.Clear();
            if (firstOpen)
            {
                CommonData.Status.Clear();
                CommonData.Status.Add("PLEASE WAIT PROJECT UPLOADED ... 0 %");
            }
                

            //CommonData.Status.Add("Parser start count: " + start.ToString());
            //stopWatch.Reset();
            //stopWatch.Start();

            if (taskP != null)
            {
                if (taskP.IsCompleted)
                {
                    newActionP = false;
                    taskP = SetVSLAsync(name);
                }
                else
                {
                    newActionP = true;
                }
            }
            else
            {
                newActionP = false;
                taskP = SetVSLAsync(name);
            }
        }

        internal static void SetVSLSync(ProgramData pData)
        {
            var map = new ProgramMap(pData.Map.Type);
            var lines = pData.Editor.TextArea.Lines;
            var path = pData.Path;

            SetVSL(path, lines, map);

            pData.Map = map;

            if (Data.ContainsKey(pData.ParseName))
            {
                Data[pData.ParseName] = pData;
            }
            else
            {
                Data.Add(pData.ParseName, pData);
            }
        }

        private static async Task SetVSLAsync(string name)
        {
            var prjData = MainWindowVM.Project.GetDictionary();           

            if (prjData.ContainsKey(name))
            {
                var pData = prjData[name];
                var map = new ProgramMap(pData.Map.Type);
                var lines = pData.Editor.TextArea.Lines;
                var path = pData.Path;

                await Task.Run(() => SetVSL(path, lines, map));

                if (firstOpen)
                {
                    CommonData.Status.Clear();
                    CommonData.Status.Add("PLEASE WAIT PROJECT UPLOADED ... 15 %");
                }


                if (map.Type == BPType.PROGRAM)
                {
                    IncludeFileOpen(map);

                    var imports = new Dictionary<string, Module>();
                    foreach (var imp in map.Imports)
                    {
                        if (!imports.ContainsKey(imp.Key))
                        {
                            imports.Add(imp.Key, imp.Value);
                        }
                    }
                    foreach (var inc in map.Includes)
                    {
                        if (Data.ContainsKey(inc.Value.OriginName))
                        {
                            foreach (var imp in Data[inc.Value.OriginName].Map.Imports)
                            {
                                if (!imports.ContainsKey(imp.Key))
                                {
                                    imports.Add(imp.Key, imp.Value);
                                }
                            }
                        }
                    }

                    map.Imports = imports;

                    ImportFileOpen(map);
                }
                else if (map.Type == BPType.MODULE)
                {
                    map.Includes.Clear();
                    var imports = new Dictionary<string, Module>();
                    foreach (var imp in map.Imports)
                    {
                        if (!imports.ContainsKey(imp.Key))
                        {
                            imports.Add(imp.Key, imp.Value);
                        }
                    }

                    map.Imports = imports;
                    ImportFileOpen(map);
                }
                else if (map.Type == BPType.INCLUDE)
                {
                    if (map.MainName != "")
                    {
                        MainFileOpen(map);
                    }

                    if (Data.ContainsKey(map.MainName))
                    {
                        var mMap = Data[map.MainName].Map;
                        var tmpName = name.ToLower();

                        if (mMap.Includes.ContainsKey(tmpName))
                        {
                            var includes = new Dictionary<string, Include>();

                            foreach (var inc in mMap.Includes)
                            {
                                if (!includes.ContainsKey(inc.Key) && inc.Key != tmpName)
                                {
                                    includes.Add(inc.Key, inc.Value);
                                }
                            }

                            map.Includes = includes;

                            IncludeFileOpen(map);


                            var imports = new Dictionary<string, Module>();

                            foreach (var imp in mMap.Imports)
                            {
                                if (!imports.ContainsKey(imp.Key))
                                {
                                    imports.Add(imp.Key, imp.Value);
                                }
                            }

                            foreach (var imp in map.Imports)
                            {
                                if (!imports.ContainsKey(imp.Key))
                                {
                                    imports.Add(imp.Key, imp.Value);
                                }
                            }

                            foreach (var inc in map.Includes)
                            {
                                if (Data.ContainsKey(inc.Value.OriginName))
                                {
                                    foreach (var imp in Data[inc.Value.OriginName].Map.Imports)
                                    {
                                        if (!imports.ContainsKey(imp.Key))
                                        {
                                            imports.Add(imp.Key, imp.Value);
                                        }
                                    }
                                }
                            }

                            map.Imports = imports;

                            ImportFileOpen(map);

                            /*
                            foreach (var v in mMap.Variables)
                            {
                                map.Variables.Add(v);
                            }

                            foreach (var s in mMap.Subroutines)
                            {
                                map.Subroutines.Add(s);
                            }

                            foreach (var l in mMap.Labels)
                            {
                                map.Labels.Add(l);
                            }
                            */
                            
                            map.Variables.AddRange(mMap.Variables);
                            map.Subroutines.AddRange(mMap.Subroutines);
                            map.Labels.AddRange(mMap.Labels);
                            
                        }
                    }
                }

                if (firstOpen)
                {
                    CommonData.Status.Clear();
                    CommonData.Status.Add("PLEASE WAIT PROJECT UPLOADED ... 95 %");
                }

                pData.Map = map;
                prjData[name] = pData;
                if (Data.ContainsKey(name))
                {
                    Data[name] = pData;
                }
                else
                {
                    Data.Add(name, pData);
                }

                //stopWatch.Stop();
                //var timer = stopWatch.ElapsedMilliseconds / 1000.0;
                //CommonData.Status.Add("Parser work time: " + timer.ToString() + " sec.");
                /*
                CommonData.Status.Add("===============================================");
                if (Data.ContainsKey(Data[name].Map.MainName))
                    CommonData.Status.Add("Main: 1");
                CommonData.Status.Add("Vars: " + Data[name].Map.Variables.Count.ToString());
                CommonData.Status.Add("Subs: " + Data[name].Map.Subroutines.Count.ToString());
                CommonData.Status.Add("Labels: " + Data[name].Map.Labels.Count.ToString());
                CommonData.Status.Add("Include: " + Data[name].Map.Includes.Count.ToString());
                CommonData.Status.Add("Import: " + Data[name].Map.Imports.Count.ToString());
                CommonData.Status.Add("===============================================");
                CommonData.Status.Add("DATA COUNT: " + Data.Count.ToString());
                */
                if (firstOpen)
                {
                    CommonData.Status.Clear();
                    CommonData.Status.Add("PLEASE WAIT PROJECT UPLOADED ... 100 %");
                    CommonData.Status.Add("PROJECT IS LOADED AND READY.");
                    firstOpen = false;
                }
            }    
        }

        private static void SetVSL(string path, LineCollection lines, ProgramMap map)
        {
            var func = false;
            var priv = false;
            var modSummary = false;
            var start = -1;
            var end = -1;
            map.Map.Clear();
            map.Variables.Clear();
            map.Includes.Clear();
            map.Imports.Clear();
            map.Subroutines.Clear();

            int j = 0;

            var tmpSummary = new List<string>();

            foreach (var line in lines)
            {
                j++;
                var txt = line.Text.Trim();

                if (j == 1 && map.Type == BPType.INCLUDE)
                {
                    if (txt.IndexOf("'") == 0 && txt.IndexOf("''''") == -1)
                    {
                        var w = txt.Replace("'", "").Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                        if (w.Length == 2 && (w[0].ToLower() == "#main"))// || w[0].ToLower() == "main"))
                        {
                            map.SetMainFullPath(w[1].Replace(".bp", "") + ".bp", path);
                        }
                    }
                }
                
                var words = Builder.GetWords(line.Text.Trim());

                if (txt.IndexOf("''''") == 0)
                {
                    tmpSummary.Add(txt.Replace("''''", "").Trim());
                    if (j == 1)
                    {
                        modSummary = true;
                    }
                }

                else if (words.Count >= 1)
                {
                    var first = words[0].Text.ToLower();
                    var firstToc = words[0].Token;

                    if (words.Count == 2)
                    {
                        if (first == "include")
                        {
                            var include = new Include(words[1].OriginText, path);

                            if (!map.Includes.ContainsKey(include.Name) && map.Type == BPType.PROGRAM)
                                map.Includes.Add(include.Name, include);

                            tmpSummary.Clear();
                        }
                        else if (first == "import")
                        {
                            var module = new Module(words[1].OriginText, path);

                            if (!map.Imports.ContainsKey(module.Name))
                                map.Imports.Add(module.Name, module);

                            tmpSummary.Clear();
                        }
                    }

                    if (first == "function")
                    {
                        func = true;
                        start = line.DisplayIndex;
                        if (end == -1)
                        {
                            for (int i = start - 1; i < lines.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(lines[i].Text))
                                {
                                    var text = lines[i].Text.Trim().ToLower();
                                    if (text == "endfunction")
                                    {
                                        end = lines[i].DisplayIndex;
                                        break;
                                    }
                                }

                                if (i == lines.Count - 1)
                                {
                                    end = lines[i].DisplayIndex;
                                }
                            }
                        }
                    }
                    else if (first == "endfunction")
                    {
                        func = false;
                        start = -1;
                        end = -1;
                        tmpSummary.Clear();
                    }
                    else if (first == "private") //&& map.Type == BPType.MODULE)
                    {
                        priv = true;
                        tmpSummary.Clear();
                    }
                    else if (txt.IndexOf("''''") == -1)
                    {

                        if (first != "number" && first != "number[]" && first != "string" && first != "string[]" && first != "sub")
                            tmpSummary.Clear();
                    }
                }
                else
                {
                    if (tmpSummary.Count > 0 && modSummary)
                    {
                        foreach (var l in tmpSummary)
                        {
                            map.Summary.Add(l);
                        }
                        modSummary = false;
                    }


                    tmpSummary.Clear();
                }

                map.Map.Add(line.DisplayIndex, func);

                foreach (var word in words)
                {
                    if (words.Count > 1 && (words[0].Text == "function" || words[0].Text == "sub") && (word.Token == Tokens.SUBNAME || word.Token == Tokens.FUNCNAME))
                    {
                        var count = GetParamCount(words);
                        var tmpF = new Function(word.OriginText, priv, count);
                        if (tmpSummary.Count > 0)
                        {
                            tmpF.Summary.Add(line.Text.Replace(words[0].OriginText, "").Trim());// + '\n');
                            foreach (var l in tmpSummary)
                            {
                                tmpF.Summary.Add(l);
                            }
                            tmpSummary.Clear();
                        }

                        if (!map.IsContains(tmpF))
                        {
                            map.Subroutines.Add(tmpF);
                        }
                    }
                    else if (word.Token == Tokens.LABELNAME)
                    {
                        var tmpV = new Label(word.OriginText.Replace(":",""), priv, func, start, end);
                        map.Labels.Add(tmpV);
                    }
                    else if (word.Token == Tokens.VARIABLE)
                    {
                        var tmpV = new Variable(word.OriginText, priv, func, start, end);
                        map.Variables.Add(tmpV);

                        if (tmpSummary.Count > 0)
                        {
                            var first = words[0].Text.Trim().ToLower();
                            if (first == "number" || first == "number[]" || first == "string" || first == "string[]")
                            {
                                tmpV.Summary.Add(line.Text.Replace(words[0].OriginText, "").Trim());// + '\n');
                                foreach (var l in tmpSummary)
                                {
                                    tmpV.Summary.Add(l);
                                }
                                tmpSummary.Clear();
                            }
                        }
                    }
                }
            }
        }

        private static int GetParamCount(List<Word> words)
        {
            var bracket = 0;
            var comma = 0;

            if (words.Count > 0)
            {
                var firstWord = words[0].Text.ToLower();
                if (firstWord == "sub" || firstWord == "thread.run") //|| firstWord == "f.start")
                {
                    comma = -1;
                }
            }

            foreach (var word in words)
            {
                if (word.Token == Tokens.DOUBLEBRACKET && bracket == 0)
                {
                    comma = -1;
                    break;
                }

                if (word.Token == Tokens.BRACKETLEFT)
                {
                    bracket++;
                }
                else if (word.Token == Tokens.BRACKETRIGHT)
                {
                    bracket--;
                }

                if (word.Token == Tokens.COMMA && bracket == 1)
                {
                    comma++;
                }
            }

            if (comma >= 0)
            {
                return comma + 1;
            }

            return 0;
        }

        #endregion

        #region Read Files

        internal static void MainFileOpen(ProgramMap map)
        {
            var name = map.MainName;
            var path = map.MainPath + name;

            if (File.Exists(path))
            {
                if (Data.ContainsKey(name))
                {
                    if (path != Data[name].FullPath)
                    {
                        Data.Remove(name);
                    }
                }

                if (!Data.ContainsKey(name))
                {
                    var text = File.ReadAllText(path);
                    var item = new NewTabItem().Create(text, name);
                    var pd = new ProgramData();
                    pd.ParseName = name;
                    pd.ClosedName = name;
                    pd.OldName = name;
                    pd.Name = name;
                    pd.AddToObjects = false;
                    pd.Item = item;
                    pd.Path = map.MainPath;
                    pd.FullPath = path;
                    pd.TextChange = false;
                    SetVSLSync(pd);
                }
            }
            else
            {
                if (Data.ContainsKey(name))
                {
                    Data.Remove(name);
                }

                map.MainName = "";
            }
        }

        internal static void IncludeFileOpen(ProgramMap map)
        {
            var stopList = new HashSet<string>();

            foreach (var inc in map.Includes)
            {
                var name = inc.Value.OriginName;
                var path = inc.Value.Path + name;

                if (File.Exists(path))
                {
                    if (Data.ContainsKey(name))
                    {
                        if (path != Data[name].FullPath)
                        {
                            Data.Remove(name);
                        }
                    }

                    if (!Data.ContainsKey(name))
                    {
                        var text = File.ReadAllText(path);
                        var item = new NewTabItem().Create(text, name);
                        var pd = new ProgramData();
                        pd.ParseName = name;
                        pd.ClosedName = name;
                        pd.OldName = name;
                        pd.Name = name;
                        pd.AddToObjects = false;
                        pd.Item = item;
                        pd.Path = inc.Value.Path;
                        pd.FullPath = path;
                        pd.TextChange = false;                        
                        SetVSLSync(pd);
                    }
                }
                else
                {
                    if (Data.ContainsKey(name))
                    {
                        Data.Remove(name);
                    }

                    stopList.Add(inc.Key);
                }
            }

            foreach (var inc in stopList)
            {
                map.Includes.Remove(inc);
            }
        }

        internal static void ImportFileOpen(ProgramMap map)
        {
            var stopList = new HashSet<string>();

            foreach (var imp in map.Imports)
            {
                var name = imp.Value.OriginName;
                var path = imp.Value.Path + name;

                if (File.Exists(path))
                {
                    if (Data.ContainsKey(name))
                    {
                        if (path != Data[name].FullPath)
                        {
                            Data.Remove(name);
                        }
                    }

                    if (!Data.ContainsKey(name))
                    {
                        var text = File.ReadAllText(path);
                        var item = new NewTabItem().Create(text, name);
                        var pd = new ProgramData();
                        pd.ParseName = name;
                        pd.ClosedName = name;
                        pd.OldName = name;
                        pd.Name = name;
                        pd.AddToObjects = false;
                        pd.Item = item;
                        pd.Path = imp.Value.Path;
                        pd.FullPath = path;
                        pd.TextChange = false;                       
                        SetVSLSync(pd);
                    }
                }
                else
                {
                    if (Data.ContainsKey(name))
                    {
                        Data.Remove(name);
                    }

                    stopList.Add(imp.Key);
                }
            }

            foreach (var imp in stopList)
            {
                map.Imports.Remove(imp);
            }
        }

        #endregion

        #region Configuration

        private static void SetHashSets()
        {
            BPKeywords = new HashSet<string>()
            {
                "for",
                "endfor",
                "if",
                "then",
                "endif",
                "else",
                "elseif",
                "while",
                "endwhile",
                "and",
                "or",
                "sub",
                "endsub",
                "goto",
                "step",
                "to",
                "import",
                "include",
                "folder",
                "in",
                "out",
                "function",
                "endfunction",
                "number",
                "number[]",
                "string",
                "string[]",
                "region",
                "endregion",
                "private",
                "break",
                "continue",
                "return"
            };

            BPObjects = new HashSet<string>()
            {
                "assert",
                "buttons",
                "byte",
                "ev3",
                "ev3file",
                "lcd",
                "mailbox",
                "math",
                "motor",
                "motora",
                "motorab",
                "motorac",
                "motorad",
                "motorb",
                "motorbc",
                "motorbd",
                "motorc",
                "motorcd",
                "motord",
                "program",
                "row",
                "sensor",
                "sensor1",
                "sensor2",
                "sensor3",
                "sensor4",
                "speaker",
                "text",
                "thread",
                "time",
                "vector"
            };
        }

        #endregion
    }
}

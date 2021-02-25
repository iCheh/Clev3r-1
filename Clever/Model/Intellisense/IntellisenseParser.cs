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
    internal class IntellisenseParser
    {
        private Task taskP;
        private bool newActionP = false;
        private bool dataAdded = false;
        internal LineBuilder Builder;
        internal HashSet<string> BPObjects;
        internal HashSet<string> BPKeywords;
        internal Dictionary<string, ProgramData> Data;
        //private int start = 0;
        //Stopwatch stopWatch = new Stopwatch();


        internal static IntellisenseParser Get { get; private set; }

        internal static void Install()
        {
            Get = new IntellisenseParser();
            Get.SetHashSets();
            Get.Builder = new LineBuilder(Get.BPObjects, Get.BPKeywords);
            Get.Data = new Dictionary<string, ProgramData>();
        }

        #region AsyncParser

        internal void UpdateMap(ProgramData data)
        {
            if (taskP != null)
            {
                if (taskP.IsCompleted)
                {
                    newActionP = false;
                    taskP = SetVSLAsync(data);
                }
                else
                {
                    newActionP = true;
                }
            }
            else
            {
                newActionP = false;
                taskP = SetVSLAsync(data);
            }
        }

        internal void SetVSLSync(ProgramData pData, Dictionary<string, ProgramData> data)
        {
            var firstType = BPType.PROGRAM;
            if (pData.ParseName.IndexOf(".bpi") != -1)
            {
                firstType = BPType.INCLUDE;
            }
            else if (pData.ParseName.IndexOf(".bpm") != -1)
            {
                firstType = BPType.MODULE;
            }

            pData.Map.Type = firstType;

            SetVSL(pData, pData.Map, data);            
        }

        private async Task SetVSLAsync(ProgramData pData)
        {
            //start++;
            //CommonData.Status.Clear();
            //CommonData.Status.Add("Parser start count: " + start.ToString());
            //stopWatch.Reset();
            //stopWatch.Start();
            var prjData = MainWindowVM.Project.GetDictionary();
            var firstType = BPType.PROGRAM;
            if (pData.ParseName.IndexOf(".bpi") != -1)
            {
                firstType = BPType.INCLUDE;
            }
            else if (pData.ParseName.IndexOf(".bpm") != -1)
            {
                firstType = BPType.MODULE;
            }
            var map = new ProgramMap(firstType);
            var data = new Dictionary<string, ProgramData>();
            foreach(var d in Get.Data)
            {
                data.Add(d.Key, d.Value);
            }
            if (!data.ContainsKey(pData.ParseName))
            {
                data.Add(pData.ParseName, pData);
            }
            else
            {
                data[pData.ParseName] = pData;
            }

            await Task.Run(() => SetVSL(pData, map, data));

            if (map.Type == BPType.PROGRAM)
            {
                var incNames = new HashSet<string>();
                OpenFile.IncludeFileOpen(map, data, incNames);
                foreach (var incName in incNames)
                {
                    if (data.ContainsKey(incName))
                    {
                        var incMap = data[incName].Map;
                        foreach (var imp in incMap.Imports)
                        {
                            if (!map.Imports.ContainsKey(imp.Key))
                            {
                                map.Imports.Add(imp.Key, imp.Value);
                            }
                        }
                    }
                }
                var impNames = new HashSet<string>();
                OpenFile.ImportFileOpen(map, data, impNames);
            }
            else if (map.Type == BPType.MODULE)
            {
                var impNames = new HashSet<string>();
                OpenFile.ImportFileOpen(map, data, impNames);
            }
            else if (map.Type == BPType.INCLUDE)
            {
                if (map.MainName != "")
                {
                    if (!data.ContainsKey(map.MainName))
                    {
                        OpenFile.MainFileOpen(map, data);
                    }

                    if (data.ContainsKey(map.MainName))
                    {
                        if (data[map.MainName].Map.Includes.ContainsKey(pData.ParseName.ToLower()))
                        {
                            map.Variables.AddRange(data[map.MainName].Map.Variables);
                            map.Subroutines.AddRange(data[map.MainName].Map.Subroutines);
                            map.Labels.AddRange(data[map.MainName].Map.Labels);

                            map.Includes = data[map.MainName].Map.Includes;

                            var imports = data[map.MainName].Map.Imports;
                            foreach (var imp in imports)
                            {
                                if (!map.Imports.ContainsKey(imp.Key))
                                {
                                    map.Imports.Add(imp.Key, imp.Value);
                                }
                            }
                            var incNames = new HashSet<string>();
                            OpenFile.IncludeFileOpen(map, data, incNames);

                            foreach (var incName in incNames)
                            {
                                if (data.ContainsKey(incName))
                                {
                                    var incMap = data[incName].Map;
                                    foreach (var imp in incMap.Imports)
                                    {
                                        if (!map.Imports.ContainsKey(imp.Key))
                                        {
                                            map.Imports.Add(imp.Key, imp.Value);
                                        }
                                    }
                                }
                            }

                            var impNames = new HashSet<string>();

                            OpenFile.ImportFileOpen(map, data, impNames);
                        }
                    }
                }
            }

            pData.Map = map;
            Data.Clear();
            Data = data;

            //stopWatch.Stop();
            //var timer = stopWatch.ElapsedMilliseconds / 1000.0;
            //CommonData.Status.Add("Parser work time: " + timer.ToString() + " sec.");           
        }

        private void SetVSL(ProgramData pData, ProgramMap map, Dictionary<string, ProgramData> data)
        {
            var lines = pData.Editor.TextArea.Lines;
            var path = pData.Path;
            var name = pData.ParseName;
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
                
                var words = Get.Builder.GetWords(line.Text.Trim());

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
                        var tmpV = new Label(word.OriginText, priv, func, start, end);
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

        private int GetParamCount(List<Word> words)
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

        #region Configuration

        private void SetHashSets()
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
                "private"
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

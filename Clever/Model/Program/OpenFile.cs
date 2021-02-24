using Clever.Model.Bplus;
using Clever.Model.Bplus.BPInterpreter;
using Clever.Model.Intellisense;
using Clever.Model.Utils;
using Clever.ViewModel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Clever.Model.Program
{
    class OpenFile
    {
        public string Open(string path, out string fullPath, out string directory, out string name)
        {
            string text = "";
            FileInfo info;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Basic plus file (*" + MainWindowVM.ext + ";*.bpm;*.bpi)|*" + MainWindowVM.ext + ";*.bpm;*.bpi";
            if (open.ShowDialog() == true)
            {
                text = File.ReadAllText(open.FileName);
                info = new FileInfo(open.FileName);
                fullPath = info.FullName;
                directory = info.DirectoryName;
                name = info.Name;
            }
            else
            {
                text = null;
                fullPath = null;
                directory = null;
                name = null;
            }
            return text;
        }

        internal static void FileOpen(ProgramMap map, Dictionary<string, ProgramData> data, HashSet<string> namesM, HashSet<string> namesI)
        {
            var imports = new Dictionary<string, Module>();
            var includes = new Dictionary<string, Include>();

            foreach (var imp in map.Imports)
            {
                if (!namesM.Contains(imp.Value.OriginName))
                {
                    namesM.Add(imp.Value.OriginName);
                    imports.Add(imp.Key, imp.Value);
                }
            }

            foreach (var inc in map.Includes)
            {
                if (!namesI.Contains(inc.Value.OriginName))
                {
                    namesI.Add(inc.Value.OriginName);
                    includes.Add(inc.Key, inc.Value);
                }
            }

            foreach (var inc in includes)
            {
                var name = inc.Value.OriginName;
                var path = inc.Value.Path + name;
                var contains = data.ContainsKey(name);

                if (File.Exists(path))
                {
                    if (contains)
                    {
                        if (path  != data[name].FullPath)
                        {
                            data.Remove(name);
                        }
                    }

                    if (!data.ContainsKey(name))
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
                        data.Add(name, pd);
                        IntellisenseParser.Get.SetVSLSync(pd, data);
                    }
                }
                else
                {
                    if (contains)
                    {
                        data.Remove(name);
                    }
                }
                
            }

            foreach (var imp in imports)
            {
                var name = imp.Value.OriginName;
                var path = imp.Value.Path + name;
                var contains = data.ContainsKey(name);

                if (File.Exists(path))
                {
                    if (contains)
                    {
                        if (path != data[name].FullPath)
                        {
                            data.Remove(name);
                        }
                    }

                    if (!data.ContainsKey(name))
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
                        data.Add(name, pd);
                        IntellisenseParser.Get.SetVSLSync(pd, data);
                        FileOpen(pd.Map, data, namesM, namesI);
                    }
                }
                else
                {
                    if (contains)
                    {
                        data.Remove(name);
                    }
                }
            }  
        }

        internal static void IncludeFileOpen(ProgramMap map, Dictionary<string, ProgramData> data, HashSet<string> namesI)
        {
            var includes = new Dictionary<string, Include>();

            foreach (var inc in map.Includes)
            {
                if (!namesI.Contains(inc.Value.OriginName))
                {
                    namesI.Add(inc.Value.OriginName);
                    includes.Add(inc.Key, inc.Value);
                }
            }

            foreach (var inc in includes)
            {
                var name = inc.Value.OriginName;
                var path = inc.Value.Path + name;
                var contains = data.ContainsKey(name);

                if (File.Exists(path))
                {
                    if (contains)
                    {
                        if (path != data[name].FullPath)
                        {
                            data.Remove(name);
                        }
                    }

                    if (!data.ContainsKey(name))
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
                        data.Add(name, pd);
                        IntellisenseParser.Get.SetVSLSync(pd, data);
                    }
                }
                else
                {
                    if (contains)
                    {
                        data.Remove(name);
                    }
                }

            }
        }

        internal static void ImportFileOpen(ProgramMap map, Dictionary<string, ProgramData> data, HashSet<string> namesM)
        {
            var imports = new Dictionary<string, Module>();

            foreach (var imp in map.Imports)
            {
                if (!namesM.Contains(imp.Value.OriginName))
                {
                    namesM.Add(imp.Value.OriginName);
                    imports.Add(imp.Key, imp.Value);
                }
            }

            foreach (var imp in imports)
            {
                var name = imp.Value.OriginName;
                var path = imp.Value.Path + name;
                var contains = data.ContainsKey(name);

                if (File.Exists(path))
                {
                    if (contains)
                    {
                        if (path != data[name].FullPath)
                        {
                            data.Remove(name);
                        }
                    }

                    if (!data.ContainsKey(name))
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
                        data.Add(name, pd);
                        IntellisenseParser.Get.SetVSLSync(pd, data);
                        ImportFileOpen(pd.Map, data, namesM);
                    }
                }
                else
                {
                    if (contains)
                    {
                        data.Remove(name);
                    }
                }
            }
        }
    }
}

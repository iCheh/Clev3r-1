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

        internal static void FileOpen(ProgramMap map, Dictionary<string, ProgramData> data, HashSet<string> names)
        {
            var imports = new Dictionary<string, Module>();
            foreach (var imp in map.Imports)
            {
                if (!names.Contains(imp.Value.OriginName))
                {
                    names.Add(imp.Value.OriginName);
                    imports.Add(imp.Key, imp.Value);
                }
            }
            var includes = map.Includes;

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
                        FileOpen(pd.Map, data, names);
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

            if (map.Type == Enums.BPType.INCLUDE)
            {
                var mainName = map.MainName;
                var mainPath = map.MainPath + mainName;
                var contains = data.ContainsKey(mainName);

                if (File.Exists(mainPath))
                {
                    
                    if (contains)
                    {
                        //MessageBox.Show(mainPath + " " + data[mainName].FullPath);
                        if (mainPath != data[mainName].FullPath)
                        {
                            data.Remove(mainName);
                        }
                    }

                    if (!data.ContainsKey(mainName))
                    {
                        //MessageBox.Show("OK");
                        var text = File.ReadAllText(mainPath);
                        var item = new NewTabItem().Create(text, mainName);
                        var pd = new ProgramData();
                        pd.ParseName = mainName;
                        pd.ClosedName = mainName;
                        pd.OldName = mainName;
                        pd.Name = mainName;
                        pd.AddToObjects = false;
                        pd.Item = item;
                        pd.Path = map.MainPath;
                        pd.FullPath = mainPath;
                        pd.TextChange = false;
                        data.Add(mainName, pd);
                        IntellisenseParser.Get.SetVSLSync(pd, data);
                    }
                }
                else
                {
                    if (contains)
                    {
                        data.Remove(mainName);
                    }
                }
            }
        }
    }
}

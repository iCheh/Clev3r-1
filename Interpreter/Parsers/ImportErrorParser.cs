using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using Interpreter.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class ImportErrorParser
    {
        private static string separator = Path.DirectorySeparatorChar.ToString();
        private static string newCallPath = "";

        internal static void Start (Line line, string callPath)
        {
            newCallPath = callPath;
            if (line.Count == 1)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 2002, ""));
            }
            else if (line.Count > 2 || line.Words.Count <= 0)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 2003, ""));
            }
            else if (line.Words[1].Token != Enums.Tokens.STRING)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 2004, ""));
            }
            else
            {
                //var importFullPath = line.Words[1].OriginText.Replace("\"","");

                var importFullPath = CreateFullPath(line.Words[1].OriginText.Replace("\"", ""), newCallPath);

                /*
                if (importFullPath.IndexOf("..\\") != -1)
                {
                    importFullPath = importFullPath.Replace("..\\", Data.Project.ModuleLibPath);
                }
                else if (importFullPath.IndexOf("../") != -1)
                {
                    importFullPath = importFullPath.Replace("../", Data.Project.ModuleLibPath);
                }
                else
                {
                    importFullPath = Data.Project.Path + line.Words[1].Text.Replace("\"", "").Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
                }
                */
                string name = GetImportName(importFullPath);
                string path = GetImportPath(importFullPath);
                newCallPath = path;
                

                string tmpPath = path + name + Extension.BPModule;

                Console.WriteLine(tmpPath);

                FileInfo fi = new FileInfo(tmpPath);
                if (!fi.Exists)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 2001, importFullPath + Extension.BPModule));
                    return;
                }

                if (!Data.Project.Modules.ContainsKey(name.ToLower()))
                {
                    var tmpText = File.ReadAllLines(tmpPath);
                    var lines = GetAllLines(tmpPath, tmpText);
                    if (Data.Errors.Count > 0)
                        return;

                    var tmpLines = new List<Line>();
                    foreach (var l in lines)
                    {
                        if (l.Type != LineType.IMPORT && l.Type != LineType.EMPTY)
                        {
                            tmpLines.Add(l);
                        }
                    }

                    ModuleErrorParser.Start(tmpLines);
                    if (Data.Errors.Count > 0)
                        return;

                    var import = new Module(name, path, tmpLines, tmpText.ToList());
                    // Парсим все объявление свойств
                    ParseModulePropertys(import);

                    // Пропарсим все имена функций
                    ParseModule(import);

                    // Переименуем вызовы собственных методов модуля в вид ИмяМодуля.ИмяМетода
                    // Также переимениуем все вызовы свойст в вид имямодуля_имясвойства
                    // Словам свойств временно назвачим токен MODULEPROPERTY для последующего корректного
                    // переименования переменных
                    RenameModleMethodsAndPropertys(import);
                    Data.Project.Modules.Add(name.ToLower(), import);

                    foreach (var l in lines)
                    {
                        if (l.Type == LineType.IMPORT)
                        {
                            Start(l, newCallPath);
                            if (Data.Errors.Count > 0)
                                return;
                        }
                    }
                } 
            }
        }

        private static string GetImportPath(string word)
        {
            var fi = new FileInfo(word + Extension.BPModule);
            if (fi.Exists)
            {
                return fi.DirectoryName + separator;
            }

            return "";
        }

        private static string GetImportName(string word)
        {
            var fi = new FileInfo(word + Extension.BPModule);

            if (fi.Exists)
            {
                return fi.Name.Replace(Extension.BPModule, "");
            }
            return "";
        }

        private static List<Line> GetAllLines(string fullName, string[] tmpText)
        {
            var lines = new List<Line>();
            var propertys = new List<string>();
            
            string moduleName = GetImportName(fullName.Replace(Extension.BPModule,""));
            bool func = false;
            int funcNum = 0;
            for (int i = 0; i < tmpText.Length; i++)
            {
                var tmpLine = new Line(LineBuilder.GetWords(tmpText[i]), tmpText[i]);
                tmpLine.Number = i + 1;
                //tmpLine.FileName = moduleName + Extension.BPModule;
                tmpLine.FileName = fullName;
                tmpLine.Type = LineBuilder.GetType(tmpLine);

                if (tmpLine.Type == LineType.INCLUDE)
                {
                    Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2005, ""));
                    return lines;
                }
                else if (tmpLine.Type == LineType.FOLDER)
                {
                    Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2006, ""));
                    return lines;
                }
                else if (tmpLine.Type == LineType.FUNCINIT)
                {
                    if (func)
                    {
                        Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 1026, ""));
                        return lines;
                    }
                    else
                    {
                        func = true;
                        funcNum = tmpLine.Number;
                    }
                }
                else if (tmpLine.Type == LineType.ONEKEYWORD && tmpLine.Words[0].ToLower() == "endfunction")
                {
                    if (!func)
                    {
                        Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 1027, ""));
                        return lines;
                    }
                    else
                    {
                        func = false;
                    }
                }
                else if (tmpLine.Type == LineType.SUBINIT)
                {
                    Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2007, ""));
                    return lines;
                }
                else if (tmpLine.Type == LineType.NUMBERINIT || tmpLine.Type == LineType.NUMBERARRAYINIT || tmpLine.Type == LineType.STRINGINIT || tmpLine.Type == LineType.STRINGARRAYINIT)
                {
                    tmpLine.Type = LineType.MODULEPROPERTY;
                    /*
                    Console.Write("\n");
                    Console.WriteLine("===>   " + tmpLine.NewLine);
                    string str = "";
                    foreach (var w in tmpLine.Words)
                    {
                        str += w.Token.ToString() + " ";
                    }
                    Console.WriteLine(str);
                    Console.Write("\n");
                    */
                    if (func)
                    {
                        Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2015, ""));
                        return lines;
                    }

                    if (tmpLine.Count != 2)
                    {
                        // Ошибка
                        Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2011, ""));
                        return lines;
                    }
                    else if (tmpLine.Words[0].Token != Tokens.KEYWORD && tmpLine.Words[0].Token != Tokens.VARIABLE)
                    {
                        // Ошибка
                        Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2012, ""));
                        return lines;
                    }
                    else if (tmpLine.Words[0].ToLower() != "number" && tmpLine.Words[0].ToLower() != "number[]" && tmpLine.Words[0].ToLower() != "string" && tmpLine.Words[0].ToLower() != "string[]")
                    {
                        // Ошибка
                        Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2013, ""));
                        return lines;
                    }
                    else if (propertys.Contains(tmpLine.Words[1].ToLower()))
                    {
                        // Ошибка
                        Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2014, tmpLine.Words[1].OriginText));
                        return lines;
                    }
                    else
                    {
                        propertys.Add(tmpLine.Words[1].ToLower());
                    }
                }
                else if (!func && tmpLine.Type != LineType.FUNCINIT)
                {
                    if (tmpLine.Type != LineType.EMPTY && tmpLine.Type != LineType.IMPORT && tmpLine.NewLine.Trim().IndexOf("'") != 0 && tmpLine.Type != LineType.ONEKEYWORD)
                    {
                        Data.Errors.Add(new Errore(tmpLine.Number, moduleName + Extension.BPModule, 2008, ""));
                        return lines;
                    }
                }

                // Парсим ошибки на количество скобок в строке
                BracketErrorParser.Start(tmpLine);
                if (Data.Errors.Count > 0)
                    return lines;

                lines.Add(tmpLine);
            }

            if (func)
            {
                Data.Errors.Add(new Errore(funcNum, moduleName + Extension.BPModule, 1028, ""));
                return lines;
            }

            return lines;
        }

        private static void ParseModulePropertys(Module module)
        {
            var lines = module.Lines;
            var privateLines = false;

            foreach (var line in lines)
            {
                if (line.Type == LineType.ONEKEYWORD && line.Words[0].ToLower() == "private")
                {
                    privateLines = true;
                }

                if (line.Type == LineType.MODULEPROPERTY && line.Count > 1)
                {
                    var name = module.Name.ToLower() + "_" + line.Words[1].ToLower();
                    //Console.WriteLine(name);
                    if (!module.Propertys.ContainsKey(name))
                    {
                        module.Propertys.Add(name, (line, privateLines));
                    }
                }
            }
        }

        private static void ParseModule(Module module)
        {
            var lines = module.Lines;
            var privateLines = false;

            int start = -1;
            int end = -1;
            string name = "";

            foreach (var line in lines)
            {
                if (line.Type == LineType.ONEKEYWORD && line.Words[0].ToLower() == "private")
                {
                    privateLines = true;
                }

                if (line.Type == LineType.FUNCINIT)
                {
                    // Проверим, что в инициализации функции нет имён свойств модуля

                    ModuleErrorParser.ParsePropertyInFuncInit(line, module.Propertys, module.Name);
                    if (Data.Errors.Count > 0)
                        return;

                    start = line.Number;
                    if (line.Words.Count > 1)
                    {
                        name = line.Words[1].ToLower() + "_" + GetParamCount(line).ToString();
                    }
                }
                else if (line.Type == LineType.ONEKEYWORD && line.Words[0].ToLower() == "endfunction")
                {
                    end = line.Number;
                }

                if (start > 0 && end > 0 && name != "")
                {
                    if (!module.Methods.ContainsKey(name))
                    {
                        var mLines = new List<Line>();
                        foreach (var l in lines)
                        {
                            if (l.Number >= start && l.Number <= end)
                            {
                                mLines.Add(l);
                            }
                            else if (l.Number > end)
                            {
                                break;
                            }
                        }
                        module.Methods.Add(name, (mLines, privateLines));
                        //Console.WriteLine(name);
                    }
                    start = -1;
                    end = -1;
                    name = "";
                }
            }
        }

        private static void RenameModleMethodsAndPropertys(Module module)
        {
            var lines = module.Lines;
            
            foreach (var line in lines)
            {
                // Проверим во всех словах обращения к свойствам модуля и переименуем их
                if (line.Count > 0)
                {
                    foreach (var word in line.Words)
                    {
                        if (word.Token == Tokens.VARIABLE)
                        {
                            var name = module.Name.ToLower() + "_" + word.ToLower();

                            if (module.Propertys.ContainsKey(name))
                            {
                                word.Token = Tokens.MODULEPROPERTY;
                                word.Text = name;
                            }
                        }
                        else if (word.Token == Tokens.MODULEPROPERTY)
                        {
                            var tmpText = word.ToLower().Replace(".","_");
                            word.Text = tmpText;
                        }
                    }
                }                

                if (line.Type == LineType.SUBCALL || line.Type == LineType.FUNCCALL)
                {
                    line.Type = LineType.MODULEMETHODCALL;
                    var words = line.Words;
                    var tmp = new List<string>();

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.SUBNAME || word.Token == Tokens.FUNCNAME)
                        {
                            word.Token = Tokens.MODULEMETHOD;
                            word.Text = module.Name + "." + word.Text;
                        }
                        tmp.Add(word.Text);
                    }
                    line.NewLine = string.Join(" ", tmp);
                }
                else if (line.Type == LineType.FUNCINIT)
                {
                    var words = line.Words;
                    var tmp = new List<string>();

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.FUNCNAME)
                        {
                            word.Text = module.Name + "_" + word.Text;
                        }
                        tmp.Add(word.Text);
                    }
                    line.NewLine = string.Join(" ", tmp);
                }
            }
        }

        private static int GetParamCount(Line line)
        {
            var bracket = 0;
            var comma = 0;

            if (line.Words.Count > 0)
            {
                var firstWord = line.Words[0].ToLower();
                if (firstWord == "sub" || firstWord == "thread.run" || firstWord == "f.start")
                {
                    comma = -1;
                }
            }

            foreach (var word in line.Words)
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
                fullPath = Data.Project.ModuleLibPath + name.Replace("Clev3r:" + separator + separator, "Lib" + separator + "Modules" + separator);
                return (fullPath).Replace(separator + separator, separator);
            }
            else if (name.IndexOf("Clever:" + separator + separator) == 0)
            {
                fullPath = Data.Project.ModuleLibPath + name.Replace("Clever:" + separator + separator, "Lib" + separator + "Modules" + separator);
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
    }
}

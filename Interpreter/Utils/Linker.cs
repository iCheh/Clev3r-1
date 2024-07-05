using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using Interpreter.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Utils
{
    internal static class Linker
    {
        private static Dictionary<string, HashSet<string>> _modulesCalling;
        //private static Dictionary<string, HashSet<string>> _propertysCalling;
        private static Dictionary<string, Line> _propertys;

        internal static void Start()
        {
            Data.Project.MainText.Clear();

            // Добавим в MAIN все включения INCLUDE по своим местам
            AddIncludesToMain();
            if (Data.Errors.Count > 0)
                return;
            
            // Пробуем парсить методы и свойства на приватность
            ParsePrivate();

            // Парсим вызовы методов модулей в тексте майн
            ParseModuleMethodsInMain();
            if (Data.Errors.Count > 0)
                return;

            // Парсим вызовы из майн (в созданном словаре вызовов методов) ищем их в модулях и добовляем в программу
            var tmpCalling = new Dictionary<string, HashSet<string>>();
            ParseModuleMethodsInModules(_modulesCalling, tmpCalling);
            if (Data.Errors.Count > 0)
                return;

            // Переименуем все вызовы методов модулей и имена модулей в вид суфикс_имямодуля_имяметода_параметры
            FuncRename();
            if (Data.Errors.Count > 0)
                return;

            // Переименуем все переменные
            // заодно переименуем все вызываемые свойства модулей
            // и вернём их словам токен VARIABLE
            // так же в словарь _propertys добавим все используемые свойства
            VarsAndLabelsRename();
            if (Data.Errors.Count > 0)
                return;

            // Вынесем все функции из MAIN в отдельный лист
            RemoveMainFunc();
            if (Data.Errors.Count > 0)
                return;

            // Вынесем все процедуры из MAIN в отдельный лист
            RemoveMainSub();
            if (Data.Errors.Count > 0)
                return;

            // Теперь имеем в Data.Project четыре листа с Line
            // 1 - лист программы MAIN
            // 2 - лист функций из программы MAIN
            // 3 - лист процедур из программы MAIN
            // 4 - лист используемых методов из модулей
            // 5 - лист используемых свойств

            // Заполним словарь функциями из MAIN
            CreateFunctionsDicionary(Data.Project.MainFuncText);
            // Заполним словарь функциями используемых методов модулей
            CreateFunctionsDicionary(Data.Project.ModuleMethodsText);
            // Заполним словарь процедурами из MAIN
            CreateSubsDicionary(Data.Project.MainSubText);
            // Заполним словарь свойствами модулей
            CreateCallingPropertyLines();
        }

        private static string GetModuleName(string text)
        {
            var name = "";
            if (text.IndexOf('.') != -1)
            {
                name = text.Substring(0, text.IndexOf('.'));
            }
            return name;
        }

        private static string GetMethodName(string text)
        {
            var name = "";
            if (text.IndexOf('.') != -1)
            {
                name = text.Substring(text.IndexOf('.') + 1);
            }
            return name;
        }

        private static int GetParamCount(Line line)
        {
            var bracket = 0;
            var comma = 0;

            if (line.Words.Count > 0)
            {
                var firstWord = line.Words[0].ToLower();
                if (firstWord == "sub" || firstWord == "thread.run") //|| firstWord == "f.start")
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

        private static void ParseModuleMethodsInModules(Dictionary<string, HashSet<string>> modulesCalling, Dictionary<string, HashSet<string>> tmpCalling)
        {
            var newCalling = new Dictionary<string, HashSet<string>>();

            foreach (var k in modulesCalling)
            {
                if (!tmpCalling.ContainsKey(k.Key))
                {
                    tmpCalling.Add(k.Key, new HashSet<string>());
                    newCalling.Add(k.Key, new HashSet<string>());
                }
                else
                {
                    if (!newCalling.ContainsKey(k.Key))
                        newCalling.Add(k.Key, new HashSet<string>());
                }

                foreach (var c in k.Value)
                {
                    if (!tmpCalling[k.Key].Contains(c))
                    {
                        tmpCalling[k.Key].Add(c);
                        newCalling[k.Key].Add(c);
                    }
                }
            }

            // Парсим вызовы из майн ищем их в модулях и добовляем в программу
            foreach (var mc in newCalling)
            {
                var modName = mc.Key;
                var methods = mc.Value;

                if (Data.Project.Modules.ContainsKey(modName))
                {
                    var module = Data.Project.Modules[modName];

                    foreach (var method in methods)
                    {
                        if (module.Methods.ContainsKey(method))
                        {
                            var count = Data.Project.ModuleMethodsText.Count;

                            var lines = module.Methods[method].Item1;
                            foreach (var line in lines)
                            {
                                Data.Project.ModuleMethodsText.Add(line);
                            }

                            if (Data.Project.ModuleMethodsText.Count > count)
                            {
                                count = Data.Project.ModuleMethodsText.Count;

                                // парсим вызовы методов из других, или этих же модулей
                                var mCalling = new Dictionary<string, HashSet<string>>();
                                foreach (var line in lines)
                                {
                                    if (line.Type == LineType.MODULEMETHODCALL)
                                    {
                                        var words = line.Words;
                                        foreach (var word in words)
                                        {
                                            if (word.Token == Tokens.MODULEMETHOD)
                                            {
                                                var name = GetModuleName(word.Text).ToLower();
                                                var met = GetMethodName(word.Text).ToLower() + "_" + GetParamCount(line);
                                                if (IsAdded(name, met))
                                                {
                                                    if (!mCalling.ContainsKey(name))
                                                    {
                                                        mCalling.Add(name, new HashSet<string>());
                                                        mCalling[name].Add(met);
                                                    }
                                                    else
                                                    {
                                                        if (!mCalling[name].Contains(met))
                                                        {
                                                            mCalling[name].Add(met);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (mCalling.Count > 0)
                                {
                                    ParseModuleMethodsInModules(mCalling, tmpCalling);
                                    if (Data.Errors.Count > 0)
                                        return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Ошибка модуля нет

                }
            }
        }

        private static void ParseModuleMethodsInMain()
        {
            var newLines = Data.Project.MainText;
            _modulesCalling = new Dictionary<string, HashSet<string>>();

            foreach (var line in newLines)
            {
                var words = line.Words;

                foreach (var word in words)
                {
                    if (word.Token == Tokens.MODULEMETHOD)
                    {
                        var name = GetModuleName(word.Text).ToLower();
                        var method = GetMethodName(word.Text).ToLower() + "_" + GetParamCount(line);
                        //Console.WriteLine("===> " + line.NewLine + " " + line.Type.ToString());
                        // Проверим, что такого вызова нет в майн и добавим в словарь новых вызовов
                        if (!_modulesCalling.ContainsKey(name))
                        {
                            _modulesCalling.Add(name, new HashSet<string>());
                            _modulesCalling[name].Add(method);
                        }
                        else
                        {
                            if (!_modulesCalling[name].Contains(method))
                            {
                                _modulesCalling[name].Add(method);
                            }
                        }
                    }
                }
            }
        }

        private static void AddIncludesToMain()
        {
            var mLines = Data.Project.Main.Lines;

            foreach (var mLine in mLines)
            {
                if (mLine.Type != LineType.EMPTY && mLine.Type != LineType.IMPORT && mLine.Type != LineType.FOLDER)
                {
                    if (!Data.Project.Includes.ContainsKey(mLine.Number))
                    {
                        Data.Project.MainText.Add(mLine);
                    }
                    else
                    {
                        var iLines = Data.Project.Includes[mLine.Number].Lines;
                        foreach (var iLine in iLines)
                        {
                            if (iLine.Type != LineType.EMPTY && iLine.Type != LineType.IMPORT && mLine.Type != LineType.FOLDER)
                            {
                                Data.Project.MainText.Add(iLine);
                            }
                        }
                    }
                }
            }
        }

        private static bool IsAdded(string name, string method)
        {
            if (_modulesCalling.ContainsKey(name))
            {
                if (_modulesCalling[name].Contains(method))
                {
                    return false;
                }

                return true;
            }
            return true;
        }

        private static void FuncRename()
        {
            var mLines = Data.Project.MainText;
            var fLines = Data.Project.ModuleMethodsText;

            var funcPref = "f_";
            var metPref = "m_";

            // Главная программа
            foreach (var line in mLines)
            {
                if (line.Type == LineType.MODULEMETHODCALL)
                {
                    var words = line.Words;
                    var tmp = new List<string>();

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.MODULEMETHOD)
                        {
                            word.Text = (metPref + word.Text.Replace('.','_') + "_" + GetParamCount(line)).ToLower();
                        }
                        tmp.Add(word.Text);
                    }
                    line.NewLine = string.Join(" ", tmp);
                }
                else if (line.Type == LineType.FUNCCALL || line.Type == LineType.FUNCINIT)
                {
                    var words = line.Words;
                    var tmp = new List<string>();

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.FUNCNAME)
                        {
                            word.Text = (funcPref + word.Text + "_" + GetParamCount(line)).ToLower();
                        }
                        tmp.Add(word.Text);
                    }
                    line.NewLine = string.Join(" ", tmp);
                }
                else if (line.Type == LineType.SUBCALL || line.Type == LineType.SUBINIT)
                {
                    var words = line.Words;
                    var tmp = new List<string>();

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.SUBNAME)
                        {
                            word.Text = (funcPref + word.Text + "_" + GetParamCount(line)).ToLower();
                        }
                        tmp.Add(word.Text);
                    }
                    line.NewLine = string.Join(" ", tmp);
                }
                else if (line.Type == LineType.METHODCALL)
                {
                    var words = line.Words;
                    var tmp = new List<string>();

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.SUBNAME)
                        {
                            word.Text = (funcPref + word.Text + "_" + GetParamCount(line)).ToLower();
                        }
                        tmp.Add(word.Text);
                    }
                    line.NewLine = string.Join(" ", tmp);
                }
            }

            // Методы модулейы
            foreach (var line in fLines)
            {
                if (line.Type == LineType.MODULEMETHODCALL)
                {
                    var words = line.Words;
                    var tmp = new List<string>();

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.MODULEMETHOD)
                        {
                            word.Text = (metPref + word.Text.Replace('.', '_') + "_" + GetParamCount(line)).ToLower();
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
                            word.Text = (metPref + word.Text + "_" + GetParamCount(line)).ToLower();
                        }
                        tmp.Add(word.Text);
                    }
                    line.NewLine = string.Join(" ", tmp);
                }
                else if (line.Type == LineType.METHODCALL)
                {
                    var words = line.Words;
                    var tmp = new List<string>();

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.SUBNAME)
                        {
                            word.Text = (funcPref + word.Text + "_" + GetParamCount(line)).ToLower();
                        }
                        tmp.Add(word.Text);
                    }
                    line.NewLine = string.Join(" ", tmp);
                }
            }
        }

        private static void VarsAndLabelsRename()
        {
            var mLines = Data.Project.MainText;
            var fLines = Data.Project.ModuleMethodsText;

            _propertys = new Dictionary<string, Line>();

            var glogalVarsPref = "gv_";
            var glogalLabelsPref = "gl_";
            var propertyPref = "pr_";

            var localVarsPrefText = "lv_";
            var localVarsPrefNumber = 0;
            var localLabelsPrefText = "ll_";
            var localLabelsPrefNumber = 0;

            var func = false;

            // Главная программа
            foreach (var line in mLines)
            {
                var words = line.Words;
                var tmp = new List<string>();

                if (line.Type == LineType.FUNCINIT)
                {
                    func = true;
                    localVarsPrefNumber++;
                    localLabelsPrefNumber++;
                }
                else if (line.Type == LineType.ONEKEYWORD && line.NewLine.ToLower().Trim() == "endfunction")
                {
                    func = false;
                    continue;
                }

                foreach (var word in words)
                {
                    if (word.Token == Tokens.VARIABLE)
                    {
                        if (!func)
                        {
                            word.Text = (glogalVarsPref + word.Text).ToLower().Replace("@", "");
                        }
                        else
                        {
                            if (word.Text.IndexOf('@') == -1)
                            {
                                word.Text = (localVarsPrefText + word.Text + "_" + localVarsPrefNumber.ToString()).ToLower();
                            }
                            else
                            {
                                word.Text = (glogalVarsPref + word.Text).ToLower().Replace("@", "");
                            }
                        }
                    }
                    else if (word.Token == Tokens.LABEL || word.Token == Tokens.LABELNAME)
                    {
                        if (!func)
                        {
                            word.Text = (glogalLabelsPref + word.Text).ToLower().Replace("@", "");
                        }
                        else
                        {
                            if (word.Text.IndexOf('@') == -1)
                            {
                                word.Text = (localLabelsPrefText + word.Text + "_" + localLabelsPrefNumber.ToString()).ToLower();
                            }
                            else
                            {
                                word.Text = (glogalVarsPref + word.Text).ToLower().Replace("@", "");
                            }
                        }

                        if (word.Token == Tokens.LABEL)
                        {
                            var tmpLabeName = word.Text.Replace(":", "") + ":";
                            word.Text = tmpLabeName;
                        }
                    }
                    else if (word.Token == Tokens.MODULEPROPERTY)
                    {
                        var name = word.ToLower().Replace(".", "_");
                        if (!_propertys.ContainsKey(name))
                        {
                            _propertys.Add(name, line);
                        }
                        word.Token = Tokens.VARIABLE;
                        word.Text = propertyPref + name;
                    }
                    tmp.Add(word.Text);
                }
                line.NewLine = string.Join(" ", tmp);
            }

            // Методы модулейы
            foreach (var line in fLines)
            {
                var words = line.Words;
                var tmp = new List<string>();

                if (line.Type == LineType.FUNCINIT)
                {
                    func = true;
                    localVarsPrefNumber++;
                }
                else if (line.Type == LineType.ONEKEYWORD && line.NewLine.ToLower().Trim() == "endfunction")
                {
                    func = false;
                    continue;
                }

                foreach (var word in words)
                {
                    if (word.Token == Tokens.VARIABLE)
                    {
                        if (word.Text.IndexOf('@') != -1)
                        {
                            // Ошибка, в модулях не может быть глобальных переменных
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2009, ""));
                            return;
                        }

                        if (!func)
                        {
                            // Ошибка, пока не может быть переменных вне методов модулей
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2008, ""));
                            return;
                        }
                        else
                        {
                            word.Text = (localVarsPrefText + word.Text + "_" + localVarsPrefNumber.ToString()).ToLower();
                        }
                    }
                    else if (word.Token == Tokens.LABEL || word.Token == Tokens.LABELNAME)
                    {
                        if (word.Text.IndexOf('@') != -1)
                        {
                            // Ошибка, в модулях не может быть глобальных переменных
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2008, ""));
                            return;
                        }

                        if (!func)
                        {
                            // Ошибка, пока не может быть переменных вне методов модулей
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2008, ""));
                            return;
                        }
                        else
                        {
                            word.Text = (localLabelsPrefText + word.Text + "_" + localLabelsPrefNumber.ToString()).ToLower();
                        }

                        if (word.Token == Tokens.LABEL)
                        {
                            var tmpLabeName = word.Text.Replace(":", "") + ":";
                            word.Text = tmpLabeName;
                        }
                    }
                    else if (word.Token == Tokens.MODULEPROPERTY)
                    {
                        var name = word.ToLower().Replace(".", "_");
                        if (!_propertys.ContainsKey(name))
                        {
                            _propertys.Add(name, line);
                        }
                        word.Token = Tokens.VARIABLE;
                        word.Text = propertyPref + name;
                    }
                    tmp.Add(word.Text);
                }
                line.NewLine = string.Join(" ", tmp);
            }
        }

        private static void RemoveMainFunc()
        {
            var mLines = Data.Project.MainText;
            var newMLines = new List<Line>();

            var func = false;

            foreach (var line in mLines)
            {
                if (line.Type == LineType.FUNCINIT)
                {
                    func = true;
                }
                else if (line.Type == LineType.ONEKEYWORD && line.NewLine.ToLower().Trim() == "endfunction")
                {
                    func = false;
                    Data.Project.MainFuncText.Add(line);
                    continue;
                }

                if (func)
                {
                    Data.Project.MainFuncText.Add(line);
                }
                else
                {
                    newMLines.Add(line);
                }
            }

            Data.Project.MainText.Clear();

            foreach (var line in newMLines)
            {
                Data.Project.MainText.Add(line);
            }
        }

        private static void RemoveMainSub()
        {
            var mLines = Data.Project.MainText;
            var newMLines = new List<Line>();

            var sub = false;

            foreach (var line in mLines)
            {
                if (line.Type == LineType.SUBINIT)
                {
                    sub = true;
                }
                else if (line.Type == LineType.ONEKEYWORD && line.NewLine.ToLower().Trim() == "endsub")
                {
                    sub = false;
                    Data.Project.MainSubText.Add(line);
                    continue;
                }

                if (sub)
                {
                    Data.Project.MainSubText.Add(line);
                }
                else
                {
                    newMLines.Add(line);
                }
            }

            Data.Project.MainText.Clear();

            foreach (var line in newMLines)
            {
                Data.Project.MainText.Add(line);
            }
        }

        private static void CreateFunctionsDicionary(List<Line> lines)
        {
            var name = "";

            foreach (var line in lines)
            {
                if (line.Type == LineType.FUNCINIT)
                {
                    var words = line.Words;
                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.FUNCNAME)
                        {
                            name = word.Text;
                            if (!Data.Project.Functions.ContainsKey(name))
                            {
                                Data.Project.Functions.Add(name, new Function(name));
                            }
                            break;
                        }
                    }
                }
                else if (line.Type == LineType.ONEKEYWORD && line.NewLine.ToLower().Trim() == "endfunction")
                {
                    if (name != "")
                    {
                        if (Data.Project.Functions.ContainsKey(name))
                        {
                            Data.Project.Functions[name].Lines.Add(line);
                        }
                    }
                    name = "";
                }

                if (name != "")
                {
                    if (Data.Project.Functions.ContainsKey(name))
                    {
                        Data.Project.Functions[name].Lines.Add(line);
                    }
                }
            }
        }

        private static void CreateSubsDicionary(List<Line> lines)
        {
            var name = "";

            foreach (var line in lines)
            {
                if (line.Type == LineType.SUBINIT)
                {
                    var words = line.Words;
                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.SUBNAME)
                        {
                            name = word.Text;
                            if (!Data.Project.Subs.ContainsKey(name))
                            {
                                Data.Project.Subs.Add(name, new Sub(name));
                            }
                            break;
                        }
                    }
                }
                else if (line.Type == LineType.ONEKEYWORD && line.NewLine.ToLower().Trim() == "endsub")
                {
                    if (name != "")
                    {
                        if (Data.Project.Subs.ContainsKey(name))
                        {
                            Data.Project.Subs[name].Lines.Add(line);
                        }
                    }
                    name = "";
                }

                if (name != "")
                {
                    if (Data.Project.Subs.ContainsKey(name))
                    {
                        Data.Project.Subs[name].Lines.Add(line);
                    }
                }
            }
        }

        private static void CreateCallingPropertyLines()
        {
            var modules = Data.Project.Modules;
            var propertyPref = "pr_";

            var moduleProperties = new Dictionary<string, Line>();
            foreach (var module in modules)
            {
                var propertys = module.Value.Propertys;

                foreach (var property in propertys)
                {
                    moduleProperties.Add(property.Key, property.Value.Item1);
                }
            }

            foreach (var property in _propertys)
            {
                Line definitionLine;
                var propertyName = property.Key;
                var propertyLine = property.Value;
                if (moduleProperties.TryGetValue(propertyName, out definitionLine))
                {
                    var varType = VariableType.NUMBER;
                    var newWords = new List<Word>();
                    var type = LineType.VARINIT;
                    newWords.Add(new Word() { Text = propertyPref + definitionLine.Words[1].ToLower(), OriginText = definitionLine.Words[1].OriginText, Token = Tokens.VARIABLE });

                    if (definitionLine.Words[0].ToLower() == "number")
                    {
                        newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                        newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                    }
                    else if (definitionLine.Words[0].ToLower() == "number[]")
                    {
                        type = LineType.VARARRAYINIT;
                        varType = VariableType.NUMBER_ARRAY;
                        newWords.Add(new Word() { Text = "[", OriginText = "[", Token = Tokens.BRACKETLEFTARRAY });
                        newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                        newWords.Add(new Word() { Text = "]", OriginText = "]", Token = Tokens.BRACKETRIGHTARRAY });
                        newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                        newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                    }
                    else if (definitionLine.Words[0].ToLower() == "string")
                    {
                        varType = VariableType.STRING;
                        newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                        newWords.Add(new Word() { Text = "\"\"", OriginText = "\"\"", Token = Tokens.STRING });
                    }
                    else if (definitionLine.Words[0].ToLower() == "string[]")
                    {
                        type = LineType.VARARRAYINIT;
                        varType = VariableType.STRING_ARRAY;
                        newWords.Add(new Word() { Text = "[", OriginText = "[", Token = Tokens.BRACKETLEFTARRAY });
                        newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                        newWords.Add(new Word() { Text = "]", OriginText = "]", Token = Tokens.BRACKETRIGHTARRAY });
                        newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                        newWords.Add(new Word() { Text = "\"\"", OriginText = "\"\"", Token = Tokens.STRING });
                    }

                    Data.Project.Propertys.Add(new Line(newWords, "") { Type = type, Number = definitionLine.Number, FileName = definitionLine.FileName });

                    var variable = new Variable("pr_" + propertyName) { Init = true, Type = varType };
                    Data.Project.Variables.Add(variable.Name, variable);
                }
                else
                {
                    Data.Errors.Add(new Errore(propertyLine.Number, propertyLine.FileName, 2020, propertyName));
                }
            }
        }

        private static void ParsePrivate()
        {
            var mLines = Data.Project.MainText;
            var modules = Data.Project.Modules;

            foreach (var line in mLines)
            {
                var words = line.Words;

                foreach (var word in words)
                {
                    if (word.Token == Tokens.MODULEPROPERTY)
                    {
                        var name = GetModuleName(word.ToLower()) + "_" + GetMethodName(word.ToLower());
                        var modName = GetModuleName(word.ToLower());

                        if (modules.ContainsKey(modName))
                        {
                            var mod = modules[modName];
                            if (mod.Propertys.ContainsKey(name))
                            {
                                if (mod.Propertys[name].Item2)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 2017, word.OriginText));
                                    return;
                                }
                            }
                        }
                    }
                    else if (word.Token == Tokens.MODULEMETHOD)
                    {
                        var name = GetMethodName(word.ToLower()) + "_" + GetParamCount(line).ToString();
                        var modName = GetModuleName(word.ToLower());

                        if (modules.ContainsKey(modName))
                        {
                            var mod = modules[modName];
                            if (mod.Methods.ContainsKey(name))
                            {
                                if (mod.Methods[name].Item2)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 2018, word.OriginText));
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            foreach (var mod in modules)
            {
                var lines = mod.Value.Lines;
                var tmpName = mod.Key;

                foreach (var line in lines)
                {
                    var words = line.Words;

                    foreach (var word in words)
                    {
                        if (word.Token == Tokens.MODULEPROPERTY)
                        {
                            var name = GetModuleName(word.ToLower()) + "_" + GetMethodName(word.ToLower());
                            var modName = GetModuleName(word.ToLower());

                            if (modules.ContainsKey(modName) && modName != tmpName)
                            {
                                var modul = modules[modName];
                                if (modul.Propertys.ContainsKey(name))
                                {
                                    if (modul.Propertys[name].Item2)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2017, word.OriginText));
                                        return;
                                    }
                                }
                            }
                        }
                        else if (word.Token == Tokens.MODULEMETHOD)
                        {
                            var name = GetMethodName(word.ToLower()) + "_" + GetParamCount(line).ToString();
                            var modName = GetModuleName(word.ToLower());

                            if (modules.ContainsKey(modName) && modName != tmpName)
                            {
                                var modul = modules[modName];
                                if (modul.Methods.ContainsKey(name))
                                {
                                    if (modul.Methods[name].Item2)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2018, word.OriginText));
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

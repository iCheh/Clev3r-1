using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using Interpreter.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Utils
{
    internal class Interpreter
    {
        internal Interpreter()
        {
        }

        internal void Start()
        {
            // Временный словарь под переменные объявленные в инициализации функций
            var variables = new Dictionary<string, (Variable, Line)>();
            // Временный лист под инициализацию переменных
            var varInit = new List<Line>();
            // Временный список имён вызовов процедур
            var callsSub = new HashSet<string>();
            // Временный список имён вызовов функций
            var callsFunc = new HashSet<string>();

            // Парсим все вызовы первый раз для заполнения словарей вызовов
            // и последующей проверки наличия функций и процедур по этим вызовам
            ParseAllCalls(callsSub, callsFunc);
            if (Data.Errors.Count > 0)
                return;

            // Пропарсим строки инициализации функций и заполним недостающие параметры
            // Так же переименуем Function/EndFunction в Sub/EndSub
            // И добавим это дело в набор OutLines текущей линии
            ParseFuncInitLine();
            if (Data.Errors.Count > 0)
                return;

            // Преобразуем вызовы функций и методов модулей следующим образом
            // Инициализация входных параметров перед вызовом
            // Инициализация выходных параметров после вызова
            ParseCalls(variables);
            if (Data.Errors.Count > 0)
                return;

            // Добавим все переменные из временного словаря variables в начало MAIN
            // т.е. проинициализируем все эти переменные
            FuncVariablesInit(variables, varInit);
            if (Data.Errors.Count > 0)
                return;

            //ЗДЕСЬ НУЖНО ПРОПАРСИТЬ ВСЕ ФУНКЦИИ, НАЙТИ ВСЕ ИНИЦИАЛИЗАЦИИ
            //ОСТАЛЬНЫХ ПЕРЕМЕННЫХ, ЗАПОЛНИТЬ СЛОВАРЬ variables
            //И ТАК ЖЕ ДОБАВИТЬ ЭТИ ПЕРЕМЕННЫЕ В НАЧАЛЕ MAIN            
            var tmpSubCalls = new HashSet<string>();
            SubVarsInit(tmpSubCalls, Data.Project.MainText);
            OtherVarsAddToMain(varInit);
            
            
            //Console.WriteLine("\n");
            /*
            foreach (var v in Data.Project.MainText)
            {
                Console.WriteLine(v.NewLine + " " + v.Type.ToString());
            }
            foreach (var v in Data.Project.Variables)
            {
                Console.WriteLine(v.Key + " " + v.Value.Name + " " + v.Value.Type.ToString());
            }
            */
            //Console.WriteLine("\n");
            

            // Наконец создадим новый лист с Line
            CreateProjectOutputLines(varInit);
            if (Data.Errors.Count > 0)
                return;

            // Перезапишем выходной текст и уберём оттуда неиспользуемые процедуры и функции
            var lines = RewriteOutLines(Data.Project.OutputLines, callsSub, callsFunc);



            // Заполним словарь переменных
            // Распарсим все инициализации переменных
            //Data.Project.Variables.Clear();
            VariableErrorParser.Start(lines);
            if (Data.Errors.Count > 0)
                return;

            // Распарсим остальные линии
            LineErrorParser.Start(lines);
            if (Data.Errors.Count > 0)
                return;

            Data.Project.OutputLines.Clear();
            foreach (var line in lines)
            {
                Data.Project.OutputLines.Add(line);
            }
        }

        private void ParseFuncInitLine()
        {
            //var funcs = Data.Project.Functions;

            foreach (var line in Data.Project.MainFuncText)
            {
                if (line.Type == LineType.FUNCINIT)
                {
                    FunctionsInitErrorParser.FuncInitLineParse(line);
                    if (Data.Errors.Count > 0)
                        return;

                    var newWords = new List<Word>();
                    newWords.Add(new Word() { Text = "Sub", OriginText = line.Words[0].OriginText, Token = line.Words[0].Token } );
                    newWords.Add(line.Words[1]);

                    line.OutLines.Add(new Line(newWords, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.SUBINIT });
                }
                else if (line.Type == LineType.ONEKEYWORD && line.Words[0].ToLower() == "endfunction")
                {
                    var newWords = new List<Word>();
                    newWords.Add(new Word() { Text = "EndSub", OriginText = line.Words[0].OriginText, Token = line.Words[0].Token });

                    line.OutLines.Add(new Line(newWords, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.ONEKEYWORD });
                }
            }

            foreach (var line in Data.Project.ModuleMethodsText)
            {
                if (line.Type == LineType.FUNCINIT)
                {
                    FunctionsInitErrorParser.FuncInitLineParse(line);
                    if (Data.Errors.Count > 0)
                        return;

                    var newWords = new List<Word>();
                    newWords.Add(new Word() { Text = "Sub", OriginText = line.Words[0].OriginText, Token = line.Words[0].Token });
                    newWords.Add(line.Words[1]);

                    line.OutLines.Add(new Line(newWords, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.SUBINIT });
                }
                else if (line.Type == LineType.ONEKEYWORD && line.Words[0].ToLower() == "endfunction")
                {
                    var newWords = new List<Word>();
                    newWords.Add(new Word() { Text = "EndSub", OriginText = line.Words[0].OriginText, Token = line.Words[0].Token });

                    line.OutLines.Add(new Line(newWords, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.ONEKEYWORD });
                }
            }
        }

        private void ParseCalls(Dictionary<string, (Variable, Line)> variables)
        {
            var main = Data.Project.MainText;
            var subs = Data.Project.MainSubText;
            var funcs = Data.Project.MainFuncText;
            var methods = Data.Project.ModuleMethodsText;

            ParseOneCall(main, variables);
            if (Data.Errors.Count > 0)
                return;

            ParseOneCall(subs, variables);
            if (Data.Errors.Count > 0)
                return;

            ParseOneCall(funcs, variables);
            if (Data.Errors.Count > 0)
                return;

            ParseOneCall(methods, variables);
            if (Data.Errors.Count > 0)
                return;
        }

        private void ParseOneCall(List<Line> lines, Dictionary<string, (Variable, Line)> variables)
        {
            bool body = false;

            foreach (var line in lines)
            {
                if (line.Type == LineType.SUBCALL || line.Type == LineType.FUNCCALL || line.Type == LineType.MODULEMETHODCALL)
                {
                    if (line.Words.Count >= 2)
                    {
                        var name = line.Words[0].ToLower();

                        if (Data.Project.Functions.ContainsKey(name))
                        {
                            var func = Data.Project.Functions[name];
                            var tmpWords = GetParamCount(line);

                            if (func.Parameters.Count == tmpWords.Count && func.Parameters.Count > 0)
                            {
                                //Console.WriteLine('\n');
                                int i = 0;
                                body = false;
                                foreach (var p in func.Parameters)
                                {
                                    var newWords = new List<Word>();
                                    var param = p.Value;
                                    if (param.paramType == ParameterType.INPUT)
                                    {
                                        if (!variables.ContainsKey(p.Key))
                                        {
                                            variables.Add(p.Key, (new Variable(p.Key) { Type = param.varType }, line));
                                        }

                                        newWords.Add(new Word() { Text = p.Key, OriginText = p.Key, Token = Tokens.VARIABLE });
                                        newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                                        foreach (var word in tmpWords[i])
                                        {
                                            newWords.Add(word);
                                        }
                                        i++;

                                        line.OutLines.Add(new Line(newWords, "") { Number = line.Number, FileName = line.FileName, Type = LineType.VARINIT });
                                        
                                        //Console.WriteLine("input =>" + line.OutLines[line.OutLines.Count-1].NewLine + "<=");
                                    }
                                    else if (param.paramType == ParameterType.OUTPUT)
                                    {
                                        if (!body)
                                        {
                                            var nw = new List<Word>();
                                            var fw = line.Words[0];
                                            if (fw.Token == Tokens.MODULEMETHOD || fw.Token == Tokens.FUNCNAME)
                                            {
                                                fw.Token = Tokens.SUBNAME;
                                            }
                                            nw.Add(fw);
                                            nw.Add(new Word() { Text = "()", OriginText = "()", Token = Tokens.DOUBLEBRACKET });
                                            line.OutLines.Add(new Line(nw, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.SUBCALL });
                                            body = true;
                                        }
                                        if (tmpWords[i].Count == 1)
                                        {
                                            if (tmpWords[i][0].Token != Tokens.VARIABLE)
                                            {
                                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1405, "( " + line.OldLine + " )"));
                                                return;
                                            }

                                            newWords.Add(tmpWords[i][0]);
                                            newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                                            newWords.Add(new Word() { Text = p.Key, OriginText = p.Key, Token = Tokens.VARIABLE });

                                            if (!variables.ContainsKey(p.Key))
                                            {
                                                variables.Add(p.Key, (new Variable(p.Key) { Type = param.varType }, line));
                                            }

                                            line.OutLines.Add(new Line(newWords, "") { Number = line.Number, FileName = line.FileName, Type = LineType.VARINIT });
                                        }
                                        else if (tmpWords[i].Count > 1)
                                        {
                                            // Из-за отсутствия этой конструкции была ошибка при попытке запихнуть в параметры элемент массива

                                            if (tmpWords[i][0].Token == Tokens.VARIABLE && tmpWords[i][0].ToLower().Trim() != "gv_" && tmpWords[i][1].Token == Tokens.BRACKETLEFTARRAY && tmpWords[i][tmpWords[i].Count - 1].Token == Tokens.BRACKETRIGHTARRAY)
                                            {
                                                foreach (var tmpW in tmpWords[i])
                                                {
                                                    newWords.Add(tmpW);
                                                }
                                                newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                                                newWords.Add(new Word() { Text = p.Key, OriginText = p.Key, Token = Tokens.VARIABLE });

                                                if (!variables.ContainsKey(p.Key))
                                                {
                                                    variables.Add(p.Key, (new Variable(p.Key) { Type = param.varType }, line));
                                                }
                                            }
                                            else
                                            {
                                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1405, "( " + line.OldLine + " )"));
                                                return;
                                            }
                                            line.OutLines.Add(new Line(newWords, "") { Number = line.Number, FileName = line.FileName, Type = LineType.VARARRAYINIT });
                                        }
                                        i++;

                                        //line.OutLines.Add(new Line(newWords, "") { Number = line.Number, FileName = line.FileName, Type = LineType.VARARRAYINIT });
                                        
                                        //Console.WriteLine("output =>" + line.OutLines[line.OutLines.Count - 1].NewLine + "<=");
                                    }
                                }
                                if (!body)
                                {
                                    var nw = new List<Word>();
                                    var fw = line.Words[0];
                                    if (fw.Token == Tokens.MODULEMETHOD || fw.Token == Tokens.FUNCNAME)
                                    {
                                        fw.Token = Tokens.SUBNAME;
                                    }
                                    nw.Add(fw);
                                    nw.Add(new Word() { Text = "()", OriginText = "()", Token = Tokens.DOUBLEBRACKET });
                                    line.OutLines.Add(new Line(nw, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.SUBCALL });
                                    body = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateProjectOutputLines(List<Line> varInit)
        {
            var main = Data.Project.MainText;
            var subs = Data.Project.MainSubText;
            var funcs = Data.Project.MainFuncText;
            var methods = Data.Project.ModuleMethodsText;
            var propertys = Data.Project.Propertys;

            foreach (var line in propertys)
            {
                if (line.OutLines.Count > 0)
                {
                    foreach (var l in line.OutLines)
                    {
                        Data.Project.OutputLines.Add(l);
                        //Console.WriteLine("===> " + l.NewLine);
                    }
                }
                else
                {
                    Data.Project.OutputLines.Add(line);
                    //Console.WriteLine("===> " + line.NewLine);
                }
            }

            foreach (var line in varInit)
            {
                if (line.OutLines.Count > 0)
                {
                    foreach (var l in line.OutLines)
                    {
                        Data.Project.OutputLines.Add(l);
                    }
                }
                else
                {
                    Data.Project.OutputLines.Add(line);
                }
            }

            foreach (var line in main)
            {
                if (line.OutLines.Count > 0)
                {
                    foreach (var l in line.OutLines)
                    {
                        Data.Project.OutputLines.Add(l);
                        //Console.WriteLine("M1 ===> " + l.NewLine + " " + l.Type.ToString());
                    }
                }
                else
                {
                    Data.Project.OutputLines.Add(line);
                    //Console.WriteLine("M ===> " + line.NewLine + " " + line.Type.ToString());
                }
            }

            foreach (var line in subs)
            {
                if (line.OutLines.Count > 0)
                {
                    foreach (var l in line.OutLines)
                    {
                        Data.Project.OutputLines.Add(l);
                    }
                }
                else
                {
                    Data.Project.OutputLines.Add(line);
                }
            }

            foreach (var line in funcs)
            {
                if (line.OutLines.Count > 0)
                {
                    foreach (var l in line.OutLines)
                    {
                        Data.Project.OutputLines.Add(l);
                    }
                }
                else
                {
                    Data.Project.OutputLines.Add(line);
                }
            }

            foreach (var line in methods)
            {
                if (line.OutLines.Count > 0)
                {
                    foreach (var l in line.OutLines)
                    {
                        Data.Project.OutputLines.Add(l);
                    }
                }
                else
                {
                    Data.Project.OutputLines.Add(line);
                }
            }
        }

        private void FuncVariablesInit(Dictionary<string, (Variable, Line)> variables, List<Line> varInit)
        {
            foreach (var v in variables)
            {
                var newWords = new List<Word>();
                var type = LineType.VARINIT;
                newWords.Add(new Word() { Text = v.Key, OriginText = v.Key, Token = Tokens.VARIABLE } );

                if (v.Value.Item1.Type == VariableType.NUMBER)
                {
                    newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                    newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                }
                else if (v.Value.Item1.Type == VariableType.NUMBER_ARRAY)
                {
                    type = LineType.VARARRAYINIT;
                    newWords.Add(new Word() { Text = "[", OriginText = "[", Token = Tokens.BRACKETLEFTARRAY });
                    newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                    newWords.Add(new Word() { Text = "]", OriginText = "]", Token = Tokens.BRACKETRIGHTARRAY });
                    newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                    newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                }
                else if (v.Value.Item1.Type == VariableType.STRING)
                {
                    newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                    newWords.Add(new Word() { Text = "\"\"", OriginText = "\"\"", Token = Tokens.STRING });
                }
                else if (v.Value.Item1.Type == VariableType.STRING_ARRAY)
                {
                    type = LineType.VARARRAYINIT;
                    newWords.Add(new Word() { Text = "[", OriginText = "[", Token = Tokens.BRACKETLEFTARRAY });
                    newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                    newWords.Add(new Word() { Text = "]", OriginText = "]", Token = Tokens.BRACKETRIGHTARRAY });
                    newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                    newWords.Add(new Word() { Text = "\"\"", OriginText = "\"\"", Token = Tokens.STRING });
                }

                varInit.Add(new Line(newWords, "") { Type = type, Number = v.Value.Item2.Number, FileName = v.Value.Item2.FileName } );
            }
        }

        private List<List<Word>> GetParamCount(Line line)
        {
            var bracket = 0;
            var words = new List<List<Word>>();

            foreach (var word in line.Words)
            {
                if (word.Token == Tokens.BRACKETLEFT)
                {
                    if (bracket == 0)
                    {
                        bracket++;
                        words.Add(new List<Word>());
                        continue;
                    }
                    else
                    {
                        bracket++;
                    }
                }
                else if (word.Token == Tokens.BRACKETRIGHT)
                {
                    bracket--;
                }

                if (bracket > 0 && words.Count > 0)
                {
                    if (word.Token == Tokens.COMMA)
                    {
                        if (bracket != 1)
                        {
                            words[words.Count - 1].Add(word);
                        }
                    }
                    else
                    {
                        words[words.Count - 1].Add(word);
                    }
                }

                if (word.Token == Tokens.COMMA && bracket == 1)
                {
                    words.Add(new List<Word>());
                }
            }

            return words;
        }

        private void SubVarsInit(HashSet<string> calls, List<Line> lines)
        {
            var subs = Data.Project.MainSubText;         

            foreach (var line in lines)
            {
                if (line.Type == LineType.VARINIT)
                {                   
                    if (VariableErrorParser.ParseInitVarError(line, VariableType.NON, false, 2, line.Count, 3))
                        return;
                }
                else if (line.Type == LineType.VARARRAYINIT)
                {
                    if (VariableErrorParser.ParseBracketLeftArray(line))
                        return;
                }
                else if (line.Type == LineType.VARDOUBLEMATH)
                {
                    if (VariableErrorParser.ParseDoubleMathError(line))
                        return;
                }
                else if (line.Type == LineType.VAREQUMATH)
                {
                    if (VariableErrorParser.ParseEquMathError(line))
                        return;
                }
                else if (line.Type == LineType.FORINIT)
                {
                    ForLineErrorParser.Start(line, Data.Project.Variables);
                    if (Data.Errors.Count > 0)
                        return;
                }
                else if (line.Type == LineType.SUBCALL)
                {
                    var name = line.Words[0].ToLower();
                    if (!calls.Contains(name))
                    {
                        calls.Add(name);
                        var body = false;
                        var newCalls = new List<Line>();
                        foreach (var l in subs)
                        {
                            if (l.Type == LineType.SUBINIT)
                            {
                                if (l.Words[1].ToLower() == name)
                                {
                                    body = true;
                                }                                
                            }
                            else if (l.Type == LineType.ONEKEYWORD && l.Words[0].ToLower() == "endsub" && body)
                            {
                                body = false;
                                SubVarsInit(calls, newCalls);
                                break;
                            }

                            if (body)
                            {
                                newCalls.Add(l);
                            }
                        }
                    }

                    
                }
            }
        }

        private void OtherVarsAddToMain(List<Line> varInit)
        {
            var variables = Data.Project.Variables;

            foreach (var v in variables)
            {
                var newWords = new List<Word>();
                var type = LineType.VARINIT;
                newWords.Add(new Word() { Text = v.Key, OriginText = v.Key, Token = Tokens.VARIABLE });

                if (v.Value.Type == VariableType.NUMBER)
                {
                    newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                    newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                }
                else if (v.Value.Type == VariableType.NUMBER_ARRAY)
                {
                    type = LineType.VARARRAYINIT;
                    newWords.Add(new Word() { Text = "[", OriginText = "[", Token = Tokens.BRACKETLEFTARRAY });
                    newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                    newWords.Add(new Word() { Text = "]", OriginText = "]", Token = Tokens.BRACKETRIGHTARRAY });
                    newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                    newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                }
                else if (v.Value.Type == VariableType.STRING)
                {
                    newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                    newWords.Add(new Word() { Text = "\"\"", OriginText = "\"\"", Token = Tokens.STRING });
                }
                else if (v.Value.Type == VariableType.STRING_ARRAY)
                {
                    type = LineType.VARARRAYINIT;
                    newWords.Add(new Word() { Text = "[", OriginText = "[", Token = Tokens.BRACKETLEFTARRAY });
                    newWords.Add(new Word() { Text = "0", OriginText = "0", Token = Tokens.NUMBER });
                    newWords.Add(new Word() { Text = "]", OriginText = "]", Token = Tokens.BRACKETRIGHTARRAY });
                    newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                    newWords.Add(new Word() { Text = "\"\"", OriginText = "\"\"", Token = Tokens.STRING });
                }

                varInit.Add(new Line(newWords, "") { Type = type, Number = v.Value.Line.Number, FileName = v.Value.Line.FileName });
            }
        }

        private void ParseAllCalls(HashSet<string> callsSub, HashSet<string> callsFunc)
        {
            var main = Data.Project.MainText;
            var subs = Data.Project.MainSubText;
            var funcs = Data.Project.MainFuncText;
            var methods = Data.Project.ModuleMethodsText;

            ParseFirstCall(main, callsSub, callsFunc);
            if (Data.Errors.Count > 0)
                return;

            ParseFirstCall(subs, callsSub, callsFunc);
            if (Data.Errors.Count > 0)
                return;

            ParseFirstCall(funcs, callsSub, callsFunc);
            if (Data.Errors.Count > 0)
                return;

            ParseFirstCall(methods, callsSub, callsFunc);
        }

        private void ParseFirstCall(List<Line> lines, HashSet<string> callsSub, HashSet<string> callsFunc)
        {
            foreach (var line in lines)
            {
                if (line.Type == LineType.SUBCALL || line.Type == LineType.FUNCCALL || line.Type == LineType.MODULEMETHODCALL)
                {
                    if (line.Words.Count >= 2)
                    {
                        var name = line.Words[0].ToLower();

                        if (line.Type == LineType.SUBCALL)
                        {
                            if (!callsSub.Contains(name))
                                callsSub.Add(name);
                        }
                        else
                        {
                            if (!callsFunc.Contains(name))
                                callsFunc.Add(name);
                        }

                        if (line.Type == LineType.SUBCALL)
                        {
                            if (!Data.Project.Subs.ContainsKey(name))
                            {
                                if (!Data.Project.Functions.ContainsKey(name))
                                {
                                    if (line.Count > 2)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + line.Words[0].OriginText + " )"));
                                        return;
                                    }
                                    else
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1606, "( " + line.Words[0].OriginText + " )"));
                                        return;
                                    }
                                } 
                            }
                        }
                        else
                        {
                            if (!Data.Project.Functions.ContainsKey(name))
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + line.Words[0].OriginText + " )"));
                                return;
                            }
                        }
                    }
                }
                else if (line.Type == LineType.METHODCALL)
                {
                    if (line.Words[0].ToLower() == "thread.run")
                    {
                        if (line.Count != 3 || line.Words[1].Token != Tokens.EQU || line.Words[2].Token != Tokens.SUBNAME)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1414, ""));
                            return;
                        }
                        else
                        {
                            var name = line.Words[2].ToLower();
                            if (!callsSub.Contains(name))
                                callsSub.Add(name);

                            if (!Data.Project.Subs.ContainsKey(name))
                            {
                                foreach (var s in Data.Project.Subs)
                                {
                                    Console.WriteLine(s.Key);
                                }
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1606, "( " + line.Words[2].OriginText + " )"));
                                return;
                            }
                        }
                    }
                }
            }
        }

        private List<Line> RewriteOutLines(List<Line> lines, HashSet<string> callsSub, HashSet<string> callsFunc)
        {
            var newLines = new List<Line>();
            bool write = true;

            foreach (var line in lines)
            {
                if (line.Type == LineType.SUBINIT)
                {
                    var name = line.Words[1].ToLower();

                    if (!callsSub.Contains(name) && !callsFunc.Contains(name))
                    {
                        write = false;
                    }
                }
                else if (line.Type == LineType.ONEKEYWORD && line.Words[0].ToLower() == "endsub")
                {
                    if (!write)
                    {
                        write = true;
                        continue;
                    }
                }

                if (write)
                {
                    newLines.Add(line);
                }
            }

            return newLines;
        }
    }
}

using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class StructErrorParser
    {
        //private static Tuple<string, int> TmpFuncVarsKey = new Tuple<string, int>("", 0);
        internal static void Start(List<Line> lines)
        {
            Stack<Tuple<int, string, string>> stackStructMain = new Stack<Tuple<int, string, string>>();
            Stack<Tuple<int, string, string>> stackStructSub = new Stack<Tuple<int, string, string>>();
            Stack<Tuple<int, string, string>> stackStructFunc = new Stack<Tuple<int, string, string>>();

            int tmpStruct = 1;
            bool sub = false;
            bool func = false;
            int subNum = 0;
            string subFileName = "";
            //string subName = "";
            int funcNum = 0;
            string funcFileName = "";
            //Tuple<string, int> tuple = new Tuple<string, int>("", 0);

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];

                var firstWord = line.FirstWord;
                if (firstWord == null)
                {
                    continue;
                }

                if (firstWord.Token == Tokens.KEYWORD)
                {
                    if (firstWord.ToLower() == "else" || firstWord.ToLower() == "endif" || firstWord.ToLower() == "endfor" || firstWord.ToLower() == "endwhile" || firstWord.ToLower() == "endsub" || firstWord.ToLower() == "endfunction")
                    {
                        if (line.Count != 1)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, Code(firstWord.ToLower()), ""));
                            return;
                        }
                    }

                    if (firstWord.ToLower() == "goto")
                    {
                        if (line.Count != 2)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1020, ""));
                            return;
                        }
                        else if (line.Words[1].Text.IndexOf(':') != -1)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1020, ""));
                            return;
                        }
                    }
                    else if (firstWord.ToLower() == "if")
                    {
                        tmpStruct = 1;
                        if (sub)
                        {
                            stackStructSub.Push(new Tuple<int, string, string>(line.Number, "endif", line.FileName));
                        }
                        else if (func)
                        {
                            stackStructFunc.Push(new Tuple<int, string, string>(line.Number, "endif", line.FileName));
                        }
                        else
                        {
                            stackStructMain.Push(new Tuple<int, string, string>(line.Number, "endif", line.FileName));
                        }
                    }
                    else if (firstWord.ToLower() == "for")
                    {
                        tmpStruct = 2;
                        if (sub)
                        {
                            stackStructSub.Push(new Tuple<int, string, string>(line.Number, "endfor", line.FileName));
                        }
                        else if (func)
                        {
                            stackStructFunc.Push(new Tuple<int, string, string>(line.Number, "endfor", line.FileName));
                        }
                        else
                        {
                            stackStructMain.Push(new Tuple<int, string, string>(line.Number, "endfor", line.FileName));
                        }
                    }
                    else if (firstWord.ToLower() == "while")
                    {
                        tmpStruct = 3;
                        if (sub)
                        {
                            stackStructSub.Push(new Tuple<int, string, string>(line.Number, "endwhile", line.FileName));
                        }
                        else if (func)
                        {
                            stackStructFunc.Push(new Tuple<int, string, string>(line.Number, "endwhile", line.FileName));
                        }
                        else
                        {
                            stackStructMain.Push(new Tuple<int, string, string>(line.Number, "endwhile", line.FileName));
                        }
                    }
                    else if (firstWord.ToLower() == "endif")
                    {
                        if (sub)
                        {
                            if (stackStructSub.Count > 0)
                            {
                                var tmpTuple = stackStructSub.Pop();
                                if (tmpTuple.Item2 != "endif")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1004, ""));
                                return;
                            }
                        }
                        else if (func)
                        {
                            if (stackStructFunc.Count > 0)
                            {
                                var tmpTuple = stackStructFunc.Pop();
                                if (tmpTuple.Item2 != "endif")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1004, ""));
                                return;
                            }
                        }
                        else
                        {
                            if (stackStructMain.Count > 0)
                            {
                                var tmpTuple = stackStructMain.Pop();
                                if (tmpTuple.Item2 != "endif")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1004, ""));
                                return;
                            }
                        }
                    }
                    else if (firstWord.ToLower() == "endfor")
                    {
                        if (sub)
                        {
                            if (stackStructSub.Count > 0)
                            {
                                var tmpTuple = stackStructSub.Pop();
                                if (tmpTuple.Item2 != "endfor")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1006, ""));
                                return;
                            }
                        }
                        else if (func)
                        {
                            if (stackStructFunc.Count > 0)
                            {
                                var tmpTuple = stackStructFunc.Pop();
                                if (tmpTuple.Item2 != "endfor")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1004, ""));
                                return;
                            }
                        }
                        else
                        {
                            if (stackStructMain.Count > 0)
                            {
                                var tmpTuple = stackStructMain.Pop();
                                if (tmpTuple.Item2 != "endfor")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1006, ""));
                                return;
                            }
                        }
                    }
                    else if (firstWord.ToLower() == "endwhile")
                    {
                        if (sub)
                        {
                            if (stackStructSub.Count > 0)
                            {
                                var tmpTuple = stackStructSub.Pop();
                                if (tmpTuple.Item2 != "endwhile")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1008, ""));
                                return;
                            }
                        }
                        else if (func)
                        {
                            if (stackStructFunc.Count > 0)
                            {
                                var tmpTuple = stackStructFunc.Pop();
                                if (tmpTuple.Item2 != "endwhile")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1004, ""));
                                return;
                            }
                        }
                        else
                        {
                            if (stackStructMain.Count > 0)
                            {
                                var tmpTuple = stackStructMain.Pop();
                                if (tmpTuple.Item2 != "endwhile")
                                {
                                    if (tmpStruct == 1)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1003, "")); return; }
                                    else if (tmpStruct == 2)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1005, "")); return; }
                                    else if (tmpStruct == 3)
                                    { Data.Errors.Add(new Errore(tmpTuple.Item1, line.FileName, 1007, "")); return; }
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1008, ""));
                                return;
                            }
                        }
                    }
                    else if (firstWord.ToLower() == "sub")
                    {
                        if (sub)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1009, ""));
                            return;
                        }
                        else if (func)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1030, ""));
                            return;
                        }
                        else
                        {
                            sub = true;
                            subNum = i;
                            subFileName = line.FileName;
                            //subName = line.Words[1].Text; 
                        }
                    }
                    else if (firstWord.ToLower() == "endsub")
                    {
                        if (!sub)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1010, ""));
                            return;
                        }
                        if (func)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1031, ""));
                            return;
                        }
                        else
                        {
                            sub = false;
                        }
                    }
                    else if (firstWord.ToLower() == "function")
                    {
                        if (sub)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1031, ""));
                            return;
                        }
                        else if (func)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1026, ""));
                            return;
                        }
                        else
                        {
                            func = true;
                            funcNum = i;
                            funcFileName = line.FileName;
                            //TmpFuncVarsKey = new Tuple<string, int>(line.Words[1].Text, line.FuncParamCount);
                        }
                    }
                    else if (firstWord.ToLower() == "endfunction")
                    {
                        if (!func)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1010, ""));
                            return;
                        }
                        if (sub)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1030, ""));
                            return;
                        }
                        else
                        {
                            func = false;
                        }
                    }
                }
                /*
                else if (firstWord.Token == Tokens.METHOD)
                {
                    if (firstWord.ToLower() == "f.returnnumber")
                    {
                        if (Builder.Project.SubNames.ContainsKey(subName))
                        {
                            Builder.Project.SubNames[subName] = VariableType.NUMBER;
                        }
                    }
                    else if (firstWord.ToLower() == "f.returntext")
                    {
                        if (Builder.Project.SubNames.ContainsKey(subName))
                        {
                            Builder.Project.SubNames[subName] = VariableType.STRING;
                        }
                    }
                    else if (firstWord.ToLower() == "f.return")
                    {
                        if (Builder.Project.SubNames.ContainsKey(subName))
                        {
                            Builder.Project.SubNames[subName] = VariableType.NON;
                        }
                    }
                }
                */
                else if (firstWord.Token == Tokens.LABEL)
                {
                    if (line.Count != 1)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1021, ""));
                        return;
                    }
                    /*
                    if (func)
                    {
                        if (Builder.Project.Functions.ContainsKey(TmpFuncVarsKey))
                        {
                            if (!Builder.Project.Functions[TmpFuncVarsKey].Labels.Contains(firstWord.Text.Replace(":", "").ToUpper()))
                            {
                                Builder.Project.Functions[TmpFuncVarsKey].Labels.Add(firstWord.Text.Replace(":", "").ToUpper());
                                line.OutLine.Clear();
                                line.OutLine.Add(firstWord.Text.Replace(":", Builder.Project.Functions[TmpFuncVarsKey].Suf + ":").ToUpper());
                            }
                            else
                            {
                                Builder.Errors.Add(new Errore(line.Number, line.FileName, 1024, "( " + firstWord.Text + " )"));
                                return;
                            }
                        }
                        else
                        {
                            Builder.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + TmpFuncVarsKey.Item1 + " (count: " + TmpFuncVarsKey.Item2.ToString() + " ) )"));
                            return;
                        }
                    }
                    else
                    {
                        if (!Builder.Project.Labels.Contains(firstWord.Text.Replace(":", "").ToUpper()))
                        {
                            Builder.Project.Labels.Add(firstWord.Text.Replace(":", "").ToUpper());
                            line.OutLine.Clear();
                            line.OutLine.Add(firstWord.Text.Replace(":", Builder.Project.MainSuf + ":").ToUpper());
                        }
                        else
                        {
                            Builder.Errors.Add(new Errore(line.Number, line.FileName, 1025, "( " + firstWord.Text + " )"));
                            return;
                        }
                    }
                    */
                }
            }

            if (sub)
            {
                Data.Errors.Add(new Errore(lines[subNum].Number, subFileName, 1011, ""));
            }
            if (func)
            {
                Data.Errors.Add(new Errore(lines[funcNum].Number, funcFileName, 1028, ""));
            }
            if (stackStructMain.Count > 0)
            {
                var tmpTuple = stackStructMain.Pop();
                if (tmpStruct == 1)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1012, "")); return; }
                else if (tmpStruct == 2)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1013, "")); return; }
                else if (tmpStruct == 3)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1014, "")); return; }
            }
            if (stackStructSub.Count > 0)
            {
                var tmpTuple = stackStructSub.Pop();
                if (tmpStruct == 1)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1012, "")); return; }
                else if (tmpStruct == 2)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1013, "")); return; }
                else if (tmpStruct == 3)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1014, "")); return; }
            }
            if (stackStructFunc.Count > 0)
            {
                var tmpTuple = stackStructFunc.Pop();
                if (tmpStruct == 1)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1012, "")); return; }
                else if (tmpStruct == 2)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1013, "")); return; }
                else if (tmpStruct == 3)
                { Data.Errors.Add(new Errore(tmpTuple.Item1, tmpTuple.Item3, 1014, "")); return; }
            }
        }

        /*
        internal static void RenameSubName(Line line, bool func, Tuple<string, int> tmpFVK)
        {
            var firstWord = line.FirstWord;
            int paramCount = 0;

            if (func)
            {
                if (Builder.Project.Functions.ContainsKey(tmpFVK))
                {
                    paramCount = GetFuncParamCountCalls(line, func, Builder.Project.Functions[tmpFVK].LocalVariables, Builder.Project.Functions[tmpFVK].Suf);
                    if (Builder.Errors.Count > 0)
                        return;
                }
                else
                {
                    Builder.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + tmpFVK.Item1 + "(count: " + tmpFVK.Item2.ToString() + ") )"));
                    return;
                }
            }
            else
            {
                paramCount = GetFuncParamCountCalls(line, func, Builder.Project.MainVariables, Builder.Project.MainSuf);
                if (Builder.Errors.Count > 0)
                    return;
            }

            if (!Builder.Project.SubNames.ContainsKey(firstWord.Text))
            {
                var tmpTuple = new Tuple<string, int>(firstWord.Text, paramCount);
                if (!Builder.Project.FuncNames.Contains(tmpTuple))
                {
                    Builder.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + tmpTuple.Item1 + "(count: " + tmpTuple.Item2.ToString() + ") )"));
                    return;
                }
                else
                {
                    firstWord.Token = Tokens.FUNCNAME;
                    line.Type = LineType.FUNCCALL;
                    Builder.Project.FuncCalls.Add(new Tuple<int, string, int>(line.Number, firstWord.Text, paramCount));
                }
            }
            else
            {
                var tmpTuple = new Tuple<string, int>(firstWord.Text, paramCount);
                if (Builder.Project.FuncNames.Contains(tmpTuple))
                {
                    Builder.Errors.Add(new Errore(line.Number, line.FileName, 1807, "( " + tmpTuple.Item1 + "(count: " + tmpTuple.Item2.ToString() + ") )"));
                    return;
                }
                else
                {
                    Builder.Project.SubCalls.Add(new Tuple<int, string>(line.Number, firstWord.Text));
                }
            }
        }

        internal static void RenameLabels(Line line, bool func, Tuple<string, int> tmpFVK)
        {
            if (func)
            {
                if (Builder.Project.Functions.ContainsKey(tmpFVK))
                {
                    if (Builder.Project.Functions[tmpFVK].Labels.Contains(line.Words[1].Text.ToUpper()))
                    {
                        line.OutLine.Clear();
                        line.OutLine.Add("Goto " + (line.Words[1].Text + Builder.Project.Functions[tmpFVK].Suf).ToUpper());
                    }
                    else
                    {
                        Builder.Errors.Add(new Errore(line.Number, line.FileName, 1035, "( " + line.Words[1].Text + " )"));
                        return;
                    }
                }
                else
                {
                    Builder.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + tmpFVK.Item1 + "(count: " + tmpFVK.Item2.ToString() + ") )"));
                    return;
                }
            }
            else
            {
                if (Builder.Project.Labels.Contains(line.Words[1].Text.ToUpper()))
                {
                    line.OutLine.Clear();
                    line.OutLine.Add("Goto " + (line.Words[1].Text + Builder.Project.MainSuf).ToUpper());
                }
                else
                {
                    Builder.Errors.Add(new Errore(line.Number, line.FileName, 1036, "( " + line.Words[1].Text + " )"));
                    return;
                }
            }
        }
        */

        private static int Code(string keyword)
        {
            if (keyword == "endif")
            {
                return 1015;
            }
            else if (keyword == "endfor")
            {
                return 1016;
            }
            else if (keyword == "endwhile")
            {
                return 1017;
            }
            else if (keyword == "endsub")
            {
                return 1018;
            }
            else if (keyword == "endfunction")
            {
                return 1029;
            }
            else
            {
                return 1019;
            }
        }

        /*
        private static int GetFuncParamCountCalls(Line line, bool func, Dictionary<string, Variable> variables, string currentSuf)
        {
            int paramCount = 0;

            if (line.NextWord != null && line.NextWord.Token != Tokens.BRACKETLEFT && line.NextWord.Token != Tokens.DOUBLEBRACKET && line.LastWord.Token != Tokens.BRACKETRIGHT)
            {
                Builder.Errors.Add(new Errore(line.Number, line.FileName, 1802, "( " + line.Words[0].Text + " )"));
                return paramCount;
            }


            //if (line.NextWord != null && line.NextWord.Token == Tokens.DOUBLEBRACKET)
            //{
                //return paramCount;
            //}


            var param = MethodErrorParser.GetParam(0, line.Count - 1, line);

            var tuple = new Tuple<string, int>(line.FirstWord.Text, param.Count);

            if (!Builder.Project.Functions.ContainsKey(tuple))
            {
                if (!Builder.Project.SubNames.ContainsKey(line.Words[0].Text))
                {
                    Builder.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + tuple.Item1 + "(count: " + tuple.Item2.ToString() + ") )"));
                    return paramCount;
                }
                else
                {
                    return 0;
                }
            }

            var function = Builder.Project.Functions[tuple];

            if (param.Count != function.Inputs.Count + function.Outputs.Count)
            {
                Builder.Errors.Add(new Errore(line.Number, line.FileName, 1304, ""));
                return paramCount;
            }

            line.OutLine.Clear();

            if (function.Inputs.Count > 0)
            {
                for (int i = 0; i < function.Inputs.Count; i++)
                {
                    var type = VariableType.NON;

                    type = MethodErrorParser.ParseOneParam(param[i], variables, line, func);
                    if (Builder.Errors.Count > 0)
                        return paramCount;

                    if (type != function.InputsType[i])
                    {
                        Builder.Errors.Add(new Errore(line.Number, line.FileName, 1820, ""));
                        return paramCount;
                    }

                    string tmpStr = "";
                    foreach (var w in param[i])
                    {
                        if (w.Item1.Token == Tokens.VARIABLE)
                        {
                            tmpStr += w.Item1.Text + currentSuf + " ";
                        }
                        else
                        {
                            tmpStr += w.Item1.Text + " ";
                        }
                    }
                    line.OutLine.Add(function.InputsOut[i] + " = " + tmpStr.Trim());
                }
            }
            if (function.Outputs.Count > 0)
            {
                for (int i = function.Inputs.Count; i < function.Inputs.Count + function.Outputs.Count; i++)
                {
                    if (i < param.Count)
                    {
                        var w = param[i];
                        if (w.Count > 0)
                        {
                            if (w.Count != 1 && w[0].Item1.Token != Tokens.VARIABLE)
                            {
                                Builder.Errors.Add(new Errore(line.Number, line.FileName, 1821, "( " + w[0].Item1.Text + " )"));
                                return paramCount;
                            }
                        }
                        else
                        {
                            Builder.Errors.Add(new Errore(line.Number, line.FileName, 1821, ""));
                            return paramCount;
                        }

                        if (variables.ContainsKey(w[0].Item1.Text))
                        {
                            if (variables[w[0].Item1.Text].Type != function.OutputsType[i - function.Inputs.Count])
                            {
                                Builder.Errors.Add(new Errore(line.Number, line.FileName, 1820, ""));
                                return paramCount;
                            }
                        }
                        else
                        {
                            variables.Add(w[0].Item1.Text, new Variable() { Init = true, OldName = w[0].Item1.Text, NewName = w[0].Item1.Text + currentSuf, Type = function.OutputsType[i - function.Inputs.Count] });
                        }

                    }
                }
            }

            line.OutLine.Add(line.FirstWord.Text + "_" + param.Count.ToString() + "()");

            if (function.Outputs.Count > 0)
            {
                for (int i = function.Inputs.Count; i < function.Inputs.Count + function.Outputs.Count; i++)
                {
                    if (i < param.Count)
                    {
                        var w = param[i];

                        line.OutLine.Add(w[0].Item1.Text + currentSuf + " = " + function.OutputsOut[i - function.Inputs.Count]);
                    }
                }
            }

            function.Call = true;

            return param.Count;
        }
        */

        internal static void ParseNames(List<Line> lines)
        {
            var funcNames = new List<string>();
            var subNames = new List<string>();
            var labelNames = new Dictionary<string, Line>();

            var labelCalls = new Dictionary<string, Line>();

            var currentName = "";

            bool func = false;

            foreach (var line in lines)
            {
                if (line.Type == LineType.ONEKEYWORD && line.Words[0].ToLower() == "endfunction")
                {
                    func = false;
                    currentName = "";
                }
                if (line.Type == LineType.SUBINIT)
                {
                    if (line.Words.Count == 1)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1603, ""));
                        return;
                    }
                    else if (line.Words.Count >= 2 && line.Words[1].Token != Tokens.SUBNAME)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1602, ""));
                        return;
                    }
                    else if (line.Words.Count > 2)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1601, ""));
                        return;
                    }

                    var name = line.Words[1].ToLower();

                    if (!subNames.Contains(name))
                    {
                        if (funcNames.Contains(name))
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1809, " " + name));
                            return;
                        }
                        else
                        {
                            subNames.Add(name);
                        }
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1607, " " + name));
                        return;
                    }
                }
                else if (line.Type == LineType.FUNCINIT)
                {
                    func = true;

                    if (line.Words.Count == 1)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1803, ""));
                        return;
                    }
                    else if (line.Words.Count >= 2 && line.Words[1].Token != Tokens.FUNCNAME)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1805, ""));
                        return;
                    }
                    else if (line.Words.Count == 2)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1801, ""));
                        return;
                    }
                    else if (line.LastWord.Token != Tokens.BRACKETRIGHT && line.LastWord.Token != Tokens.DOUBLEBRACKET)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1802, ""));
                        return;
                    }

                    var name = line.Words[1].ToLower() + GetParamCount(line, true).ToString();
                    currentName = name;

                    if (!funcNames.Contains(name))
                    {
                        if (subNames.Contains(name))
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1607, " " + name));
                            return;
                        }
                        else
                        {
                            funcNames.Add(name);
                        }
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1809, " " + name));
                        return;
                    }
                }
                else if (line.Type == LineType.LABELINIT)
                {
                    var name = line.Words[0].ToLower().Replace(":","") + "*" + currentName;

                    if (!labelNames.ContainsKey(name))
                    {
                        labelNames.Add(name, line);
                    }
                    else
                    {
                        if (!func)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1025, " " + name));
                            return;
                        }
                        else
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1024, " " + name));
                            return;
                        }
                    }
                }
                else if (line.Type == LineType.LABELCALL)
                {
                    var name = line.Words[1].ToLower() + "*" + currentName;

                    if (!labelCalls.ContainsKey(name))
                    {
                        labelCalls.Add(name, line);
                    }
                }
            }

            foreach (var lc in labelCalls)
            {
                if (!labelNames.ContainsKey(lc.Key))
                {
                    if (lc.Key.Length > lc.Key.IndexOf("*") + 1)
                    {
                        Data.Errors.Add(new Errore(lc.Value.Number, lc.Value.FileName, 1035, " " + lc.Key.Substring(0, lc.Key.IndexOf("*"))));
                        return;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(lc.Value.Number, lc.Value.FileName, 1036, " " + lc.Key.Substring(0, lc.Key.IndexOf("*"))));
                        return;
                    }
                }
                else
                {
                    labelNames.Remove(lc.Key);
                }
            }

            if (labelNames.Count > 0)
            {
                // Ошибка есть метка, но к ней нет ни одного перехода
            }
        }

        private static int GetParamCount(Line line, bool funcInit)
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
    }
}

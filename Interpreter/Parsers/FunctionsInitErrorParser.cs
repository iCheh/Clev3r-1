using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class FunctionsInitErrorParser
    {
        internal static void Start()
        {
        }

        internal static void FuncInitLineParse(Line line)
        {
            bool start = false;
            bool input = false;
            bool output = false;
            int vars = 0;
            string name = "";
            Function function = null;
            VariableType tmpType = VariableType.NON;

            if (line.Count < 3)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1801, "( " + line.OldLine + " )"));
                return;
            }

            if (line.Words[1].Token != Tokens.FUNCNAME)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1803, "( " + line.Words[1].Text + " = " + line.Words[1].Token + " )"));
                return;
            }
            else
            {
                name = line.Words[1].Text;

                if (!Data.Project.Functions.ContainsKey(name))
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + line.Words[1].OriginText + " )"));
                    return;
                }

                function = Data.Project.Functions[name];
            }

            if (line.Words[2].Token != Tokens.BRACKETLEFT && line.Words[2].Token != Tokens.DOUBLEBRACKET)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1802, ""));
                return;
            }

            if (line.Words[2].Token == Tokens.DOUBLEBRACKET)
            {
                //ParamCount = 0;
                return;
            }

            for (int i = 2; i < line.Count; i++)
            {
                var word = line.Words[i];

                if (word.Token == Tokens.BRACKETLEFT)
                {
                    if (!start)
                    {
                        start = true;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1805, "( " + line.Words[i].Text + " )"));
                        return;
                    }
                }
                else if (word.Token == Tokens.KEYWORD)
                {
                    if (word.ToLower() == "in")
                    {
                        if (vars == 0)
                        {
                            vars = 1;
                            input = true;
                            output = false;
                        }
                        else
                        {
                            if (vars == 1)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1813, ""));
                                return;
                            }
                            else if (vars == 2)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1817, ""));
                                return;
                            }
                            else if (vars == 3)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1811, ""));
                                return;
                            }
                        }
                    }
                    else if (word.ToLower() == "out")
                    {
                        if (vars == 0)
                        {
                            vars = 1;
                            input = false;
                            output = true;
                        }
                        else
                        {
                            if (vars == 1)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1813, ""));
                                return;
                            }
                            else if (vars == 2)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1817, ""));
                                return;
                            }
                            else if (vars == 3)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1811, ""));
                                return;
                            }
                        }
                    }
                    else if (word.ToLower() == "number")
                    {
                        if (vars == 1)
                        {
                            vars = 2;
                            tmpType = VariableType.NUMBER;
                        }
                        else
                        {
                            if (vars == 0)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1816, ""));
                                return;
                            }
                            else if (vars == 2)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1817, ""));
                                return;
                            }
                            else if (vars == 3)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1816, ""));
                                return;
                            }
                        }
                    }
                    else if (word.ToLower() == "number[]")
                    {
                        if (vars == 1)
                        {
                            vars = 2;
                            tmpType = VariableType.NUMBER_ARRAY;
                        }
                        else
                        {
                            if (vars == 0)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1816, ""));
                                return;
                            }
                            else if (vars == 2)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1817, ""));
                                return;
                            }
                            else if (vars == 3)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1816, ""));
                                return;
                            }
                        }
                    }
                    else if (word.ToLower() == "string")
                    {
                        if (vars == 1)
                        {
                            vars = 2;
                            tmpType = VariableType.STRING;
                        }
                        else
                        {
                            if (vars == 0)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1816, ""));
                                return;
                            }
                            else if (vars == 2)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1817, ""));
                                return;
                            }
                            else if (vars == 3)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1816, ""));
                                return;
                            }
                        }
                    }
                    else if (word.ToLower() == "string[]")
                    {
                        if (vars == 1)
                        {
                            vars = 2;
                            tmpType = VariableType.STRING_ARRAY;
                        }
                        else
                        {
                            if (vars == 0)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1816, ""));
                                return;
                            }
                            else if (vars == 2)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1817, ""));
                                return;
                            }
                            else if (vars == 3)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1816, ""));
                                return;
                            }
                        }
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1804, "( " + line.Words[i].Text + " )"));
                        return;
                    }
                }
                else if (word.Token == Tokens.VARIABLE)
                {
                    if (vars == 2)
                    {
                        vars = 3;
                        if (word.Text.IndexOf("gv_") == 0)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1822, "( " + line.Words[i].Text + " )"));
                            return;
                        }
                        if (tmpType == VariableType.NON)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1819, "( " + line.Words[i].Text + " )"));
                            return;
                        }

                        if (input)
                        {
                            var paramName = word.ToLower();

                            if (function == null)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + line.Words[1].OriginText + " )"));
                                return;
                            }

                            if (!function.Parameters.ContainsKey(paramName))
                            {
                                function.Parameters.Add(paramName, (varType: tmpType, paramType: ParameterType.INPUT));
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1810, " " + word.OriginText));
                                return;
                            }
                        }
                        else if (output)
                        {
                            var paramName = word.ToLower();

                            if (function == null)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1806, "( " + line.Words[1].OriginText + " )"));
                                return;
                            }

                            if (!function.Parameters.ContainsKey(paramName))
                            {
                                function.Parameters.Add(paramName, (varType: tmpType, paramType: ParameterType.OUTPUT));
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1810, " " + word.OriginText));
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (vars == 0)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1818, ""));
                            return;
                        }
                        else if (vars == 1)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1814, ""));
                            return;
                        }
                        else if (vars == 3)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1811, ""));
                            return;
                        }
                    }
                }
                else if (word.Token == Tokens.COMMA)
                {
                    if (vars == 3)
                    {
                        vars = 0;
                        input = false;
                        output = false;
                    }
                    else
                    {
                        if (vars == 1)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1813, "( " + line.Words[i].Text + " )"));
                            return;
                        }
                        else if (vars == 2)
                        {

                        }
                        else if (vars == 0)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1812, "( " + line.Words[i].Text + " )"));
                            return;
                        }
                    }
                }
                else if (word.Token == Tokens.BRACKETRIGHT)
                {
                    if (i != line.Count - 1)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1805, "( " + line.Words[i].Text + " )"));
                        return;
                    }
                }
                else if (word.Token == Tokens.DOUBLEBRACKET)
                {
                    if (i != line.Count - 1)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1805, "( " + line.Words[i].Text + " )"));
                        return;
                    }
                }
                else
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1805, "( " + line.Words[i].Text + " )"));
                    return;
                }
            }

            if (vars == 1)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1813, ""));
                return;
            }
            else if (vars == 2)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1817, ""));
                return;
            }
            else if (vars == 0)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1815, ""));
                return;
            }
        }
    }
}

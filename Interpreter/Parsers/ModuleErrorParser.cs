using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class ModuleErrorParser
    {
        internal static void Start(List<Line> lines)
        {
            var func = false;
            var propertys = new HashSet<string>();
            var functions = new HashSet<string>();

            // Пропарсим модуль и заполним словарь свойств
            ParsePropertys(lines, propertys);
            if (Data.Errors.Count > 0)
                return;

            foreach (var line in lines)
            {
                StructErrorParser.Start(lines);
                if (Data.Errors.Count > 0)
                    return;

                if (line.Type == LineType.FUNCINIT)
                {
                    if (func)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1026, ""));
                        return;
                    }

                    ParseFuncInit(line, functions, propertys);
                    if (Data.Errors.Count > 0)
                        return;

                    func = true;
                }
                else if (line.Type == LineType.SUBINIT)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 2007, ""));
                    return;
                }
                else if (line.Type == LineType.INCLUDE)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 2005, ""));
                    return;
                }
                else if (line.Type == LineType.FOLDER)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 2006, ""));
                    return;
                }
                else if (line.Type == LineType.ONEKEYWORD)
                {
                    if (line.Words[0].ToLower() == "private")
                    {
                        if (line.Count > 1)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2016, ""));
                            return;
                        }
                    }
                    else if (line.Words[0].ToLower() == "endfunction")
                    {
                        func = false;
                    }
                }
                else if (line.Type == LineType.NON)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1032, ""));
                    return;
                }
                else if (line.Type == LineType.NUMBERINIT || line.Type == LineType.NUMBERARRAYINIT || line.Type == LineType.STRINGINIT || line.Type == LineType.STRINGARRAYINIT || line.Type == LineType.MODULEPROPERTY)
                {
                    continue;
                }
                else
                {
                    if (!func)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2008, ""));
                        return;
                    }
                }
            }
        }

        private static void ParseFuncInit(Line line, HashSet<string> functions, HashSet<string> propertys)
        {
            var variables = new HashSet<string>();
            //bool input = false;
            //bool output = false;
            int vars = 0;
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

            if (line.Words[2].Token != Tokens.BRACKETLEFT && line.Words[2].Token != Tokens.DOUBLEBRACKET)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1802, ""));
                return;
            }           

            var name = line.Words[1].ToLower();

            if (line.Words[2].Token == Tokens.DOUBLEBRACKET)
            {
                if (functions.Contains(name))
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1809, "( " + line.Words[1].OriginText + " (0) " + " )"));
                    return;
                }

                functions.Add(name);

                return;
            }

            if (line.LastWord.Token != Tokens.BRACKETRIGHT)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1802, ""));
                return;
            }

            if (line.LastWord.Token != Tokens.BRACKETLEFT)
            {
                if (line.Count - 1 <= 3)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1801, ""));
                    return;
                }

                for (int i = 3; i < line.Count - 1; i++)
                {
                    var word = line.Words[i];

                    if (word.Token == Tokens.VARIABLE)
                    {
                        if (word.Text.IndexOf("@") != -1)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1822, "( " + word.OriginText +  " )"));
                            return;
                        }

                        if (propertys.Contains(word.ToLower()))
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2014, "( " + word.OriginText + " )"));
                            return;
                        }

                        if (variables.Contains(word.ToLower()))
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1810, "( " + word.OriginText + " )"));
                            return;
                        }
                        else
                        {
                            variables.Add(word.ToLower());
                        }
                    }

                    if (word.Token == Tokens.KEYWORD)
                    {
                        if (word.ToLower() == "in")
                        {
                            if (vars == 0)
                            {
                                vars = 1;
                                //input = true;
                                //output = false;
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
                                //input = false;
                                //output = true;
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

                            if (tmpType == VariableType.NON)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1819, "( " + line.Words[i].Text + " )"));
                                return;
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
                            //input = false;
                            //output = false;
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
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1805, "( " + line.Words[i].Text + " )"));
                        return;
                    }
                }
                
                if (vars != 3)
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
                    else if (vars == 2)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1817, ""));
                        return;
                    }
                }
            }
        }

        private static void ParsePropertys(List<Line> lines, HashSet<string> propertys)
        {
            var func = false;

            foreach (var line in lines)
            {
                if (line.Type == LineType.FUNCINIT)
                {
                    func = true;
                }
                else if (line.Type == LineType.ONEKEYWORD && line.Words[0].ToLower() == "endfunction")
                {
                    func = false;
                }
                else if (line.Type == LineType.NUMBERINIT)
                {
                    if (func)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2015, ""));
                        return;
                    }

                    if (line.Count > 2)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2011, ""));
                        return;
                    }

                    if (line.Words[0].ToLower() != "number")
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2013, ""));
                        return;
                    }

                    if (line.Words[1].Token != Tokens.VARIABLE)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2012, ""));
                        return;
                    }
                    else
                    {
                        var name = line.Words[1].ToLower();

                        if (propertys.Contains(name))
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2014, "( " + line.Words[1].OriginText + " )"));
                            return;
                        }
                        else
                        {
                            propertys.Add(name);
                        }
                    }
                }
                else if (line.Type == LineType.NUMBERARRAYINIT)
                {
                    if (func)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2015, ""));
                        return;
                    }

                    if (line.Count > 2)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2011, ""));
                        return;
                    }

                    if (line.Words[0].ToLower() != "number[]")
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2013, ""));
                        return;
                    }

                    if (line.Words[1].Token != Tokens.VARIABLE)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2012, ""));
                        return;
                    }
                    else
                    {
                        var name = line.Words[1].ToLower();

                        if (propertys.Contains(name))
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2014, "( " + line.Words[1].OriginText + " )"));
                            return;
                        }
                        else
                        {
                            propertys.Add(name);
                        }
                    }
                }
                else if (line.Type == LineType.STRINGINIT)
                {
                    if (func)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2015, ""));
                        return;
                    }

                    if (line.Count > 2)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2011, ""));
                        return;
                    }

                    if (line.Words[0].ToLower() != "string")
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2013, ""));
                        return;
                    }

                    if (line.Words[1].Token != Tokens.VARIABLE)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2012, ""));
                        return;
                    }
                    else
                    {
                        var name = line.Words[1].ToLower();

                        if (propertys.Contains(name))
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2014, "( " + line.Words[1].OriginText + " )"));
                            return;
                        }
                        else
                        {
                            propertys.Add(name);
                        }
                    }
                }
                else if (line.Type == LineType.STRINGARRAYINIT)
                {
                    if (func)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2015, ""));
                        return;
                    }

                    if (line.Count > 2)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2011, ""));
                        return;
                    }

                    if (line.Words[0].ToLower() != "string[]")
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2013, ""));
                        return;
                    }

                    if (line.Words[1].Token != Tokens.VARIABLE)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2012, ""));
                        return;
                    }
                    else
                    {
                        var name = line.Words[1].ToLower();

                        if (propertys.Contains(name))
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 2014, "( " + line.Words[1].OriginText + " )"));
                            return;
                        }
                        else
                        {
                            propertys.Add(name);
                        }
                    }
                }
            }
        }

        internal static void ParsePropertyInFuncInit(Line line, Dictionary<string, (Line, bool)> propertys, string moduleName)
        {
            foreach (var word in line.Words)
            {
                if (word.Token == Tokens.VARIABLE)
                {
                    var name = moduleName.ToLower() + "_" + word.ToLower();

                    if (propertys.ContainsKey(name))
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 2019, ""));
                        return;
                    }
                }
            }
        }
    }
}

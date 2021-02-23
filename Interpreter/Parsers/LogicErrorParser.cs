using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class LogicErrorParser
    {
        internal static void Start(Line line, int startPos, Dictionary<string, Variable> variables)
        {
            LastIndex = 0;
            bool logic = true;
            var type = VariableType.NON;
            bool math = true;
            bool plus = false;
            bool addMath = false;
            bool firstOperand = true;
            bool doubleOperand = false;
            bool oneOperand = false;

            for (int i = startPos; i < line.Count; i++)
            {

                var word = line.Words[i];

                if (word.Token == Tokens.BOOLOPERATOR || word.Token == Tokens.EQU)
                {
                    oneOperand = false;

                    if (logic)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1909, ""));
                        return;
                    }

                    if (firstOperand)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1910, ""));
                        return;
                    }

                    if (doubleOperand)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1912, ""));
                        return;
                    }

                    logic = true;

                    if (!math)
                    {
                        math = true;
                    }
                    else
                    {
                        if (word.Text == "-")
                        {
                            if (i != startPos + 1)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                                return;
                            }
                            else if (i - 1 >= 0 && i - 1 < line.Count && line.Words[i - 1].Token != Tokens.BRACKETLEFT)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                                return;
                            }
                        }
                    }

                    continue;
                }
                if (word.Token == Tokens.MATHOPERATOR)
                {
                    if (i - 1 >= 0 && line.Words[i - 1].Token != Tokens.VARIABLE && line.Words[i - 1].Token != Tokens.NUMBER && line.Words[i - 1].Token != Tokens.METHOD && line.Words[i - 1].Token != Tokens.STRING && line.Words[i - 1].Token != Tokens.BRACKETRIGHT && line.Words[i - 1].Token != Tokens.BRACKETRIGHTARRAY && line.Words[i - 1].Token != Tokens.DOUBLEBRACKET && line.Words[i - 1].Token != Tokens.DOUBLEBRACKETARRAY)
                    {
                        if (word.Text != "-")
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1409, ""));
                            return;
                        }
                    }

                    addMath = true;

                    if (!firstOperand && !doubleOperand && !logic)
                    {
                        firstOperand = true;
                        oneOperand = true;
                    }
                    else if (!firstOperand && doubleOperand)
                        doubleOperand = false;

                    logic = true;

                    plus = false;
                    if (word.Text == "+")
                    {
                        plus = true;
                    }

                    if (!math)
                    {
                        math = true;
                    }
                    else
                    {
                        if (word.Text == "-")
                        {
                            if (i + 1 < line.Count && line.Words[i + 1].Token != Tokens.NUMBER && line.Words[i + 1].Token != Tokens.VARIABLE && line.Words[i + 1].Token != Tokens.METHOD)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, "2"));
                                return;
                            }
                        }
                    }
                }
                else if (word.Token == Tokens.NUMBER)
                {
                    if ((!firstOperand && doubleOperand) || doubleOperand)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1912, ""));
                        return;
                    }

                    if (!doubleOperand && !firstOperand)
                    {
                        doubleOperand = true;
                        oneOperand = false;
                    }
                    if (firstOperand)
                        firstOperand = false;

                    if (!logic)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1905, ""));
                        return;
                    }

                    logic = false;

                    if (math)
                    {
                        math = false;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                        return;
                    }

                    if (type == VariableType.NON)
                    {
                        type = VariableType.NUMBER;
                    }
                    else if (type != VariableType.NUMBER && type != VariableType.ANY)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                        return;
                    }
                }
                else if (word.Token == Tokens.STRING)
                {
                    oneOperand = true;

                    if (!logic)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1905, ""));
                        return;
                    }

                    if (!plus && addMath)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                        return;
                    }

                    logic = false;

                    if (math)
                    {
                        math = false;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                        return;
                    }

                    if (type == VariableType.NON)
                    {
                        type = VariableType.STRING;
                    }
                    else if (type != VariableType.STRING && type != VariableType.ANY)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                        return;
                    }
                }
                else if (word.Token == Tokens.VARIABLE)
                {
                    if ((!firstOperand && doubleOperand) || doubleOperand)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1912, ""));
                        return;
                    }

                    if (!doubleOperand && !firstOperand)
                    {
                        doubleOperand = true;
                        oneOperand = false;
                    }
                    if (firstOperand)
                        firstOperand = false;

                    if (!logic)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1905, ""));
                        return;
                    }

                    logic = false;

                    if (math)
                    {
                        math = false;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                        return;
                    }

                    if (!variables.ContainsKey(word.Text))
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1405, "( " + word.OriginText + " )"));
                        return;
                    }
                    else
                    {
                        var v = variables[word.Text];
                        if (v.Type == VariableType.NUMBER_ARRAY)
                        {
                            if (i + 1 < line.Count && line.Words[i + 1].Token == Tokens.BRACKETLEFTARRAY)
                            {
                                ArrayIndexErrorParser.Start(line, i + 1, variables);
                                if (Data.Errors.Count > 0)
                                    return;

                                i = ArrayIndexErrorParser.LastIndex;

                                if (type == VariableType.NON)
                                {
                                    type = VariableType.NUMBER;
                                }
                                else if (type != VariableType.NUMBER && type != VariableType.ANY)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                    return;
                                }

                                if (line.Words[LastIndex].Token == Tokens.BRACKETRIGHTARRAY)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1906, "( " + word.Text + " )"));
                                return;
                            }
                        }
                        else if (v.Type == VariableType.STRING_ARRAY)
                        {
                            if (i + 1 < line.Count && line.Words[i + 1].Token == Tokens.BRACKETLEFTARRAY)
                            {
                                ArrayIndexErrorParser.Start(line, i + 1, variables);
                                if (Data.Errors.Count > 0)
                                    return;

                                i = ArrayIndexErrorParser.LastIndex;

                                if (!plus && addMath)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                                    return;
                                }

                                if (type == VariableType.NON)
                                {
                                    type = VariableType.STRING;
                                }
                                else if (type != VariableType.STRING && type != VariableType.ANY)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                    return;
                                }

                                if (line.Words[LastIndex].Token == Tokens.BRACKETRIGHTARRAY)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1906, "( " + word.Text + " )"));
                                return;
                            }
                        }
                        else if (v.Type == VariableType.NUMBER)
                        {
                            if (type == VariableType.NON)
                            {
                                type = VariableType.NUMBER;
                            }
                            else if (type != VariableType.NUMBER && type != VariableType.ANY)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                return;
                            }
                        }
                        else if (v.Type == VariableType.STRING)
                        {
                            if (!plus && addMath)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                                return;
                            }

                            if (type == VariableType.NON)
                            {
                                type = VariableType.STRING;
                            }
                            else if (type != VariableType.STRING && type != VariableType.ANY)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                return;
                            }
                        }
                        else
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1906, "( " + word.Text + " )"));
                            return;
                        }
                    }


                }
                else if (word.Token == Tokens.METHOD)
                {
                    if ((!firstOperand && doubleOperand) || doubleOperand)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1912, ""));
                        return;
                    }

                    //Console.WriteLine("=> " + firstOperand + "  " + doubleOperand);
                    if (!doubleOperand && !firstOperand)
                    {
                        doubleOperand = true;
                        oneOperand = false;
                    }
                    if (firstOperand)
                        firstOperand = false;
                    //Console.WriteLine("=> " + firstOperand + "  " + doubleOperand);

                    if (!logic)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1905, ""));
                        return;
                    }

                    logic = false;

                    if (math)
                    {
                        math = false;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                        return;
                    }

                    var lastIndex = MethodErrorParser.GetMethodLastIndex(i, line, word.Text);
                    var param = MethodErrorParser.GetParam(i, lastIndex, line);
                    MethodErrorParser.Start(param, word.Text, variables, line);

                    if (Data.Errors.Count > 0)
                        return;

                    var sign = DefaultObjectList.Objects[word.ToLower()];

                    if (sign.OutputType == VariableType.NON)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1313, ""));
                        return;
                    }
                    else if (sign.OutputType == VariableType.STRING)
                    {
                        //Console.WriteLine("=> " + oneOperand);

                        if (!plus && addMath)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                            return;
                        }
                    }

                    i = lastIndex;

                    if (type == VariableType.NON)
                    {
                        type = sign.OutputType;
                    }
                    else if (sign.OutputType != VariableType.ANY && type != sign.OutputType)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                        return;
                    }

                    if (sign.OutputType == VariableType.ANY || sign.OutputType == VariableType.STRING)
                    {
                        if (!doubleOperand)
                            oneOperand = true;
                    }
                }
                else if (word.Token == Tokens.BRACKETLEFT || word.Token == Tokens.BRACKETRIGHT)
                {
                    continue;
                }
                else if (word.Token == Tokens.KEYWORD)
                {
                    if (word.ToLower() == "and" || word.ToLower() == "or" || word.ToLower() == "then")
                    {
                        if (word.ToLower() == "and" || word.ToLower() == "or")
                        {
                            if (i + 1 < line.Count && (line.Words[i + 1].ToLower() == "and" || line.Words[i + 1].ToLower() == "or" || line.Words[i + 1].ToLower() == "then"))
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1912, ""));
                                return;
                            }
                        }
                        if (logic && !doubleOperand)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1911, ""));
                            return;
                        }
                        if (!firstOperand && !doubleOperand && !oneOperand)
                        {
                            //Console.WriteLine(firstOperand + " " + doubleOperand + " " + oneOperand);
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1911, ""));
                            return;
                        }

                        LastIndex = i;
                        return;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1034, "( " + word.Text + " )"));
                        return;
                    }
                }
                else
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1034, "( " + word.Text + " )"));
                    return;
                }
            }

            if (logic && !doubleOperand)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1911, ""));
                return;
            }
            if (!firstOperand && !doubleOperand && !oneOperand)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1911, ""));
                return;
            }

            LastIndex = line.Count;
        }

        internal static int LastIndex { get; set; }
    }
}

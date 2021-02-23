using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class ForLineErrorParser
    {
        internal static void Start(Line line, Dictionary<string, Variable> variables)
        {
            var type = VariableType.NON;
            int lastPos = 0;
            int lastPosStep = 0;
            bool step = false;

            if (line.NewLine.ToLower().IndexOf("to") == -1)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1901, ""));
                return;
            }

            if (line.Count < 6)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1902, ""));
                return;
            }

            if (line.Words[1].Token != Tokens.VARIABLE && line.Words[2].Token != Tokens.EQU)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1903, ""));
                return;
            }

            if (variables.ContainsKey(line.Words[1].Text))
            {
                if (variables[line.Words[1].Text].Type != VariableType.NUMBER)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1904, ""));
                    return;
                }

                type = VariableType.NUMBER;
            }

            for (int i = 0; i < line.Words.Count; i++)
            {
                if (line.Words[i].ToLower() == "to")
                {
                    lastPos = i;
                    continue;
                }
                else if (line.Words[i].ToLower() == "step")
                {
                    lastPosStep = i;
                    step = true;
                    break;
                }
            }

            VariableErrorParser.ParseInitVarError(line, type, false, 3, lastPos, 6);

            if (Data.Errors.Count > 0)
                return;

            if (variables.ContainsKey(line.Words[1].Text))
            {
                if (variables[line.Words[1].Text].Type != VariableType.NUMBER)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1904, ""));
                    return;
                }
            }

            if (step)
            {
                if (ParseValue(lastPos + 1, lastPosStep, line, variables, step))
                    return;
                if (ParseValue(lastPosStep + 1, line.Count, line, variables, step))
                    return;
            }
            else
            {
                if (ParseValue(lastPos + 1, line.Count, line, variables, step))
                    return;
            }
        }

        private static bool ParseValue(int startPos, int lastPos, Line line, Dictionary<string, Variable> variables, bool step)
        {
            bool math = true;

            for (int i = startPos; i < lastPos; i++)
            {
                var word = line.Words[i];

                if (word.Token == Tokens.BRACKETLEFTARRAY)
                {
                    ArrayIndexErrorParser.Start(line, 1, variables);
                    if (Data.Errors.Count > 0)
                        return true;

                    i = ArrayIndexErrorParser.LastIndex;

                    if (line.Words[ArrayIndexErrorParser.LastIndex].Token == Tokens.BRACKETRIGHTARRAY)
                    {
                        continue;
                    }
                }
                else if (word.Token == Tokens.MATHOPERATOR)
                {
                    if (!math)
                    {
                        math = true;
                    }
                    else
                    {
                        if (word.Text == "-")
                        {
                            if (i == startPos && step)
                                continue;

                            if (i + 1 < line.Count && line.Words[i + 1].Token != Tokens.NUMBER && line.Words[i + 1].Token != Tokens.VARIABLE && line.Words[i + 1].Token != Tokens.METHOD)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                                return true;
                            }
                        }
                    }
                }
                else if (word.Token == Tokens.NUMBER)
                {
                    if (math)
                    {
                        math = false;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                        return true;
                    }
                }
                else if (word.Token == Tokens.VARIABLE)
                {
                    if (math)
                    {
                        math = false;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                        return true;
                    }

                    if (!variables.ContainsKey(word.Text))
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1405, "( " + word.OriginText + " )"));
                        return true;
                    }
                    else
                    {
                        var v = variables[word.Text];
                        if (v.Type == VariableType.NUMBER_ARRAY)
                        {
                            if (i + 1 < lastPos && line.Words[i + 1].Token == Tokens.BRACKETLEFTARRAY)
                            {
                                ArrayIndexErrorParser.Start(line, i + 1, variables);
                                if (Data.Errors.Count > 0)
                                    return true;

                                i = ArrayIndexErrorParser.LastIndex;

                                if (line.Words[ArrayIndexErrorParser.LastIndex].Token == Tokens.BRACKETRIGHTARRAY)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1425, "( " + word.Text + " )"));
                                return true;
                            }
                        }
                        else if (v.Type != VariableType.NUMBER)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1423, "( " + word.Text + " )"));
                            return true;
                        }
                    }
                }
                else if (word.Token == Tokens.METHOD)
                {
                    if (math)
                    {
                        math = false;
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                        return true;
                    }

                    var lastIndex = MethodErrorParser.GetMethodLastIndex(i, line, word.Text);
                    var param = MethodErrorParser.GetParam(i, lastIndex, line);
                    MethodErrorParser.Start(param, word.Text, variables, line);

                    if (Data.Errors.Count > 0)
                        return true;

                    var sign = DefaultObjectList.Objects[word.ToLower()];

                    if (sign.OutputType != VariableType.NUMBER)
                    {
                        if (sign.OutputType != VariableType.ANY && word.ToLower() != "f.get")
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1424, ""));
                            return true;
                        }
                    }

                    i = lastIndex;
                }
                else if (word.Token == Tokens.BRACKETLEFT || word.Token == Tokens.BRACKETRIGHT)
                {
                    continue;
                }
                else
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1427, "( " + word.Text + " )"));
                    return true;
                }
            }

            return false;
        }
    }
}
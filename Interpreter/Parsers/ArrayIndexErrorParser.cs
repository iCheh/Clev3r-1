using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class ArrayIndexErrorParser
    {
        internal static void Start(Line line, int startPos, Dictionary<string, Variable> variables)
        {
            int bracket = 0;
            LastIndex = 0;
            bool math = true;

            for (int i = startPos; i < line.Count; i++)
            {
                var word = line.Words[i];
                if (word.Token == Tokens.BRACKETLEFTARRAY)
                {
                    bracket++;
                }
                else if (word.Token == Tokens.BRACKETRIGHTARRAY)
                {
                    bracket--;
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
                            if (i + 1 < line.Count && line.Words[i + 1].Token != Tokens.NUMBER && line.Words[i + 1].Token != Tokens.VARIABLE && line.Words[i + 1].Token != Tokens.METHOD)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1426, ""));
                                return;
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
                        return;
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
                                Start(line, i + 1, variables);
                                if (Data.Errors.Count > 0)
                                    return;

                                i = LastIndex;

                                if (line.Words[LastIndex].Token == Tokens.BRACKETRIGHTARRAY)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1425, "( " + word.Text + " )"));
                                return;
                            }
                        }
                        else if (v.Type != VariableType.NUMBER)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1423, "( " + word.Text + " )"));
                            return;
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
                        return;
                    }

                    var lastIndex = MethodErrorParser.GetMethodLastIndex(i, line, word.Text);
                    var param = MethodErrorParser.GetParam(i, lastIndex, line);
                    MethodErrorParser.Start(param, word.Text, variables, line);

                    if (Data.Errors.Count > 0)
                        return;

                    var sign = DefaultObjectList.Objects[word.ToLower()];

                    if (sign.OutputType != VariableType.NUMBER)
                    {
                        if (sign.OutputType != VariableType.ANY && word.ToLower() != "f.get")
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1424, ""));
                            return;
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
                    //Builder.Project.WriteTextAndTokens(line);
                    //Console.WriteLine(i);
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1427, "( " + word.Text + " )"));
                    return;
                }

                if (bracket == 0)
                {
                    LastIndex = i;
                    break;
                }
            }
        }

        internal static int LastIndex { get; set; }
    }
}

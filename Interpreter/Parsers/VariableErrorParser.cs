using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class VariableErrorParser
    {
        internal static void Start(List<Line> lines)
        {
            foreach (var line in lines)
            {
                if (line.Type == LineType.VARINIT)
                {
                    if (line.Words[0].Token == Tokens.VARIABLE)
                    {
                        if (Data.Project.Variables.ContainsKey(line.Words[0].Text))
                        {
                            var variable = Data.Project.Variables[line.Words[0].Text];
                            if (ParseInitVarError(line, variable.Type, false, 2, line.Count, 3))
                                return;
                        }
                        else
                        {
                            if (ParseInitVarError(line, VariableType.NON, false, 2, line.Count, 3))
                                return;
                        }
                    }
                    
                }
                else if (line.Type == LineType.VARARRAYINIT)
                {
                    if (ParseBracketLeftArray(line))
                        return;
                }
                else if (line.Type == LineType.VARDOUBLEMATH)
                {
                    if (ParseDoubleMathError(line))
                        return;
                }
                else if (line.Type == LineType.VAREQUMATH)
                {
                    if (ParseEquMathError(line))
                        return;
                }
                else if (line.Type == LineType.FORINIT)
                {
                    ForLineErrorParser.Start(line, Data.Project.Variables);
                    if (Data.Errors.Count > 0)
                        return;
                }
            }
        }

        internal static bool ParseInitVarError(Line line, VariableType type, bool array, int startPos, int endPos, int minimumWords)
        {
            bool math = true;
            bool plus = false;
            bool addMath = false;
            string varName = "";
            bool badString = false;
            var firstType = type;
            VariableType oldType = VariableType.NON;
            if (line.Words[0].Text.ToLower() != "for")
            {
                varName = line.Words[0].Text;
            }
            else
            {
                varName = line.Words[1].Text;
            }
            if (line.Count < minimumWords) // default 3
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1411, ""));
                return true;
            }

            for (int j = startPos; j < endPos; j++) // default startPos = 2, endPos = line.Count
            {
                oldType = type;
                var word = line.Words[j];
                var token = word.Token;
                if (token == Tokens.MATHOPERATOR)
                {
                    plus = false;
                    addMath = true;
                    if (word.Text == "+")
                    {
                        plus = true;
                    }
                    if (word.Text == "-")
                    {
                        if (j == 2)
                        {
                            if (line.Count >= 4 && line.Words[3].Token == Tokens.NUMBER)
                            {
                                if (!Data.Project.Variables.ContainsKey(varName))
                                {
                                    //Builder.Project.MainVariables.Add(varName, new Variable() { Init = true, OldName = varName, NewName = varName, Type = VariableType.NUMBER });
                                    if (type == VariableType.NON)
                                    {
                                        type = VariableType.NUMBER;
                                    }
                                    else if (type != VariableType.NUMBER)
                                    {
                                        if (firstType == VariableType.NON)
                                        {
                                            if (type == VariableType.STRING)
                                            {
                                                badString = true;
                                                type = VariableType.NUMBER;
                                            }
                                        }
                                        if (!badString)
                                        {
                                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    var v = Data.Project.Variables[varName];
                                    if (v.Type != VariableType.NUMBER)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                        return true;
                                    }
                                    return false;
                                }
                            }
                            else
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1415, ""));
                                return true;
                            }
                        }
                        else
                        {
                            if (!math)
                                math = true;
                            else
                            {
                                if (j + 1 < line.Words.Count && line.Words[j + 1] != null && (line.Words[j + 1].Token == Tokens.NUMBER || line.Words[j + 1].Token == Tokens.VARIABLE || line.Words[j + 1].Token == Tokens.METHOD))
                                {
                                    continue;
                                }
                                else
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1409, "( " + word.Text + " )"));
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!math)
                            math = true;
                        else
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1408, ""));
                            return true;
                        }
                    }
                }
                else if (token == Tokens.NUMBER)
                {
                    if (!math)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1416, ""));
                        return true;
                    }
                    else
                    {
                        math = false;
                    }
                    if (type == VariableType.NON)
                    {
                        type = VariableType.NUMBER;
                    }
                    else if (type != VariableType.NUMBER)
                    {
                        if (firstType == VariableType.NON)
                        {
                            if (type == VariableType.STRING)
                            {
                                badString = true;
                                type = VariableType.NUMBER;
                            }
                        }
                        else if (firstType == VariableType.STRING)
                        {
                            badString = true;
                        }

                        if (!badString)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                            return true;
                        }
                    }
                }
                else if (token == Tokens.STRING)
                {
                    if (!math)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1416, ""));
                        return true;
                    }
                    else
                    {
                        math = false;
                    }

                    if (type == VariableType.NON)
                    {
                        type = VariableType.STRING;
                    }
                    else if (type != VariableType.STRING)
                    {
                        if (firstType == VariableType.NON)
                        {
                            if (type == VariableType.NUMBER)
                            {
                                badString = true;
                                type = VariableType.STRING;
                            }
                        }
                        if (!badString)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                            return true;
                        }
                    }
                }
                else if (token == Tokens.VARIABLE)
                {
                    if (!VarContains(line, word.Text, word.OriginText))
                        return true;

                    var v = Data.Project.Variables[word.Text];

                    if (v.Type == VariableType.NUMBER)
                    {
                        if (!math)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1416, ""));
                            return true;
                        }
                        else
                        {
                            math = false;
                        }
                        if (type == VariableType.NON)
                        {
                            type = VariableType.NUMBER;
                        }
                        else if (type != VariableType.NUMBER)
                        {
                            if (firstType == VariableType.NON)
                            {
                                if (type == VariableType.STRING)
                                {
                                    badString = true;
                                    type = VariableType.NUMBER;
                                }
                            }
                            else if (firstType == VariableType.STRING)
                            {
                                badString = true;
                            }

                            if (!badString)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                return true;
                            }
                        }
                    }
                    else if (v.Type == VariableType.STRING)
                    {
                        if (!math)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1416, ""));
                            return true;
                        }
                        else
                        {
                            math = false;
                        }

                        if (type == VariableType.NON)
                        {
                            type = VariableType.STRING;
                        }
                        else if (type != VariableType.STRING)
                        {
                            if (firstType == VariableType.NON)
                            {
                                if (type == VariableType.NUMBER)
                                {
                                    badString = true;
                                    type = VariableType.STRING;
                                }
                            }
                            if (!badString)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                return true;
                            }
                        }
                    }
                    else if (v.Type == VariableType.NUMBER_ARRAY)
                    {
                        if (j + 1 < line.Count && line.Words[j + 1].Text == "[")
                        {
                            ArrayIndexErrorParser.Start(line, j + 1, Data.Project.Variables);
                            if (Data.Errors.Count > 0)
                                return true;

                            j = ArrayIndexErrorParser.LastIndex;

                            if (!math)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1416, ""));
                                return true;
                            }
                            else
                            {
                                math = false;
                            }
                            if (type == VariableType.NON)
                            {
                                type = VariableType.NUMBER;
                            }
                            else if (type != VariableType.NUMBER)
                            {
                                if (firstType == VariableType.NON)
                                {
                                    if (type == VariableType.STRING)
                                    {
                                        badString = true;
                                        type = VariableType.NUMBER;
                                    }
                                }
                                if (!badString)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (array)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1425, ""));
                                return true;
                            }
                            if (addMath)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1418, ""));
                                return true;
                            }

                            if (type == VariableType.NON)
                            {
                                type = VariableType.NUMBER_ARRAY;
                            }
                            else if (type != VariableType.NUMBER_ARRAY)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                return true;
                            }
                        }
                    }
                    else if (v.Type == VariableType.STRING_ARRAY)
                    {
                        if (j + 1 < line.Count && line.Words[j + 1].Text == "[")
                        {
                            ArrayIndexErrorParser.Start(line, j + 1, Data.Project.Variables);
                            if (Data.Errors.Count > 0)
                                return true;

                            j = ArrayIndexErrorParser.LastIndex;

                            if (!math)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1416, ""));
                                return true;
                            }
                            else
                            {
                                math = false;
                            }

                            if (type == VariableType.NON)
                            {
                                type = VariableType.STRING;
                            }
                            else if (type != VariableType.STRING)
                            {
                                if (firstType == VariableType.NON)
                                {
                                    if (type == VariableType.NUMBER)
                                    {
                                        badString = true;
                                        type = VariableType.STRING;
                                    }
                                }
                                if (!badString)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (array)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1425, ""));
                                return true;
                            }
                            if (addMath)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1418, ""));
                                return true;
                            }

                            if (type == VariableType.NON)
                            {
                                type = VariableType.STRING_ARRAY;
                            }
                            else if (type != VariableType.STRING_ARRAY)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                return true;
                            }
                        }
                    }
                }
                else if (token == Tokens.METHOD)
                {
                    var tmpIndex = MethodErrorParser.GetMethodLastIndex(j, line, word.Text);
                    if (tmpIndex == -1)
                    {
                        if (Data.Errors.Count > 0)
                            return true;

                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1314, ""));
                        return true;
                    }
                    else if (Data.Errors.Count > 0)
                    {
                        return true;
                    }
                    else if (ParseMethodError(j, word.ToLower(), line))
                    {
                        return true;
                    }

                    var sign = DefaultObjectList.Get(word.ToLower());

                    if (sign.OutputType == VariableType.NON)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1306, ""));
                        return true;
                    }
                    else if (sign.OutputType == VariableType.NUMBER)
                    {
                        if (!math)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1416, ""));
                            return true;
                        }
                        else
                        {
                            math = false;
                        }
                        if (type == VariableType.NON)
                        {
                            type = VariableType.NUMBER;
                        }
                        else if (type != VariableType.NUMBER)
                        {
                            if (firstType == VariableType.NON)
                            {
                                if (type == VariableType.STRING)
                                {
                                    badString = true;
                                    type = VariableType.NUMBER;
                                }
                            }
                            else if (firstType == VariableType.STRING)
                            {
                                badString = true;
                            }

                            if (!badString)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                return true;
                            }
                        }
                    }
                    else if (sign.OutputType == VariableType.NUMBER_ARRAY)
                    {
                        if (array)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1425, ""));
                            return true;
                        }
                        if (addMath)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1418, ""));
                            return true;
                        }

                        if (type == VariableType.NON)
                        {
                            type = VariableType.NUMBER_ARRAY;
                        }
                        else if (type != VariableType.NUMBER_ARRAY)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                            return true;
                        }
                    }
                    else if (sign.OutputType == VariableType.STRING)
                    {
                        if (!math)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1416, ""));
                            return true;
                        }
                        else
                        {
                            math = false;
                        }

                        if (type == VariableType.NON)
                        {
                            type = VariableType.STRING;
                        }
                        else if (type != VariableType.STRING)
                        {
                            if (firstType == VariableType.NON)
                            {
                                if (type == VariableType.NUMBER)
                                {
                                    badString = true;
                                    type = VariableType.STRING;
                                }
                            }
                            if (!badString)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                return true;
                            }
                        }
                    }
                    else if (sign.OutputType == VariableType.STRING_ARRAY)
                    {
                        if (array)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1425, ""));
                            return true;
                        }
                        if (addMath)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1418, ""));
                            return true;
                        }

                        if (type == VariableType.NON)
                        {
                            type = VariableType.STRING_ARRAY;
                        }
                        else if (type != VariableType.STRING_ARRAY)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                            return true;
                        }
                    }
                    else if (sign.OutputType == VariableType.ANY)
                    {
                        if (word.ToLower().IndexOf("f.call") != -1)
                        {
                            type = VariableType.ANY;
                            /*
                            if (j + 2 < line.Count)
                            {
                                var tmpName = line.Words[j + 2].Text.Replace("\"", "");
                                if (Builder.Project.SubNames.ContainsKey(tmpName))
                                {
                                    if (type == VariableType.NON)
                                    {
                                        type = Builder.Project.SubNames[tmpName];
                                    }
                                    else if (type != Builder.Project.SubNames[tmpName])
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                                        return true;
                                    }
                                }
                            }
                            */
                        }
                    }

                    j = tmpIndex;
                }
                else if (token == Tokens.BRACKETLEFT || token == Tokens.BRACKETRIGHT || token == Tokens.BRACKETLEFTARRAY || token == Tokens.BRACKETRIGHTARRAY)
                {
                    continue;
                }
                else
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1401, word.Text + " " + token));
                    return true;
                }
                if (!plus && addMath)
                {
                    if (oldType != VariableType.NON && ((oldType == VariableType.NUMBER && type == VariableType.STRING) || (oldType == VariableType.STRING && type == VariableType.NUMBER) || (oldType == VariableType.STRING && type == VariableType.STRING)))
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                        return true;
                    }
                }
            }

            if (type != VariableType.NON)
            {
                if (badString && type == VariableType.NUMBER)
                {
                    type = VariableType.STRING;
                }
                if (!Data.Project.Variables.ContainsKey(varName))
                {
                    //Console.WriteLine("=================> " + varName + " " + type + " " + line.Number.ToString());
                    if (!array)
                    {
                        Data.Project.Variables.Add(varName, new Variable(varName) { Init = true, Type = type, Line = line });
                    }
                    else
                    {
                        if (type == VariableType.NUMBER || type == VariableType.NUMBER_ARRAY)
                        {
                            Data.Project.Variables.Add(varName, new Variable(varName) { Init = true, Type = VariableType.NUMBER_ARRAY, Line = line });
                        }
                        else if (type == VariableType.STRING || type == VariableType.STRING_ARRAY)
                        {
                            Data.Project.Variables.Add(varName, new Variable(varName) { Init = true, Type = VariableType.STRING_ARRAY, Line = line });
                        }
                    }
                }
            }
            else
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1401, ""));
                return true;
            }

            return false;
        }

        private static bool VarContains(Line line, string text, string originText)
        {
            if (!Data.Project.Variables.ContainsKey(text))
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1405, "( " + originText + " )"));
                return false;
            }
            else if (!Data.Project.Variables[text].Init && Data.Project.Variables[text].Type == VariableType.NON)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1406, "( " + originText + " )"));
                return false;
            }

            return true;
        }

        private static bool ParseMethodError(int startPos, string methodName, Line line)
        {
            MethodErrorParser.Start(startPos, line, methodName, Data.Project.Variables);

            if (Data.Errors.Count > 0)
                return true;

            return false;
        }

        internal static bool ParseDoubleMathError(Line line)
        {
            if (!VarContains(line, line.Words[0].Text, line.Words[0].OriginText))
                return true;

            var type = Data.Project.Variables[line.Words[0].Text].Type;

            if (line.Count > 2)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1414, ""));
                return true;
            }

            line.OutLines.Clear();

            if (line.Words[1].Text == "++")
            {
                if (type != VariableType.NUMBER)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1412, "( " + line.Words[0].OriginText + " )"));
                    return true;
                }
                //line.OutLine.Add(currentWord.Text + suf + " = " + currentWord.Text + suf + " + 1");
                var newWords = new List<Word>();
                newWords.Add(line.Words[0]);
                newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                newWords.Add(line.Words[0]);
                newWords.Add(new Word() { Text = "+", OriginText = "+", Token = Tokens.MATHOPERATOR });
                newWords.Add(new Word() { Text = "1", OriginText = "1", Token = Tokens.NUMBER });

                var newLine = new Line(newWords, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.VARINIT };
                line.OutLines.Add(newLine);
            }
            else
            {
                if (type != VariableType.NUMBER)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1413, "( " + line.Words[0].OriginText + " )"));
                    return true;
                }
                //line.OutLine.Add(currentWord.Text + suf + " = " + currentWord.Text + suf + " - 1");
                var newWords = new List<Word>();
                newWords.Add(line.Words[0]);
                newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
                newWords.Add(line.Words[0]);
                newWords.Add(new Word() { Text = "-", OriginText = "-", Token = Tokens.MATHOPERATOR });
                newWords.Add(new Word() { Text = "1", OriginText = "1", Token = Tokens.NUMBER });

                var newLine = new Line(newWords, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.VARINIT };
                line.OutLines.Add(newLine);
            }

            return false;
        }

        internal static bool ParseBracketLeftArray(Line line)
        {
            if (line.Count < 6)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1401, "( " + line.NewLine + " )"));
                return true;
            }

            int startPos = 0;
            var type = VariableType.NON;

            if (Data.Project.Variables.ContainsKey(line.FirstWord.Text))
            {
                type = Data.Project.Variables[line.FirstWord.Text].Type;
            }

            var v = Data.Project.Variables;

            ArrayIndexErrorParser.Start(line, 1, v);
            if (Data.Errors.Count > 0)
                return true;

            if (type == VariableType.NUMBER_ARRAY)
            {
                type = VariableType.NUMBER;
            }
            else if (type == VariableType.STRING_ARRAY)
            {
                type = VariableType.STRING;
            }

            if (ArrayIndexErrorParser.LastIndex + 1 < line.Count && line.Words[ArrayIndexErrorParser.LastIndex + 1].Token == Tokens.EQU && ArrayIndexErrorParser.LastIndex + 2 < line.Count)
            {
                startPos = ArrayIndexErrorParser.LastIndex + 2;
            }
            else
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1401, ""));
                return true;
            }

            if (ParseInitVarError(line, type, true, startPos, line.Count, 6))
                return true;

            return false;
        }

        internal static bool ParseEquMathError(Line line)
        {
            if (!VarContains(line, line.Words[0].Text, line.Words[0].OriginText))
                return true;

            var type = Data.Project.Variables[line.Words[0].Text].Type;

            if (type == VariableType.NUMBER_ARRAY || type == VariableType.STRING_ARRAY)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1421, ""));
                return true;
            }

            if (line.Count < 3)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1414, ""));
                return true;
            }

            if (line.Words[2].Token == Tokens.NUMBER)
            {
                if (line.Count != 3)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1414, ""));
                    return true;
                }
                if (type != VariableType.NUMBER)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                    return true;
                }
            }
            else if (line.Words[2].Token == Tokens.STRING)
            {
                if (line.Count != 3)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1414, ""));
                    return true;
                }
                if (type != VariableType.STRING)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                    return true;
                }
            }
            else if (line.Words[2].Token == Tokens.MATHOPERATOR)
            {
                if (line.Words[2].Text != "-" && line.Count != 4 && line.Words[3].Token != Tokens.NUMBER)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1414, ""));
                    return true;
                }
                if (type != VariableType.NUMBER)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                    return true;
                }
            }
            else if (line.Words[2].Token == Tokens.METHOD)
            {
                MethodErrorParser.Start(2, line, line.Words[2].Text, Data.Project.Variables);
                if (Data.Errors.Count > 0)
                    return true;

                if (Data.Errors.Count > 0)
                    return true;

                var sign = DefaultObjectList.Objects[line.Words[2].ToLower()];

                if (sign.OutputType == VariableType.NUMBER_ARRAY || sign.OutputType == VariableType.STRING_ARRAY || sign.OutputType == VariableType.ANY)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1428, ""));
                    return true;
                }

                if (sign.OutputType != type)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                    return true;
                }
            }
            else if (line.Words[2].Token == Tokens.VARIABLE)
            {
                if (line.Count != 3)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1414, ""));
                    return true;
                }

                if (!VarContains(line, line.Words[2].Text, line.Words[2].OriginText))
                    return true;

                var v = Data.Project.Variables;

                if (v[line.Words[2].Text].Type == VariableType.NUMBER_ARRAY || v[line.Words[2].Text].Type == VariableType.STRING_ARRAY || v[line.Words[2].Text].Type == VariableType.ANY)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1428, ""));
                    return true;
                }

                if (v[line.Words[2].Text].Type != type)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1407, ""));
                    return true;
                }
            }
            else
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1414, ""));
                return true;
            }

            line.OutLines.Clear();

            var newWords = new List<Word>();
            newWords.Add(line.Words[0]);
            newWords.Add(new Word() { Text = "=", OriginText = "=", Token = Tokens.EQU });
            newWords.Add(line.Words[0]);

            if (line.Words[1].Text == "+=")
            {
                newWords.Add(new Word() { Text = "+", OriginText = "+", Token = Tokens.MATHOPERATOR });
            }
            else if (line.Words[1].Text == "-=")
            {
                newWords.Add(new Word() { Text = "-", OriginText = "-", Token = Tokens.MATHOPERATOR });
            }
            else if (line.Words[1].Text == "*=")
            {
                newWords.Add(new Word() { Text = "*", OriginText = "*", Token = Tokens.MATHOPERATOR });
            }
            else if (line.Words[1].Text == "/=")
            {
                newWords.Add(new Word() { Text = "/", OriginText = "/", Token = Tokens.MATHOPERATOR });
            }

            if (line.Words.Count >= 3)
            {
                for (int i = 2; i < line.Words.Count; i++)
                {
                    newWords.Add(line.Words[i]);
                }
            }

            var newLine = new Line(newWords, line.OldLine) { Number = line.Number, FileName = line.FileName, Type = LineType.VARINIT };
            line.OutLines.Add(newLine);

            return false;
        }
    }
}

using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using Interpreter.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class MethodErrorParser
    {
        internal static HashSet<string> MediaLines { get; set; }

        internal static void Start(int startPos, Line line, string methodName, Dictionary<string, Variable> variables)
        {
            var lastPos = GetMethodLastIndex(startPos, line, methodName.ToLower());
            if (Data.Errors.Count > 0)
                return;
            var param = GetParam(startPos, lastPos, line);

            if (Data.Errors.Count > 0)
                return;
            Start(param, methodName.ToLower(), variables, line);
            if (Data.Errors.Count > 0)
                return;

        }
        internal static void Start(List<List<Tuple<Word, int>>> param, string methodName, Dictionary<string, Variable> variables, Line line)
        {
            if (!DefaultObjectList.Objects.ContainsKey(methodName.ToLower()))
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1301, ""));
                return;
            }

            var signature = DefaultObjectList.Objects[methodName.ToLower()];

            if (signature.InputCount != param.Count)
            {
                
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1304, "( " + methodName + " )"));
                return;
            }

            for (int i = 0; i < param.Count; i++)
            {
                var type = VariableType.NON;
                var oldType = VariableType.NON;
                bool math = false;
                bool first = true;

                if (param[i].Count <= 0)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1312, ""));
                    return;
                }

                for (int j = 0; j < param[i].Count; j++)
                {
                    var word = param[i][j].Item1;
                    if (word.Token == Tokens.MATHOPERATOR)
                    {
                        if (first)
                        {
                            if (word.Text != "-")
                            {
                                if (j + 1 < param[i].Count && param[i][j + 1].Item1.Token == Tokens.NUMBER)
                                {
                                    continue;
                                }
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1309, ""));
                                return;
                            }
                        }
                        else
                        {
                            if (word.Text != "-" && word.Text != "+" && word.Text != "*" && word.Text != "/")
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1309, ""));
                                return;
                            }
                            else
                            {
                                if (!math)
                                {
                                    math = true;
                                }
                                else
                                {
                                    if (word.Text == "-")
                                    {
                                        if (j - 1 >= 0 && j + 1 < param[i].Count && (param[i][j + 1].Item1.Token == Tokens.NUMBER || param[i][j + 1].Item1.Token == Tokens.VARIABLE || param[i][j + 1].Item1.Token == Tokens.METHOD))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1311, ""));
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1311, ""));
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else if (word.Token == Tokens.NUMBER)
                    {
                        if (!first)
                        {
                            if (!math)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1308, ""));
                                return;
                            }
                            else
                            {
                                math = false;
                            }
                        }
                        else
                        {
                            first = false;
                            oldType = VariableType.NUMBER;
                        }
                        if (oldType != VariableType.NON && oldType != VariableType.NUMBER)
                        {
                            if (oldType != VariableType.STRING && signature.OutputType != VariableType.ANY)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                return;
                            }
                        }
                        type = VariableType.NUMBER;
                    }
                    else if (word.Token == Tokens.STRING)
                    {
                        if (!first)
                        {
                            if (!math)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1308, ""));
                                return;
                            }
                            else
                            {
                                math = false;
                            }
                        }
                        else
                        {
                            first = false;
                            oldType = VariableType.STRING;
                        }
                        if (oldType != VariableType.NON && oldType != VariableType.STRING)
                        {
                            if (oldType != VariableType.NUMBER && signature.OutputType != VariableType.ANY)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                return;
                            }
                        }
                        type = VariableType.STRING;
                    }
                    else if (word.Token == Tokens.VARIABLE)
                    {
                        var variable = new Variable("");
                        if (!first)
                        {
                            if (!math)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1308, ""));
                                return;
                            }
                            else
                            {
                                math = false;
                            }
                        }
                        if (variables.ContainsKey(word.Text))
                        {
                            variable = variables[word.Text];
                        }
                        else
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1305, ""));
                            return;
                        }

                        if (!variable.Init || variable.Type == VariableType.NON)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1305, ""));
                            return;
                        }

                        if (variable.Type == VariableType.STRING_ARRAY)
                        {
                            if (j + 1 < param[i].Count && param[i][j + 1].Item1.Text == "[")
                            {
                                ArrayIndexErrorParser.Start(line, param[i][j + 1].Item2, variables);
                                if (Data.Errors.Count > 0)
                                    return;
                                j = ArrayIndexErrorParser.LastIndex;

                                if (first)
                                {
                                    first = false;
                                    oldType = VariableType.STRING;
                                }
                                if (oldType != VariableType.NON && oldType != VariableType.STRING)
                                {
                                    if (oldType != VariableType.NUMBER && signature.OutputType != VariableType.ANY)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                        return;
                                    }
                                }
                                type = VariableType.STRING;
                            }
                            else
                            {
                                if (first)
                                {
                                    first = false;
                                    oldType = VariableType.STRING_ARRAY;
                                }
                                if (oldType != VariableType.NON && oldType != VariableType.STRING_ARRAY)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                    return;
                                }
                                type = VariableType.STRING_ARRAY;
                            }
                        }
                        else if (variable.Type == VariableType.NUMBER_ARRAY)
                        {
                            if (j + 1 < param[i].Count && param[i][j + 1].Item1.Text == "[")
                            {
                                ArrayIndexErrorParser.Start(line, param[i][j + 1].Item2, variables);
                                if (Data.Errors.Count > 0)
                                    return;
                                j = ArrayIndexErrorParser.LastIndex;

                                if (first)
                                {
                                    first = false;
                                    oldType = VariableType.NUMBER;
                                }
                                if (oldType != VariableType.NON && oldType != VariableType.NUMBER)
                                {
                                    if (oldType != VariableType.STRING && signature.OutputType != VariableType.ANY)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                        return;
                                    }
                                }
                                type = VariableType.NUMBER;
                            }
                            else
                            {
                                if (first)
                                {
                                    first = false;
                                    oldType = VariableType.NUMBER_ARRAY;
                                }
                                if (oldType != VariableType.NON && oldType != VariableType.NUMBER_ARRAY)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                    return;
                                }
                                type = VariableType.NUMBER_ARRAY;
                            }
                        }
                        else if (variable.Type == VariableType.NUMBER)
                        {
                            if (first)
                            {
                                first = false;
                                oldType = VariableType.NUMBER;
                            }
                            if (oldType != VariableType.NON && oldType != VariableType.NUMBER)
                            {
                                if (oldType != VariableType.STRING && signature.OutputType != VariableType.ANY)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                    return;
                                }
                            }
                            type = VariableType.NUMBER;
                        }
                        else if (variable.Type == VariableType.STRING)
                        {
                            if (first)
                            {
                                first = false;
                                oldType = VariableType.STRING;
                            }
                            if (oldType != VariableType.NON && oldType != VariableType.STRING)
                            {
                                if (oldType != VariableType.NUMBER && signature.OutputType != VariableType.ANY)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                    return;
                                }
                            }
                            type = VariableType.STRING;
                        }
                    }
                    else if (word.Token == Tokens.METHOD)
                    {

                        if (DefaultObjectList.Objects.ContainsKey(word.Text.ToLower()))
                        {

                            var sign = DefaultObjectList.Objects[word.Text.ToLower()];

                            //=====================
                            int tmpLast = GetMethodLastIndex(param[i][j].Item2, line, word.Text);
                            var tmpParam = GetParam(param[i][j].Item2, tmpLast, line);
                            Start(tmpParam, word.Text, variables, line);
                            if (Data.Errors.Count > 0)
                                return;

                            if (tmpLast >= 0 && ((tmpLast - param[i][j].Item2) + j) < param[i].Count)
                            {
                                j += (tmpLast - param[i][j].Item2);
                            }
                            //=====================

                            if (sign.OutputType == VariableType.NON)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1306, ""));
                                return;
                            }
                            else
                            {
                                if (!first)
                                {
                                    if (!math)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1308, ""));
                                        return;
                                    }
                                    else
                                    {
                                        math = false;
                                    }
                                }
                                else
                                {
                                    if (param[i][j].Item2 == tmpLast)
                                    {
                                        first = false;
                                    }
                                    oldType = sign.OutputType;
                                }
                                if (oldType != VariableType.NON && oldType != sign.OutputType)
                                {
                                    if (sign.OutputType != VariableType.ANY && oldType != VariableType.NUMBER && oldType != VariableType.STRING)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                        return;
                                    }
                                }
                                type = sign.OutputType;
                            }
                        }
                        else
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1301, ""));
                            return;
                        }
                    }
                    else if (word.Token == Tokens.NON)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1032, "( " + word.Text + " )"));
                        return;
                    }
                }

                if (i < signature.InputType.Count && type != signature.InputType[i])
                {
                    if (type == VariableType.NUMBER || type == VariableType.STRING)
                    {
                        if (signature.InputType[i] != VariableType.ANY)
                        {

                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1307, "( " + methodName + " )"));
                            return;
                        }
                    }
                    else if (param[i].Count > 0)
                    {
                        if (type != VariableType.ANY && param[i][0].Item1.ToLower() != "f.get")
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1307, "( " + methodName + " )"));
                            return;
                        }
                    }
                    else if (methodName.ToLower() != "f.returnnumber" && methodName.ToLower() != "f.returnstring")
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1307, "( " + methodName + " )"));
                        return;
                    }
                }
            }

            if (methodName.ToLower() == "lcd.bmpfile" || methodName.ToLower() == "speaker.play" || methodName.ToLower() == "ev3file.openwrite" || methodName.ToLower() == "ev3file.openappend" || methodName.ToLower() == "ev3file.openread" || methodName.ToLower() == "ev3file.tablelookup")
            {
                var name = line.FileName + "_" + line.Number.ToString();
                if (!MediaLines.Contains(name))
                {
                    new MediaBuilder().ParseMedia(line);
                    MediaLines.Add(name);
                }
            }
        }

        internal static List<List<Tuple<Word, int>>> GetParam(int startPos, int lastPos, Line line)
        {
            var words = new List<List<Tuple<Word, int>>>();
            int bracket = 0;
            int brk = 0;
            int start = 0;

            for (int f = startPos; f <= lastPos; f++)
            {
                var word = line.Words[f];

                if (word.Token == Tokens.BRACKETLEFT)
                {
                    brk++;
                    if (bracket == 0)
                    {
                        bracket = 1;
                        start = f + 1;
                        continue;
                    }
                }
                else if (word.Token == Tokens.BRACKETRIGHT)
                {
                    brk--;
                }
                if (bracket == 1)
                {
                    if (word.Token == Tokens.COMMA && brk == 1)
                    {
                        if (start < lastPos)
                        {
                            var tmpWords = new List<Tuple<Word, int>>();
                            for (int j = start; j < f; j++)
                            {
                                tmpWords.Add(new Tuple<Word, int>(line.Words[j], j));
                            }
                            words.Add(tmpWords);
                            start = f + 1;
                        }
                    }
                }
                if (f == lastPos && bracket == 1)
                {
                    if (start < lastPos)
                    {
                        var tmpWords = new List<Tuple<Word, int>>();
                        for (int j = start; j < f; j++)
                        {
                            tmpWords.Add(new Tuple<Word, int>(line.Words[j], j));
                        }
                        words.Add(tmpWords);
                    }
                }
            }

            return words;
        }

        internal static int GetMethodLastIndex(int startPos, Line line, string methodName)
        {
            int li = -1;
            int bracket = 0;
            bool start = false;

            if (DefaultObjectList.Objects.ContainsKey(methodName.ToLower()))
            {
                if (DefaultObjectList.Objects[methodName.ToLower()].Type == ObjectType.EVENT || DefaultObjectList.Objects[methodName.ToLower()].Type == ObjectType.PROPERTY)
                {
                    if (startPos + 1 < line.Words.Count)
                    {
                        if (line.Words[startPos + 1].Token == Tokens.BRACKETLEFT || line.Words[startPos + 1].Token == Tokens.DOUBLEBRACKET)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1302, "( " + methodName + " )"));
                            return li;
                        }
                    }
                    return startPos;
                }
                else if (DefaultObjectList.Objects[methodName.ToLower()].Type == ObjectType.METHOD)
                {
                    if (startPos + 1 < line.Words.Count)
                    {
                        if (line.Words[startPos + 1].Token != Tokens.BRACKETLEFT && line.Words[startPos + 1].Token != Tokens.DOUBLEBRACKET)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1303, "( " + methodName + " )"));
                            return li;
                        }
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1303, "( " + methodName + " )"));
                        return li;
                    }
                }
            }
            else
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1301, "( " + methodName + " )"));
                return li;
            }

            for (int i = startPos; i < line.Count; i++)
            {
                var word = line.Words[i];

                if (word.Token == Tokens.BRACKETLEFT)
                {
                    bracket++;
                    start = true;
                }
                else if (word.Token == Tokens.BRACKETRIGHT)
                {
                    bracket--;
                }
                else if (word.Token == Tokens.DOUBLEBRACKET && bracket == 0)
                {
                    return i;
                }

                if (start && bracket == 0)
                {
                    return i;
                }
            }

            return li;
        }

        internal static VariableType ParseOneParam(List<Tuple<Word, int>> param, Dictionary<string, Variable> variables, Line line, bool func)
        {
            var type = VariableType.NON;
            var oldType = VariableType.NON;
            bool math = false;
            bool first = true;
            bool plus = false;
            bool addMath = false;

            for (int j = 0; j < param.Count; j++)
            {
                var word = param[j].Item1;
                if (word.Token == Tokens.MATHOPERATOR)
                {
                    addMath = true;
                    if (first)
                    {
                        if (word.Text != "-")
                        {
                            if (j + 1 < param.Count && param[j + 1].Item1.Token == Tokens.NUMBER)
                            {
                                continue;
                            }
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1309, ""));
                            return type;
                        }
                    }
                    else
                    {
                        if (word.Text != "-" && word.Text != "+" && word.Text != "*" && word.Text != "/")
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1309, ""));
                            return type;
                        }
                        else
                        {
                            plus = false;
                            if (!math)
                            {
                                math = true;
                                if (word.Text == "+")
                                {
                                    plus = true;
                                }
                            }
                            else
                            {
                                if (word.Text == "-")
                                {
                                    if (j - 1 >= 0 && j - 1 < param.Count && j + 1 < param.Count && param[j + 1].Item1.Token == Tokens.NUMBER && param[j - 1].Item1.Token == Tokens.BRACKETLEFT)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1311, ""));
                                        return type;
                                    }
                                }
                                else
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1311, ""));
                                    return type;
                                }
                            }
                        }
                    }
                }
                else if (word.Token == Tokens.NUMBER)
                {
                    if (!first)
                    {
                        if (!math)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1308, ""));
                            return type;
                        }
                        else
                        {
                            math = false;
                        }
                    }
                    else
                    {
                        first = false;
                        oldType = VariableType.NUMBER;
                    }
                    if (oldType != VariableType.NON && oldType != VariableType.NUMBER)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                        return type;
                    }
                    type = VariableType.NUMBER;
                }
                else if (word.Token == Tokens.STRING)
                {
                    if (!first)
                    {
                        if (!math)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1308, ""));
                            return type;
                        }
                        else
                        {
                            if (!plus)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                                return type;
                            }
                            math = false;
                        }
                    }
                    else
                    {
                        first = false;
                        oldType = VariableType.STRING;
                    }
                    if (oldType != VariableType.NON && oldType != VariableType.STRING)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                        return type;
                    }
                    type = VariableType.STRING;
                }
                else if (word.Token == Tokens.VARIABLE)
                {
                    var variable = new Variable("");
                    if (!first)
                    {
                        if (!math)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1308, ""));
                            return type;
                        }
                        else
                        {
                            math = false;
                        }
                    }
                    if (variables.ContainsKey(word.Text))
                    {
                        variable = variables[word.Text];
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1305, ""));
                        return type;
                    }

                    if (!variable.Init || variable.Type == VariableType.NON)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1305, ""));
                        return type;
                    }

                    if (variable.Type == VariableType.STRING_ARRAY)
                    {
                        if (j + 1 < param.Count && param[j + 1].Item1.Text == "[")
                        {
                            if (first)
                            {
                                first = false;
                                oldType = VariableType.STRING;
                            }
                            else
                            {
                                if (!plus)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                                    return type;
                                }
                            }
                            if (oldType != VariableType.NON && oldType != VariableType.STRING)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                return type;
                            }
                            type = VariableType.STRING;
                        }
                        else
                        {
                            if (first)
                            {
                                first = false;
                                oldType = VariableType.STRING_ARRAY;
                            }
                            else
                            {
                                if (addMath)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1418, ""));
                                    return type;
                                }
                            }
                            if (oldType != VariableType.NON && oldType != VariableType.STRING_ARRAY)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                return type;
                            }
                            type = VariableType.STRING_ARRAY;
                        }
                    }
                    else if (variable.Type == VariableType.NUMBER_ARRAY)
                    {
                        if (j + 1 < param.Count && param[j + 1].Item1.Text == "[")
                        {
                            if (first)
                            {
                                first = false;
                                oldType = VariableType.NUMBER;
                            }
                            if (oldType != VariableType.NON && oldType != VariableType.NUMBER)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                return type;
                            }
                            type = VariableType.NUMBER;
                        }
                        else
                        {
                            if (first)
                            {
                                first = false;
                                oldType = VariableType.NUMBER_ARRAY;
                            }
                            else
                            {
                                if (addMath)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1418, ""));
                                    return type;
                                }
                            }
                            if (oldType != VariableType.NON && oldType != VariableType.NUMBER_ARRAY)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                return type;
                            }
                            type = VariableType.NUMBER_ARRAY;
                        }
                    }
                    else if (variable.Type == VariableType.NUMBER)
                    {
                        if (first)
                        {
                            first = false;
                            oldType = VariableType.NUMBER;
                        }
                        if (oldType != VariableType.NON && oldType != VariableType.NUMBER)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                            return type;
                        }
                        type = VariableType.NUMBER;
                    }
                    else if (variable.Type == VariableType.STRING)
                    {
                        if (first)
                        {
                            first = false;
                            oldType = VariableType.STRING;
                        }
                        else
                        {
                            if (!plus)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                                return type;
                            }
                        }
                        if (oldType != VariableType.NON && oldType != VariableType.STRING)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                            return type;
                        }
                        type = VariableType.STRING;
                    }
                }
                else if (word.Token == Tokens.METHOD)
                {
                    if (DefaultObjectList.Objects.ContainsKey(word.Text.ToLower()))
                    {
                        var sign = DefaultObjectList.Objects[word.Text.ToLower()];

                        //=====================
                        int tmpLast = GetMethodLastIndex(param[j].Item2, line, word.Text);
                        var tmpParam = GetParam(param[j].Item2, tmpLast, line);
                        Start(tmpParam, word.Text, variables, line);
                        if (Data.Errors.Count > 0)
                            return type;

                        if (tmpLast >= 0 && ((tmpLast - param[j].Item2) + j) < param.Count)
                        {
                            j += (tmpLast - param[j].Item2);
                        }
                        //=====================

                        if (sign.OutputType == VariableType.NON)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1306, ""));
                            return type;
                        }
                        else
                        {
                            if (!first)
                            {
                                if (!math)
                                {
                                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1308, ""));
                                    return type;
                                }
                                else
                                {
                                    math = false;
                                }

                                if (sign.OutputType == VariableType.STRING)
                                {
                                    if (!plus)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1417, ""));
                                        return type;
                                    }
                                }
                                else if (sign.OutputType == VariableType.NUMBER_ARRAY || sign.OutputType == VariableType.STRING_ARRAY)
                                {
                                    if (addMath)
                                    {
                                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1418, ""));
                                        return type;
                                    }
                                }
                            }
                            else
                            {
                                if (param[j].Item2 == tmpLast)
                                {
                                    first = false;
                                }
                                oldType = sign.OutputType;
                            }
                            if (oldType != VariableType.NON && oldType != sign.OutputType)
                            {
                                Data.Errors.Add(new Errore(line.Number, line.FileName, 1310, ""));
                                return type;
                            }
                            type = sign.OutputType;
                        }
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1301, ""));
                        return type;
                    }
                }
                else if (word.Token == Tokens.NON)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1032, "( " + word.Text + " )"));
                    return type;
                }
            }

            return type;
        }
    }
}

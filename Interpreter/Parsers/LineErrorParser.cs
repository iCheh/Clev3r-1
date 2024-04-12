using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class LineErrorParser
    {
        internal static void Start(List<Line> lines)
        {
            foreach (var line in lines)
            {
                if (line.Type == LineType.FORINIT)
                {
                    ForLineErrorParser.Start(line, Data.Project.Variables);
                    if (Data.Errors.Count > 0)
                        return;
                }
                else if (line.Type == LineType.IFINIT || line.Type == LineType.ELSEIFINIT)
                {
                    if (line.LastWord.ToLower() != "then")
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1907, ""));
                        return;
                    }
                    if (line.Count < 3)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1908, ""));
                        return;
                    }

                    for (int d = 1; d < line.Count - 1; d++)
                    {
                        LogicErrorParser.Start(line, d, Data.Project.Variables);
                        if (Data.Errors.Count > 0)
                            return;

                        d = LogicErrorParser.LastIndex;
                    }
                }
                else if (line.Type == LineType.WHILEINIT)
                {
                    if (line.Count < 2)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1908, ""));
                        return;
                    }

                    for (int d = 1; d < line.Count; d++)
                    {
                        LogicErrorParser.Start(line, d, Data.Project.Variables);
                        if (Data.Errors.Count > 0)
                            return;

                        d = LogicErrorParser.LastIndex;
                    }
                }
                else if (line.Type == LineType.METHODCALL)
                {
                    ParseMethodError(0, line.FirstWord.Text, line);
                    if (Data.Errors.Count > 0)
                        return;

                    var sign = DefaultObjectList.Objects[line.FirstWord.ToLower()];
                    if (sign.Type == ObjectType.METHOD)
                    {
                        if (line.LastWord.Token != Tokens.BRACKETRIGHT && line.LastWord.Token != Tokens.DOUBLEBRACKET)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1034, "( " + line.LastWord.Text + " )"));
                            return;
                        }
                        else if (line.LastWord.Token == Tokens.DOUBLEBRACKET && line.Count > 2)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1034, "( " + line.LastWord.Text + " )"));
                            return;
                        }
                    }
                    else if (sign.Type == ObjectType.PROPERTY)
                    {
                        if (line.Count > 1)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1034, "( " + line.NewLine + " )"));
                            return;
                        }
                    }
                    else if (sign.Type == ObjectType.EVENT)
                    {
                        if (line.Count != 3)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1034, "( " + line.NewLine + " )"));
                            return;
                        }
                        else if (line.Words[1].Token != Tokens.EQU && line.Words[2].Token != Tokens.SUBNAME)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1034, "( " + line.NewLine + " )"));
                            return;
                        }
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1034, "( " + line.NewLine + " )"));
                        return;
                    }
                }
                else if (line.Type == LineType.NON)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1032, ""));
                    return;
                }
            }

            ParseJupmOperatorsErrore(lines);
            ParseJumpOperators(lines, LineType.FORINIT);
            ParseJumpOperators(lines, LineType.WHILEINIT);
        }

        private static bool ParseMethodError(int startPos, string methodName, Line line)
        {
            MethodErrorParser.Start(startPos, line, methodName, Data.Project.Variables);

            if (Data.Errors.Count > 0)
                return true;

            return false;
        }

        private static void ParseJumpOperators(List<Line> lines, LineType type)
        {
            int start = -1;
            int stop = -1;
            int flagLoop = 0;
            var lastWord = "";

            if (type == LineType.FORINIT)
            {
                lastWord = "endfor";
            }
            else if (type == LineType.WHILEINIT)
            {
                lastWord = "endwhile";
            }

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Type == type)
                {
                    flagLoop++;
                    start = i;

                    for (int j = i; j < lines.Count; j++)
                    {
                        if (j == i)
                            continue;

                        if (lines[j].Type == type)
                        {
                            flagLoop++;
                        }
                        else if (lines[j].Type == LineType.ONEKEYWORD && lines[j].Words[0].ToLower() == lastWord)
                        {
                            if (flagLoop == 1)
                            {
                                stop = j;

                                if (start != -1 && stop != -1 && stop > start)
                                {
                                    ParseBrakeAndContinue(lines, start, stop, lastWord, "continue");
                                    if (Data.Errors.Count > 0)
                                        return;
                                    ParseBrakeAndContinue(lines, start, stop, lastWord, "break");
                                    if (Data.Errors.Count > 0)
                                        return;
                                }

                                start = -1;
                                stop = -1;
                                flagLoop = 0;
                                break;
                            }

                            flagLoop--;
                        }
                    }
                }
            }
        }

        private static void ParseBrakeAndContinue(List<Line> lines, int start, int stop, string lastWord, string jumpWord)
        {
            var flagLoop = 0;
            var flagLabelName = false;
            var labelName = "";

            for (int i = start; i < stop; i++)
            {
                if (i == start)
                    continue;

                if (lines[i].Type == LineType.FORINIT || lines[i].Type == LineType.WHILEINIT)
                {
                    flagLoop++;
                }

                if (flagLoop == 0)
                {
                    if (lines[i].Type == LineType.ONEKEYWORD && lines[i].Words[0].ToLower() == jumpWord)
                    {
                        if (!flagLabelName)
                        {
                            Data.Project.BreakPoint++;
                            labelName = jumpWord + "_" + Data.Project.BreakPoint.ToString();
                            flagLabelName = true;
                        }

                        var tmpWords = new List<Word>();

                        tmpWords.Add(new Word() { Text = "Goto", OriginText = "Goto", Token = Tokens.KEYWORD });
                        tmpWords.Add(new Word() { Text = labelName, OriginText = labelName, Token = Tokens.LABELNAME });

                        lines[i].OutLines.Add(new Line(tmpWords, lines[i].OldLine));
                    }
                }

                if (lines[i].Type == LineType.ONEKEYWORD && lines[i].Words[0].ToLower() == lastWord)
                {
                    flagLoop--;
                }
            }

            if (flagLabelName)
            {
                if (jumpWord == "continue")
                {
                    var tmpWords = new List<Word>();
                    var tmpText = labelName + ":";
                    tmpWords.Add(new Word() { Text = tmpText, OriginText = tmpText, Token = Tokens.LABEL });

                    lines[stop].OutLines.Add(new Line(tmpWords, tmpText));

                    lines[stop].OutLines.Add(lines[stop]);
                }
                else if (jumpWord == "break")
                {
                    if (lines[stop].OutLines.Count == 0)
                    {
                        lines[stop].OutLines.Add(lines[stop]);
                    }

                    var tmpWords = new List<Word>();
                    var tmpText = labelName + ":";
                    tmpWords.Add(new Word() { Text = tmpText, OriginText = tmpText, Token = Tokens.LABEL });

                    lines[stop].OutLines.Add(new Line(tmpWords, tmpText));
                }
            }
            
        }

        private static void ParseJupmOperatorsErrore(List<Line> lines)
        {
            var flagForWhile = 0;
            foreach (var line in lines)
            {
                if (line.Type == LineType.FORINIT)
                {
                    // Изменим состояния флага структуры For ... EndFor или While ... EndWhile
                    flagForWhile++;
                }
                else if (line.Type == LineType.WHILEINIT)
                {
                    // Изменим состояния флага структуры For ... EndFor или While ... EndWhile
                    flagForWhile++;
                }
                else if (line.Type == LineType.ONEKEYWORD)
                {
                    if (line.Words[0].ToLower() == "continue")
                    {
                        if (line.Count > 1)
                        {
                            // Ошибка, в строке не может быть других слов
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1915, ""));
                            return;
                        }

                        if (flagForWhile == 0)
                        {
                            // Ошибка, ключевое слово Continue используется только в структурах For ... EndFor или While ... EndWhile
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1916, ""));
                            return;
                        }
                    }
                    else if (line.Words[0].ToLower() == "break")
                    {
                        if (line.Count > 1)
                        {
                            // Ошибка, в строке не может быть других слов
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1913, ""));
                            return;
                        }

                        if (flagForWhile == 0)
                        {
                            // Ошибка, ключевое слово Break используется только в структурах For ... EndFor или While ... EndWhile
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1914, ""));
                            return;
                        }
                    }
                    else if (line.Words[0].ToLower() == "endfor" || line.Words[0].ToLower() == "endwhile")
                    {
                        flagForWhile--;
                    }
                }
            }
        }
    }
}

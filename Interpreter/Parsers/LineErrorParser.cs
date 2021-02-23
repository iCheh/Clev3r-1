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
        }

        private static bool ParseMethodError(int startPos, string methodName, Line line)
        {
            MethodErrorParser.Start(startPos, line, methodName, Data.Project.Variables);

            if (Data.Errors.Count > 0)
                return true;

            return false;
        }
    }
}

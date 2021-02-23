using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Parsers
{
    internal static class BracketErrorParser
    {
        internal static void Start(Line line)
        {
            if (line.Type == LineType.EMPTY)
            {
                return;
            }

            Stack<string> stackBracket = new Stack<string>();

            foreach (var word in line.Words)
            {
                if (word.Token == Enums.Tokens.BRACKETLEFT)
                {
                    stackBracket.Push(")");
                }
                else if (word.Token == Enums.Tokens.BRACKETLEFTARRAY)
                {
                    stackBracket.Push("]");
                }
                else if (word.Token == Enums.Tokens.BRACKETRIGHT || word.Token == Enums.Tokens.BRACKETRIGHTARRAY)
                {
                    if (stackBracket.Count > 0)
                    {
                        var tmpStr = stackBracket.Pop();
                        if (tmpStr != word.Text)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1002, ""));
                            return;
                        }
                    }
                    else
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1001, ""));
                        return;
                    }
                }
            }

            if (stackBracket.Count > 0)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1001, ""));
                return;
            }

            stackBracket.Clear();
        }
    }
}

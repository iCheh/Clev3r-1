using Clever.Model.Bplus.BPInterpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.Model.Utils
{
    internal struct TextLine
    {
        internal TextLine(List<Word> words, int number, string indentString, bool header)
        {
            Words = words;
            Number = number;
            IndentString = indentString;
            Header = header;
        }

        internal List<Word> Words { get; private set; }
        internal int Number { get; private set; }
        internal string IndentString { get; private set; }
        internal bool Header { get; private set; }

        internal string LineTextToString()
        {
            var result = "";

            foreach (var word in Words)
            {
                result += word.OriginText + " ";
            }


            return result.TrimEnd();
        }

        internal string LineTokensToString()
        {
            var result = "";

            foreach (var word in Words)
            {
                result += word.Token.ToString() + " ";
            }


            return result.TrimEnd();
        }
    }
}

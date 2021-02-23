using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.Model.Bplus.BPInterpreter
{
    internal struct Word
    {
        internal Word(string originText, string text, Tokens token)
        {
            OriginText = originText;
            Text = text;
            Token = token;
        }

        internal string OriginText { get; private set; }
        internal string Text { get; private set; }
        internal Tokens Token { get; private set; }
    }
}

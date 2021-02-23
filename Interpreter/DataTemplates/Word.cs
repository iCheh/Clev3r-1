using Interpreter.Enums;
using System.Collections.Generic;

namespace Interpreter.DataTemplates
{
    internal class Word
    {
        internal Word()
        {
            Text = "";
            OriginText = "";
            Token = Tokens.NON;
        }

        private string _text;
        internal string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Length = value.Length;
            }
        }

        internal string OriginText { get; set; }

        internal Tokens Token { get; set; }

        internal int Length { get; private set; }

        internal int Number { get; set; }

        internal string ToLower()
        {
            return Text.ToLower();
        }

        internal string ToUpper()
        {
            return Text.ToUpper();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.Model.Bplus.BPInterpreter
{
    internal enum Tokens
    {
        METHOD,
        STRING,
        VARIABLE,
        EQU,
        SUBNAME,
        NUMBER,
        KEYWORD,
        LABEL,
        LABELNAME,
        MATHOPERATOR,
        MODULEMETHOD,
        DOUBLEMATH,
        EQUMATH,
        BOOLOPERATOR,
        BRACKETLEFT,
        BRACKETRIGHT,
        BRACKETLEFTARRAY,
        BRACKETRIGHTARRAY,
        DOUBLEBRACKET,
        DOUBLEBRACKETARRAY,
        COMMA,
        FUNCNAME,
        COMMENT,
        NON
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Enums
{
    internal enum LineType
    {
        VARINIT,
        VARDOUBLEMATH,
        VAREQUMATH,
        VARARRAYINIT,
        SUBINIT,
        SUBCALL,
        FUNCINIT,
        FUNCCALL,
        METHODCALL,
        MODULEMETHODCALL,
        MODULEPROPERTY,
        ONEKEYWORD,
        LABELINIT,
        LABELCALL,
        FORINIT,
        IFINIT,
        ELSEIFINIT,
        WHILEINIT,
        INCLUDE,
        FOLDER,
        IMPORT,
        EMPTY,
        NUMBERINIT,
        NUMBERARRAYINIT,
        STRINGINIT,
        STRINGARRAYINIT,
        OLDFUNC,
        PREPROCESSOR,
        NON
    }
}

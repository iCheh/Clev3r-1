using System;

namespace Interpreter.Compiler
{
    internal class CompileException : Exception
    {
        internal CompileException(string message) : base(message)
        {
        }
    }
}


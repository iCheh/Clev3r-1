using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplates
{
    internal class Variable
    {
        internal Variable(string name)
        {
            Name = name;
            Type = VariableType.NON;
            Init = false;
            Line = null;
        }

        internal string Name { get; set; }
        internal VariableType Type { get; set; }
        internal bool Init { get; set; }
        internal Line Line { get; set; }

        public override string ToString()
        {
            return "name: " + Name + " type: " + Type + " init: " + Init;
        }
    }
}

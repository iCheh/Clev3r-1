using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplates
{
    internal class Function
    {
        internal Function(string name)
        {
            Name = name;
            Lines = new List<Line>();
            Parameters = new Dictionary<string, (VariableType varType, ParameterType paramType)>();
        }

        internal List<Line> Lines { get; private set; }
        internal string Name { get; private set; }
        internal Dictionary<string, (VariableType varType, ParameterType paramType)> Parameters {get; private set;}
    }
}

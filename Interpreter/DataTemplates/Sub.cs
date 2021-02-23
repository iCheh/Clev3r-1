using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplates
{
    internal class Sub
    {
        internal Sub (string name)
        {
            Name = name;
            Lines = new List<Line>();
        }

        internal List<Line> Lines { get; private set; }
        internal string Name { get; private set; }
    }
}

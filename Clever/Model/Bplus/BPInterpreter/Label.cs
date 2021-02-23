using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.Model.Bplus.BPInterpreter
{
    internal class Label
    {
        internal Label(string name, bool priv, bool func, int start, int end)
        {
            Name = name;
            Private = priv;
            FromFunction = func;
            Interval = (start, end);
        }

        internal string Name { get; private set; }
        internal bool Private { get; private set; }
        internal bool FromFunction { get; private set; }
        internal (int, int) Interval { get; private set; }
    }
}

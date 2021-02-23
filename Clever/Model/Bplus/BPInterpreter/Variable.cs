using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.Model.Bplus.BPInterpreter
{
    internal class Variable
    {
        internal Variable(string name, bool priv, bool func, int start, int end)
        {
            Name = name;
            Private = priv;
            FromFunction = func;
            Interval = (start, end);
            Summary = new List<string>();
        }

        internal string Name { get; private set; }
        internal bool Private { get; set; }
        internal bool FromFunction { get; set; }
        internal (int,int) Interval { get; private set; }
        internal List<string> Summary { get; private set; }
    }
}

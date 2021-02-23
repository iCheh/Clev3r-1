using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.Model.Bplus.BPInterpreter
{
    internal class Function
    {
        internal Function(string name, bool priv, int count)
        {
            Name = name;
            Private = priv;
            ParamCount = count;
            Summary = new List<string>();
        }

        internal string Name { get; private set; }
        internal bool Private { get; private set; }
        internal int ParamCount { get; private set; }
        internal List<string> Summary { get; private set; }
    }
}

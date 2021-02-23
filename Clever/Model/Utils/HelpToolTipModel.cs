using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.Model.Utils
{
    internal static class HelpToolTipModel
    {
        internal static string Name { get; set; }
        internal static string Summary { get; set; }

        internal static void Create(string name, string summary)
        {
            Name = name;
            Summary = summary;
        }

        internal static void Clear()
        {
            Name = "";
            Summary = "";
        }
    }
}

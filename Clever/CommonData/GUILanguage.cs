using Clever.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.CommonData
{
    internal static class GUILanguage
    {
        internal static Dictionary<string, string> Items { get; private set; }

        internal static void Install()
        {
            Items = new Localizator().ReadInterface();
        }

        internal static string GetItem(string key)
        {
            if (Items.ContainsKey(key))
                return Items[key];
            else
                return "";
        }
    }
}

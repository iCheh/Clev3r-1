using Interpreter.CommonData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplates
{
    internal class Include
    {
        internal Include(string name, string path, List<Line> lines, List<string> oldText)
        {
            Name = name;
            Path = path;
            Ext = Extension.BPInclude;
            Lines = lines;
            OldText = oldText;
        }

        internal string Name { get; private set; } // Без расширения
        internal string Path { get; private set; }
        internal string Ext { get; private set; }
        internal List<Line> Lines { get; private set; }
        internal List<string> OldText { get; private set; }
        internal string FullName
        {
            get
            {
                return Path + Name + Ext;
            }
        }
    }
}

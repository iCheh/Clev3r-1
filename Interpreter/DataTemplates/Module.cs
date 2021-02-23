using Interpreter.CommonData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplates
{
    internal class Module
    {
        internal Module(string name, string path, List<Line> lines, List<string> oldText)
        {
            Name = name;
            Path = path;
            Ext = Extension.BPModule;
            Lines = lines;
            OldText = oldText;
            Methods = new Dictionary<string, (List<Line>, bool)>();
            Propertys = new Dictionary<string, (Line, bool)>();
        }

        internal string Name { get; private set; } // Без расширения
        internal string Path { get; private set; }
        internal string Ext { get; private set; }
        internal List<Line> Lines { get; private set; }
        internal List<string> OldText { get; private set; }
        internal Dictionary<string, (List<Line>, bool)> Methods { get; private set; }
        internal Dictionary<string, (Line, bool)> Propertys { get; private set; }
        internal string FullName
        {
            get
            {
                return Path + Name + Ext;
            }
        }
    }
}

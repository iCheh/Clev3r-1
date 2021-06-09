using Interpreter.CommonData;
using Interpreter.Enums;
using Interpreter.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplates
{
    internal class Program
    {
        internal Program(string name, string path, List<string> text, ProjectType type)
        {
            Name = name;
            Path = path;

            if (type == ProjectType.BP)
                Ext = Extension.BPProgram;
            if (type == ProjectType.LMS)
                Ext = Extension.LMSProgram;
            
            Lines = new List<Line>();

            if (text != null && text.Count > 0)
            {
                OldText = text;
                for (int i = 0; i < text.Count; i++)
                {
                    var words = LineBuilder.GetWords(text[i].Trim());
                    Line newLine = new Line(words, text[i].Trim());
                    //newLine.FileName = Name + Ext;
                    newLine.FileName = Path + Name.Replace(Ext, "") + Ext;
                    newLine.Number = i + 1;
                    newLine.Type = LineBuilder.GetType(newLine);
                    Lines.Add(newLine);
                    /*
                    if (newLine.Type == LineType.NON)
                    {
                        Console.WriteLine("===> " + newLine.NewLine + " " + newLine.Type.ToString());
                        var str = "";
                        foreach (var w in newLine.Words)
                        {
                            str += w.Token.ToString() + " ";
                        }
                        Console.WriteLine(str);
                    }
                    */
                }
            }
            else
            {
                OldText = new List<string>();
            }
        }

        internal string Name { get; set; } // Без расширения
        internal string Path { get; set; }
        internal string Ext { get; set; }
        internal List<Line> Lines { get; set; }
        internal List<string> OldText { get; set; }
        internal string FullName
        {
            get 
            { 
                return Path + Name + Ext; 
            }
        }
    }
}

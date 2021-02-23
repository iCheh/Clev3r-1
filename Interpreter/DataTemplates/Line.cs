using Interpreter.Enums;
using System;
using System.Collections.Generic;

namespace Interpreter.DataTemplates
{
    internal class Line
    {
        internal Line(List<Word> words, string oldLine)
        {
            Words = words;
            if (words != null)
            {
                Count = words.Count;
            }

            if (words.Count > 0)
            {
                var tmp = new List<string>();
                for (int i = 0; i < words.Count; i++)
                {
                    words[i].Number = i + 1;
                    tmp.Add(words[i].Text);
                }
                NewLine = string.Join(" ", tmp);
            }
            else
            {
                NewLine = "";
            }

            OldLine = oldLine;
            OutLines = new List<Line>();
            Number = 0;
            FileName = "";
            ModuleName = "";
            ModuleElemetn = "";
            ElementPrivate = false;
        }

        internal List<Word> Words { get; set; }
        internal string NewLine { get; set; }
        internal string OldLine { get; set; }
        internal List<Line> OutLines { get; set; }
        internal int Number { get; set; }
        internal int Count { get; private set; }
        internal string FileName { get; set; }
        internal LineType Type { get; set; }

        internal string ModuleName { get; set; }
        internal string ModuleElemetn { get; set; }
        internal bool ElementPrivate { get; set; }

        internal Word FirstWord
        {
            get
            {

                if (Words.Count > 0)
                    return Words[0];
                else
                    return null;
            }
        }
        internal Word LastWord
        {
            get
            {

                if (Words.Count > 0)
                    return Words[Words.Count - 1];
                else
                    return null;
            }
        }

        internal bool FindText(string word, bool lower)
        {
            foreach(var w in Words)
            {
                if (lower)
                {
                    if (w.Text.ToLower() == word)
                    {
                        return true;
                    }
                }
                else
                {
                    if (w.Text == word)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override string ToString()
        {
            return NewLine;
        }
        internal string TokenToString()
        {
            string tmp = "";

            foreach(var w in Words)
            {
                tmp += w.Token + " ";
            }

            return tmp.Trim();
        }
    }
}

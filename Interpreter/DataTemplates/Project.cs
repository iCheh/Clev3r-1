using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplates
{
    internal class Project
    {
        internal Project(ProjectType type)
        {
            Type = type;
            Includes = new Dictionary<int, Include>();
            Modules = new Dictionary<string, Module>();
            MainText = new List<Line>();
            MainFuncText = new List<Line>();
            MainSubText = new List<Line>();
            ModuleMethodsText = new List<Line>();
            IsFolder = false;
            Functions = new Dictionary<string, Function>();
            Subs = new Dictionary<string, Sub>();
            OutputLines = new List<Line>();
            Variables = new Dictionary<string, Variable>();
            Propertys = new List<Line>();

            ImageList = new List<string>();
            ImagePath = new List<string>();
            ImageFullName = new List<string>();
            SoundList = new List<string>();
            SoundPath = new List<string>();
            SoundFullName = new List<string>();
            FileList = new List<string>();
            FilePath = new List<string>();
            FileFullName = new List<string>();

            Folder = "prjs";
            ProjectName = "";
            MainName = "";
        }
        internal string MainName { get; set; }
        internal ProjectType Type { get; set; }
        internal Program Main { get; set; }
        internal Dictionary<int, Include> Includes { get; private set; }
        internal Dictionary<string, Module> Modules { get; private set; }
        internal string Path { get; set; }
        internal string ModuleLibPath { get; set; }
        internal List<Line> MainText { get; private set; }
        internal List<Line> MainFuncText { get; private set; }
        internal List<Line> MainSubText { get; private set; }
        internal List<Line> ModuleMethodsText { get; private set; }
        internal bool IsFolder { get; set; }
        internal Dictionary<string, Function> Functions { get; private set; }
        internal Dictionary<string, Sub> Subs { get; private set; }
        internal List<Line> OutputLines { get; private set; }
        internal Dictionary<string, Variable> Variables { get; private set; }

        internal List<string> ImageList { get; private set; }
        internal List<string> ImagePath { get; private set; }
        internal List<string> ImageFullName { get; private set; }
        internal List<string> SoundList { get; private set; }
        internal List<string> SoundPath { get; private set; }
        internal List<string> SoundFullName { get; private set; }
        internal List<string> FileList { get; private set; }
        internal List<string> FilePath { get; private set; }
        internal List<string> FileFullName { get; private set; }

        internal List<Line> Propertys { get; private set; }

        internal string Folder { get; set; }
        internal string ProjectName { get; set; }
    }
}

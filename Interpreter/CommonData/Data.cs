using Interpreter.DataTemplate;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.CommonData
{
    internal static class Data
    {
        internal static Project Project { get; private set; }
        internal static List<Errore> Errors { get; private set; }
        internal static void Install(ProjectType projectType)
        {
            Project = new Project(projectType);
            Errors = new List<Errore>();

            switch (Language.Type)
            {
                case LanguageType.RU:
                    ErrorsCodeList.SetRU();
                    break;
                case LanguageType.EN:
                    ErrorsCodeList.SetEN();
                    break;
                case LanguageType.UA:
                    ErrorsCodeList.SetUA();
                    break;
                default:
                    ErrorsCodeList.SetEN();
                    break;
            }
        }
    }
}

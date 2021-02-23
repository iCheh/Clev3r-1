using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using Interpreter.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Utils
{
    internal class Preprocessor
    {
        internal Preprocessor() { }

        internal void Start(string mainName, string mainPath, List<string> mainText)
        {
            // Сброс медиа
            MethodErrorParser.MediaLines = new HashSet<string>();

            var program = new Program(mainName, mainPath, mainText, Data.Project.Type);
            Data.Project.Main = program;

            // Активация листа описания методов
            DefaultObjectList.Install();

            // Первый проход, парсим INCLUDE IMPORT FOLDER, проверяем скобки
            FirstFindFiles(mainPath);
            if (Data.Errors.Count > 0)
                return;

            // Парсим созданные файлы на целостность структуры
            ParseStruct();
            if (Data.Errors.Count > 0)
                return;

            // Парсим имена функций, процедур и меток на предмет уникальности
            ParseNames();
            if (Data.Errors.Count > 0)
                return;

            // Собираем файл
            Linker.Start();
            if (Data.Errors.Count > 0)
                return;
        }

        private void FirstFindFiles(string path)
        {
            var lines = Data.Project.Main.Lines;

            foreach (var line in lines)
            {
                // Парсим ошибки на количество скобок в строке
                BracketErrorParser.Start(line);
                if (Data.Errors.Count > 0)
                    return;

                if (line.Type == LineType.INCLUDE)
                {
                    ParseInclude(line);
                    if (Data.Errors.Count > 0)
                        return;
                }
                else if (line.Type == LineType.IMPORT)
                {
                    ParseImport(line, path);
                    if (Data.Errors.Count > 0)
                        return;
                }
                else if (line.Type == LineType.FOLDER)
                {
                    ParseFolder(line);
                    if (Data.Errors.Count > 0)
                        return;

                    Data.Project.Folder = line.Words[1].OriginText.Replace("\"", "");
                    Data.Project.ProjectName = line.Words[2].OriginText.Replace("\"", "");
                }
            }
        }

        private void ParseInclude(Line line)
        {
            IncludeErrorParser.Start(line);
            if (Data.Errors.Count > 0)
                return;
            
            var include = new Include(IncludeErrorParser.IncludeName, IncludeErrorParser.IncludePath, IncludeErrorParser.Lines, IncludeErrorParser.OldText);
            
            if (!Data.Project.Includes.ContainsKey(line.Number))
                Data.Project.Includes.Add(line.Number, include);
        }

        private void ParseImport(Line line, string path)
        {
            ImportErrorParser.Start(line, Data.Project.Path);
        }

        private void ParseFolder(Line line)
        {
            FolderErrorParser.Start(line);
        }

        private void ParseStruct()
        {
            StructErrorParser.Start(Data.Project.Main.Lines);
            if (Data.Errors.Count > 0)
                return;

            foreach (var include in Data.Project.Includes)
            {
                StructErrorParser.Start(include.Value.Lines);
                if (Data.Errors.Count > 0)
                    return;
            }

            /*
            foreach (var module in Data.Project.Modules)
            {
                StructErrorParser.Start(module.Value.Lines);
                if (Data.Errors.Count > 0)
                    return;
            }
            */
        }

        private void ParseNames()
        {
            StructErrorParser.ParseNames(Data.Project.Main.Lines);
            if (Data.Errors.Count > 0)
                return;

            foreach (var include in Data.Project.Includes)
            {
                StructErrorParser.ParseNames(include.Value.Lines);
                if (Data.Errors.Count > 0)
                    return;
            }

            foreach (var module in Data.Project.Modules)
            {
                StructErrorParser.ParseNames(module.Value.Lines);
                if (Data.Errors.Count > 0)
                    return;
            }
        }
    }
}

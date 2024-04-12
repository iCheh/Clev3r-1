using System;
using System.Collections.Generic;
using System.IO;
using Interpreter.CommonData;
using Interpreter.DataTemplate;
using Interpreter.DataTemplates;
using Interpreter.Utils;

namespace Interpreter
{
    public class Builder
    {
        private MemoryStream _streamI;
        private MemoryStream _streamC;
        private MemoryStream _streamA;

        private Compiler.Compiler _compiler;
        private Assembler.Assembler _assembler;

        private string _outFolder;

        public Builder(string language)
        {
            Status = true;

            Extension.BPProgram = ".bp";
            Extension.BPInclude = ".bpi";
            Extension.BPModule = ".bpm";
            Extension.LMSProgram = ".lms";

            _streamI = new MemoryStream();
            _streamC = new MemoryStream();
            _streamA = new MemoryStream();

            _compiler = new Compiler.Compiler();
            _assembler = new Assembler.Assembler();

            _outFolder = "";

            switch (language.ToLower().Trim())
            {
                case "ru":
                    Language.Type = Enums.LanguageType.RU;
                    break;
                case "en":
                    Language.Type = Enums.LanguageType.EN;
                    break;
                case "ua":
                    Language.Type = Enums.LanguageType.UA;
                    break;
                default:
                    Language.Type = Enums.LanguageType.EN;
                    break;
            }

            Data.Install(Enums.ProjectType.BP);
        }

        public void BPStart(string mainName, string mainPath, List<string> mainText, string moduleLibPath, string outFolderPrefix)
        {
            Status = true;

            string tmpName = mainName;
            var ind = mainName.LastIndexOf('.');
            if (ind != -1)
            {
                var ext = mainName.Substring(ind);
                if (ext == ".bp")
                    Data.Project.Type = Enums.ProjectType.BP;
                else if (ext == ".lms")
                    Data.Project.Type = Enums.ProjectType.LMS;

                tmpName = mainName.Substring(0, mainName.LastIndexOf('.'));
            }
            Data.Project.ModuleLibPath = moduleLibPath;
            Data.Project.Path = mainPath;
            Data.Project.MainName = mainName;

            _outFolder = outFolderPrefix + Data.Project.MainName.Replace(Extension.BPProgram, "");

            Console.Write("Interpreter start ... ");

            new Preprocessor().Start(tmpName, mainPath, mainText);
            
            if (Data.Errors.Count > 0)
            {
                Console.Write("\n");
                new ErrorShow().ShowToConsole();
                Status = false;
                return;
            }

            new Utils.Interpreter().Start();
            if (Data.Errors.Count > 0)
            {
                Console.Write("\n");
                new ErrorShow().ShowToConsole();
                Status = false;
                return;
            }

            var file = GetOutFile();
            WriteOutFile(file, outFolderPrefix);
            Console.Write("OK");

            // Компилятор
            Console.Write("\n");
            Console.Write("Compiler start ... ");
            WriteToStream(file, _streamI);
            _streamI.Position = 0;
            var errors = new List<string>();
            _compiler.Start(_streamI, _streamC, errors);
            if (errors.Count > 0)
            {
                Console.Write("\n");
                new ErrorShow().ShowToConsole(errors);
                Status = false;
                return;
            }
            _streamC.Position = 0;
            WriteStreamToFile(_streamC, ".lmsb");
            Console.Write("OK");

            // Ассемблер
            Console.Write("\n");
            Console.Write("Assembler start ... ");
            _streamC.Position = 0;
            _assembler.Start(_streamC, _streamA, errors);
            if (errors.Count > 0)
            {
                Console.Write("\n");
                new ErrorShow().ShowToConsole(errors);
                Status = false;
                return;
            }
            var content = _streamA.ToArray();
            WriteByteToFile(content, ".rbf");
            Console.Write("OK");
            Console.Write("\n");
        }

        public void StartInterpreter(string[] lines, string path, string name, string appPath, string mainExt, string includeExt, string moduleExt, string pref, string language, List<string> errors, MemoryStream stream)
        {
            Status = true;

            Extension.BPProgram = mainExt;
            Extension.BPInclude = includeExt;
            Extension.BPModule = moduleExt;

            // Установим нужный язык для сообщений об ошибках
            if (!SetLanguage(language))
                return;

            // Интерпретатор
            try
            {
                var text = new List<string>();
                foreach (var l in lines)
                {
                    text.Add(l);
                }
                // =========
                string tmpName = name;
                Data.Project.Type = Enums.ProjectType.BP;
                Data.Project.ModuleLibPath = appPath;
                Data.Project.Path = path;
                Data.Project.MainName = name;

                _outFolder = pref + Data.Project.MainName.Replace(Extension.BPProgram, "");

                new Preprocessor().Start(tmpName, path, text);
                if (Data.Errors.Count > 0)
                {
                    foreach (var e in Data.Errors)
                    {
                        errors.Add(e.ToString());
                    }
                    Status = false;
                    return;
                }

                new Utils.Interpreter().Start();
                if (Data.Errors.Count > 0)
                {
                    foreach (var e in Data.Errors)
                    {
                        errors.Add(e.ToString());
                    }
                    Status = false;
                    return;
                }
                // =========                
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return;
            }
            var file = GetOutFile();
            WriteOutFile(file, pref, errors);
            if (errors.Count > 0)
                return;
            WriteToStream(file, stream, errors);
            if (errors.Count > 0)
                return;
            stream.Position = 0;
        }
        public void StartCompiler(MemoryStream streamIn, MemoryStream streamOut, List<string> errors)
        {
            Status = true;
            streamIn.Position = 0;
            _compiler.Start(streamIn, streamOut, errors);
            if (errors.Count > 0)
            {
                Status = false;
                return;
            }
            streamOut.Position = 0;
            WriteStreamToFile(streamOut, ".lmsb", errors);
        }
        public void StartAssembler(MemoryStream streamIn, MemoryStream streamOut, List<string> errors)
        {
            Status = true;
            streamIn.Position = 0;
            _assembler.Start(streamIn, streamOut, errors);
            if (errors.Count > 0)
            {
                Status = false;
                return;
            }
            var content = streamOut.ToArray();
            WriteByteToFile(content, ".rbf", errors);
        }

        public bool Status { get; private set; }

        private bool SetLanguage(string language)
        {
            switch (language)
            {
                case "ru":
                    ErrorsCodeList.SetRU();
                    break;
                case "ua":
                    ErrorsCodeList.SetUA();
                    break;
                case "en":
                    ErrorsCodeList.SetEN();
                    break;
                default:
                    Console.WriteLine("Invalid language. Should be \"ru\", or \"ua\", or \"en\".");
                    return false;
            }

            return true;
        }

        // Only ide Clev3r
        public string GetFolderName()
        {
            return Data.Project.Folder;
        }
        public string GetProjectName()
        {
            return Data.Project.ProjectName;
        }
        public bool GetIsFolder()
        {
            return Data.Project.IsFolder;
        }
        public List<string> GetImageList()
        {
            return Data.Project.ImageList;
        }
        public List<string> GetImagePath()
        {
            return Data.Project.ImagePath;
        }
        public List<string> GetImageFullName()
        {
            return Data.Project.ImageFullName;
        }
        public List<string> GetSoundList()
        {
            return Data.Project.SoundList;
        }
        public List<string> GetSoundPath()
        {
            return Data.Project.SoundPath;
        }
        public List<string> GetSoundFullName()
        {
            return Data.Project.SoundFullName;
        }
        public List<string> GetFileList()
        {
            return Data.Project.FileList;
        }
        public List<string> GetFilePath()
        {
            return Data.Project.FilePath;
        }
        public List<string> GetFileFullName()
        {
            return Data.Project.FileFullName;
        }

        public List<string> GetPrograms()
        {
            var outRes = new List<string>();

            outRes.Add(Data.Project.Main.FullName);

            foreach (var curPrg in Data.Project.Includes)
            {
                outRes.Add(curPrg.Value.FullName);
            }

            foreach (var curPrg in Data.Project.Modules)
            {
                outRes.Add(curPrg.Value.FullName);
            }

            return outRes;
        }

        private void WriteToStream(List<string> outFile, Stream stream)
        {
            try
            {
                StreamWriter writer = new StreamWriter(stream);
                foreach (var s in outFile)
                {
                    writer.WriteLine(s);
                }
                writer.Flush();
            }
            catch (Exception ex)
            {
                Console.Write("\n");
                Console.WriteLine(ex.Message);
            }
        }
        private void WriteToStream(List<string> outFile, Stream stream, List<string> errors)
        {
            try
            {
                StreamWriter writer = new StreamWriter(stream);
                foreach (var s in outFile)
                {
                    writer.WriteLine(s);
                }
                writer.Flush();
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }
        private void WriteStreamToFile(MemoryStream stream, string ext)
        {
            try
            {
                var sr = new StreamReader(stream);

                DirectoryInfo di = new DirectoryInfo(Path.Combine(Data.Project.Path, _outFolder));
                if (!di.Exists)
                {
                    di.Create();
                }

                var file = new List<string>();

                while (!sr.EndOfStream)
                {
                    file.Add(sr.ReadLine());
                }

                File.WriteAllLines(Path.Combine(Data.Project.Path, _outFolder, Data.Project.MainName.Replace(Extension.BPProgram, ext)), file);
            }
            catch (Exception ex)
            {
                Console.Write("\n");
                Console.WriteLine(ex.Message);
            }

        }
        private void WriteStreamToFile(MemoryStream stream, string ext, List<string> errors)
        {
            try
            {
                var sr = new StreamReader(stream);

                DirectoryInfo di = new DirectoryInfo(Path.Combine(Data.Project.Path, _outFolder));
                if (!di.Exists)
                {
                    di.Create();
                }

                var file = new List<string>();

                while (!sr.EndOfStream)
                {
                    file.Add(sr.ReadLine());
                }

                File.WriteAllLines(Path.Combine(Data.Project.Path, _outFolder, Data.Project.MainName.Replace(Extension.BPProgram, ext)), file);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

        }
        private void WriteByteToFile(byte[] content, string ext)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(Data.Project.Path, _outFolder));
                if (!di.Exists)
                {
                    di.Create();
                }

                File.WriteAllBytes(Path.Combine(Data.Project.Path, _outFolder, Data.Project.MainName.Replace(Extension.BPProgram, ext)), content);
            }
            catch (Exception ex)
            {
                Console.Write("\n");
                Console.WriteLine(ex.Message);
            }
        }
        private void WriteByteToFile(byte[] content, string ext, List<string> errors)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(Data.Project.Path, _outFolder));
                if (!di.Exists)
                {
                    di.Create();
                }

                File.WriteAllBytes(Path.Combine(Data.Project.Path, _outFolder, Data.Project.MainName.Replace(Extension.BPProgram, ext)), content);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }
        private void WriteOutFile(List<string> file, string pref)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(Data.Project.Path, _outFolder));
                if (!di.Exists)
                {
                    di.Create();
                }

                File.WriteAllLines(Path.Combine(Data.Project.Path, _outFolder, pref + Data.Project.MainName), file);
            }
            catch (Exception ex)
            {
                Console.Write("\n");
                Console.WriteLine(ex.Message);
            }
        }
        private void WriteOutFile(List<string> file, string pref, List<string> errors)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(Data.Project.Path, _outFolder));
                if (!di.Exists)
                {
                    di.Create();
                }

                File.WriteAllLines(Path.Combine(Data.Project.Path, _outFolder, pref + Data.Project.MainName), file);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }

        private List<string> GetOutFile()
        {
            var outFile = new List<string>();

            foreach (var line in Data.Project.OutputLines)
            {
                if (line.OutLines.Count > 0)
                {
                    foreach (var ol in line.OutLines)
                    {
                        outFile.Add(ol.NewLine);
                    }
                }
                else
                {
                    outFile.Add(line.NewLine);
                }
            }

            return outFile;
        }
    }
}

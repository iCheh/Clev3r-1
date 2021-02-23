using System;
using System.IO;
using System.Linq;
using Interpreter;

namespace InterpreterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                var arg1 = args[0].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
                var arg2 = args[1].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar); ;
                FileInfo fi = new FileInfo(arg1);
                if (!fi.Exists)
                {
                    Console.WriteLine("File: " + arg1 + " - not found.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(arg2) && !string.IsNullOrWhiteSpace(arg2))
                    {
                        DirectoryInfo di = new DirectoryInfo(arg2);
                        if (!di.Exists)
                        {
                            Console.WriteLine("Directory: " + arg2 + " - not found.");
                        }
                        else
                        {
                            var text = File.ReadAllLines(arg1).ToList();

                            var _builder = new Builder("ru");
                            _builder.BPStart(fi.Name, fi.DirectoryName + Path.DirectorySeparatorChar, text, arg2 + Path.DirectorySeparatorChar, "~");
                        }
                    }
                    else
                    {
                        var text = File.ReadAllLines(arg1).ToList();

                        var _builder = new Builder("ru");
                        _builder.BPStart(fi.Name, fi.DirectoryName + Path.DirectorySeparatorChar, text, "", "~");
                    }
                }
            }
            else
            {
                Console.WriteLine("Please specify file to compile");
            }
        }
    }
}

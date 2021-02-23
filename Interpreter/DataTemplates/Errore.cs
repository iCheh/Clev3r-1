using Interpreter.DataTemplate;

namespace Interpreter.DataTemplates
{
    struct Errore
    {
        internal Errore(int lineNumber, string fileName, int code, string message)
        {
            LineNumber = lineNumber;
            FileName = fileName;
            Code = code;
            Message = message;
        }
        internal int LineNumber { get; set; }
        internal string FileName { get; set; }
        internal int Code { get; set; }
        internal string Message { get; set; }

        public override string ToString()
        {
            return "file: " + FileName + " line: " + LineNumber.ToString() + " | code: " + Code.ToString() + " ===> " + ErrorsCodeList.GetError(Code) + " " + Message;
        }
    }
}

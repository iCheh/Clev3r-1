using System.Collections.Generic;

namespace Clever.Model.Intellisense
{
    public class IntellisenseEvents
    {
        public IntellisenseEvents()
        {
            Type = IntellisenseType.None;
            Name = "";
            Summary = new List<string>();
            ParamName = new List<string>();
            ParamSummary = new List<string>();
            Return = "";
            Example = "";
            Code = new List<string>();
        }
        public IntellisenseType Type { get; set; }
        public string Name { get; set; }
        public List<string> Summary { get; set; }
        public List<string> ParamName { get; set; }
        public List<string> ParamSummary { get; set; }
        public string Return { get; set; }
        public string Example { get; set; }
        public List<string> Code { get; set; }
    }
}

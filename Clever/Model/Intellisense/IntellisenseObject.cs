using System.Collections.Generic;

namespace Clever.Model.Intellisense
{
    public class IntellisenseObject
    {
        public IntellisenseObject()
        {
            Name = "";
            Methods = new List<IntellisenseMethod>();
            Property = new List<IntellisenseProperty>();
            Event = new List<IntellisenseEvents>();
            Keywords = new List<IntellisenseKeywords>();

        }

        public string Name { get; set; }
        public IntellisenseClass Class { get; set; }
        public List<IntellisenseMethod> Methods { get; set; }
        public List<IntellisenseProperty> Property { get; set; }
        public List<IntellisenseEvents> Event { get; set; }
        public List<IntellisenseKeywords> Keywords { get; set; }
    }
}

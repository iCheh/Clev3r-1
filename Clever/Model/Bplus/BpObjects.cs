//The following Copyright applies to SB-Prime for Small Basic and files in the namespace EV3BasicPlus. 
//Copyright (C) <2017> litdev@hotmail.co.uk 
//This file is part of SB-Prime for Small Basic. 

//SB-Prime for Small Basic is free software: you can redistribute it and/or modify 
//it under the terms of the GNU General Public License as published by 
//the Free Software Foundation, either version 3 of the License, or 
//(at your option) any later version. 

//SB-Prime for Small Basic is distributed in the hope that it will be useful, 
//but WITHOUT ANY WARRANTY; without even the implied warranty of 
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
//GNU General Public License for more details.  

//You should have received a copy of the GNU General Public License 
//along with SB-Prime for Small Basic.  If not, see <http://www.gnu.org/licenses/>. 

using Clever.Model.Bplus.BPInterpreter;
using Clever.Model.Intellisense;
using Clever.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clever.Model.Bplus
{
    internal class BpObjects
    {
        internal static List<BPObject> objects = new List<BPObject>();
        internal static List<Member> keywords = new List<Member>(); // { "Sub", "EndSub", "For", "To", "Step", "EndFor", "If", "Then", "Else", "ElseIf", "EndIf", "While", "EndWhile", "Goto" };
        internal static List<string> methods = new List<string>();
        internal static bool UpdateWork = false;
        internal static int TimeT = 0;
        
        //internal List<string> variables = new List<string>();
        //internal List<string> subroutines = new List<string>();
        //internal List<string> labels = new List<string>();

        internal string GetKeywords(string input)
        {
            string result = "";
            List<string> data = new List<string>();
            foreach (Member member in keywords)
            {
                if (member.name.ToUpper().StartsWith(input.ToUpper()))
                {
                    data.Add(member.name + "?0");
                }
            }
            data = data.Distinct().ToList();
            data.Sort();
            foreach (string value in data)
            {
                result += value + " ";
            }
            return result;
        }

        internal string GetObjects(string input)
        {
            string result = "";
            List<string> data = new List<string>();
            foreach (BPObject label in objects)
            {
                if (label.name.ToUpper().StartsWith(input.ToUpper()))
                {
                    data.Add(label.name + "?1");
                }
            }
            data = data.Distinct().ToList();
            data.Sort();
            foreach (string value in data)
            {
                result += value + " ";
            }
            return result;
        }

        internal string GetModuleObjects(string input, string name)
        {
            string result = "";
            List<string> data = new List<string>();

            if (IntellisenseParser.Get.Data.ContainsKey(name))
            {
                var tmpModule = IntellisenseParser.Get.Data[name].Map.Imports;
                foreach (var m in tmpModule)
                {
                    if (m.Key.StartsWith(input.ToLower()))
                    {
                        data.Add(m.Value.OriginName.Replace(".bpm","") + "?8");
                    }
                }
            }

            data = data.Distinct().ToList();
            data.Sort();
            foreach (string value in data)
            {
                result += value + " ";
            }

            return result;
        }

        internal string GetMembers(string obj, string input)
        {
            string result = "";
            input = input.ToUpper();
            List<string> data = new List<string>();
            foreach (BPObject label in objects)
            {
                if (obj.ToUpper() == label.name.ToUpper())
                {
                    foreach (Member member in label.members)
                    {
                        if (member.name.ToUpper().StartsWith(input.ToUpper()))
                        {
                            switch (member.type)
                            {
                                case MemberTypes.Method:
                                    data.Add(member.name + "?2");
                                    break;
                                case MemberTypes.Property:
                                    data.Add(member.name + "?3");
                                    break;
                                case MemberTypes.Event:
                                    data.Add(member.name + "?4");
                                    break;
                            }
                        }
                    }
                    break;
                }
            }
            data = data.Distinct().ToList();
            //data.Sort();
            foreach (string value in data)
            {
                result += value + " ";
            }

            return result;
        }

        internal string GetModuleMembers(string obj, string input, string name)
        {
            string result = "";
            input = input.ToUpper();
            List<string> data = new List<string>();

            if (IntellisenseParser.Get.Data.ContainsKey(name))
            {
                var vars = IntellisenseParser.Get.Data[name].Map.GetModuleVariables(obj, name);
                var mem = IntellisenseParser.Get.Data[name].Map.GetModuleMethods(obj, name);

                if (vars.Count > 0)
                {
                    foreach (string v in vars)
                    {
                        if (v.ToLower().StartsWith(input.ToLower()))
                        {
                            data.Add(v + "?3");
                        }
                    }
                }

                if (mem.Count > 0)
                {
                    foreach (string m in mem)
                    {
                        if (m.ToLower().StartsWith(input.ToLower()))
                        {
                            data.Add(m + "?2");
                        }
                    }
                }
            }

            foreach (string value in data)
            {
                result += value + " ";
            }

            return result;
        }

        internal string GetVariables(string input, int lineNumber, string name)
        {
            string result = "";

            if (IntellisenseParser.Get.Data.ContainsKey(name))
            {
                var vars = IntellisenseParser.Get.Data[name].Map.GetVariables(lineNumber, name);

                /*
                CommonData.Status.Clear();
                foreach (var vv in vars)
                {
                    CommonData.Status.Add("var => " + vv);
                }
                */

                if (vars.Count > 0)
                {
                    var data = new List<string>();

                    foreach (string v in vars)
                    {
                        if (v.ToLower().StartsWith(input.ToLower()))
                        {
                            data.Add(v + "?5");
                        }
                    }

                    foreach (string value in data)
                    {
                        result += value + " ";
                    }
                }
            }
            return result;
        }

        internal string GetSubroutines(string input, string name)
        {
            string result = "";
            List<string> data = new List<string>();

            if (IntellisenseParser.Get.Data.ContainsKey(name))
            {
                var mem = IntellisenseParser.Get.Data[name].Map.GetSubroutines(name);

                if (mem.Count > 0)
                {
                    foreach (var m in mem)
                    {
                        if (m.ToLower().StartsWith(input.ToLower()))
                        {
                            data.Add(m + "?6");
                        }
                    }
                }
            }
            /*
            foreach (string label in subroutines)
            {
                if (label.ToUpper().StartsWith(input.ToUpper()))
                {
                    data.Add(label + "?6");
                }
            }
            */

            data = data.Distinct().ToList();
            data.Sort();
            foreach (string value in data)
            {
                result += value + " ";
            }
            return result;
        }

        internal string GetSubSymmary(string input, string name, string lineText, bool parseLine)
        {
            string result = "";
            List<string> data = new List<string>();
            var tmpSubs = new List<(string, int)>();

            if (IntellisenseParser.Get.Data.ContainsKey(name))
            {
                var mem = IntellisenseParser.Get.Data[name].Map.Subroutines;
                var inc = IntellisenseParser.Get.Data[name].Map.Includes;

                if (mem.Count > 0)
                {
                    foreach (var m in mem)
                    {
                        var tmpS = (m.Name.ToLower(), m.ParamCount);
                        if (!tmpSubs.Contains(tmpS))
                        {
                            if (m.Name.ToLower() == input.ToLower())
                            {
                                
                                if (data.Count > 0 && m.Summary.Count > 0)
                                {
                                    data.Add('\n'.ToString());
                                }
                                

                                foreach (var s in m.Summary)
                                {
                                    data.Add(s);
                                }
                            }
                            tmpSubs.Add(tmpS);
                        }
                    }
                }

                if (inc.Count > 0)
                {
                    //CommonData.Status.Clear();
                    foreach (var ii in inc)
                    {
                        var nn = ii.Value.OriginName;
                        if (IntellisenseParser.Get.Data.ContainsKey(nn))
                        {
                            //CommonData.Status.Add(nn);
                            var subs = IntellisenseParser.Get.Data[nn].Map.Subroutines;

                            if (subs.Count > 0)
                            {
                                foreach (var m in subs)
                                {
                                    var tmpS = (m.Name.ToLower(), m.ParamCount);
                                    if (!tmpSubs.Contains(tmpS))
                                    {
                                        if (m.Name.ToLower() == input.ToLower())
                                        {
                                            
                                            if (data.Count > 0 && m.Summary.Count > 0)
                                            {
                                                data.Add('\n'.ToString());
                                            }
                                            

                                            foreach (var s in m.Summary)
                                            {
                                                data.Add(s);
                                            }
                                        }
                                        tmpSubs.Add(tmpS);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < data.Count; i++)
            {
                result += data[i];
                if (i < data.Count - 1)
                {
                    result += '\n';
                }
            }
            return result;
        }

        internal string GetModuleSymmary(string input)
        {
            string result = "";
            List<string> data = new List<string>();
            

            if (IntellisenseParser.Get.Data.ContainsKey(input + ".bpm"))
            {
                
                var mem = IntellisenseParser.Get.Data[input + ".bpm"].Map;
                
                if (mem.Summary.Count > 0)
                {
                    data.Add(input + '\n');
                    foreach (var s in mem.Summary)
                    {
                        data.Add(s);
                    }
                }
            }

            for (int i = 0; i < data.Count; i++)
            {
                result += data[i];
                if (i < data.Count - 1)
                {
                    result += '\n';
                }
            }
            return result;
        }

        internal string GetVarSummary(string input, string name)
        {
            string result = "";
            List<string> data = new List<string>();

            if (IntellisenseParser.Get.Data.ContainsKey(name))
            {
                var mem = IntellisenseParser.Get.Data[name].Map.Variables;
                
                if (mem.Count > 0)
                {
                    foreach (var m in mem)
                    {
                        if (m.Name.ToLower() == input.ToLower())
                        {
                            foreach (var s in m.Summary)
                            {
                                data.Add(s);
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < data.Count; i++)
            {
                result += data[i];
                if (i < data.Count - 1)
                {
                    result += '\n';
                }
            }
            return result;
        }

        internal string GetLabels(string input, int lineNumber, string name)
        {
            string result = "";

            if (IntellisenseParser.Get.Data.ContainsKey(name))
            {
                var labs = IntellisenseParser.Get.Data[name].Map.GetLabels(lineNumber);
                if (labs.Count > 0)
                {
                    var data = new List<string>();

                    foreach (string l in labs)
                    {
                        if (l.ToLower().StartsWith(input.ToLower()))
                        {
                            data.Add(l + "?5");
                        }
                    }

                    foreach (string value in data)
                    {
                        result += value + " ";
                    }
                }
            }

            return result;
        }
    }

    public class BPObject : IComparable
    {
        public string extension;
        public string name;
        public string summary;
        public List<Member> members = new List<Member>();

        public int CompareTo(object obj)
        {
            BPObject _obj = (BPObject)obj;
            if (extension == _obj.extension) return name.CompareTo(_obj.name);
            else return extension.CompareTo(_obj.extension);
        }
    }

    public class Member : IComparable
    {
        public string name;
        public MemberTypes type;
        public string summary;
        public Dictionary<string, string> arguments = new Dictionary<string, string>();
        public string returns;
        public Dictionary<string, string> other = new Dictionary<string, string>();

        public int CompareTo(object obj)
        {
            Member member = (Member)obj;
            if (type == member.type) return name.CompareTo(member.name);
            else return -type.CompareTo(member.type);
        }
    }
}

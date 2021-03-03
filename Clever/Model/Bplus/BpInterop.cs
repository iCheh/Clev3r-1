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

using Clever;
using Clever.Model.Bplus;
using Clever.Model.Intellisense;
using System.Reflection;

namespace Clever.Model.Bplus
{
    public class BPInterop
    {
        public BPInterop()
        {
            SetBPObjects();
            SetMethods();
        }
        private void SetBPObjects()
        {
            //ObservableCollection<IntellisenseObject> Iobjects = App.IntellisenseObjects;

            BpObjects bpObj = new BpObjects();

            foreach (var obj in App.IntellisenseObjects)
            {
                if (obj.Class.Type == IntellisenseType.Keyword)
                {
                    foreach (var kw in obj.Keywords)
                    {
                        Member member = new Member();
                        member.name = kw.Name;
                        member.type = MemberTypes.Custom;
                        foreach (var w in kw.Summary)
                        {
                            member.summary += w;
                            member.summary += " ";
                        }
                        member.other.Add("example", kw.Example);
                        BpObjects.keywords.Add(member);
                    }
                }
                else
                {
                    var sbObj = new BPObject();
                    BpObjects.objects.Add(sbObj);
                    sbObj.extension = "SmallBasicEV3Extension";
                    sbObj.name = obj.Class.Name;
                    foreach (var w in obj.Class.Summary)
                    {
                        sbObj.summary += w;
                        sbObj.summary += " ";
                    }


                    foreach (var method in obj.Methods)
                    {
                        var member = new Member();
                        sbObj.members.Add(member);
                        member.name = method.Name;
                        member.type = MemberTypes.Method;
                        foreach (var w in method.Summary)
                        {
                            member.summary += w;
                            member.summary += " ";
                        }

                        member.returns = method.Return;
                        int i = 0;
                        foreach (var param in method.ParamName)
                        {
                            member.arguments.Add(param, method.ParamSummary[i]);
                            i++;
                        }
                        if (method.Example != "")
                        {
                            member.other.Add("example", method.Example);
                        }
                    }

                    foreach (var prop in obj.Property)
                    {
                        var member = new Member();
                        sbObj.members.Add(member);
                        member.name = prop.Name;
                        member.type = MemberTypes.Method;
                        foreach (var w in prop.Summary)
                        {
                            member.summary += w;
                            member.summary += " ";
                        }
                        member.returns = prop.Return;
                        int i = 0;
                        foreach (var param in prop.ParamName)
                        {
                            member.arguments.Add(param, prop.ParamSummary[i]);
                            i++;
                        }
                        if (prop.Example != "")
                        {
                            member.other.Add("example", prop.Example);
                        }
                    }

                    foreach (var ev in obj.Event)
                    {
                        var member = new Member();
                        sbObj.members.Add(member);
                        member.name = ev.Name;
                        member.type = MemberTypes.Method;
                        foreach (var w in ev.Summary)
                        {
                            member.summary += w;
                            member.summary += " ";
                        }
                        member.returns = ev.Return;
                        int i = 0;
                        foreach (var param in ev.ParamName)
                        {
                            member.arguments.Add(param, ev.ParamSummary[i]);
                            i++;
                        }
                        if (ev.Example != "")
                        {
                            member.other.Add("example", ev.Example);
                        }
                    }
                }
            }
        }

        private void SetMethods()
        {
            BpObjects.methods.Add("assert");
            BpObjects.methods.Add("buttons");
            BpObjects.methods.Add("byte");
            BpObjects.methods.Add("ev3");
            BpObjects.methods.Add("ev3file");
            BpObjects.methods.Add("f");
            BpObjects.methods.Add("lcd");
            BpObjects.methods.Add("mailbox");
            BpObjects.methods.Add("math");
            BpObjects.methods.Add("motor");
            BpObjects.methods.Add("motorA");
            BpObjects.methods.Add("motorAB");
            BpObjects.methods.Add("motorAC");
            BpObjects.methods.Add("motorAD");
            BpObjects.methods.Add("motorB");
            BpObjects.methods.Add("motorBC");
            BpObjects.methods.Add("motorBD");
            BpObjects.methods.Add("motorC");
            BpObjects.methods.Add("motorCD");
            BpObjects.methods.Add("motorD");
            BpObjects.methods.Add("program");
            BpObjects.methods.Add("row");
            BpObjects.methods.Add("sensor");
            BpObjects.methods.Add("sensor1");
            BpObjects.methods.Add("sensor2");
            BpObjects.methods.Add("sensor3");
            BpObjects.methods.Add("sensor4");
            BpObjects.methods.Add("speaker");
            BpObjects.methods.Add("text");
            BpObjects.methods.Add("thread");
            BpObjects.methods.Add("time");
            BpObjects.methods.Add("vector");
        }
    }
}

using Clever.CommonData;
using Clever.Model.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml;

namespace Clever.Model.Intellisense
{
    class ReadXml
    {
        ObservableCollection<IntellisenseObject> intellisenseObjects;

        public ReadXml()
        {
            intellisenseObjects = new ObservableCollection<IntellisenseObject>();
        }
        public ObservableCollection<IntellisenseObject> Read()
        {
            string path = Application.ResourceAssembly.Location.Replace("Clever.exe", "Help") + "\\" + Configurations.Get.Language + "\\";
            IntellisenseOperators.Text = "";

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                string keywordsFile = "";
                foreach (var f in files)
                {
                    if (f.IndexOf("Keywords") != -1)
                    {
                        keywordsFile = f;
                        break;
                    }
                }

                FileInfo fi = new FileInfo(keywordsFile);
                if (fi.Extension == ".xml")
                {
                    Parse(File.ReadAllText(keywordsFile));
                }

                foreach (var file in files)
                {
                    fi = new FileInfo(file);
                    if (fi.Extension == ".xml" && file.IndexOf("Keywords") == -1)
                    {
                        Parse(File.ReadAllText(file));
                    }
                    else if (fi.Extension == ".txt" && file.IndexOf("Operators") != -1)
                    {
                        IntellisenseOperators.Text = File.ReadAllText(file);
                    }
                }

            }

            return intellisenseObjects;
        }

        private void Parse(string res)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(res);
            var obj = new IntellisenseObject();
            foreach (XmlNode xmlNode in doc.SelectNodes("/doc/members/member"))
            {
                string name = xmlNode.Attributes.Item(0).InnerText.Trim();

                if (name[0] == 'T' || name[0] == 'W')
                {
                    var objC = new IntellisenseClass();
                    if (name[0] == 'T')
                    {
                        objC.Name = name.Replace("T:", "");
                        obj.Name = name.Replace("T:", "");
                        objC.Type = IntellisenseType.Class;
                    }
                    else if (name[0] == 'W')
                    {
                        objC.Name = name.Replace("W:", "");
                        obj.Name = name.Replace("W:", "");
                        objC.Type = IntellisenseType.Keyword;
                    }

                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "summary":
                                var words = node.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var w in words)
                                {
                                    if (w != "")
                                    {
                                        objC.Summary.Add(w);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    obj.Class = objC;
                }
                else if (name[0] == 'M')
                {
                    var objM = new IntellisenseMethod();
                    objM.Name = name.Replace("M:", "");
                    objM.Type = IntellisenseType.Method;
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "summary":
                                var words = node.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var w in words)
                                {
                                    if (w != "")
                                    {
                                        objM.Summary.Add(w);
                                    }
                                }
                                break;
                            case "param":
                                string paramName = node.Attributes.Item(0).InnerText.Trim();
                                objM.ParamName.Add(paramName);
                                objM.ParamSummary.Add(node.InnerText.Trim());
                                break;
                            case "returns":
                                objM.Return = node.InnerText.Trim();
                                break;
                            case "example":
                                foreach (XmlNode nodeM in node)
                                {
                                    switch (nodeM.Name)
                                    {
                                        case "code":
                                            var wm = nodeM.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var w in wm)
                                            {
                                                objM.Code.Add(w);
                                            }
                                            break;
                                        case "text":
                                            objM.Example = nodeM.InnerText.Trim();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case "remarks":
                                //MainWindowVM.Status.Add(node.InnerText.Trim());
                                break;
                            case "value":
                                //MainWindowVM.Status.Add(node.InnerText.Trim());
                                break;
                            default:
                                break;
                        }
                    }
                    obj.Methods.Add(objM);
                }
                else if (name[0] == 'E')
                {
                    var objE = new IntellisenseEvents();
                    objE.Name = name.Replace("E:", "");
                    objE.Type = IntellisenseType.Event;
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "summary":
                                var words = node.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var w in words)
                                {
                                    if (w != "")
                                    {
                                        objE.Summary.Add(w);
                                    }
                                }
                                break;
                            case "param":
                                string paramName = node.Attributes.Item(0).InnerText.Trim();
                                objE.ParamName.Add(paramName);
                                objE.ParamSummary.Add(node.InnerText.Trim());
                                break;
                            case "returns":
                                objE.Return = node.InnerText.Trim();
                                break;
                            case "example":
                                foreach (XmlNode nodeE in node)
                                {
                                    switch (nodeE.Name)
                                    {
                                        case "code":
                                            var wm = nodeE.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var w in wm)
                                            {
                                                objE.Code.Add(w);
                                            }
                                            break;
                                        case "text":
                                            objE.Example = nodeE.InnerText.Trim();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case "remarks":
                                //MainWindowVM.Status.Add(node.InnerText.Trim());
                                break;
                            case "value":
                                //MainWindowVM.Status.Add(node.InnerText.Trim());
                                break;
                            default:
                                break;
                        }
                    }
                    obj.Event.Add(objE);
                }
                else if (name[0] == 'P')
                {
                    var objP = new IntellisenseProperty();
                    objP.Name = name.Replace("P:", "");
                    objP.Type = IntellisenseType.Property;
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "summary":
                                var words = node.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var w in words)
                                {
                                    if (w != "")
                                    {
                                        objP.Summary.Add(w);
                                    }
                                }
                                break;
                            case "param":
                                string paramName = node.Attributes.Item(0).InnerText.Trim();
                                objP.ParamName.Add(paramName);
                                objP.ParamSummary.Add(node.InnerText.Trim());
                                break;
                            case "returns":
                                objP.Return = node.InnerText.Trim();
                                break;
                            case "example":
                                foreach (XmlNode nodeP in node)
                                {
                                    switch (nodeP.Name)
                                    {
                                        case "code":
                                            var wm = nodeP.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var w in wm)
                                            {
                                                objP.Code.Add(w);
                                            }
                                            break;
                                        case "text":
                                            objP.Example = nodeP.InnerText.Trim();
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case "remarks":
                                //MainWindowVM.Status.Add(node.InnerText.Trim());
                                break;
                            case "value":
                                //MainWindowVM.Status.Add(node.InnerText.Trim());
                                break;
                            default:
                                break;
                        }
                    }
                    obj.Property.Add(objP);
                }
                else if (name[0] == 'K')
                {
                    //MessageBox.Show(name);
                    var objK = new IntellisenseKeywords();
                    objK.Name = name.Replace("K:", "");
                    objK.Type = IntellisenseType.Keyword;
                    foreach (XmlNode node in xmlNode.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "summary":
                                var words = node.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var w in words)
                                {
                                    if (w != "")
                                    {
                                        objK.Summary.Add(w);
                                    }
                                }
                                break;
                            case "param":
                                string paramName = node.Attributes.Item(0).InnerText.Trim();
                                objK.ParamName.Add(paramName);
                                objK.ParamSummary.Add(node.InnerText.Trim());
                                break;
                            case "returns":
                                objK.Return = node.InnerText.Trim();
                                break;
                            case "example":
                                foreach (XmlNode nodeK in node)
                                {
                                    switch (nodeK.Name)
                                    {
                                        case "code":
                                            var wm = nodeK.InnerText.Trim().Split(new string[] { "\n", "\r", "\r\n", "\n\r", "\v", "\t", "\0", "\b", "    " }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var w in wm)
                                            {
                                                string tmp = w.Trim();
                                                if (w != "")
                                                    objK.Code.Add(w.Trim());
                                            }
                                            /*
                                            if (objK.Name == "For")
                                            {
                                                MessageBox.Show("code => " + nodeK.InnerText.Trim());
                                            }
                                            */
                                            break;
                                        case "text":
                                            objK.Example = nodeK.InnerText.Trim();
                                            /*
                                            if (objK.Name == "For")
                                            {
                                                MessageBox.Show("text => " + nodeK.InnerText.Trim());
                                            }
                                            */
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case "remarks":
                                //MainWindowVM.Status.Add(node.InnerText.Trim());
                                break;
                            case "value":
                                //MainWindowVM.Status.Add(node.InnerText.Trim());
                                break;
                            default:
                                break;
                        }
                    }
                    obj.Keywords.Add(objK);
                }
            }
            intellisenseObjects.Add(obj);
        }
    }
}

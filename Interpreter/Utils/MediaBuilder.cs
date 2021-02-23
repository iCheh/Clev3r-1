using Interpreter.CommonData;
using Interpreter.DataTemplates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.Utils
{
    internal class MediaBuilder
    {
        private string mediaPath = "";
        private string mediaName = "";
        private string mediaFullName = "";
        internal void ParseMedia(Line line)
        {
            if (!Data.Project.IsFolder)
            {
                return;
            }

            string text = line.NewLine;
            if (text.ToLower().IndexOf("lcd.bmpfile") != -1 && (text.IndexOf("'") == -1 || text.ToLower().IndexOf("lcd.bmpfile") > text.IndexOf("'")))
            {
                if (text.IndexOf("\"") != -1 && text.LastIndexOf("\"") != -1 && text.LastIndexOf("\"") > text.IndexOf("\""))
                {
                    mediaPath = "";
                    mediaName = "";
                    mediaFullName = "";
                    var t = ReplaseImageLine(text);
                    var tmpLine = new Line(LineBuilder.GetWords(t), t);
                    line.NewLine = tmpLine.NewLine;
                    line.Words.Clear();
                    foreach (var w in tmpLine.Words)
                    {
                        line.Words.Add(w);
                    }
                }
            }
            else if (text.ToLower().IndexOf("speaker.play") != -1 && (text.IndexOf("'") == -1 || text.ToLower().IndexOf("speaker.play") > text.IndexOf("'")))
            {
                if (text.IndexOf("\"") != -1 && text.LastIndexOf("\"") != -1 && text.LastIndexOf("\"") > text.IndexOf("\""))
                {
                    mediaPath = "";
                    mediaName = "";
                    mediaFullName = "";
                    var t = ReplaseSoundLine(text);
                    var tmpLine = new Line(LineBuilder.GetWords(t), t);
                    line.NewLine = tmpLine.NewLine;
                    line.Words.Clear();
                    foreach (var w in tmpLine.Words)
                    {
                        line.Words.Add(w);
                    }
                }
            }
            else if (text.ToLower().IndexOf("ev3file.openwrite") != -1 && (text.IndexOf("'") == -1 || text.ToLower().IndexOf("ev3file.openwrite") > text.IndexOf("'")))
            {
                if (text.IndexOf("\"") != -1 && text.LastIndexOf("\"") != -1 && text.LastIndexOf("\"") > text.IndexOf("\""))
                {
                    mediaPath = "";
                    mediaName = "";
                    mediaFullName = "";
                    var t = ReplaseOpenLine(text);
                    var tmpLine = new Line(LineBuilder.GetWords(t), t);
                    line.NewLine = tmpLine.NewLine;
                    line.Words.Clear();
                    foreach (var w in tmpLine.Words)
                    {
                        line.Words.Add(w);
                    }
                }
            }
            else if (text.ToLower().IndexOf("ev3file.openappend") != -1 && (text.IndexOf("'") == -1 || text.ToLower().IndexOf("ev3file.openappend") > text.IndexOf("'")))
            {
                if (text.IndexOf("\"") != -1 && text.LastIndexOf("\"") != -1 && text.LastIndexOf("\"") > text.IndexOf("\""))
                {
                    mediaPath = "";
                    mediaName = "";
                    mediaFullName = "";
                    var t = ReplaseOpenLine(text);
                    var tmpLine = new Line(LineBuilder.GetWords(t), t);
                    line.NewLine = tmpLine.NewLine;
                    line.Words.Clear();
                    foreach (var w in tmpLine.Words)
                    {
                        line.Words.Add(w);
                    }
                }
            }
            else if (text.ToLower().IndexOf("ev3file.openread") != -1 && (text.IndexOf("'") == -1 || text.ToLower().IndexOf("ev3file.openread") > text.IndexOf("'")))
            {
                if (text.IndexOf("\"") != -1 && text.LastIndexOf("\"") != -1 && text.LastIndexOf("\"") > text.IndexOf("\""))
                {
                    mediaPath = "";
                    mediaName = "";
                    mediaFullName = "";
                    var t = ReplaseOpenLine(text);
                    var tmpLine = new Line(LineBuilder.GetWords(t), t);
                    line.NewLine = tmpLine.NewLine;
                    line.Words.Clear();
                    foreach (var w in tmpLine.Words)
                    {
                        line.Words.Add(w);
                    }
                }
            }
            else if (text.ToLower().IndexOf("ev3file.tablelookup") != -1 && (text.IndexOf("'") == -1 || text.ToLower().IndexOf("ev3file.tablelookup") > text.IndexOf("'")))
            {
                if (text.IndexOf("\"") != -1 && text.LastIndexOf("\"") != -1 && text.LastIndexOf("\"") > text.IndexOf("\""))
                {
                    mediaPath = "";
                    mediaName = "";
                    mediaFullName = "";
                    var t = ReplaseTableLookupLine(text);
                    var tmpLine = new Line(LineBuilder.GetWords(t), t);
                    line.NewLine = tmpLine.NewLine;
                    line.Words.Clear();
                    foreach (var w in tmpLine.Words)
                    {
                        line.Words.Add(w);
                    }
                }
            }
        }

        private string ReplaseImageLine(string line)
        {
            int start = line.IndexOf("\"") + 1;
            int end = line.LastIndexOf("\"");
            string name = line.Substring(start, end - start);
            string play = line.Substring(0, start - 1);
            string tmp = AddPathMedia(play, name);

            if (mediaName != "" && !Data.Project.ImageList.Contains(mediaName + ".rgf"))
            {
                Data.Project.ImageList.Add(mediaName + ".rgf");
                Data.Project.ImagePath.Add(mediaPath);
                if (mediaFullName != "no")
                    Data.Project.ImageFullName.Add(mediaFullName + ".rgf");
                else
                    Data.Project.ImageFullName.Add("no");
            }

            return tmp;
        }
        private string ReplaseSoundLine(string line)
        {
            int start = line.IndexOf("\"") + 1;
            int end = line.LastIndexOf("\"");
            string name = line.Substring(start, end - start);
            string play = line.Substring(0, start - 1);
            string tmp = AddPathMedia(play, name);

            if (mediaName != "" && !Data.Project.SoundList.Contains(mediaName + ".rsf"))
            {
                Data.Project.SoundList.Add(mediaName + ".rsf");
                Data.Project.SoundPath.Add(mediaPath);
                if (mediaFullName != "no")
                    Data.Project.SoundFullName.Add(mediaFullName + ".rsf");
                else
                    Data.Project.SoundFullName.Add("no");
            }

            return tmp;
        }
        private string ReplaseOpenLine(string line)
        {
            int start = line.IndexOf("\"") + 1;
            int end = line.LastIndexOf("\"");
            string name = line.Substring(start, end - start);
            string play = line.Substring(0, start - 1);
            string tmp = AddPathFile(play, name);

            if (mediaName != "" && !Data.Project.FileList.Contains(mediaName))
            {
                Data.Project.FileList.Add(mediaName);
                Data.Project.FilePath.Add(mediaPath);
                Data.Project.FileFullName.Add(mediaFullName);
            }

            return tmp;
        }
        private string ReplaseTableLookupLine(string line)
        {
            int start = line.IndexOf("\"") + 1;
            int end = line.LastIndexOf("\"");
            string name = line.Substring(start, end - start);
            string play = line.Substring(0, start - 1);
            string other = line.Substring(end + 1);
            string tmp = AddPathTableLookupFile(play, name, other);

            if (mediaName != "" && !Data.Project.FileList.Contains(mediaName))
            {
                Data.Project.FileList.Add(mediaName);
                Data.Project.FilePath.Add(mediaPath);
                Data.Project.FileFullName.Add(mediaFullName);
            }

            return tmp;
        }
        private string AddPathMedia(string play, string name)
        {
            string newLine = "";

            if (Data.Project.Folder.ToUpper() == "PRJS")
            {
                if (Data.Project.ProjectName != "")
                {
                    newLine = play + "\"" + Data.Project.ProjectName + "/Media/" + name + "\")";
                    mediaName = name;
                    mediaPath = Data.Project.ProjectName + "/Media/";
                    mediaFullName = Data.Project.ProjectName + "/Media/" + name;
                }
                else
                {
                    newLine = play + "\"" + name + "\")";
                    mediaName = name;
                    mediaPath = "no";
                    mediaFullName = "no";
                }
            }
            else if (Data.Project.Folder.ToUpper() == "SD")
            {
                if (Data.Project.ProjectName != "")
                {
                    newLine = play + "\"SD_Card/" + Data.Project.ProjectName + "/Media/" + name + "\")";
                    mediaName = name;
                    mediaPath = "SD_Card/" + Data.Project.ProjectName + "/Media/";
                    mediaFullName = "SD_Card/" + Data.Project.ProjectName + "/Media/" + name;
                }
                else
                {
                    newLine = play + "\"" + name + "\")";
                    mediaName = name;
                    mediaPath = "no";
                    mediaFullName = "no";
                }
            }
            else
            {
                newLine = play + "\"" + name + "\")";
                mediaName = name;
                mediaPath = "no";
                mediaFullName = "no";
            }

            return newLine;
        }
        private string AddPathFile(string play, string name)
        {
            string newLine = "";

            if (Data.Project.Folder.ToUpper() == "PRJS")
            {
                if (Data.Project.ProjectName != "")
                {
                    newLine = play + "\"" + Data.Project.ProjectName + "/Files/" + name + "\")";
                    mediaName = name;
                    mediaPath = Data.Project.ProjectName + "/Files/";
                    mediaFullName = Data.Project.ProjectName + "/Files/" + name;
                }
                else
                {
                    newLine = play + "\"" + name + "\")";
                    mediaName = name;
                    mediaPath = "no";
                    mediaFullName = "no";
                }
            }
            else if (Data.Project.Folder.ToUpper() == "SD")
            {
                if (Data.Project.ProjectName != "")
                {
                    newLine = play + "\"SD_Card/" + Data.Project.ProjectName + "/Files/" + name + "\")";
                    mediaName = name;
                    mediaPath = "SD_Card/" + Data.Project.ProjectName + "/Files/";
                    mediaFullName = "SD_Card/" + Data.Project.ProjectName + "/Files/" + name;
                }
                else
                {
                    newLine = play + "\"" + name + "\")";
                    mediaName = name;
                    mediaPath = "no";
                    mediaFullName = "no";
                }
            }
            else
            {
                newLine = play + "\"" + name + "\")";
                mediaName = name;
                mediaPath = "no";
                mediaFullName = "no";
            }

            return newLine;
        }
        private string AddPathTableLookupFile(string play, string name, string other)
        {
            string newLine = "";

            if (Data.Project.Folder.ToUpper() == "PRJS")
            {
                if (Data.Project.ProjectName != "")
                {
                    newLine = play + "\"" + Data.Project.ProjectName + "/Files/" + name + "\"" + other + ")";
                    mediaName = name;
                    mediaPath = Data.Project.ProjectName + "/Files/";
                    mediaFullName = Data.Project.ProjectName + "/Files/" + name;
                }
                else
                {
                    newLine = play + "\"" + name + "\"" + other + ")";
                    mediaName = name;
                    mediaPath = "no";
                    mediaFullName = "no";
                }
            }
            else if (Data.Project.Folder.ToUpper() == "SD")
            {
                if (Data.Project.ProjectName != "")
                {
                    newLine = play + "\"SD_Card/" + Data.Project.ProjectName + "/Files/" + name + "\"" + other + ")";
                    mediaName = name;
                    mediaPath = "SD_Card/" + Data.Project.ProjectName + "/Files/";
                    mediaFullName = "SD_Card/" + Data.Project.ProjectName + "/Files/" + name;
                }
                else
                {
                    newLine = play + "\"" + name + "\"" + other + ")";
                    mediaName = name;
                    mediaPath = "no";
                    mediaFullName = "no";
                }
            }
            else
            {
                newLine = play + "\"" + name + "\"" + other + ")";
                mediaName = name;
                mediaPath = "no";
                mediaFullName = "no";
            }

            return newLine;
        }
    }
}

using Interpreter.CommonData;
using Interpreter.DataTemplates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Interpreter.Parsers
{
    internal static class FolderErrorParser
    {
        internal static void Start(Line line)
        {
            if (Data.Project.IsFolder)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1208, ""));
                return;
            }
            else
            {
                Data.Project.IsFolder = true;
            }

            if (line.Words.Count != 3)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1201, ""));
                return;
            }
            else if (line.Words[1].Token != Enums.Tokens.STRING || line.Words[2].Token != Enums.Tokens.STRING)
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1202, ""));
                return;
            }
            else if (line.Words[1].Text.ToLower().Replace("\"","") != "prjs" && line.Words[1].Text.ToLower().Replace("\"", "") != "sd")
            {
                Data.Errors.Add(new Errore(line.Number, line.FileName, 1203, ""));
                return;
            }
            else
            {
                var folderName = line.Words[2].Text.Replace("\"", "");
                if (folderName.Length > 32)
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1204, ""));
                    return;
                }
                else if (folderName.Length == 0 || folderName.Trim() == "")
                {
                    Data.Errors.Add(new Errore(line.Number, line.FileName, 1205, ""));
                    return;
                }
                else
                {
                    var c = folderName[0];
                    Regex regex1 = new Regex("[a-zA-Z]");
                    MatchCollection matches1 = regex1.Matches(c.ToString());
                    if (matches1.Count != 1)
                    {
                        Data.Errors.Add(new Errore(line.Number, line.FileName, 1206, ""));
                        return;
                    }
                    else
                    {
                        Regex regex2 = new Regex("[0-9a-zA-Z_]");
                        MatchCollection matches2 = regex2.Matches(folderName);
                        if (matches2.Count != folderName.Length)
                        {
                            Data.Errors.Add(new Errore(line.Number, line.FileName, 1207, ""));
                            return;
                        }
                    }
                }
            }
        }
    }
}

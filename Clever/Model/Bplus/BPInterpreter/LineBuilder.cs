using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clever.Model.Bplus.BPInterpreter
{
    internal class LineBuilder
    {
        private HashSet<string> _className;
        private HashSet<string> _keywords;

        internal LineBuilder(HashSet<string> className, HashSet<string> keywords)
        {
            _className = className;
            _keywords = keywords;
        }
        internal List<Word> GetWords(string strLine)
        {
            List<string> words = new List<string>();
            string line = "";
            words.Clear();

            // Проверим, что строка не начинается с комментария
            int comment = strLine.IndexOf("\'");
            if (comment == -1)
            {
                line = strLine.Trim();
            }
            else if (comment > 0)
            {
                string tmp = "";
                for (int i = 0; i < comment; i++)
                {
                    tmp += strLine[i];
                }
                line = tmp.Trim();
            }

            var tmpWords = new string[] { };
            // Проверим в строке наличие String ( " " )
            int index = line.IndexOf("\"");
            if (index != -1)
            {
                var listTmp = new List<string>();
                bool stop = false;
                string tmpS = "";
                foreach (var ch in line)
                {
                    if (!stop)
                    {
                        if (ch == '"')
                        {
                            stop = true;
                            if (tmpS != "")
                            {
                                var w = tmpS.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var ww in w)
                                {
                                    listTmp.Add(ww);
                                }
                            }
                            tmpS = "\"";
                        }
                        else
                        {
                            tmpS += ch;
                        }
                    }
                    else
                    {
                        if (ch == '"')
                        {
                            stop = false;
                            tmpS += ch;
                            listTmp.Add(tmpS);
                            tmpS = "";
                        }
                        else
                        {
                            tmpS += ch;
                        }
                    }
                }

                if (tmpS != "")
                {
                    var w = tmpS.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var ww in w)
                    {
                        listTmp.Add(ww);
                    }
                }

                tmpWords = listTmp.ToArray();
            }
            else
            {
                tmpWords = line.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            var tmpList = new List<string>();

            for (int i = 0; i < tmpWords.Length; i++)
            {
                string tmp = "";
                if (tmpWords[i].IndexOf("\"") != -1)
                {
                    tmpList.Add(tmpWords[i]);
                    continue;
                }
                for (int j = 0; j < tmpWords[i].Length; j++)
                {
                    char ch = tmpWords[i][j];
                    if (ch == '+' || ch == '-' || ch == '/' || ch == '*' || ch == '(' || ch == ')' || ch == '{' || ch == '}' || ch == ',' || ch == '=' || ch == '<' || ch == '>' || ch == '!' || ch == '|' || ch == '&' || ch == '[' || ch == ']' || ch == '#' || ch == ';' || ch == '%' || ch == '^' || ch == '@')
                    {
                        if (tmp != "")
                        {
                            if (ch == '[' && (tmp.ToLower() == "string" || tmp.ToLower() == "number" || tmp.ToLower() == "bool"))
                            {
                                tmp += ch;
                                continue;
                            }
                            else if (ch == ']' && (tmp.ToLower() == "string[" || tmp.ToLower() == "number[" || tmp.ToLower() == "bool["))
                            {
                                tmp += ch;
                                continue;
                            }
                            else
                            {
                                tmpList.Add(tmp);
                                tmp = "";
                            }
                        }
                        tmpList.Add(ch.ToString());
                        continue;
                    }
                    else
                    {
                        tmp += ch;
                    }
                }
                if (tmp != "")
                {
                    tmpList.Add(tmp);
                }
            }

            for (int j = 0; j < tmpList.Count; j++)
            {
                if (j < tmpList.Count - 1)
                {
                    if (tmpList[j] == "<" || tmpList[j] == ">" || tmpList[j] == "=" || tmpList[j] == "!" || tmpList[j] == "+" || tmpList[j] == "-" || tmpList[j] == "/" || tmpList[j] == "*")
                    {
                        if (tmpList[j + 1] == "=")
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "&")
                    {
                        if (tmpList[j + 1] == "&")
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "|")
                    {
                        if (tmpList[j + 1] == "|")
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "(")
                    {
                        if (tmpList[j + 1] == ")")
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "{")
                    {
                        if (tmpList[j + 1] == "}")
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "[")
                    {
                        if (tmpList[j + 1] == "]")
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "+")
                    {
                        if (tmpList[j + 1] == "+")
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "-")
                    {
                        if (tmpList[j + 1] == "-" && j + 2 == tmpList.Count)
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "<")
                    {
                        if (tmpList[j + 1] == ">")
                        {
                            words.Add(tmpList[j] + tmpList[j + 1]);
                            j++;
                            continue;
                        }
                    }
                    if (tmpList[j] == "@")
                    {
                        words.Add(tmpList[j] + tmpList[j + 1]);
                        j++;
                        continue;
                    }
                }
                words.Add(tmpList[j]);
            }

            var listWords = new List<Word>();

            
            for (int i = 0; i < words.Count; i++)
            {
                if (i == 0)
                {
                    if (i < words.Count - 1)
                    {
                        listWords.Add(new Word(words[i], words[i].ToLower(), new TokenBuilder(_className, _keywords).GetToken(words[i].ToLower(), "", words[i + 1], strLine.Trim().ToLower())));
                    }
                    else
                    {
                        listWords.Add(new Word(words[i], words[i].ToLower(), new TokenBuilder(_className, _keywords).GetToken(words[i].ToLower(), "", "", strLine.Trim().ToLower())));
                    }
                }
                else
                {
                    if (i < words.Count - 1)
                    {
                        listWords.Add(new Word(words[i], words[i].ToLower(), new TokenBuilder(_className, _keywords).GetToken(words[i].ToLower(), words[i - 1], words[i + 1], strLine.Trim().ToLower())));
                    }
                    else
                    {
                        listWords.Add(new Word(words[i], words[i].ToLower(), new TokenBuilder(_className, _keywords).GetToken(words[i].ToLower(), words[i - 1], "", strLine.Trim().ToLower())));
                    }
                }
            }
            

            return listWords;
        }
    }
}

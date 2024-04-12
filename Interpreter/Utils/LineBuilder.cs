using Interpreter.CommonData;
using Interpreter.DataTemplates;
using Interpreter.Enums;
using System;
using System.Collections.Generic;

namespace Interpreter.Utils
{
    internal static class LineBuilder
    {
        internal static List<Word> GetWords(string strLine)
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
                                var w = tmpS.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                            if (ch == '[' && (tmp.ToLower() == "string" || tmp.ToLower() == "number"))
                            {
                                tmp += ch;
                                continue;
                            }
                            else if (ch == ']' && (tmp.ToLower() == "string[" || tmp.ToLower() == "number["))
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
                words[i] = words[i].Trim();
            }

            for (int i = 0; i < words.Count; i++)
            {
                if (i == 0)
                {
                    if (i < words.Count - 1)
                    {
                        listWords.Add(new Word() { Text = words[i], OriginText = words[i], Token = new TokenBuilder().GetToken(words[i], "", words[i + 1], strLine) });
                        if (listWords[listWords.Count - 1].Token == Tokens.VARIABLE || listWords[listWords.Count - 1].Token == Tokens.SUBNAME || listWords[listWords.Count - 1].Token == Tokens.FUNCNAME || listWords[listWords.Count - 1].Token == Tokens.LABELNAME || listWords[listWords.Count - 1].Token == Tokens.LABEL)
                        {
                            string tmpVar = listWords[listWords.Count - 1].Text.ToUpper();
                            listWords[listWords.Count - 1].Text = tmpVar;
                        }
                    }
                    else
                    {
                        listWords.Add(new Word() { Text = words[i], OriginText = words[i], Token = new TokenBuilder().GetToken(words[i], "", "", strLine) });
                        if (listWords[listWords.Count - 1].Token == Tokens.VARIABLE || listWords[listWords.Count - 1].Token == Tokens.SUBNAME || listWords[listWords.Count - 1].Token == Tokens.FUNCNAME || listWords[listWords.Count - 1].Token == Tokens.LABELNAME || listWords[listWords.Count - 1].Token == Tokens.LABEL)
                        {
                            string tmpVar = listWords[listWords.Count - 1].Text.ToUpper();
                            listWords[listWords.Count - 1].Text = tmpVar;
                        }
                    }
                }
                else
                {
                    if (i < words.Count - 1)
                    {
                        listWords.Add(new Word() { Text = words[i], OriginText = words[i], Token = new TokenBuilder().GetToken(words[i], words[i - 1], words[i + 1], strLine) });
                        if (listWords[listWords.Count - 1].Token == Tokens.VARIABLE || listWords[listWords.Count - 1].Token == Tokens.SUBNAME || listWords[listWords.Count - 1].Token == Tokens.FUNCNAME || listWords[listWords.Count - 1].Token == Tokens.LABELNAME || listWords[listWords.Count - 1].Token == Tokens.LABEL)
                        {
                            string tmpVar = listWords[listWords.Count - 1].Text.ToUpper();
                            listWords[listWords.Count - 1].Text = tmpVar;
                        }
                    }
                    else
                    {
                        listWords.Add(new Word() { Text = words[i], OriginText = words[i], Token = new TokenBuilder().GetToken(words[i], words[i - 1], "", strLine) });
                        if (listWords[listWords.Count - 1].Token == Tokens.VARIABLE || listWords[listWords.Count - 1].Token == Tokens.SUBNAME || listWords[listWords.Count - 1].Token == Tokens.FUNCNAME || listWords[listWords.Count - 1].Token == Tokens.LABELNAME || listWords[listWords.Count - 1].Token == Tokens.LABEL)
                        {
                            string tmpVar = listWords[listWords.Count - 1].Text.ToUpper();
                            listWords[listWords.Count - 1].Text = tmpVar;
                        }
                    }
                }
                //Console.WriteLine(listWords[listWords.Count - 1].Token);
            }

            return listWords;
        }

        
        internal static LineType  GetType (Line line)
        {
            var type = LineType.NON;

            var firstWord = line.FirstWord;

            if (firstWord != null)
            {
                if (firstWord.Token == Tokens.KEYWORD)
                {
                    if (firstWord.ToLower() == "include")
                    {
                        type = LineType.INCLUDE; //!!!
                    }
                    else if (firstWord.ToLower() == "folder")
                    {
                        type = LineType.FOLDER; //!!!
                    }
                    else if (firstWord.ToLower() == "import")
                    {
                        type = LineType.IMPORT; //!!!
                    }
                    else if (firstWord.ToLower() == "sub")
                    {
                        type = LineType.SUBINIT;
                    }
                    else if (firstWord.ToLower() == "function")
                    {
                        type = LineType.FUNCINIT;
                    }
                    else if (firstWord.ToLower() == "for")
                    {
                        type = LineType.FORINIT;
                    }
                    else if (firstWord.ToLower() == "if")
                    {
                        type = LineType.IFINIT;
                    }
                    else if (firstWord.ToLower() == "elseif")
                    {
                        type = LineType.ELSEIFINIT;
                    }
                    else if (firstWord.ToLower() == "while")
                    {
                        type = LineType.WHILEINIT;
                    }
                    else if (firstWord.ToLower() == "goto")
                    {
                        type = LineType.LABELCALL;
                    }
                    else if (firstWord.ToLower() == "number")
                    {
                        type = LineType.NUMBERINIT;
                    }
                    else if (firstWord.ToLower() == "number[]")
                    {
                        type = LineType.NUMBERARRAYINIT;
                    }
                    else if (firstWord.ToLower() == "string")
                    {
                        type = LineType.STRINGINIT;
                    }
                    else if (firstWord.ToLower() == "string[]")
                    {
                        type = LineType.STRINGARRAYINIT;
                    }
                    else if (firstWord.ToLower() == "endfor" || firstWord.ToLower() == "endif" || firstWord.ToLower() == "endwhile" || firstWord.ToLower() == "endsub" || firstWord.ToLower() == "endfunction" || firstWord.ToLower() == "else" || firstWord.ToLower() == "private" || firstWord.ToLower() == "break" || firstWord.ToLower() == "continue")
                    {
                        type = LineType.ONEKEYWORD;
                    }
                }
                else if (firstWord.Token == Tokens.LABEL)
                {
                    type = LineType.LABELINIT;
                }
                else if (firstWord.Token == Tokens.METHOD)
                {
                    type = LineType.METHODCALL;
                }
                else if (firstWord.Token == Tokens.SUBNAME)
                {
                    type = LineType.SUBCALL;
                }
                else if (firstWord.Token == Tokens.FUNCNAME)
                {
                    type = LineType.FUNCCALL;
                }
                else if (firstWord.Token == Tokens.MODULEMETHOD)
                {
                    type = LineType.MODULEMETHODCALL;
                }
                else if (firstWord.Token == Tokens.MODULEPROPERTY)
                {
                    type = LineType.MODULEPROPERTY;
                }
                else if (firstWord.Token == Tokens.VARIABLE) //!!!
                {
                    if (line.Count > 1)
                    {
                        var nextWord = line.Words[1];
                        if (nextWord.Token == Tokens.EQU)
                        {
                            type = LineType.VARINIT; //!!!
                        }
                        else if (nextWord.Token == Tokens.EQUMATH)
                        {
                            type = LineType.VAREQUMATH; //!!!
                        }
                        else if (nextWord.Token == Tokens.DOUBLEMATH)
                        {
                            type = LineType.VARDOUBLEMATH; //!!!
                        }
                        else if (nextWord.Token == Tokens.BRACKETLEFTARRAY)
                        {
                            type = LineType.VARARRAYINIT; //!!!
                        }
                    }
                }
                else if (firstWord.Token == Tokens.PREPROCESSOR)
                {
                    type = LineType.EMPTY;
                }
            }
            else
            {
                type = LineType.EMPTY;
            }

            return type;
        }
    }
}

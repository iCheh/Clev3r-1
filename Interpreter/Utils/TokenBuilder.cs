using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Interpreter.Utils
{
    internal class TokenBuilder
    {
        private static List<string> _className;
        internal TokenBuilder()
        {
            _className = new List<string>();
            SetClassName();
        }

        internal Tokens GetToken(string word, string prevWord, string follWord, string line)
        {
            if (word.IndexOf("\"") != -1 && word.LastIndexOf("\"") != -1 && word.LastIndexOf("\"") > word.IndexOf("\""))
            {
                return Tokens.STRING;
            }
            else if (word.ToLower() == "#")
            {
                return Tokens.PREPROCESSOR;
            }
            else if (word.IndexOf(".") != -1)
            {
                Regex regex = new Regex("[0-9.]");
                MatchCollection matches = regex.Matches(word);
                if (matches.Count != word.Length)
                {
                    var firstWord = word.Substring(0, word.IndexOf("."));
                    if (_className.Contains(firstWord.ToLower()))
                    {
                        return Tokens.METHOD;
                    }
                    else
                    {
                        if (follWord == "(" || follWord == "()")
                            return Tokens.MODULEMETHOD;
                        else
                            return Tokens.MODULEPROPERTY;
                    }
                }
                else
                {
                    return Tokens.NUMBER;
                }
            }
            else if (word.ToLower() == "for" || word.ToLower() == "endfor" || word.ToLower() == "if" || word.ToLower() == "then" || word.ToLower() == "endif" || word.ToLower() == "else" || word.ToLower() == "elseif" || word.ToLower() == "while" || word.ToLower() == "endwhile" || word.ToLower() == "and" || word.ToLower() == "or" || word.ToLower() == "sub" || word.ToLower() == "endsub" || word.ToLower() == "goto" || word.ToLower() == "step" || word.ToLower() == "to" || word.ToLower() == "import" || word.ToLower() == "include" || word.ToLower() == "folder" || word.ToLower() == "in" || word.ToLower() == "out" || word.ToLower() == "function" || word.ToLower() == "endfunction" || word.ToLower() == "number" || word.ToLower() == "number[]" || word.ToLower() == "string" || word.ToLower() == "string[]" || word.ToLower() == "private" || word.ToLower() == "region" || word.ToLower() == "endregion" || word.ToLower() == "break" || word.ToLower() == "continue" || word.ToLower() == "return")
            {
                return Tokens.KEYWORD;
            }
            else if (line.ToLower().IndexOf("sub") != -1 && line.Length > 3 && line[0].ToString().ToLower() == "s" && line[1].ToString().ToLower() == "u" && line[2].ToString().ToLower() == "b" && line[3].ToString() == " ")
            {
                if (word.ToLower() == "sub" || word.ToLower() == "endsub")
                {
                    return Tokens.KEYWORD;
                }
                else if (word.ToLower() == "(")
                {
                    return Tokens.BRACKETLEFT;
                }
                else if (word.ToLower() == ")")
                {
                    return Tokens.BRACKETRIGHT;
                }
                else if (word.ToLower() == "()")
                {
                    return Tokens.DOUBLEBRACKET;
                }

                Regex regex = new Regex("[0-9a-zA-Z_]");
                MatchCollection matches = regex.Matches(word);
                if (matches.Count == word.Length)
                {
                    if (prevWord.ToLower() == "sub")
                        return Tokens.SUBNAME;
                }
                else if (word.Trim() == ",")
                {
                    return Tokens.COMMA;
                }
            }
            else if (line.ToLower().IndexOf("function") != -1 && line.Length > 8 && line[0].ToString().ToLower() == "f" && line[1].ToString().ToLower() == "u" && line[2].ToString().ToLower() == "n" && line[3].ToString().ToLower() == "c" && line[4].ToString().ToLower() == "t" && line[5].ToString().ToLower() == "i" && line[6].ToString().ToLower() == "o" && line[7].ToString().ToLower() == "n" && line[8].ToString() == " ")
            {
                if (word.ToLower() == "function" || word.ToLower() == "endfunction")
                {
                    return Tokens.KEYWORD;
                }
                else if (word.ToLower() == "(")
                {
                    return Tokens.BRACKETLEFT;
                }
                else if (word.ToLower() == ")")
                {
                    return Tokens.BRACKETRIGHT;
                }
                else if (word.ToLower() == "()")
                {
                    return Tokens.DOUBLEBRACKET;
                }

                Regex regex = new Regex("[0-9a-zA-Z_]");
                MatchCollection matches = regex.Matches(word);
                if (matches.Count == word.Length)
                {
                    if (prevWord.ToLower() == "function")
                    {
                        return Tokens.FUNCNAME;
                    }
                    else
                    {
                        if (word.Length > 0 && ((word[0] >= 'A' && word[0] <= 'Z') || (word[0] >= 'a' && word[0] <= 'z')) && (prevWord.ToLower() == "number" || prevWord.ToLower() == "number[]" || prevWord.ToLower() == "string" || prevWord.ToLower() == "string[]"))
                        {
                            return Tokens.VARIABLE;
                        }
                    }  
                }
                else if (word.Trim() == ",")
                {
                    return Tokens.COMMA;
                }
            }
            else if (word.IndexOf(":") != -1)
            {
                return Tokens.LABEL;
            }
            else if (word.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) != -1)
            {
                Regex regex = new Regex("[0-9]");
                MatchCollection matches = regex.Matches(word);
                if (matches.Count == word.Length)
                {
                    return Tokens.NUMBER;
                }
                else if (word.Length > 1 && word[0] == '@')
                {
                    return Tokens.VARIABLE;
                }
                else
                {
                    Regex regex2 = new Regex("[0-9a-zA-Z_]");
                    MatchCollection matches2 = regex2.Matches(word);

                    if (matches2.Count == word.Length)
                    {
                        if (follWord != "" && follWord.IndexOfAny(new char[] { '(' }) != -1)
                        {
                            return Tokens.SUBNAME;
                        }
                        else if (line.ToLower().IndexOf("thread.run") != -1)
                        {
                            //Console.WriteLine("===>" + word);
                            int i1 = line.ToLower().IndexOf("thread.run");
                            int i2 = line.ToLower().IndexOf("=");
                            int i3 = line.IndexOf(word);
                            if (i2 != -1 && i1 < i2 && i2 < i3)
                            {
                                return Tokens.SUBNAME;
                            }
                        }
                        else if (prevWord.ToLower() == "goto")
                        {
                            return Tokens.LABELNAME;
                        }
                        else
                        {
                            return Tokens.VARIABLE;
                        }
                    }
                }
            }
            else if (word.IndexOfAny(new char[] { '+', '-', '*', '/' }) != -1)
            {
                if (word == "++" || word == "--")
                {
                    return Tokens.DOUBLEMATH;
                }
                else if (word == "+=" || word == "-=" || word == "*=" || word == "/=")
                {
                    return Tokens.EQUMATH;
                }
                else
                {
                    return Tokens.MATHOPERATOR;
                }
            }
            else if (word == "<>" || word == "<=" || word == ">=" || word == "<" || word == ">")
            {
                return Tokens.BOOLOPERATOR;
            }
            else if (word.IndexOfAny(new char[] { '(', ')', '[', ']' }) != -1)
            {
                if (word == "()")
                {
                    return Tokens.DOUBLEBRACKET;
                }
                else if (word == "[]")
                {
                    return Tokens.DOUBLEBRACKETARRAY;
                }
                else if (word == "(")
                {
                    return Tokens.BRACKETLEFT;
                }
                else if (word == ")")
                {
                    return Tokens.BRACKETRIGHT;
                }
                else if (word == "[")
                {
                    return Tokens.BRACKETLEFTARRAY;
                }
                else if (word == "]")
                {
                    return Tokens.BRACKETRIGHTARRAY;
                }
            }
            else if (word.Trim() == ",")
            {
                return Tokens.COMMA;
            }
            else if (word == "=")
            {
                return Tokens.EQU;
            }
            else if (word.Length > 1 && word[0] == '@')
            {
                return Tokens.VARIABLE;
            }
            else
            {
                Regex regex = new Regex("[0-9a-zA-Z_]");
                MatchCollection matches = regex.Matches(word);
                if (matches.Count == word.Length)
                {
                    if (follWord != "" && follWord.IndexOfAny(new char[] { '(' }) != -1)
                    {
                        return Tokens.SUBNAME;
                    }
                    else if (line.ToLower().IndexOf("thread.run") != -1)
                    {
                        int i1 = line.ToLower().IndexOf("thread.run");
                        int i2 = line.ToLower().IndexOf("=");
                        int i3 = line.IndexOf(word);
                        if (i2 != -1 && i1 < i2 && i2 < i3)
                        {
                            return Tokens.SUBNAME;
                        }
                    }
                    else if (prevWord.ToLower() == "goto")
                    {
                        return Tokens.LABELNAME;
                    }
                    else
                    {
                        return Tokens.VARIABLE;
                    }
                }
            }

            return Tokens.NON;
        }

        private void SetClassName()
        {
            _className.Add("assert");
            _className.Add("buttons");
            _className.Add("byte");
            _className.Add("ev3");
            _className.Add("ev3file");
            _className.Add("lcd");
            _className.Add("mailbox");
            _className.Add("math");
            _className.Add("motor");
            _className.Add("motora");
            _className.Add("motorab");
            _className.Add("motorac");
            _className.Add("motorad");
            _className.Add("motorb");
            _className.Add("motorbc");
            _className.Add("motorbd");
            _className.Add("motorc");
            _className.Add("motorcd");
            _className.Add("motord");
            _className.Add("program");
            _className.Add("row");
            _className.Add("sensor");
            _className.Add("sensor1");
            _className.Add("sensor2");
            _className.Add("sensor3");
            _className.Add("sensor4");
            _className.Add("speaker");
            _className.Add("text");
            _className.Add("thread");
            _className.Add("time");
            _className.Add("vector");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Clever.Model.Bplus.BPInterpreter
{
    internal class TokenBuilder
    {
        private static HashSet<string> _className;
        private HashSet<string> _keywords;
        Regex _regex;

        internal TokenBuilder(HashSet<string> className, HashSet<string> keywords)
        {
            _className = className;
            _keywords = keywords;
            _regex = new Regex("[0-9a-zA-Z_]");
        }

        internal Tokens GetToken(string word, string prevWord, string follWord, string line)
        {
            if (prevWord.ToLower() == "region")
            {
                return Tokens.NON;
            }
            else if (word.IndexOf("\"") != -1 && word.LastIndexOf("\"") != -1 && word.LastIndexOf("\"") > word.IndexOf("\""))
            {
                return Tokens.STRING;
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
                        return Tokens.MODULEMETHOD;
                    }
                }
                else
                {
                    return Tokens.NUMBER;
                }
            }
            else if (_keywords.Contains(word))
            {
                return Tokens.KEYWORD;
            }
            else if (line.IndexOf("sub") == 0 && line.Length > 3 && line[3] == ' ')
            {
                if (word == "sub" || word == "endsub")
                {
                    return Tokens.KEYWORD;
                }
                else if (word == "(")
                {
                    return Tokens.BRACKETLEFT;
                }
                else if (word == ")")
                {
                    return Tokens.BRACKETRIGHT;
                }
                else if (word == "()")
                {
                    return Tokens.DOUBLEBRACKET;
                }

                //Regex regex = new Regex("[0-9a-zA-Z_]");
                MatchCollection matches = _regex.Matches(word);
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
            else if (line.IndexOf("function") == 0 && line.Length > 8 && line[8] == ' ')
            {
                if (word == "function" || word == "endfunction")
                {
                    return Tokens.KEYWORD;
                }
                else if (word == "(")
                {
                    return Tokens.BRACKETLEFT;
                }
                else if (word == ")")
                {
                    return Tokens.BRACKETRIGHT;
                }
                else if (word == "()")
                {
                    return Tokens.DOUBLEBRACKET;
                }

                //Regex regex = new Regex("[0-9a-zA-Z_]");
                MatchCollection matches = _regex.Matches(word);
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
                //return Tokens.LABEL;
                return Tokens.LABELNAME;
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
                    //Regex regex2 = new Regex("[0-9a-zA-Z_]");
                    MatchCollection matches2 = _regex.Matches(word);

                    if (matches2.Count == word.Length)
                    {
                        if (follWord != "" && follWord.IndexOfAny(new char[] { '(' }) != -1)
                        {
                            //return Tokens.SUBNAME;
                            return Tokens.NON;
                        }
                        else if (line.IndexOf("thread.run") != -1)
                        {
                            //Console.WriteLine("===>" + word);
                            int i1 = line.IndexOf("thread.run");
                            int i2 = line.IndexOf("=");
                            int i3 = line.IndexOf(word);
                            if (i2 != -1 && i1 < i2 && i2 < i3)
                            {
                                //return Tokens.SUBNAME;
                                return Tokens.NON;
                            }
                        }
                        else if (follWord.ToLower() == ":")
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
                //Regex regex = new Regex("[0-9a-zA-Z_]");
                MatchCollection matches = _regex.Matches(word);
                if (matches.Count == word.Length)
                {
                    if (follWord != "" && follWord.IndexOfAny(new char[] { '(' }) != -1)
                    {
                        //return Tokens.SUBNAME;
                        return Tokens.NON;
                    }
                    else if (line.IndexOf("thread.run") != -1)
                    {
                        //Console.WriteLine("===>" + word);
                        int i1 = line.IndexOf("thread.run");
                        int i2 = line.IndexOf("=");
                        int i3 = line.IndexOf(word);
                        if (i2 != -1 && i1 < i2 && i2 < i3)
                        {
                            //return Tokens.SUBNAME;
                            return Tokens.NON;
                        }
                    }
                    else if (follWord.ToLower() == ":")
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
    }
}

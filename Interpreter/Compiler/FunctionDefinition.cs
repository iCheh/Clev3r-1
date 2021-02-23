/*  EV3-Basic: A basic compiler to target the Lego EV3 brick
    Copyright (C) 2015 Reinhard Grafl

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Interpreter.Compiler
{

    internal class FunctionDefinition
    {
        internal readonly string fname;
        internal readonly string startsub;
        internal readonly string[] paramnames;
        internal readonly Object[] defaultvalues;

        private Dictionary<ExpressionType, int> reservedtemporaries;
        private Dictionary<ExpressionType, int> maxreservedtemporaries;
        private ExpressionType returnType;

        internal FunctionDefinition(string fname, string startsub, string[] paramnames, Object[] defaultvalues)
        {
            this.fname = fname;
            this.startsub = startsub;
            this.paramnames = paramnames;
            this.defaultvalues = defaultvalues;

            reservedtemporaries = new Dictionary<ExpressionType, int>();
            maxreservedtemporaries = new Dictionary<ExpressionType, int>();
            returnType = ExpressionType.Void;
        }

        public override string ToString()
        {
            string s = "\"" + fname + "\" " + startsub + "(";
            for (int i = 0; i < paramnames.Length; i++)
            {
                s = s + " " + paramnames[i] + ":" + tostring(defaultvalues[i]);
            }
            s = s + "): " + returnType;
            return s;
        }
        private static string tostring(Object v)
        {
            if (v is double[])
            {
                string s = "" + ((double[])v)[0];
                if (s.IndexOf('.') < 0) s = s + ".0";
                return s;
            }
            return "'" + v.ToString() + "'";
        }

        internal int findParameter(string name)
        {
            for (int i = 0; i < paramnames.Length; i++)
            {
                if (paramnames[i].Equals(name))
                {
                    return i;
                }
            }
            return -1;
        }

        internal int getParameterNumber()
        {
            return this.paramnames.Length;
        }

        internal string getParameterDefaultLiteral(int index)
        {
            return tostring(defaultvalues[index]);
        }

        internal ExpressionType getParameterType(int index)
        {
            if (defaultvalues[index] is string) return ExpressionType.Text;
            else return ExpressionType.Number;
        }

        internal string getParameterVariable(int index)
        {
            switch (getParameterType(index))
            {
                case ExpressionType.Number:
                    return "F" + fname + "." + paramnames[index];
                case ExpressionType.Text:
                    return "S" + fname + "." + paramnames[index];
                default:
                    return null;
            }
        }

        internal ExpressionType getReturnType()
        {
            return returnType;
        }

        internal string getReturnVariable()
        {
            switch (getReturnType())
            {
                case ExpressionType.Number:
                    return "F" + fname + ".";
                case ExpressionType.Text:
                    return "S" + fname + ".";
                default:
                    return "";
            }
        }

        internal void setReturnType(ExpressionType t)
        {
            returnType = t;
        }

        internal string reserveVariable(ExpressionType type)
        {
            if (!reservedtemporaries.ContainsKey(type))
            {
                reservedtemporaries[type] = 0;
                maxreservedtemporaries[type] = 0;
            }
            int n = reservedtemporaries[type] + 1;
            reservedtemporaries[type] = n;
            maxreservedtemporaries[type] = Math.Max(n, maxreservedtemporaries[type]);
            switch (type)
            {
                case ExpressionType.Number:
                    return "F" + fname + "." + (n - 1);
                case ExpressionType.Text:
                    return "S" + fname + "." + (n - 1);
                default:
                    return null;
            }
        }

        internal void releaseVariable(ExpressionType type)
        {
            reservedtemporaries[type]--;
        }

        internal int getMaxReserved(ExpressionType type)
        {
            return maxreservedtemporaries.ContainsKey(type) ? maxreservedtemporaries[type] : 0;
        }

        internal List<string> getAllLocalVariables(ExpressionType type)
        {
            string prefix = (type == ExpressionType.Number ? "F" : "S") + fname + ".";

            List<string> l = new List<string>();
            if (getReturnType() == type)
            {
                l.Add(prefix);
            }
            for (int i = 0; i < paramnames.Length; i++)
            {
                if (getParameterType(i) == type)
                {
                    l.Add(prefix + paramnames[i]);
                }
            }
            for (int i = 0; i < getMaxReserved(type); i++)
            {
                l.Add(prefix + i);
            }
            return l;
        }

        internal List<string> getCurrentLocalVariables(ExpressionType type)
        {
            string prefix = (type == ExpressionType.Number ? "F" : "S") + fname + ".";

            List<string> l = new List<string>();
            for (int i = 0; i < paramnames.Length; i++)
            {
                if (getParameterType(i) == type)
                {
                    l.Add(prefix + paramnames[i]);
                }
            }
            if (reservedtemporaries.ContainsKey(type))
            {
                for (int i = 0; i < reservedtemporaries[type]; i++)
                {
                    l.Add(prefix + i);
                }
            }
            return l;
        }


        internal static FunctionDefinition make(string fname, string startsub, string pardeclarator)
        {
            double val;
            string[] parlist = pardeclarator.Split(new Char[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
            Object[] defaultvalues = new Object[parlist.Length];

            for (int i = 0; i < parlist.Length; i++)
            {
                int colon = parlist[i].IndexOf(':');
                if (colon > 0)
                {
                    string v = parlist[i].Substring(colon + 1);
                    parlist[i] = parlist[i].Substring(0, colon).ToUpperInvariant();
                    if (double.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
                    {
                        defaultvalues[i] = new double[] { val };
                    }
                    else
                    {
                        defaultvalues[i] = v;
                    }
                }
                else
                {
                    parlist[i] = parlist[i].ToUpperInvariant();
                    defaultvalues[i] = new double[] { 0.0 };
                }
            }

            return new FunctionDefinition(fname, startsub, parlist, defaultvalues);
        }
    }
}

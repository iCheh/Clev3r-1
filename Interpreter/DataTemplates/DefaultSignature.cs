using Interpreter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interpreter.DataTemplates
{
    struct DefaultSignature
    {
        internal DefaultSignature(ObjectType type, int inputCount, List<VariableType> inputType, VariableType outputType)
        {
            Type = type;
            InputCount = inputCount;
            InputType = inputType;
            OutputType = outputType;
        }

        internal ObjectType Type;
        internal int InputCount;
        internal List<VariableType> InputType;
        internal VariableType OutputType;

        public override string ToString()
        {
            string tmp = "";
            tmp += "type: " + Type + " inputs: " + InputCount.ToString();
            if (InputCount > 0)
            {
                tmp += " (";
                for (int i = 0; i < InputType.Count; i++)
                {
                    if (i < InputType.Count - 1)
                    {
                        tmp += InputType[i] + ", ";
                    }
                    else
                    {
                        tmp += InputType[i] + ") ";
                    }
                }
            }
            tmp += "return: " + OutputType;

            return tmp;
        }
    }
}

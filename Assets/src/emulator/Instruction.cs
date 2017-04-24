using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace src.emulator
{
    public class Instruction
    {
        public enum Type
        {
            [Params("R,M R,M,N")] MOV,
            [Params("R,M R,M,N")] ADD,
            [Params("R,M R,M,N")] SUB,
            [Params("R,M R,M,N")] MUL,
            [Params("R,M M")] LEA,

            [Params("R,M,N R,M,N")] CMP,

            [Params("L")] JMP,
            [Params("L")] JEQ,
            [Params("L")] JNE,
            [Params("L")] JGT,
            [Params("L")] JLT,
            [Params("L")] JGE,
            [Params("L")] JlE,
            [Params("L")] JCZ,
            [Params("L")] LOOP,

            [Params("R,M R,M,N")] SHR,
            [Params("R,M R,M,N")] SHL,
            [Params("R,M R,M,N")] XOR,
            [Params("R,M R,M,N")] OR,
            [Params("R,M R,M,N")] AND,
            [Params("R,M")] NOT,

            [Params("R,M")] INC,
            [Params("R,M")] DEC,

            //Halting instuctions
            [Params("R,M,N")] LOC,
            [Params("M")] SCA,
            [Params("")] TUL,
            [Params("")] TUR,
            [Halt] [Params("")] HLT,

            [Params("")] NOP,

            [Params("")] NONE,
            [Params("R,N,M")] DBG
        }

        public enum ParamType
        {
            REG, MEM, NUM, LBL, NONE
        }

        private class Halt : Attribute
        {

        }

        public class Params : Attribute
        {
            ParamType[] first;
            ParamType[] second;

            internal Params(string info)
            {
                if (info.Length > 0)
                {
                    string[] sep = info.Split(' ');

                    string[] firstParam = sep[1].Split(',');
                    first = new ParamType[firstParam.Length];
                    for (int i = 0; i < first.Length; i++)
                    {
                        first[i] = FromString(firstParam[i]);
                    }
                    if (sep.Length > 1)
                    {
                        string[] secondParam = sep[1].Split(',');
                        second = new ParamType[secondParam.Length];
                        for (int i = 0; i < second.Length; i++)
                        {
                            second[i] = FromString(secondParam[i]);
                        }
                    }
                }
            }

            public ParamType[] GetFirstParams()
            {
                return first;
            }

            public ParamType[] GetSecondParams()
            {
                return second;
            }

            public ParamType FromString(string p)
            {
                switch (p)
                {
                    case "R":
                        return ParamType.REG;
                    case "M":
                        return ParamType.MEM;
                    case "N":
                        return ParamType.NUM;
                    case "L":
                        return ParamType.LBL;
                    default:
                        return ParamType.NONE;
                }
            }

            public bool HasFirstParam()
            {
                return first != null;
            }

            public bool HasSecondParam()
            {
                return second != null;
            }
        }

        public class Parameter
        {
            public ParamType type;
            public string data;

            public Parameter(ParamType type, string data)
            {
                this.type = type;
                this.data = data;
            }
        }

        private Type type;
        private Parameter paramOne;
        private Parameter paramTwo;

        public Instruction(Type type, Parameter p1, Parameter p2)
        {
            this.type = type;
            this.paramOne = p1;
            this.paramTwo = p2;
        }

        public Instruction.Type OpType
        {
            get
            {
                return type;
            }
        }

        public Parameter[] GetParameters()
        {
            return new Parameter[] { paramOne, paramTwo };
        }

        public Parameter ParamOne
        {
            get
            {
                return paramOne;
            }
        }

        public Parameter ParamTwo
        {
            get
            {
                return paramTwo;
            }
        }

        public int GetNumParams()
        {
            if (paramOne != null && paramTwo != null)
            {
                return 2;
            } else if (paramOne == null && paramTwo == null)
            {
                return 0;
            }
            return 1;
        }

        public bool ShouldHalt()
        {
            return Attribute.GetCustomAttribute(InsturctionTypeExtension.ForValue(type), typeof(Halt)) != null;
        }

        public static Type TypeFromString(string s)
        {
            foreach (Type ty in Enum.GetValues(typeof(Type)))
            {
                if (s.Trim().ToUpper() == ty.ToString().ToUpper())
                {
                    return ty;
                }
            }
            return Type.NONE;
        }
    }

    public static class InsturctionTypeExtension
    {
        public static bool IsVaidParam(this Instruction.Type it, int num, Instruction.ParamType type)
        {
            if (num > 1 || num < 0)
            {
                return false;
            }
            if (num == 0 && GetAttr(it).HasFirstParam())
            {
                return GetAttr(it).GetFirstParams().Contains(type);
            }
            if (num == 1 && GetAttr(it).HasSecondParam())
            {
                return GetAttr(it).GetSecondParams().Contains(type);
            }
            return false;
        }

        private static Instruction.Params GetAttr(Instruction.Type it)
        {
            return (Instruction.Params)Attribute.GetCustomAttribute(ForValue(it), typeof(Instruction.Params));
        }

        public static MemberInfo ForValue(Instruction.Type p)
        {
            return typeof(Instruction.Type).GetField(Enum.GetName(typeof(Instruction.Type), p));
        }
    }
}

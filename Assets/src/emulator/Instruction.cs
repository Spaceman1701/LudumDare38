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
            [Params("R,M R,M,N")] NOT,

            [Params("R,M")] INC,
            [Params("R,M")] DEC,


            [Params("R,M M")] LEA,

            //Halting instuctions
            [Params("R,M,N")] LOC,
            [Params("")] SHA,
            [Params("")] TUL,
            [Params("")] TUR,
            [Params("")] HLT,

            [Params("")] NONE
        }

        public enum ParamType
        {
            REG, MEM, NUM, LBL, NONE
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

        private static MemberInfo ForValue(Instruction.Type p)
        {
            return typeof(Instruction.Type).GetField(Enum.GetName(typeof(Instruction.Type), p));
        }
    }
}

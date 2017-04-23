using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.src.emulator
{
    class Instruction
    {
        enum Type
        {
            [Params("R,M R,M,N")] MOV,
            [Params("R,M R,M,N")] ADD,
            [Params("R,M R,M,N")] SUB,
            [Params("R,M R,M,N")] MUL,

            [Params("R,M,N R,M,N")] CMP,

            [Params("L")] JMP,
            [Params("L")] JE,
            [Params("L")] JNE,
            [Params("L")] JGT,
            [Params("L")] JLT,
            [Params("L")] JGE,
            [Params("L")] JlE,
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
            [Params("")] HLT
        }

        enum ParamType
        {
            REG, MEM, NUM, LBL, NONE
        }

        class Params : Attribute
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
                    for (int i = 0; i < second.Length; i++)
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

            private ParamType FromString(string p)
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
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace src.emulator
{
    class Compiler
    {
        class TokenizedLine
        {
            public string label;
            public Instruction.Type inst;
            public string paramOne;
            public string paramTwo;


            public TokenizedLine()
            {
                label = null;
                inst = Instruction.Type.NONE;
                paramOne = null;
                paramTwo = null;
            }

            public override string ToString()
            {
                return "LBL: " + label + ", INST: " + inst.ToString() + ", P1: " + paramOne + ", P2: " + paramTwo; 
            }
        }

        class TokenizedProgram
        {
            public Dictionary<string, string> constants;
            public Dictionary<string, int> arrays;
            public IList<TokenizedLine> instructions;

            public TokenizedProgram(Dictionary<string, string> constants, Dictionary<string, int> arrays, IList<TokenizedLine> instructions)
            {
                this.constants = constants;
                this.arrays = arrays;
                this.instructions = instructions;
            }
        }

        private Compiler() { }


        public static Program Compile(string code, int reservedMemory)
        {
            TokenizedProgram p = PreProcess(code, reservedMemory);
            foreach (TokenizedLine l in p.instructions)
            {
                Debug.Log(l.ToString());
            }


            List<Instruction> instructions = new List<Instruction>();


            Dictionary<string, int> labels = new Dictionary<string, int>();
            Dictionary<string, byte> constants = new Dictionary<string, byte>();

            foreach (string name in p.constants.Keys)
            {
                byte value = Parse.Byte(p.constants[name]);
                constants[name] = value;
            }
            foreach (string name in p.arrays.Keys)
            {
                byte value = (byte)p.arrays[name];
                constants[name] = value;
            }

            foreach (TokenizedLine line in p.instructions)
            {
                Instruction.Parameter p1 = null, p2 = null;
                if (line.paramOne != null)
                {
                    p1 = ParseParameter(line.paramOne);
                }
                if (line.paramTwo != null)
                {
                    p2 = ParseParameter(line.paramTwo);
                }

                Instruction i = new Instruction(line.inst, p1, p2);
                instructions.Add(i);

                if (line.label != null)
                {
                    labels[line.label] = instructions.Count() - 1; 
                }
            }


            return new Program(constants, labels, instructions);
        }

        public static string ProcessMemoryPtr(string ptr)
        {
            ptr = ptr.Replace("+", "|+|").Replace("-", "|-|");
            string output = "";
            string[] exp = ptr.Split('|');
            Stack<string> opStack = new Stack<string>();
            for (int i = 0; i < exp.Length; i++)
            {
                string token = exp[i];
                if (token == "+" || token == "-")
                {
                    while (opStack.Count > 0)
                    {
                        output += opStack.Pop() + "|";
                    }
                    opStack.Push(token);
                } else
                {
                    output += token + "|";
                }
            }

            while (opStack.Count > 0)
            {
                output += opStack.Pop();
                if (opStack.Count > 0)
                {
                    output += "|";
                }
            }
            return output;
        }

        private static Instruction.Parameter ParseParameter(string data)
        {
            if (data.StartsWith("["))
            {
                return new Instruction.Parameter(Instruction.ParamType.MEM, ProcessMemoryPtr(data.Replace("[", "").Replace("]", "")));
            } else if (data.StartsWith("%"))
            {
                return new Instruction.Parameter(Instruction.ParamType.REG, data.Replace("%", ""));
            } else if (data.StartsWith("$"))
            {
                return new Instruction.Parameter(Instruction.ParamType.NUM, data.Replace("$", ""));
            } else
            {
                return new Instruction.Parameter(Instruction.ParamType.LBL, data);
            }
        }

        private static TokenizedProgram PreProcess(string raw, int reversedMemory)
        {
            string[] lines = raw.Split('\n');
            bool dataSection = false;

            Dictionary<string, string> constants = new Dictionary<string, string>();
            Dictionary<string, int> arrays = new Dictionary<string, int>();
            IList<TokenizedLine> tokenizedLines = new List<TokenizedLine>();

            for (int i = 0; i < lines.Length; i++) { //sanatize input some
                string line = lines[i];
                line = line.Replace(", ", ",");
                line = line.Replace(" ,", ",");

                line = line.Replace("+ ", "+");
                line = line.Replace(" +", "+");

                line = line.Replace("- ", "-");
                line = line.Replace(" -", "-");

                line = line.Replace(": ", ":");
                line = line.Replace(":", ":~");

                line = line.Replace("[ ", "[");
                line = line.Replace(" ]", "]");

                line = line.Trim();
                lines[i] = line;
            }

            int heapHead = reversedMemory;
            for (int i = 0; i < lines.Length; i++)
            {
                string currentLine = lines[i];

                if (currentLine.StartsWith(".data"))
                {
                    dataSection = true;
                    continue;
                } else if (currentLine.StartsWith(".text"))
                {
                    dataSection = false;
                    continue;
                }

                if (dataSection)
                {
                    if (currentLine.Trim() != "")
                    {
                        string[] lineData = currentLine.Split(' ');
                        string name = lineData[0];
                        if (lineData[1].Contains('['))
                        {
                            string number = lineData[1].Replace("[", "").Replace("]", "");
                            Debug.Log(number);
                            int value = int.Parse(number);
                            arrays[name] = heapHead;
                            heapHead += value;
                        } else
                        {
                            constants[name] = lineData[1].Trim();
                        }
                    }
                } else
                {
                    if (currentLine.Trim() != "")
                    {
                        tokenizedLines.Add(TokenizeLine(currentLine));
                    }
                }

            }
            return new TokenizedProgram(constants, arrays, tokenizedLines);
        }

        private static TokenizedLine TokenizeLine(string line)
        {
            TokenizedLine tl = new TokenizedLine();
            string[] lblSep = line.Split(':');
            string instLine;
            if (lblSep.Length > 1)
            {
                tl.label = lblSep[0];
                instLine = lblSep[1];
                if (instLine.Trim() == "~")
                {
                    tl.inst = Instruction.Type.NOP;
                    return tl;
                }
                instLine = instLine.Replace("~", "");
            } else
            {
                instLine = lblSep[0]; //no lablel
            }

            string[] lineData = instLine.Split(' ');

            Instruction.Type instType = Instruction.TypeFromString(lineData[0]);
            tl.inst = instType;

            if (lineData.Length > 1)
            {
                string[] parameters = lineData[1].Split(',');
                tl.paramOne = parameters[0];
                if (parameters.Length > 1)
                {
                    tl.paramTwo = parameters[1];
                }
            }

            return tl;
        }

        private static bool HasLabel(string line)
        {
            return line.Split(':').Length > 1;
        }
    }
}

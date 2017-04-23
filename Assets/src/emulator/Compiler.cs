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


        public static Program Compile(string code)
        {
            TokenizedProgram p = PreProcess(code);
            foreach (TokenizedLine l in p.instructions)
            {
                Debug.Log(l.ToString());
            }
            return null;
        }

        private static TokenizedProgram PreProcess(string raw)
        {
            string[] lines = raw.Split('\n');
            bool dataSection = false;

            Dictionary<string, string> constants = new Dictionary<string, string>();
            Dictionary<string, int> arrays = new Dictionary<string, int>();
            IList<TokenizedLine> tokenizedLines = new List<TokenizedLine>();

            for (int i = 0; i < lines.Length; i++) {
                string line = lines[i];
                line = line.Replace(", ", ",");
                line = line.Replace(" ,", ",");

                line = line.Replace("+ ", "+");
                line = line.Replace(" +", "+");

                line = line.Replace("- ", "-");
                line = line.Replace(" -", "-");

                line = line.Replace(": ", ":");
                line = line.Replace(":", ":~");

                line = line.Trim();
                lines[i] = line;
            }

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
                    return tl;
                }
                instLine = instLine.Replace("~", "");
            } else
            {
                instLine = lblSep[0]; //no lablel
            }

            string[] lineData = instLine.Split(' ');

            Instruction.Type instType = InstructionTypeFromString(lineData[0]);
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

        private static Instruction.Type InstructionTypeFromString(string s)
        {
            foreach (Instruction.Type ty in Enum.GetValues(typeof(Instruction.Type)))
            {
                if (s.Trim().ToUpper() == ty.ToString().ToUpper())
                {
                    return ty;
                }
            }
            return Instruction.Type.NONE;
        }

        private static bool HasLabel(string line)
        {
            return line.Split(':').Length > 1;
        }
    }
}

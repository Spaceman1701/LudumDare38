using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace src.emulator
{
    public class Program
    {

        private readonly IDictionary<string, int> registerNames = new Dictionary<string, int>()
        {
            {"ax", 0},
            {"bx", 1},
            {"cx", 2},
            {"dx", 3}
        };

        private readonly IDictionary<string, byte> constants;
        private readonly IDictionary<string, int> labels;
        private readonly IList<Instruction> instructions;


        public Program(IDictionary<string, byte> constants, IDictionary<string, int> labels, IList<Instruction> instructions)
        {
            this.constants = constants;
            this.labels = labels;
            this.instructions = instructions;
        }

        public int LineCount()
        {
            return instructions.Count();
        }

        public Instruction GetLine(int i)
        {
            if (i >= LineCount())
            {
                return new Instruction(Instruction.Type.NOP, null, null);
            }
            return instructions[i];
        }

        public int EvaulateMemoryPtr(string ptr, byte[] registers)
        {
            Debug.Log(ptr);
            Stack<byte> stack = new Stack<byte>();
            string[] exp = ptr.Split('|');

            foreach (string t in exp)
            {
                if (t == "+" || t == "-")
                {
                    byte r = stack.Pop();
                    byte l = stack.Pop();
                    if (t == "+")
                    {
                        stack.Push((byte)(r + l));
                    }
                    else
                    {
                        stack.Push((byte)(l - r));
                    }
                }
                else
                {
                    if (t.StartsWith("%"))
                    {
                        stack.Push(registers[EvaluateRegisterName(t.Replace("%", ""))]);
                    } else if (t.Trim() != "")
                    {
                        stack.Push(EvaluateNumber(t.Replace("$", "")));
                    }
                }
            }

            return stack.Pop();
        }

        public int EvaluateRegisterName(string name)
        {
            Debug.Log(name);
            return registerNames[name.ToLower()];
        }

        public byte EvaluateNumber(string number)
        {
            if (constants.ContainsKey(number))
            {
                return constants[number];
            } else
            {
                return Parse.Byte(number);
            }
        }

        public int EvaluateLabel(string lbl)
        {
            Debug.Log("label at: " + instructions[labels[lbl]].OpType);
            return labels[lbl];
        }
    }
}

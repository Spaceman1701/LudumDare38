using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace src.emulator
{
    public class CPU
    {
        public delegate void ExternalInst(byte[] memory, byte[] registers, Program p, Instruction i);

        private const int AX = 0;
        private const int BX = 1;
        private const int CX = 2;
        private const int DX = 3;

        private const byte GREATER_THAN = 2;
        private const byte EQUAL_TO = 1;
        private const byte LESS_THAN = 0;

        public byte[] memory;
        public byte[] registers;

        public byte flags;

        Program p;

        int currentInst;
        int nextInst;

        private IDictionary<Instruction.Type, MethodInfo> methodDict;
        private IDictionary<Instruction.Type, ExternalInst> externalDict;

        public CPU(int mem, int reg, IDictionary<Instruction.Type, ExternalInst> externalInstructions)
        {
            memory = new byte[mem];
            registers = new byte[reg];
            this.externalDict = externalInstructions;
            CreateMethodDictionary();
        }


        private void CreateMethodDictionary()
        {
            methodDict = new Dictionary<Instruction.Type, MethodInfo>();
            MethodInfo[] methods = GetType().GetMethods();
            foreach (MethodInfo m in methods)
            {
                Instruction.Type type = Instruction.TypeFromString(m.Name);
                if (type != Instruction.Type.NONE)
                {
                    methodDict[type] = m;
                }
            }
        }

        public void Reset()
        {
            memory = new byte[memory.Length];
            registers = new byte[registers.Length];
            currentInst = 0;
        }

        public void LoadProgram(Program p)
        {
            this.p = p;
        }

        public void ExecuteUntilHalt()
        {
            Instruction i = p.GetLine(currentInst);
            nextInst = currentInst + 1;
            while(!i.ShouldHalt() && currentInst < p.LineCount() && currentInst < 2)
            {
                i = p.GetLine(currentInst);
                nextInst = currentInst + 1;
                if (i.OpType != Instruction.Type.NONE)
                {
                    if (methodDict.ContainsKey(i.OpType))
                    {
                        methodDict[i.OpType].Invoke(this, new System.Object[] { i });
                    }
                    else if (externalDict.ContainsKey(i.OpType))
                    {
                        externalDict[i.OpType].Invoke(memory, registers, p, i);
                    }
                }
                currentInst = nextInst;
            }

        }

        public bool ExecuteSingleLine()
        {
            Instruction i = p.GetLine(currentInst);
            nextInst = currentInst + 1;
            Debug.Log(i.OpType);
  
            if (i.OpType != Instruction.Type.NONE)
            {
                if (methodDict.ContainsKey(i.OpType))
                {
                    methodDict[i.OpType].Invoke(this, new System.Object[] { i });
                }
                else if (externalDict.ContainsKey(i.OpType))
                {
                    externalDict[i.OpType].Invoke(memory, registers, p, i);
                }
            }
            currentInst = nextInst;

            return !i.ShouldHalt();
        }

        private void AssignValue(byte value, Instruction.Parameter write)
        {
            switch(write.type)
            {
                case Instruction.ParamType.MEM:
                    memory[p.EvaulateMemoryPtr(write.data, registers)] = value;
                    break;
                case Instruction.ParamType.REG:
                    registers[p.EvaluateRegisterName(write.data)] = value;
                    break;
            }
        }

        private byte GetValue(Instruction.Parameter read)
        {
            switch (read.type)
            {
                case Instruction.ParamType.REG:
                    return registers[p.EvaluateRegisterName(read.data)];
                case Instruction.ParamType.MEM:
                    return memory[p.EvaulateMemoryPtr(read.data, registers)];
                case Instruction.ParamType.NUM:
                    return p.EvaluateNumber(read.data);
            }
            return 0;
        }

        /**
         * INSTRUCTION IMPLEMETNATIONS
         * */

        public void Mov(Instruction i)
        {
            AssignValue(GetValue(i.ParamTwo), i.ParamOne);
        }

        public void Add(Instruction i)
        {
            byte a = (byte)(GetValue(i.ParamOne) + GetValue(i.ParamTwo));
            AssignValue(a, i.ParamOne);
        }

        public void Sub(Instruction i)
        {
            byte a = (byte)(GetValue(i.ParamOne) - GetValue(i.ParamTwo));
            AssignValue(a, i.ParamOne);
        }

        public void Mul(Instruction i)
        {
            byte a = (byte)(GetValue(i.ParamOne) * GetValue(i.ParamTwo));
            AssignValue(a, i.ParamOne);
        }

        public void Lea(Instruction i)
        {
            int mem = p.EvaulateMemoryPtr(i.ParamTwo.data, registers);
            AssignValue((byte)mem, i.ParamOne);
        }

        public void Cmp(Instruction i)
        {
            byte l = GetValue(i.ParamOne);
            byte r = GetValue(i.ParamTwo);
            
            if (l > r)
            {
                flags = GREATER_THAN;
            } else if (l == r)
            {
                flags = EQUAL_TO;
            } else if (l < r)
            {
                flags = LESS_THAN;
            }
        }

        public void Jmp(Instruction i)
        {
            nextInst = p.EvaluateLabel(i.ParamOne.data);
        }

        public void Jeq(Instruction i)
        {
            if (flags == EQUAL_TO)
            {
                Jmp(i);
            }
        }

        public void Jne(Instruction i)
        {
            if (flags != EQUAL_TO)
            {
                Jmp(i);
            }
        }

        public void Jgt(Instruction i)
        {
            if (flags == GREATER_THAN)
            {
                Jmp(i);
            }
        }

        public void Jlt(Instruction i)
        {
            if (flags == LESS_THAN)
            {
                Jmp(i);
            }
        }

        public void Jge(Instruction i)
        {
            if (flags == EQUAL_TO || flags == GREATER_THAN)
            {
                Jmp(i);
            }
        }

        public void Jle(Instruction i)
        {
            if (flags == LESS_THAN || flags == GREATER_THAN)
            {
                Jmp(i);
            }
        }

        public void Jcz(Instruction i)
        {
            if (registers[CX] == 0)
            {
                Jmp(i);
            }
        }

        public void Loop(Instruction i)
        {
            if (registers[CX] != 0)
            {
                registers[CX]--;
                Jmp(i);
            }
        }

        public void Shr(Instruction i)
        {
            byte v = GetValue(i.ParamOne);
            byte s = GetValue(i.ParamTwo);

            AssignValue((byte)(v >> s), i.ParamTwo);
        }

        public void Shl(Instruction i)
        {
            byte v = GetValue(i.ParamOne);
            byte s = GetValue(i.ParamTwo);

            AssignValue((byte)(v << s), i.ParamTwo);
        }

        public void Xor(Instruction i)
        {
            byte v = GetValue(i.ParamOne);
            byte s = GetValue(i.ParamTwo);

            AssignValue((byte)(v ^ s), i.ParamTwo);
        }

        public void Or(Instruction i)
        {
            byte v = GetValue(i.ParamOne);
            byte s = GetValue(i.ParamTwo);

            AssignValue((byte)(v | s), i.ParamTwo);
        }

        public void And(Instruction i)
        {
            byte v = GetValue(i.ParamOne);
            byte s = GetValue(i.ParamTwo);

            AssignValue((byte)(v & s), i.ParamTwo);
        }

        public void Not(Instruction i)
        {
            byte v = GetValue(i.ParamOne);
            AssignValue((byte)(~v), i.ParamOne);
        }

        public void Inc(Instruction i)
        {
            byte v = GetValue(i.ParamOne);
            AssignValue((++v), i.ParamOne);
        }

        public void Dec(Instruction i)
        {
            byte v = GetValue(i.ParamOne);
            AssignValue((--v), i.ParamOne);
        }

        public void Hlt(Instruction i)
        {

        }
    }
}


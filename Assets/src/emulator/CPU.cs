using UnityEngine;
using System.Collections;

namespace src.emulator
{
    public class CPU
    {
        byte[] memory;
        byte[] registers;

        public CPU(int mem, int reg)
        {
            memory = new byte[mem];
            registers = new byte[reg];
        }
    }
}


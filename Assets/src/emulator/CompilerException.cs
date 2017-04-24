using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace src.emulator
{
    class CompilerException : Exception
    {
        public CompilerException(string msg) : base(msg)
        {

        }
    }
}

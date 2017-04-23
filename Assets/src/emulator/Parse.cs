using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace src.emulator
{
    class Parse
    {
        public static byte Byte(string s)
        {
            if (s.ToLower().StartsWith("0x"))
            {
                return byte.Parse(s.Substring(2, s.Length - 2), NumberStyles.AllowHexSpecifier);
            }
            return byte.Parse(s);
        }

        public static bool TryByte(string s, out byte value)
        {
            if (s.ToLower().StartsWith("0x"))
            {
                try
                {
                    value = byte.Parse(s.Substring(2, s.Length - 2), NumberStyles.AllowHexSpecifier);
                    return true;
                }
                catch
                {
                    value = 0;
                    return false;
                }
            }
            return byte.TryParse(s, out value);
        }
    }
}

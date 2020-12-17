using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU
{
    public static class ConversorBinario
    {
        public static byte[] Decimal(int n)
        {
            byte[] a = new byte[ALU.Bits + 1];
            for (int i = 1; n > 0; i++)
            {
                a[i] = (byte)(n % 2);
                n = n / 2;
            }
            return a;
        }

        public static string OpCode(int n)
        {
            byte[] OpCode = new byte[CPU.LocgitudOpCode];

            for (int i = 0; n > 0; i++)
            {
                OpCode[i] = (byte)(n % 2);
                n = n / 2;
            }
            return string.Join(string.Empty, OpCode);
        }

        public static byte[] Direccion(int n)
        {
            byte[] direccion = new byte[CPU.LongitudDireccion];

            for (int i = 0; n > 0; i++)
            {
                direccion[i] = (byte)(n % 2);
                n = n / 2;
            }

            return direccion;
        }
    }
}

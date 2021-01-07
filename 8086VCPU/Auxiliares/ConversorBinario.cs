using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Auxiliares
{
    public static class ConversorBinario
    {

        public static int Binario(string numero)
        {
            return Convert.ToInt32(numero, fromBase: 2);
        }
        public static int BinarioConSigno(string numero)
        {
            int n = Binario(numero.Substring(1));
            if (numero[0] == '1')
            {
                n *= -1;
            }
            return n;
        }
        public static bool[] Decimal(int numero)
        {
            List<bool> binario;
            if (numero < 0)
            {
                return CPU.Alu.COMPLEMENTO_2(Decimal(Math.Abs(numero)));
            }

            binario = Convert.ToString(numero, 2).Select(x => x == '1').ToList();
            if (binario.Count < Alu.Alu.Byte)
            {
                while (binario.Count < Alu.Alu.Byte)
                {
                    binario.Insert(0, false);
                }
            }
            else if (binario.Count < Alu.Alu.Palabra)
            {
                while (binario.Count < Alu.Alu.Palabra)
                {
                    binario.Insert(0, false);
                }
            }
            return binario.ToArray();
        }

        internal static bool[] Palabra(int numero)
        {
            List<bool> binario;
            if (numero < 0)
            {
                return CPU.Alu.COMPLEMENTO_2(Palabra(Math.Abs(numero)));
            }

            binario = Convert.ToString(numero, 2).Select(x => x == '1').ToList();
            if (binario.Count < Alu.Alu.Palabra)
            {
                while (binario.Count < Alu.Alu.Palabra)
                {
                    binario.Insert(0, false);
                }
            }
            return binario.ToArray();
        }
    }
}

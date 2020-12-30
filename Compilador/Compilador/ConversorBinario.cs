using _8086VCPU.Alu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador
{
    public static class ConversorBinario
    {
        public static bool[] Palabra()
        {
            bool[] palabra = new bool[Alu.Byte];

            return palabra;
        }
        public static bool[] DoblePalabra()
        {
            bool[] palabra = new bool[Alu.Byte * 2];

            return palabra;
        }
        public static int Binario(string numero)
        {
            return Convert.ToInt32(numero, fromBase: 2);
        }

        internal static bool[] Decimal(int numero)
        {
            List<bool> binario;
            if (numero < 0)
            {
                return _8086VCPU.CPU.Alu.COMPLEMENTO_2(Decimal(Math.Abs(numero)));
            }

            binario = Convert.ToString(numero, 2).Select(x => x == '1').ToList();
            if (binario.Count < Alu.Byte)
            {
                while (binario.Count < Alu.Byte)
                {
                    binario.Insert(0, false);
                }
            }
            else if (binario.Count < Alu.Palabra)
            {
                while (binario.Count < Alu.Palabra)
                {
                    binario.Insert(0, false);
                }
            }
            return binario.ToArray();
        }
    }
}

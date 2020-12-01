using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU
{
    public class ALU
    {
        const int Bits = 4;

        public int[] Operador1 = new int[Bits];
        public int[] Operador2 = new int[Bits];

        public int[] Resultado = new int[Bits * 2];
        public int Acarreo;
        public void SetOperadores(int[] Op1, int[] Op2)
        {
            Operador1 = Op1;
            Operador2 = Op2;
        }
        public void Imprimir()
        {
            Console.Write(Acarreo);
            Console.Write("-");
            for (int i = 0; i < Bits * 2; i++)
            {
                Console.Write(Resultado[i]);
                Console.Write(",");
            }
            Console.Write("\n");
        }

        public void ADD()
        {
            for (int i = Bits - 1; i >= 0; i--)
            {
                Resultado[i] = Operador1[i] + Operador2[i] + Acarreo;
                Acarreo = 0;
                if (Resultado[i] == 2)
                {
                    Resultado[i] = 0;
                    Acarreo = 1;
                }
                else if (Resultado[i] == 3)
                {
                    Resultado[i] = 1;
                    Acarreo = 1;
                }
            }
        }
        public void AND()
        {
            Acarreo = 0;
            for (int i = Bits - 1; i >= 0; i--)
            {
                if (Operador1[i] == 1 && Operador2[i] == 1)
                {
                    Resultado[i] = 1;
                }
                else
                {
                    Resultado[i] = 0;
                }
            }
        }
        public void OR()
        {
            Acarreo = 0;
            for (int i = Bits - 1; i >= 0; i--)
            {
                if (Operador1[i] == 0 && Operador2[i] == 0)
                {
                    Resultado[i] = 0;
                }
                else
                {
                    Resultado[i] = 1;
                }
            }
        }
        public void MUL()
        {
            int offset;
            for (int i = Bits - 1; i >= 0; i--)
            {
                offset = Bits - i - 1;
                int op1 = Operador2[i];
                for (int j = Bits - 1; j >= 0; j--)
                {
                    int op2 = Operador1[j];
                    Resultado[(Bits * 2) - 1 - offset] += op1 * op2;
                    offset++;
                }

            }
            for (int k = Resultado.Length - 1; k >= 0; k--)
            {
                if (Resultado[k] % 2 == 0 && Resultado[k] > 1)
                {

                    Resultado[k - 1] += (int)Math.Round(Resultado[k] / 2f, 0);
                    Resultado[k] = 0;
                }
                if (Resultado[k] % 2 != 0 && Resultado[k] > 1)
                {
                    Resultado[k - 1] += (int)Math.Truncate(Resultado[k] / 2f);
                    Resultado[k] = 1;
                }
            }
        }

    }
}

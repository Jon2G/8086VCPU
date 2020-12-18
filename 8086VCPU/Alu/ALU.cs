using _8086VCPU.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _8086VCPU.Alu
{
    public class ALU
    {
        public const int Bits = 4;


        public byte[] Resultado = new byte[Bits * 2 + 1];


        private byte _Acarreo;
        private byte Acarreo
        {
            get => _Acarreo;
            set
            {
                if (value > 0)
                {
                    CPU.Banderas.Carry = true;
                }
                else
                {
                    CPU.Banderas.Carry = false;
                }
                _Acarreo = value;
            }
        }


        public void ADD(byte[] Operador1, byte[] Operador2)
        {
            Resultado = new byte[Operador1.Length];
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                Resultado[i] = (byte)(Operador1[i] + Operador2[i] + Acarreo);
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

            CPU.Banderas.OverFlow = (Resultado[0] == 1);
        }
        public void SUB(byte[] Operador1, byte[] Operador2)
        {
            Operador2 = Complemento2(Operador2);
            ADD(Operador1, Operador2);

            CPU.Banderas.Signo = (Resultado[0] == 1);
            if (CPU.Banderas.Signo && CPU.Banderas.OverFlow)
            {
                CPU.Banderas.OverFlow = false; //falso positivo por suma negativa
            }
        }
        private byte[] Complemento2(byte[] Operador1)
        {
            int i = Array.LastIndexOf(Operador1, (byte)1);
            if (i > 0)
            {
                i--;
                for (; i >= 0; i--)
                {
                    Operador1[i] = (byte)(Operador1[i] == 1 ? 0 : 1);
                }
            }
            return Operador1;
        }
        public void AND(byte[] Operador1, byte[] Operador2)
        {
            Resultado = new byte[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
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
        public void OR(byte[] Operador1, byte[] Operador2)
        {
            Resultado = new byte[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
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
        public void NAND(byte[] Operador1, byte[] Operador2)
        {
            Resultado = new byte[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                if (Operador1[i] == 1 && Operador2[i] == 1)
                {
                    Resultado[i] = 0;
                }
                else
                {
                    Resultado[i] = 1;
                }
            }
        }
        public void NOR(byte[] Operador1, byte[] Operador2)
        {
            Resultado = new byte[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                if (Operador1[i] == 0 && Operador2[i] == 0)
                {
                    Resultado[i] = 1;
                }
                else
                {
                    Resultado[i] = 0;
                }
            }
        }
        public void MUL(byte[] Operador2)
        {
            byte[] Operador1;
            if (Operador2.Length == Bits)
            {
                Registros.Registros.AX.HabilitarLeectura(true);
                Operador1 = Registros.Registros.AX.GetLow();
                Registros.Registros.AX.HabilitarLeectura(false);
                Resultado = new byte[Bits * 2];
            }
            else
            {
                Registros.Registros.AX.HabilitarLeectura(true);
                Operador1 = Registros.Registros.AX.Get();
                Registros.Registros.AX.HabilitarLeectura(false);
                Resultado = new byte[Bits * 4];
            }

            byte signo = (byte)(Operador2[0] * Operador1[0]);

            int offset;
            for (int i = Operador2.Length - 1; i > 0; i--)
            {
                offset = Operador2.Length - i - 1;
                byte op1 = Operador2[i];
                for (int j = Operador2.Length - 1; j > 0; j--)
                {
                    byte op2 = Operador1[j];
                    Resultado[Operador2.Length * 2 - 1 - offset] += (byte)(op1 * op2);
                    offset++;
                }

            }
            for (int k = Resultado.Length - 1; k > 0; k--)
            {
                if (Resultado[k] % 2 == 0 && Resultado[k] > 1)
                {

                    Resultado[k - 1] += (byte)Math.Round(Resultado[k] / 2f, 0);
                    Resultado[k] = 0;
                }
                if (Resultado[k] % 2 != 0 && Resultado[k] > 1)
                {
                    Resultado[k - 1] += (byte)Math.Truncate(Resultado[k] / 2f);
                    Resultado[k] = 1;
                }
            }
            if (Operador2.Length == Bits)
            {
                Registros.Registros.AX.HabilitarEscritura(true);
                Registros.Registros.AX.Set(Resultado);
                Registros.Registros.AX.HabilitarEscritura(false);
            }
            else
            {
                byte[] temporal = new byte[Bits * 2];
                Array.Copy(Resultado, temporal, Bits * 2);
                Registros.Registros.DX.HabilitarEscritura(true);
                Registros.Registros.DX.Set(temporal);
                Registros.Registros.DX.HabilitarEscritura(false);

                temporal = new byte[Bits * 2];
                Array.Copy(Resultado, Bits * 2, temporal, 0, Bits * 2);
                Registros.Registros.AX.HabilitarEscritura(true);
                Registros.Registros.AX.Set(temporal);
                Registros.Registros.AX.HabilitarEscritura(false);

            }


            if (signo == 1)
            {
                CPU.Banderas.Signo = true;
            }
            if (Resultado[0] == 1 && signo == 0)
            {
                CPU.Banderas.OverFlow = true;
            }
        }
        public void NOT(byte[] Operador1)
        {
            for (int i = 0; i < Operador1.Length; i++)
            {
                if (Operador1[i] == 1)
                {
                    Operador1[i] = 0;
                }
                else
                {
                    Operador1[i] = 1;
                }
            }

        }
        public void XOR(byte[] Operador1, byte[] Operador2)
        {
            Resultado = new byte[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                if (Operador1[i] == Operador2[i])
                {
                    Resultado[i] = 0;
                }
                else
                {
                    Resultado[i] = 1;
                }
            }
        }
        public void XNOR(byte[] Operador1, byte[] Operador2)
        {
            Resultado = new byte[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                if (Operador1[i] != Operador2[i])
                {
                    Resultado[i] = 0;
                }
                else
                {
                    Resultado[i] = 1;
                }
            }
        }
    }
}

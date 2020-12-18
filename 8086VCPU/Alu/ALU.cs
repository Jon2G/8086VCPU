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
            byte[] temporal = new byte[Operador2.Length];
            Array.Copy(Operador2, temporal, Operador2.Length);


            temporal = Complemento2(temporal);
            ADD(Operador1, temporal);

            CPU.Banderas.Signo = (Resultado[0] == 1);
            if (CPU.Banderas.Signo && CPU.Banderas.OverFlow)
            {
                CPU.Banderas.OverFlow = false; //falso positivo por suma negativa
            }

            CPU.Banderas.Zero = (!Resultado.Any(x => x == 1));
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
                Registros.Registros.AX.EnableLectura(true);
                Operador1 = Registros.Registros.AX.GetLow();
                Registros.Registros.AX.EnableLectura(false);
                Resultado = new byte[Bits * 2];
            }
            else
            {
                Registros.Registros.AX.EnableLectura(true);
                Operador1 = Registros.Registros.AX.Get();
                Registros.Registros.AX.EnableLectura(false);
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
        public void DIV(byte[] Divisor)
        {




            byte[] Cociente;
            byte[] Residuo;

            byte[] Dividendo;
            if (Divisor.Length == ALU.Bits)
            {
                Cociente = new byte[ALU.Bits * 2];
                Residuo = new byte[ALU.Bits * 2];

                List<byte> DivisorAumentado = new List<byte>();
                for (int i = 0; i < ALU.Bits; i++)
                {
                    DivisorAumentado.Add(0);
                }
                DivisorAumentado.AddRange(Divisor);

                Divisor = DivisorAumentado.ToArray();

                Registros.Registros.AX.EnableLectura(true);
                Dividendo = Registros.Registros.AX.Get();
                Registros.Registros.AX.EnableLectura(false);
            }
            else
            {
                Cociente = new byte[ALU.Bits * 4];
                Residuo = new byte[ALU.Bits * 4];

                List<byte> unidos = new List<byte>();
                unidos.AddRange(Registros.Registros.DX.Get());
                unidos.AddRange(Registros.Registros.AX.Get());
                Dividendo = unidos.ToArray();
            }

            //si el divisor cabe en el primer numero de izq a derecha
            //Divisor    =0000 00 11 3
            //Dividendo  =0000 0110 6
            //Resultado  =0000 0010
            byte[] temporal;
            for (int i = 0; i < Dividendo.Length; i++)
            {
                //
                temporal = new byte[Dividendo.Length];
                Array.Copy(Dividendo, 0, temporal, temporal.Length - (i + 1), i + 1);
                //
                Acarreo = 0;
                SUB(Divisor, temporal);
                if (CPU.Banderas.Zero || CPU.Banderas.Signo)
                {
                    Cociente[i] = 1;
                    //
                    if (CPU.Banderas.Zero)
                    {
                        if (i == ALU.Bits * 2 - 1 || Dividendo[i + 1] == 0)
                        {
                            Resultado = Cociente;
                            break;
                        }
                    }
                    else
                    {
                        SUB(temporal, Divisor);
                        Residuo = Resultado;

                        if (i < ALU.Bits * 2 - 1)
                        {
                            List<byte> recorrer = new List<byte>(Residuo);
                            recorrer.Add(Dividendo[i + 1]);

                            temporal = new byte[Dividendo.Length];
                            Array.Copy(recorrer.ToArray(), 1, Dividendo, 0, ALU.Bits * 2);

                            Residuo = new byte[ALU.Bits * 2];
                        }
                    }
                    //Resultado[]
                    //1 cociente=0
                    //1
                    //11 cociente=01
                    // 11
                    //-11
                }
            }

            if (Divisor.Length == ALU.Bits * 2)
            {
                temporal = new byte[ALU.Bits];
                Array.Copy(Resultado, ALU.Bits, temporal, 0, ALU.Bits);

                Registros.Registros.AX.HabilitarEscritura(true);
                Registros.Registros.AX.SetLow(temporal);
                Registros.Registros.AX.HabilitarEscritura(false);

                temporal = new byte[ALU.Bits];
                Array.Copy(Residuo, ALU.Bits, temporal, 0, ALU.Bits);

                Registros.Registros.AX.HabilitarEscritura(true);
                Registros.Registros.AX.SetHigh(temporal);
                Registros.Registros.AX.HabilitarEscritura(false);

            }
            else
            {

            }
            //si CMP==Mayor...
            //si CMP=Menor..

        }
    }
}

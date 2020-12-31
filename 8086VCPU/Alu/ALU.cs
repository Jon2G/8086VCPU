using _8086VCPU.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _8086VCPU.Alu
{
    public class Alu
    {
        public const int Byte = 4;
        public const int Palabra = Byte * 2;


        public bool[] Resultado = new bool[Byte * 2 + 1];


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
        public void ADD(bool[] Operador1, bool[] Operador2)
        {
            Operador1 = Operador1.Reverse().ToArray();
            Operador2 = Operador2.Reverse().ToArray();

            CPU.Banderas.Carry = false;

            Resultado = new bool[Operador1.Length];

            bool tmp = HALF_ADD(Operador1[0], Operador2[0]);
            Resultado[0] = tmp;

            for (int i = 1; i < Operador1.Length; ++i)
            {
                tmp = FULL_ADD(Operador1[i], Operador2[i]);
                Resultado[i] = tmp;
            }
            Resultado = Resultado.Reverse().ToArray();
            CPU.Banderas.OverFlow = (CPU.Banderas.Carry && Resultado[0]);
        }
        private bool HALF_ADD(bool A, bool B)
        {
            CPU.Banderas.Carry = AND(A, B);
            return XOR(A, B);
        }
        private bool FULL_ADD(bool A, bool B)
        {
            bool xor = XOR(A, B);
            bool ret = XOR(CPU.Banderas.Carry, xor);
            CPU.Banderas.Carry = (CPU.Banderas.Carry & xor) | (A & B);
            return ret;
        }
        public void SUB(bool[] Operador1, bool[] Operador2)
        {
            bool[] temporal = new bool[Operador2.Length];
            Array.Copy(Operador2, temporal, Operador2.Length);


            temporal = COMPLEMENTO_2(temporal);
            ADD(Operador1, temporal);

            CPU.Banderas.Signo = (Resultado[0]);
            if (CPU.Banderas.Signo && CPU.Banderas.OverFlow)
            {
                CPU.Banderas.OverFlow = false; //falso positivo por suma negativa
            }

            CPU.Banderas.Zero = (!Resultado.Any(x => x));
        }
        public bool[] COMPLEMENTO_2(bool[] Operador1)
        {
            int i = Array.LastIndexOf(Operador1, true);
            if (i > 0)
            {
                i--;
                for (; i >= 0; i--)
                {
                    Operador1[i] = (!Operador1[i]);
                }
            }
            return Operador1;
        }
        private bool AND(bool A, bool B)
        {
            return A && B;
        }
        public void AND(bool[] Operador1, bool[] Operador2)
        {
            Resultado = new bool[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                Resultado[i] = AND(Operador1[i], Operador2[i]);
            }
        }
        public void OR(bool[] Operador1, bool[] Operador2)
        {
            Resultado = new bool[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                if (!Operador1[i] && !Operador2[i])
                {
                    Resultado[i] = false;
                }
                else
                {
                    Resultado[i] = true;
                }
            }
        }
        public void NAND(bool[] Operador1, bool[] Operador2)
        {
            Resultado = new bool[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                Resultado[i] = !(Operador1[i] && Operador2[i]);
            }
        }
        public void NOR(bool[] Operador1, bool[] Operador2)
        {
            Resultado = new bool[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                Resultado[i] = (!Operador1[i] && !Operador2[i]);
            }
        }
        public void MUL(bool[] Operador2)
        {
            bool[] Operador1;
            if (Operador2.Length == Byte)
            {
                Registros.Registros.AX.EnableLectura(true);
                Operador1 = Registros.Registros.AX.GetLow();
                Registros.Registros.AX.EnableLectura(false);
                Resultado = new bool[Byte * 2];
            }
            else
            {
                Registros.Registros.AX.EnableLectura(true);
                Operador1 = Registros.Registros.AX.Get();
                Registros.Registros.AX.EnableLectura(false);
                Resultado = new bool[Byte * 4];
            }


            byte[] TResultado = new byte[Resultado.Length];
            int offset;
            for (int i = Operador2.Length - 1; i >= 0; i--)
            {
                offset = Operador2.Length - i - 1;
                bool op1 = Operador2[i];
                for (int j = Operador2.Length - 1; j > 0; j--)
                {
                    bool op2 = Operador1[j];
                    TResultado[Operador2.Length * 2 - 1 - offset] += AND(op1, op2) ? 1 : 0;
                    offset++;
                }

            }
            for (int k = Resultado.Length - 1; k >= 0; k--)
            {
                if (TResultado[k] % 2 == 0 && TResultado[k] > 1)
                {

                    TResultado[k - 1] += (byte)Math.Round(TResultado[k] / 2f, 0);
                    TResultado[k] = 0;
                }
                if (TResultado[k] % 2 != 0 && TResultado[k] > 1)
                {
                    TResultado[k - 1] += (byte)Math.Truncate(TResultado[k] / 2f);
                    TResultado[k] = 1;
                }
            }
            Resultado = TResultado.Select(x => x == 1).ToArray();


            if (Operador2.Length == Byte)
            {
                Registros.Registros.AX.HabilitarEscritura(true);
                Registros.Registros.AX.Set(Resultado);
                Registros.Registros.AX.HabilitarEscritura(false);
            }
            else
            {
                bool[] temporal = new bool[Byte * 2];
                Array.Copy(Resultado, temporal, Byte * 2);
                Registros.Registros.DX.HabilitarEscritura(true);
                Registros.Registros.DX.Set(temporal);
                Registros.Registros.DX.HabilitarEscritura(false);

                temporal = new bool[Byte * 2];
                Array.Copy(Resultado, Byte * 2, temporal, 0, Byte * 2);
                Registros.Registros.AX.HabilitarEscritura(true);
                Registros.Registros.AX.Set(temporal);
                Registros.Registros.AX.HabilitarEscritura(false);

            }
            if (Resultado[0])
            {
                CPU.Banderas.OverFlow = true;
            }
        }
        public void NOT(bool[] Operador1)
        {
            for (int i = 0; i < Operador1.Length; i++)
            {
                Operador1[i] = !Operador1[i];
            }
        }
        private bool XOR(bool Operador1, bool Operador2)
        {
            return !(Operador1 == Operador2);
        }
        public void XOR(bool[] Operador1, bool[] Operador2)
        {
            Resultado = new bool[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                Resultado[i] = XOR(Operador1[i], Operador2[i]);
            }
        }
        public void XNOR(bool[] Operador1, bool[] Operador2)
        {
            Resultado = new bool[Operador1.Length];
            Acarreo = 0;
            for (int i = Operador1.Length - 1; i >= 0; i--)
            {
                if (Operador1[i] != Operador2[i])
                {
                    Resultado[i] = false;
                }
                else
                {
                    Resultado[i] = true;
                }
            }
        }
        public void DIV(bool[] Divisor)
        {
            bool[] Cociente;
            bool[] Residuo;

            bool[] Dividendo;
            if (Divisor.Length == Alu.Byte)
            {
                Cociente = new bool[Alu.Byte * 2];
                Residuo = new bool[Alu.Byte * 2];

                List<bool> DivisorAumentado = new List<bool>();
                for (int i = 0; i < Alu.Byte; i++)
                {
                    DivisorAumentado.Add(false);
                }
                DivisorAumentado.AddRange(Divisor);

                Divisor = DivisorAumentado.ToArray();

                Registros.Registros.AX.EnableLectura(true);
                Dividendo = Registros.Registros.AX.Get();
                Registros.Registros.AX.EnableLectura(false);
            }
            else
            {
                Cociente = new bool[Alu.Byte * 4];
                Residuo = new bool[Alu.Byte * 4];

                List<bool> unidos = new List<bool>();
                unidos.AddRange(Registros.Registros.DX.Get());
                unidos.AddRange(Registros.Registros.AX.Get());
                Dividendo = unidos.ToArray();
            }

            //si el divisor cabe en el primer numero de izq a derecha
            //Divisor    =0000 00 11 3
            //Dividendo  =0000 0110 6
            //Resultado  =0000 0010
            bool[] temporal;
            for (int i = 0; i < Dividendo.Length; i++)
            {
                //
                temporal = new bool[Dividendo.Length];
                Array.Copy(Dividendo, 0, temporal, temporal.Length - (i + 1), i + 1);
                //
                Acarreo = 0;
                SUB(Divisor, temporal);
                if (CPU.Banderas.Zero || CPU.Banderas.Signo)
                {
                    Cociente[i] = true;
                    //
                    if (CPU.Banderas.Zero)
                    {
                        if (i == Alu.Byte * 2 - 1 || !Dividendo[i + 1])
                        {
                            Resultado = Cociente;
                            break;
                        }
                    }
                    else
                    {
                        SUB(temporal, Divisor);
                        Residuo = Resultado;

                        if (i < Alu.Byte * 2 - 1)
                        {
                            List<bool> recorrer = new List<bool>(Residuo);
                            recorrer.Add(Dividendo[i + 1]);

                            temporal = new bool[Dividendo.Length];
                            Array.Copy(recorrer.ToArray(), 1, Dividendo, 0, Alu.Byte * 2);

                            Residuo = new bool[Alu.Byte * 2];
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

            if (Divisor.Length == Alu.Byte * 2)
            {
                temporal = new bool[Alu.Byte];
                Array.Copy(Resultado, Alu.Byte, temporal, 0, Alu.Byte);

                Registros.Registros.AX.HabilitarEscritura(true);
                Registros.Registros.AX.SetLow(temporal);
                Registros.Registros.AX.HabilitarEscritura(false);

                temporal = new bool[Alu.Byte];
                Array.Copy(Residuo, Alu.Byte, temporal, 0, Alu.Byte);

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

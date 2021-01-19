using _8086VCPU.Alu;
using _8086VCPU.Auxiliares;
using _8086VCPU.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU
{
    public static class CPU
    {
        public const int LocgitudOpCode = 6;
        public const int LongitudDireccion = 20;
        //public Banderas Banderas { get; set; }
        public static Alu.Alu Alu { get; private set; }
        public static Banderas Banderas { get; private set; }
        public static Memoria Memoria { get; private set; }
        static CPU()
        {
            CPU.Alu = new Alu.Alu();
            CPU.Banderas = new Banderas();
            CPU.Memoria = new Memoria();
            Reset();
        }
        public static void Reset()
        {
            CPU.Banderas.Clear();
            CPU.Memoria.Clear();
            Registros.Registros.Reset();
        }
        public static void Ejecutar(bool[] Operacion, bool[] Modificador, bool[] Operador1, bool[] Operador2)
        {
            //DIRECTO	MOV AX,[00H]
            //     1                     1            0               1                0 
            if (Operacion[0] && Operacion[1] && !Operacion[2] && Operacion[3] && !Operacion[4])
            {
                return;
            }
            bool[] ValorOperador1 = CPU.ObtenerRegistro(Operador1);
            bool[] ValorOperador2 = null;
            //POR REGISTRO	MOV AX,AX
            //           0                0                     0                   1 
            if (!Modificador[0] && !Modificador[1] && !Modificador[2] && Modificador[3])
            {
                ValorOperador2 = CPU.ObtenerRegistro(Operador2);
            }
            //DIRECTO	MOV AX,[00H]
            //              0                0                     1                   0 
            else if (!Modificador[0] && !Modificador[1] && Modificador[2] && !Modificador[3])
            {
                ValorOperador2 = CPU.Memoria.Leer(Operador2);
            }
            //INMEDIATO	MOV AX,09H
            //              0                0                     1                   1 
            else if (!Modificador[0] && !Modificador[1] && Modificador[2] && Modificador[3])
            {
                ValorOperador2 = Operador2;
                //El valor del operador 2 ya representa al número en binario
            }
            //INDIRECTO	MOV AX,[SI/DI]
            //              0                1                     0                  0 
            else if (!Modificador[0] && Modificador[1] && !Modificador[2] && !Modificador[3])
            {
                ValorOperador2 = ObtenerRegistro(Operador2);
                ValorOperador2 = CPU.Memoria.Leer(ValorOperador2);
            }
            //INDEXADO	MOV AX[BX+SI/DI]
            //              0                1                     0                  1
            else if (!Modificador[0] && Modificador[1] && !Modificador[2] && Modificador[3])
            {
                //Obtener valor de BX
                Registros.Registros.BX.EnableLectura(true);
                bool[] valor_bx = Registros.Registros.BX.Get();
                Registros.Registros.BX.EnableLectura(false);
                //Obtener valor de SI/DI
                bool[] desplazamiento = ObtenerRegistro(Operador2);
                //Calcular dirección BX+SI/DI
                CPU.Alu.ADD(valor_bx, desplazamiento);
                //Leer el valor en la dirección calculada
                ValorOperador2 = CPU.Memoria.Leer(CPU.Alu.Resultado);

            }
            //SIMPLE	MUL AX
            //              0                1                     1                  0 
            else if (!Modificador[0] && Modificador[1] && Modificador[2] && !Modificador[3])
            {
                //la función no tiene segundo operador
            }
            //SALTO	[JMP*] [DIRECCION]
            //              0                1                     1                  1
            else if (!Modificador[0] && Modificador[1] && Modificador[2] && Modificador[3])
            {
                ValorOperador1 = Operador1;
                Saltar(Operacion, ValorOperador1);
                return;
            } //INDIRECTO MOV [SI/DI],AX
            //              1                0                     1                 1
            else if (Modificador[0] && !Modificador[1] && Modificador[2] && Modificador[3])
            {
                ValorOperador2 = ObtenerRegistro(Operador2);
                CPU.Memoria[ValorOperador1] = ValorOperador2;
                return;
            }//INDEXADO MOV [BX+SI/DI],AX
            //              0                1                     1                  1
            else if (!Modificador[0] && Modificador[1] && Modificador[2] && Modificador[3])
            {

            }//DIRECTO	MOV [00H],AX
            //              1                0                     0                  1
            else if (Modificador[0] && !Modificador[1] && !Modificador[2] && Modificador[3])
            {
                ValorOperador1 = ObtenerRegistro(Operador1);
                CPU.Memoria[Operador2] = ValorOperador1;
                return;
            }

            EjecutarOperacion(Operacion, ValorOperador1, ValorOperador2, Operador1);
        }
        private static void EjecutarOperacion(bool[] Operacion, bool[] Operador1, bool[] Operador2, bool[] RegistroDestino)
        {
            //MOV
            if (!Operacion[0] && !Operacion[1] && !Operacion[2] && !Operacion[3] && !Operacion[4] && Operacion[5])
            {
                GuardarEnRegistro(RegistroDestino, Operador2);
            }
            else if (!Operacion[0] && !Operacion[1] && !Operacion[2] && !Operacion[3] && Operacion[4] && !Operacion[5])
            {
                CPU.Alu.ADD(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }
            else if (!Operacion[0] && !Operacion[1] && !Operacion[2] && !Operacion[3] && Operacion[4] && Operacion[5])
            {
                CPU.Alu.SUB(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }
            else if (!Operacion[0] && !Operacion[1] && !Operacion[2] && Operacion[3] && !Operacion[4] && !Operacion[5])
            {
                CPU.Alu.DIV(Operador1);//Se guarda internamente por la ALU
            }
            else if (!Operacion[0] && !Operacion[1] && !Operacion[2] && Operacion[3] && !Operacion[4] && Operacion[5])
            {
                CPU.Alu.MUL(Operador1);//Se guarda internamente por la ALU
            }
            else if (!Operacion[0] && !Operacion[1] && !Operacion[2] && Operacion[3] && Operacion[4] && !Operacion[5])
            {
                CPU.Alu.NOT(Operador1);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);

            }//OR          0             0                 1                1           1 
            else if (!Operacion[0] && !Operacion[1] && !Operacion[2] && Operacion[3] && Operacion[4] && Operacion[5])
            {
                CPU.Alu.OR(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//NOR          0             1                 0               0         0
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && !Operacion[3] && !Operacion[4] && !Operacion[5])
            {
                CPU.Alu.NOR(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//XOR          0             1                 0                0                1         
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && !Operacion[3] && !Operacion[4] && Operacion[5])
            {
                CPU.Alu.XOR(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//XNOR          0             1                 0                1           0         
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && !Operacion[3] && Operacion[4] && !Operacion[5])
            {
                CPU.Alu.XNOR(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//AND          0             1                 0                1             1
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && !Operacion[3] && Operacion[4] && Operacion[5])
            {
                CPU.Alu.AND(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//NAND          0             1               1                0           0    
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && Operacion[3] && !Operacion[4] && !Operacion[5] && !Operacion[4])
            {
                CPU.Alu.NAND(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//CMP          0                 0             1               1                0           1    
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && Operacion[3] && !Operacion[4] && Operacion[5])
            {
                CPU.Alu.SUB(Operador1, Operador2);
            }
        }
        private static void Saltar(bool[] Salto, bool[] Direccion)
        {
            bool DeboSaltar = false;
            //01110 JMP
            if (!Salto[0] && !Salto[1] && Salto[2] && Salto[3] && Salto[4] && !Salto[5])
            {
                DeboSaltar = true;
            }
            //01111,10000   JZ,JE
            else if ((!Salto[0] && !Salto[1] && Salto[2] && Salto[3] && Salto[4] && Salto[5]) ||
                (!Salto[0] && Salto[1] && !Salto[2] && !Salto[3] && !Salto[4] && !Salto[5]))
            {
                DeboSaltar = CPU.Banderas.Zero && !CPU.Banderas.Signo;
            }//10001,10010  JNZ,JNE
            else if ((!Salto[0] && Salto[1] && !Salto[2] && !Salto[3] && !Salto[4] && Salto[5]) ||
                (!Salto[0] && Salto[1] && !Salto[2] && !Salto[3] && Salto[4] && !Salto[5]))
            {
                DeboSaltar = !(CPU.Banderas.Zero && !CPU.Banderas.Signo);
            }//10011 JC
            else if (!Salto[0] && !Salto[1] && Salto[2] && Salto[3] && !Salto[4] && !Salto[5])
            {
                DeboSaltar = CPU.Banderas.Carry;
            }//10100 JA
            else if (!Salto[0] && Salto[1] && !Salto[2] && Salto[3] && !Salto[4] && !Salto[5])
            {
                DeboSaltar = (!CPU.Banderas.Signo && !CPU.Banderas.Zero);
            }//10101 JAE
            else if (!Salto[0] && Salto[1] && !Salto[2] && Salto[3] && !Salto[4] && Salto[5])
            {
                DeboSaltar = (CPU.Banderas.Zero || !CPU.Banderas.Signo) && CPU.Banderas.Carry;
            }//10110 JLE
            else if (!Salto[0] && Salto[1] && !Salto[2] && Salto[3] && Salto[4] && !Salto[5])
            {
                DeboSaltar = CPU.Banderas.Zero || CPU.Banderas.Signo;
            }//10111 JO
            else if (!Salto[0] && Salto[1] && !Salto[2] && Salto[3] && Salto[4] && Salto[5])
            {
                DeboSaltar = CPU.Banderas.OverFlow;
            }//11000 JNS
            else if (!Salto[0] && Salto[1] && Salto[2] && !Salto[3] && !Salto[4] && !Salto[5])
            {
                DeboSaltar = !CPU.Banderas.Signo;
            }//11001 JNO
            else if (!Salto[0] && Salto[1] && Salto[2] && !Salto[3] && !Salto[4] && Salto[5])
            {
                DeboSaltar = !CPU.Banderas.OverFlow;
            } //11011  JL
            else if (!Salto[0] && Salto[1] && Salto[2] && !Salto[3] && Salto[4] && Salto[5])
            {
                DeboSaltar = (CPU.Banderas.Signo && !CPU.Banderas.Zero);
            } //LOOP
            else if (!Salto[0] && Salto[1] && Salto[2] && Salto[3] && !Salto[4] && Salto[5])
            {
                Registros.Registros.CX.EnableLectura(true);
                bool[] valorCx = Registros.Registros.CX.Get();
                Registros.Registros.CX.EnableLectura(false);
                Alu.SUB(valorCx, ConversorBinario.Palabra(1));
                valorCx = Alu.Resultado;
                Registros.Registros.CX.EnableEscritura(true);
                Registros.Registros.CX.Set(valorCx);
                Registros.Registros.CX.EnableEscritura(false);

                DeboSaltar = !CPU.Banderas.Zero;
            }

            if (DeboSaltar)
            {
                Registros.Registros.IP.EnableEscritura(true);
                Registros.Registros.IP.Set(Direccion);
                Registros.Registros.IP.EnableEscritura(false);
            }
        }
        private static void GuardarEnRegistro(bool[] RegistroDestino, bool[] Valor)
        {
            //AX     0              0          0           1
            if (!RegistroDestino[28] && !RegistroDestino[29] && !RegistroDestino[30] && RegistroDestino[31])
            {
                Registros.Registros.AX.EnableEscritura(true);
                Registros.Registros.AX.Set(Valor);
                Registros.Registros.AX.EnableEscritura(false);
            }//AH     0              0          1          0
            else if (!RegistroDestino[28] && !RegistroDestino[29] && RegistroDestino[30] && !RegistroDestino[31])
            {
                Registros.Registros.AX.EnableEscritura(true);
                Registros.Registros.AX.SetHigh(Valor);
                Registros.Registros.AX.EnableEscritura(false);
            }//AL     0              0          1           1
            else if (!RegistroDestino[28] && !RegistroDestino[29] && RegistroDestino[30] && RegistroDestino[31])
            {
                Registros.Registros.AX.EnableEscritura(true);
                Registros.Registros.AX.SetLow(Valor);
                Registros.Registros.AX.EnableEscritura(false);
            } //BX     0              1          0           0
            else if (!RegistroDestino[28] && RegistroDestino[29] && !RegistroDestino[30] && !RegistroDestino[31])
            {
                Registros.Registros.BX.EnableEscritura(true);
                Registros.Registros.BX.Set(Valor);
                Registros.Registros.BX.EnableEscritura(false);
            }//BH     0              1          0          1
            else if (!RegistroDestino[28] && RegistroDestino[29] && !RegistroDestino[30] && RegistroDestino[31])
            {
                Registros.Registros.BX.EnableEscritura(true);
                Registros.Registros.BX.SetHigh(Valor);
                Registros.Registros.BX.EnableEscritura(false);
            }//BL     0              1          1           0
            else if (!RegistroDestino[28] && RegistroDestino[29] && RegistroDestino[30] && !RegistroDestino[31])
            {
                Registros.Registros.BX.EnableEscritura(true);
                Registros.Registros.BX.SetLow(Valor);
                Registros.Registros.BX.EnableEscritura(false);
            }//CX     0              1          1           1
            else if (!RegistroDestino[28] && RegistroDestino[29] && RegistroDestino[30] && RegistroDestino[31])
            {
                Registros.Registros.CX.EnableEscritura(true);
                Registros.Registros.CX.Set(Valor);
                Registros.Registros.CX.EnableEscritura(false);
            }//CH     1              0          0           0
            else if (RegistroDestino[28] && !RegistroDestino[29] && !RegistroDestino[30] && !RegistroDestino[31])
            {
                Registros.Registros.CX.EnableEscritura(true);
                Registros.Registros.CX.SetHigh(Valor);
                Registros.Registros.CX.EnableEscritura(false);
            }//CL     1              0          0           1
            else if (RegistroDestino[28] && !RegistroDestino[29] && !RegistroDestino[30] && RegistroDestino[31])
            {
                Registros.Registros.CX.EnableEscritura(true);
                Registros.Registros.CX.SetLow(Valor);
                Registros.Registros.CX.EnableEscritura(false);
            }//DX     1              0          1           0
            else if (RegistroDestino[28] && !RegistroDestino[29] && RegistroDestino[30] && !RegistroDestino[31])
            {
                Registros.Registros.DX.EnableEscritura(true);
                Registros.Registros.DX.Set(Valor);
                Registros.Registros.DX.EnableEscritura(false);

            }//DH     1              0          1           1
            else if (RegistroDestino[28] && !RegistroDestino[29] && RegistroDestino[30] && RegistroDestino[31])
            {
                Registros.Registros.DX.EnableEscritura(true);
                Registros.Registros.DX.SetHigh(Valor);
                Registros.Registros.DX.EnableEscritura(false);

            }//DL     1              1          0           0
            else if (RegistroDestino[28] && RegistroDestino[29] && !RegistroDestino[30] && !RegistroDestino[31])
            {
                Registros.Registros.DX.EnableEscritura(true);
                Registros.Registros.DX.SetLow(Valor);
                Registros.Registros.DX.EnableEscritura(false);

            }//SI     1              1          0           1
            else if (RegistroDestino[28] && RegistroDestino[29] && !RegistroDestino[30] && RegistroDestino[31])
            {
                Registros.Registros.SI.EnableEscritura(true);
                Registros.Registros.SI.Set(Valor);
                Registros.Registros.SI.EnableEscritura(false);

            }//DI     1              1          1           0
            else if (RegistroDestino[28] && RegistroDestino[29] && RegistroDestino[30] && !RegistroDestino[31])
            {
                Registros.Registros.DI.EnableEscritura(true);
                Registros.Registros.DI.Set(Valor);
                Registros.Registros.DI.EnableEscritura(false);

            }
        }
        private static bool[] ObtenerRegistro(bool[] Valor)
        {
            bool[] valorRegistro = null;
            //AX     0              0          0           1
            if (!Valor[28] && !Valor[29] && !Valor[30] && Valor[31])
            {
                Registros.Registros.AX.EnableLectura(true);
                valorRegistro = Registros.Registros.AX.Get();
                Registros.Registros.AX.EnableLectura(false);
            }//AH     0              0          1          0
            else if (!Valor[28] && !Valor[29] && Valor[30] && !Valor[31])
            {
                Registros.Registros.AX.EnableLectura(true);
                valorRegistro = Registros.Registros.AX.GetHigh();
                Registros.Registros.AX.EnableLectura(false);
            }//AL     0              0          1           1
            else if (!Valor[28] && !Valor[29] && Valor[30] && Valor[31])
            {
                Registros.Registros.AX.EnableLectura(true);
                valorRegistro = Registros.Registros.AX.GetLow();
                Registros.Registros.AX.EnableLectura(false);
            } //BX     0              1          0           0
            else if (!Valor[28] && Valor[29] && !Valor[30] && !Valor[31])
            {
                Registros.Registros.BX.EnableLectura(true);
                valorRegistro = Registros.Registros.BX.Get();
                Registros.Registros.BX.EnableLectura(false);
            }//BH     0              1          0          1
            else if (!Valor[28] && Valor[29] && !Valor[30] && Valor[31])
            {
                Registros.Registros.BX.EnableLectura(true);
                valorRegistro = Registros.Registros.BX.GetHigh();
                Registros.Registros.BX.EnableLectura(false);
            }//BL     0              1          1           0
            else if (!Valor[28] && Valor[29] && Valor[30] && !Valor[31])
            {
                Registros.Registros.BX.EnableLectura(true);
                valorRegistro = Registros.Registros.BX.GetLow();
                Registros.Registros.BX.EnableLectura(false);
            }//CX     0              1          1           1
            else if (!Valor[28] && Valor[29] && Valor[30] && Valor[31])
            {
                Registros.Registros.CX.EnableLectura(true);
                valorRegistro = Registros.Registros.CX.Get();
                Registros.Registros.CX.EnableLectura(false);
            }//CH     1              0          0           0
            else if (Valor[28] && !Valor[29] && !Valor[30] && !Valor[31])
            {
                Registros.Registros.CX.EnableLectura(true);
                valorRegistro = Registros.Registros.CX.GetHigh();
                Registros.Registros.CX.EnableLectura(false);
            }//CL     1              0          0           1
            else if (Valor[28] && !Valor[29] && !Valor[30] && Valor[31])
            {
                Registros.Registros.CX.EnableLectura(true);
                valorRegistro = Registros.Registros.CX.GetLow();
                Registros.Registros.CX.EnableLectura(false);
            }//DX     1              0          1           0
            else if (Valor[28] && !Valor[29] && Valor[30] && !Valor[31])
            {
                Registros.Registros.DX.EnableLectura(true);
                valorRegistro = Registros.Registros.DX.Get();
                Registros.Registros.DX.EnableLectura(false);

            }//DH     1              0          1           1
            else if (Valor[28] && !Valor[29] && Valor[30] && Valor[31])
            {
                Registros.Registros.DX.EnableLectura(true);
                valorRegistro = Registros.Registros.DX.GetHigh();
                Registros.Registros.DX.EnableLectura(false);

            }//DL     1              1          0           0
            else if (Valor[28] && Valor[29] && !Valor[30] && !Valor[31])
            {
                Registros.Registros.DX.EnableLectura(true);
                valorRegistro = Registros.Registros.DX.GetLow();
                Registros.Registros.DX.EnableLectura(false);

            }//SI     1              1          0           1
            else if (Valor[28] && Valor[29] && !Valor[30] && Valor[31])
            {
                Registros.Registros.SI.EnableLectura(true);
                valorRegistro = Registros.Registros.SI.Get();
                Registros.Registros.SI.EnableLectura(false);

            }//DI     1              1          1           0
            else if (Valor[28] && Valor[29] && Valor[30] && !Valor[31])
            {
                Registros.Registros.DI.EnableLectura(true);
                valorRegistro = Registros.Registros.DI.Get();
                Registros.Registros.DI.EnableLectura(false);

            }
            return valorRegistro;
        }
    }
}

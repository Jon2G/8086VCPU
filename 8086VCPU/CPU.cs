﻿using _8086VCPU.Alu;
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
        }
        public static void Ejecutar(bool[] Operacion, bool[] Modificador, bool[] Operador1, bool[] Operador2)
        {
            bool[] ValorOperador1 = CPU.ObtenerRegistro(Operador1);
            bool[] ValorOperador2 = null;
            //POR REGISTRO	MOV AX,AX
            //     0                     0                   1 
            if (!Modificador[0] && !Modificador[1] && Modificador[2])
            {
                ValorOperador2 = CPU.ObtenerRegistro(Operador2);
            }
            //DIRECTO	MOV AX,[00H]
            //     0                     1                   0 
            else if (!Modificador[0] && Modificador[1] && !Modificador[2])
            {
                ValorOperador2 = CPU.Memoria.Leer(Operador2);
            }
            //INMEDIATO	MOV AX,09H
            //     0                     1                   1 
            else if (!Modificador[0] && Modificador[1] && Modificador[2])
            {
                ValorOperador1 = Operador2;
                //El valor del operador 2 ya representa al número en binario
            }
            //INDIRECTO	MOV AX,[SI/DI]
            //     1                     0                   0 
            else if (Modificador[0] && !Modificador[1] && !Modificador[2])
            {
                ValorOperador2 = ObtenerRegistro(Operador2);
                ValorOperador2 = CPU.Memoria.Leer(ValorOperador2);
            }
            //INDEXADO	MOV AX[BX+SI/DI]
            //     1                     0                   1 
            else if (Modificador[0] && !Modificador[1] && Modificador[2])
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
            //     1                     1                   0 
            else if (Modificador[0] && Modificador[1] && !Modificador[2])
            {
                //la función no tiene segundo operador
            }
            EjecutarOperacion(Operacion, ValorOperador1, ValorOperador2, Operador1);
        }
        private static void EjecutarOperacion(bool[] Operacion, bool[] Operador1, bool[] Operador2, bool[] RegistroDestino)
        {
            //MOV
            if (!Operacion[0] && !Operacion[1] && !Operacion[2] && !Operacion[3] && Operacion[4])
            {
                GuardarEnRegistro(RegistroDestino, Operador1);
            }
            else if (!Operacion[0] && !Operacion[1] && !Operacion[2] && Operacion[3] && !Operacion[4])
            {
                CPU.Alu.ADD(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }
            else if (!Operacion[0] && !Operacion[1] && !Operacion[2] && Operacion[3] && Operacion[4])
            {
                CPU.Alu.SUB(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && !Operacion[3] && !Operacion[4])
            {
                CPU.Alu.DIV(Operador1);//Se guarda internamente por la ALU
            }
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && !Operacion[3] && Operacion[4])
            {
                CPU.Alu.MUL(Operador1);//Se guarda internamente por la ALU
            }
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && Operacion[3] && !Operacion[4])
            {
                CPU.Alu.NOT(Operador1);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);

            }//OR          0             0                 1                1           1 
            else if (!Operacion[0] && !Operacion[1] && Operacion[2] && Operacion[3] && Operacion[4])
            {
                CPU.Alu.OR(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//NOR          0             1                 0               0         0
            else if (!Operacion[0] && Operacion[1] && !Operacion[2] && !Operacion[3] && !Operacion[4])
            {
                CPU.Alu.NOR(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//XOR          0             1                 0                0                1         
            else if (!Operacion[0] && Operacion[1] && !Operacion[2] && !Operacion[3] && Operacion[4])
            {
                CPU.Alu.XOR(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//XNOR          0             1                 0                1           0         
            else if (!Operacion[0] && Operacion[1] && !Operacion[2] && Operacion[3] && !Operacion[4])
            {
                CPU.Alu.XNOR(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//AND          0             1                 0                1             1
            else if (!Operacion[0] && Operacion[1] && !Operacion[2] && Operacion[3] && Operacion[4])
            {
                CPU.Alu.AND(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
            }//NAND          0             1               1                0           0    
            else if (!Operacion[0] && Operacion[1] && Operacion[2] && !Operacion[3] && !Operacion[4])
            {
                CPU.Alu.NAND(Operador1, Operador2);
                //Se guarda el valor en el primer operador
                GuardarEnRegistro(RegistroDestino, CPU.Alu.Resultado);
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

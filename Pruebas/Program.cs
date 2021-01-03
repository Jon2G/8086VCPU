using _8086VCPU;
using _8086VCPU.Alu;
using _8086VCPU.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            CPU.Alu.ToString();
            //  MOV AX,[BX]
            //  MOV [BX],AX

            //bool[] operador1 = Registros.CX.GetLow(); //Leer 4 bits CL
            //bool[] operador1 = Registros.CX.GetHigh(); //   Leer 4 bits CH
            //bool[] operador1 = Registros.CX.Get(); //   Leer 8 bits CX

            //MOV CL,3d
            Registros.CX.EnableEscritura(true);
                                            //0010
            Registros.CX.SetLow(new bool[] { false, false, true, false });
            Registros.CX.EnableEscritura(false);

            //MOV AX,6d
            Registros.AX.EnableEscritura(true);
            Registros.AX.Set(new bool[] { false, false, false, false, false, true, true, false });
            Registros.AX.EnableEscritura(false);

            //DIV CL
            Registros.CX.EnableLectura(true);
            bool[] operador1 = Registros.CX.GetLow();
            Registros.CX.EnableLectura(false);
            CPU.Alu.DIV(operador1);



            //MOV CL,5d
            Registros.CX.EnableEscritura(true);
            Registros.CX.SetLow(new bool[] { true, true, true, true });
            Registros.CX.EnableEscritura(false);
            //MOV DL,2d
            Registros.DX.EnableEscritura(true);
            Registros.DX.SetLow(new bool[] { true, true, true, true });
            Registros.DX.EnableEscritura(false);
            //ADD CL,DL
            Registros.CX.EnableLectura(true);
            operador1 = Registros.CX.GetLow();
            Registros.CX.EnableLectura(false);

            Registros.DX.EnableLectura(true);
            bool[] operador2 = Registros.DX.GetLow();
            Registros.DX.EnableLectura(false);

            CPU.Alu.ADD(operador1, operador2);
            Registros.CX.EnableEscritura(true);
            Registros.CX.SetLow(CPU.Alu.Resultado);
            Registros.CX.EnableEscritura(true);

            //ADD CX,DX
            Registros.CX.EnableLectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.EnableLectura(false);

            Registros.DX.EnableLectura(true);
            operador2 = Registros.DX.Get();
            Registros.DX.EnableLectura(false);

            CPU.Alu.ADD(operador1, operador2);
            Registros.CX.EnableEscritura(true);
            Registros.CX.Set(CPU.Alu.Resultado);
            Registros.CX.EnableEscritura(true);

            //MUL Cl
            Registros.CX.EnableEscritura(true);
            Registros.CX.SetLow(new bool[] { false, false, true, true });
            Registros.CX.EnableEscritura(true);

            Registros.AX.EnableEscritura(true);
            Registros.AX.SetLow(new bool[] { false, false, true, false });
            Registros.AX.EnableEscritura(true);

            Registros.CX.EnableLectura(true);
            operador1 = Registros.CX.GetLow();
            Registros.CX.EnableLectura(false);
            CPU.Alu.MUL(operador1);

            //MUL CX
            Registros.CX.EnableLectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.EnableLectura(false);
            CPU.Alu.MUL(operador1);


            Registros.CX.EnableLectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.EnableLectura(false);

            Registros.AX.EnableLectura(true);
            operador2 = Registros.AX.Get();
            Registros.AX.EnableLectura(false);

            CPU.Alu.OR(operador1, operador2);
            //CPU.Alu.XOR(operador1, operador2);
            //CPU.Alu.NOR(operador1, operador2);
            //alu.NOR(operador1, operador2);
            //alu.AND(operador1, operador2);
            //alu.NOT(operador1);


            Registros.CX.EnableEscritura(true);
            Registros.CX.Set(CPU.Alu.Resultado);
            Registros.CX.EnableEscritura(false);


            Console.ReadKey(); //Leer una tecla para que no se cierre la consola

        }
    }
}

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
            //MOV CL,5d
            Registros.CX.HabilitarEscritura(true);
            Registros.CX.SetLow(new byte[] { 0, 1, 0, 1 });
            Registros.CX.HabilitarEscritura(false);
            //MOV DL,2d
            Registros.DX.HabilitarEscritura(true);
            Registros.DX.SetLow(new byte[] { 0, 0, 1, 0 });
            Registros.DX.HabilitarEscritura(false);
            //ADD CL,DL
            Registros.CX.HabilitarLeectura(true);
            byte[] operador1 = Registros.CX.GetLow();
            Registros.CX.HabilitarLeectura(false);

            Registros.DX.HabilitarLeectura(true);
            byte[] operador2 = Registros.DX.GetLow();
            Registros.DX.HabilitarLeectura(false);

            CPU.Alu.ADD(operador1, operador2);
            Registros.CX.HabilitarEscritura(true);
            Registros.CX.SetLow(CPU.Alu.Resultado);
            Registros.CX.HabilitarEscritura(true);

            //ADD CX,DX
            Registros.CX.HabilitarLeectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.HabilitarLeectura(false);

            Registros.DX.HabilitarLeectura(true);
            operador2 = Registros.DX.Get();
            Registros.DX.HabilitarLeectura(false);

            CPU.Alu.ADD(operador1, operador2);
            Registros.CX.HabilitarEscritura(true);
            Registros.CX.Set(CPU.Alu.Resultado);
            Registros.CX.HabilitarEscritura(true);

            //MUL Cl
            Registros.CX.HabilitarEscritura(true);
            Registros.CX.SetLow(new byte[] { 0, 0, 1, 1 });
            Registros.CX.HabilitarEscritura(true);

            Registros.AX.HabilitarEscritura(true);
            Registros.AX.SetLow(new byte[] { 0, 0, 1, 0 });
            Registros.AX.HabilitarEscritura(true);

            Registros.CX.HabilitarLeectura(true);
            operador1 = Registros.CX.GetLow();
            Registros.CX.HabilitarLeectura(false);
            CPU.Alu.MUL(operador1);

            //MUL CX
            Registros.CX.HabilitarLeectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.HabilitarLeectura(false);
            CPU.Alu.MUL(operador1);


            Registros.CX.HabilitarLeectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.HabilitarLeectura(false);

            Registros.AX.HabilitarLeectura(true);
            operador2 = Registros.AX.Get();
            Registros.AX.HabilitarLeectura(false);

            CPU.Alu.OR(operador1, operador2);
            //alu.NOR(operador1, operador2);
            //alu.AND(operador1, operador2);
            //alu.NOT(operador1);


            Registros.CX.HabilitarEscritura(true);
            Registros.CX.Set(CPU.Alu.Resultado);
            Registros.CX.HabilitarEscritura(false);


            Console.ReadKey(); //Leer una tecla para que no se cierre la consola

        }
    }
}

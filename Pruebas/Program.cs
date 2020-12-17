using _8086VCPU;
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
            ALU alu = new ALU();

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

            alu.ADD(operador1, operador2);
            Registros.CX.HabilitarEscritura(true);
            Registros.CX.SetLow(alu.Resultado);
            Registros.CX.HabilitarEscritura(true);

            //ADD CX,DX
            Registros.CX.HabilitarLeectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.HabilitarLeectura(false);

            Registros.DX.HabilitarLeectura(true);
            operador2 = Registros.DX.Get();
            Registros.DX.HabilitarLeectura(false);

            alu.ADD(operador1, operador2);
            Registros.CX.HabilitarEscritura(true);
            Registros.CX.Set(alu.Resultado);
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
            alu.MUL(operador1);

            //MUL CX
            Registros.CX.HabilitarLeectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.HabilitarLeectura(false);
            alu.MUL(operador1);


            Registros.CX.HabilitarLeectura(true);
            operador1 = Registros.CX.Get();
            Registros.CX.HabilitarLeectura(false);

            Registros.AX.HabilitarLeectura(true);
            operador2 = Registros.AX.Get();
            Registros.AX.HabilitarLeectura(false);

            //alu.OR(operador1, operador2);
            alu.AND(operador1, operador2);

            Registros.CX.HabilitarEscritura(true);
            Registros.CX.Set(alu.Resultado);
            Registros.CX.HabilitarEscritura(false);




            //alu.SetOperadores(new byte[] { 1, 0, 0, 1 }, new byte[] { 1, 1, 1, 1 });
            //alu.ADD();
            //alu.Imprimir();
            //alu.AND();
            //alu.Imprimir();
            //alu.OR();
            //alu.Imprimir();
            //alu.MUL();
            //alu.Imprimir();//10000111
            Console.ReadKey(); //Leer una tecla para que no se cierre la consola




            //Registro Ax = new Registro();


            //Ax.HabilitarEscritura(true);
            //Ax.SetLow(new byte[] { 0, 1, 1, 1 });
            //Ax.HabilitarEscritura(false);

            //Ax.HabilitarLeectura(true);
            //byte[] valor = Ax.Get();
            //valor = Ax.GetLow();
            //Ax.HabilitarLeectura(false);

        }
    }
}

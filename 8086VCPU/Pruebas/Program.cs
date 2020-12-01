using _8086VCPU;
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
            alu.SetOperadores(new int[] { 1, 0, 0, 1 }, new int[] { 1, 1, 1, 1 });
            alu.ADD();
            alu.Imprimir();
            alu.AND();
            alu.Imprimir();
            alu.OR();
            alu.Imprimir()
            alu.NAND();
            alu.Imprimir(); 
            alu.NOR();

            Console.ReadKey(); //Leer una tecla para que no se cierre la consola

      
        }
    }
}

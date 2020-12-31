using _8086VCPU.Alu;
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
        }
    }
}

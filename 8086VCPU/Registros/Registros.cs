using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Registros
{
    public static class Registros
    {
        public static Registro AX { get; private set; }
        public static Registro BX { get; private set; }
        public static Registro CX { get; private set; }
        public static Registro DX { get; private set; }
        static Registros()
        {
            Registros.AX = new Registro("AX");
            Registros.BX = new Registro("BX");
            Registros.CX = new Registro("CX");
            Registros.DX = new Registro("DX");
        }
    }
}

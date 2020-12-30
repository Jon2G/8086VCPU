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
        public static Registro SI { get; private set; }
        public static Registro DI { get; private set; }
        static Registros()
        {
            Registros.AX = new Registro("AX");
            Registros.BX = new Registro("BX");
            Registros.SI = new Registro("CX");
            Registros.DI = new Registro("DX");
        }

        public static Registro PorNombre(string nombre)
        {
            switch (nombre.ToUpper())
            {
                case "AX":
                case "AH":
                case "AL":
                    return Registros.AX;
                case "BX":
                case "BH":
                case "BL":
                    return Registros.BX;
                case "CX":
                case "CH":
                case "CL":
                    return Registros.CX;
                case "DX":
                case "DH":
                case "DL":
                    return Registros.DX;
                case "SI":
                    return SI;
                case "DI":
                    return DI;
            }
            return null;
        }
    }
}

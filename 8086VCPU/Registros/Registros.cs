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
        public static Registro IP { get; private set; }
        public static Registro IA { get; private set; }
        public static Registro IR { get; private set; }
        static Registros()
        {
            Registros.AX = new Registro("AX");
            Registros.BX = new Registro("BX");
            Registros.CX = new Registro("CX");
            Registros.DX = new Registro("DX");

            Registros.SI = new Registro("SI");
            Registros.DI = new Registro("DI");

            Registros.IP = new Registro("IP");

            Registros.IA = new Registro("IA");
            Registros.IR = new Registro("IR");
        }
        internal static void Reset()
        {
            Registros.AX.Clear();
            Registros.BX.Clear();
            Registros.CX.Clear();
            Registros.DX.Clear();

            Registros.SI.Clear();
            Registros.DI.Clear();
            Registros.IP.Clear();
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
        public static string OpCode(string nombre)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Alu.Alu.Palabra - 4; i++)
            {
                sb.Append("0");
            }

            switch (nombre.ToUpper())
            {
                case "AX":
                    sb.Append("0001");
                    break;
                case "AH":
                    sb.Append("0010");
                    break;
                case "AL":
                    sb.Append("0011");
                    break;
                case "BX":
                    sb.Append("0100");
                    break;
                case "BH":
                    sb.Append("0101");
                    break;
                case "BL":
                    sb.Append("0110");
                    break;
                case "CX":
                    sb.Append("0111");
                    break;
                case "CH":
                    sb.Append("1000");
                    break;
                case "CL":
                    sb.Append("1001");
                    break;
                case "DX":
                    sb.Append("1010");
                    break;
                case "DH":
                    sb.Append("1011");
                    break;
                case "DL":
                    sb.Append("1100");
                    break;
                case "SI":
                    sb.Append("1101");
                    break;
                case "DI":
                    sb.Append("1110");
                    break;
            }
            return sb.ToString();
        }
        internal static string PorOpCode(string op1)
        {
            int n_registro = Convert.ToInt32(op1, 2);
            switch (n_registro)
            {
                case 1:
                    return "AX";
                case 2:
                    return "AH";
                case 3:
                    return "AL";
                case 4:
                    return "BX";
                case 5:
                    return "BH";
                case 6:
                    return "BL";
                case 7:
                    return "CX";
                case 8:
                    return "CH";
                case 9:
                    return "CL";
                case 10:
                    return "DX";
                case 11:
                    return "DH";
                case 12:
                    return "DL";
                case 13:
                    return "SI";
                case 14:
                    return "DI";
            }
            return null;
        }
    }
}

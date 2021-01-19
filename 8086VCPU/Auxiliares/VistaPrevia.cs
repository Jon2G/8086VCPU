using Kit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8086VCPU.Auxiliares
{
    public class VistaPrevia : ViewModelBase<VistaPrevia>
    {
        private int _OpCode;
        public int OpCode
        {
            get => _OpCode;
            private set { _OpCode = value; OnPropertyChanged(); }
        }
        private int _ModCode;
        public int ModCode
        {
            get => _ModCode;
            private set { _ModCode = value; OnPropertyChanged(); }
        }

        private string _OpInstruccion;
        public string OpInstruccion
        {
            get => _OpInstruccion;
            private set { _OpInstruccion = value; OnPropertyChanged(); }
        }
        private string _Operador1;
        private string _Operador2;
        public VistaPrevia()
        {
            ModCode = 0;
            OpCode = 0;
            OpInstruccion = "~-~";
            ObtenerInstruccion();
        }
        protected void GetOpCode(bool[] Operacion)
        {
            OpCode =Convert.ToInt32(string.Concat(Operacion.Select(x => x ? "1" : "0")),2);
        }
        protected void GetModCode(bool[] Modificador)
        {
            ModCode = Convert.ToInt32(string.Concat(Modificador.Select(x => x ? "1" : "0")),2);
        }
        protected void SetOperadores(bool[] operador1, bool[] operador2)
        {
            if (operador1.Length != 32 || operador2.Length != 32)
            {

            }
            _Operador1 = string.Join("", operador1.Select(x => x ? "1" : "0"));
            _Operador2 = string.Join("", operador2.Select(x => x ? "1" : "0"));
            
        }
        protected void CalcularVistaPrevia()
        {
            StringBuilder sb = new StringBuilder(ObtenerInstruccion());
            sb.Append(" ");
            switch (ModCode)
            {
                case 1:
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",");
                    sb.Append(Registros.Registros.PorOpCode(_Operador2));
                    break;
                case 2:
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",[");
                    sb.Append(Convert.ToInt32(_Operador2, 2));
                    sb.Append("d]");
                    break;
                case 3:
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",");
                    sb.Append(Convert.ToInt32(_Operador2, 2));
                    sb.Append("d");
                    break;
                case 4:
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",[");
                    sb.Append(Registros.Registros.PorOpCode(_Operador2));
                    sb.Append("]");
                    break;
                case 5:
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",[BX");
                    string desplazamiento = Registros.Registros.PorOpCode(_Operador2);
                    if (desplazamiento != "BX")
                    {
                        sb.Append("+");
                        sb.Append(desplazamiento);
                    }
                    sb.Append("]");
                    break;
                case 6:
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    break;
                case 9:
                    sb.Append("[");
                    sb.Append(Convert.ToInt32(_Operador2, 2));
                    sb.Append("d],");
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    break;
                case 10:
                    sb.Append("[");
                    sb.Append(Convert.ToInt32(_Operador1, 2));
                    sb.Append("d],");
                    sb.Append(Registros.Registros.PorOpCode(_Operador2));
                    break;
                case 11:
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",[BX");
                    string desplazamientoI = Registros.Registros.PorOpCode(_Operador2);
                    if (desplazamientoI != "BX")
                    {
                        sb.Append("+");
                        sb.Append(desplazamientoI);
                    }
                    sb.Append("]");
                    break;
            }
            this.OpInstruccion = sb.ToString();
        }
        private string ObtenerInstruccion()
        {
            switch (OpCode)
            {
                case 0:
                    return "NOP";
                case 1:
                    return "MOV";
                case 2:
                    return "ADD";
                case 3:
                    return "SUB";
                case 4:
                    return "DIV";
                case 5:
                    return "MUL";
                case 6:
                    return "NOT";
                case 7:
                    return "OR";
                case 8:
                    return "NOR";
                case 9:
                    return "XOR";
                case 10:
                    return "XNOR";
                case 11:
                    return "AND";
                case 12:
                    return "NAND";
                case 13:
                    return "CMP";
                case 14:
                    return "JMP";
                case 15:
                    return "JZ";
                case 16:
                    return "JE";
                case 17:
                    return "JNZ";
                case 18:
                    return "JNE";
                case 19:
                    return "JC";
                case 20:
                    return "JA";
                case 21:
                    return "JAE";
                case 22:
                    return "JLE";
                case 23:
                    return "JO";
                case 24:
                    return "JNS";
                case 25:
                    return "JNO";
                case 26:
                    return "Etiqueta";
                case 27:
                    return "JL";
                case 28:
                    return "begin";
                case 29:
                    return "LOOP";
                case 30:
                    return "DB";
                case 31:
                    return "FIN PROGRAMA";
                default:
                    throw new Exception();
            }
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}

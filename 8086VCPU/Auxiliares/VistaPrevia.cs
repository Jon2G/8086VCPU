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
        private string _OpCode;
        public string OpCode
        {
            get => _OpCode;
            private set { _OpCode = value; OnPropertyChanged(); }
        }
        private string _ModCode;
        public string ModCode
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
        public string _Operador1;
        public string _Operador2;
        public VistaPrevia()
        {
            ModCode = "000";
            OpCode = "00000";
            OpInstruccion = "~-~";
            ObtenerInstruccion();
        }
        protected void GetOpCode(bool[] Operacion)
        {
            OpCode = string.Concat(Operacion.Select(x => x ? "1" : "0"));
        }
        protected void GetModCode(bool[] Modificador)
        {
            ModCode = string.Concat(Modificador.Select(x => x ? "1" : "0"));
        }
        protected void SetOperadores(bool[] operador1, bool[] operador2)
        {
            _Operador1 = string.Join("", operador1.Select(x => x ? "1" : "0"));
            _Operador2 = string.Join("", operador2.Select(x => x ? "1" : "0"));
        }
        protected void CalcularVistaPrevia()
        {
            StringBuilder sb = new StringBuilder(ObtenerInstruccion());
            sb.Append(" ");
            switch (ModCode)
            {
                case "001":
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",");
                    sb.Append(Registros.Registros.PorOpCode(_Operador2));
                    break;
                case "010":
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",[");
                    sb.Append(Convert.ToInt32(_Operador2, 2));
                    sb.Append("d]");
                    break;
                case "011":
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",");
                    sb.Append(Convert.ToInt32(_Operador2, 2));
                    sb.Append("d");
                    break;
                case "100":
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    sb.Append(",[");
                    sb.Append(Registros.Registros.PorOpCode(_Operador2));
                    sb.Append("]");
                    break;
                case "101":
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

                case "110":
                    sb.Append(Registros.Registros.PorOpCode(_Operador1));
                    break;
            }
            this.OpInstruccion = sb.ToString();
        }
        private string ObtenerInstruccion()
        {
            switch (OpCode)
            {
                case "00001":
                    return "MOV";
                case "00010":
                    return "ADD";
                case "00101":
                    return "MUL";
                case "00011":
                    return "SUB";
                case "00100":
                    return "DIV";
                case "00110":
                    return "NOT";
                case "00111":
                    return "OR";
                case "01000":
                    return "NOR";
                case "01001":
                    return "XOR";
                case "01010":
                    return "XNOR";
                case "01011":
                    return "AND";
                case "01100":
                    return "NAND";
                case "11010":
                    return "Etiqueta";
                case "01101":
                    return "CMP";
                case "01110":
                    return "JMP";
                case "01111":
                    return "JZ";
                case "10000":
                    return "JE";
                case "10001":
                    return "JNZ";
                case "10010":
                    return "JNE";
                case "10011":
                    return "JC";
                case "10100":
                    return "JA";
                case "10101":
                    return "JAE";
                case "10110":
                    return "JLE";
                case "10111":
                    return "JO";
                case "11000":
                    return "JNS";
                case "11001":
                    return "JNO";
                case "00000":
                    return "NOP";
                case "11011":
                    return "JL";
                case "11111":
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

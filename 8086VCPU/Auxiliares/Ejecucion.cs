using _8086VCPU;
using Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Alu.Alu;
using static _8086VCPU.Registros.Registros;

namespace _8086VCPU.Auxiliares
{
    public class Ejecucion : ViewModelBase<Ejecucion>
    {
        public string CodigoMaquina { get; private set; }
        private string _Modificador;
        public string Modificador { get => _Modificador; private set { _Modificador = value; OnPropertyChanged(); } }
        private string _OpCode;
        public string OpCode { get => _OpCode; private set { _OpCode = value; OnPropertyChanged(); } }

        private string _Instruccion;
        public string Instruccion { get => _Instruccion; private set { _Instruccion = value; OnPropertyChanged(); } }

        public bool[] ValorActual { get; set; }
        public int LongitudOperacion { get; set; }
        private string Operador1;
        private bool[] BinOperador1 { get; set; }
        private bool[] BinOperador2 { get; set; }
        private bool[] BinModificador { get; set; }
        private bool[] BinOpCode { get; set; }

        private string Operador2;
        public bool Reiniciar { get; private set; }
        public Ejecucion(string CodigoMaquina)
        {
            this.CodigoMaquina = CodigoMaquina;
            Redo();
        }
        public bool Siguiente()
        {
            //IP.EnableEscritura(true);
            //IP.Set(ConversorBinario.Palabra(IP.Decimal + LongitudOperacion));
            //IP.EnableEscritura(false);

            IP.EnableLectura(true);
            ValorActual = CPU.Memoria.Leer(IP.Get());
            IP.EnableLectura(false);


            if (ValorActual.All(x => !x))
            {
                InstruccionCompleta();
                Instruccion = "~-~";
                Modificador = "000";
                OpCode = "00000";
                Reiniciar = true;
                return false;
            }

            OpCode = string.Concat(
                ValorActual[Palabra - 8] ? "1" : "0"
                , ValorActual[Palabra - 7] ? "1" : "0"
                , ValorActual[Palabra - 6] ? "1" : "0"
                , ValorActual[Palabra - 5] ? "1" : "0"
                , ValorActual[Palabra - 4] ? "1" : "0");


            Modificador = string.Concat(
                 ValorActual[Palabra - 3] ? "1" : "0"
                , ValorActual[Palabra - 2] ? "1" : "0"
                , ValorActual[Palabra - 1] ? "1" : "0");

            switch (Modificador)
            {
                case "011":
                case "010":
                case "001":
                case "100":
                case "101":
                    LongitudOperacion = 3;
                    break;
                case "110":
                    LongitudOperacion = 2;
                    break;
            }

            Instruccion = InstruccionCompleta();

            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(IP.Decimal + LongitudOperacion));
            IP.EnableEscritura(false);
            return true;

        }
        private string InstruccionCompleta()
        {
            int ip_actual = IP.Decimal;
            if (ip_actual > 0)
            {
                CPU.Ejecutar(BinOpCode, BinModificador, BinOperador1, BinOperador2);
            }

            BinOpCode = OpCode.Select(x => x == '1').ToArray();
            BinModificador = Modificador.Select(x => x == '1').ToArray();
            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(IP.Decimal + 1));
            IP.EnableEscritura(false);

            IP.EnableLectura(true);
            BinOperador1 = CPU.Memoria.Leer(IP.Get());
            Operador1 = string.Join("", BinOperador1.Select(x => x ? "1" : "0"));
            IP.EnableLectura(false);

            Operador2 = string.Empty;

            if (this.LongitudOperacion == 3)
            {
                IP.EnableEscritura(true);
                IP.Set(ConversorBinario.Palabra(IP.Decimal + 1));
                IP.EnableEscritura(false);

                IP.EnableLectura(true);
                BinOperador2 = CPU.Memoria.Leer(IP.Get());
                Operador2 = string.Join("", BinOperador2.Select(x => x ? "1" : "0"));
                IP.EnableLectura(false);
            }
            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(ip_actual));
            IP.EnableEscritura(false);



            StringBuilder sb = new StringBuilder(ObtenerInstruccion());
            sb.Append(" ");
            switch (Modificador)
            {
                case "001":
                    sb.Append(Registros.Registros.PorOpCode(Operador1));
                    sb.Append(",");
                    sb.Append(Registros.Registros.PorOpCode(Operador2));
                    break;
                case "010":
                    sb.Append(Registros.Registros.PorOpCode(Operador1));
                    sb.Append(",[");
                    sb.Append(Convert.ToInt32(Operador2, 2));
                    sb.Append("d]");
                    break;
                case "011":
                    sb.Append(Registros.Registros.PorOpCode(Operador1));
                    sb.Append(",");
                    sb.Append(Convert.ToInt32(Operador2, 2));
                    sb.Append("d");
                    break;
                case "100":
                    sb.Append(Registros.Registros.PorOpCode(Operador1));
                    sb.Append(",[");
                    sb.Append(Registros.Registros.PorOpCode(Operador2));
                    sb.Append("]");
                    break;
                case "101":
                    sb.Append(Registros.Registros.PorOpCode(Operador1));
                    sb.Append(",[BX");
                    string desplazamiento = Registros.Registros.PorOpCode(Operador2);
                    if (desplazamiento != "BX")
                    {
                        sb.Append("+");
                        sb.Append(desplazamiento);
                    }
                    sb.Append("]");
                    break;

                case "110":
                    sb.Append(Registros.Registros.PorOpCode(Operador1));
                    break;
            }
            return sb.ToString();
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
            }
            return "~-~";
        }
        public void Redo()
        {
            Reiniciar = false;
            Instruccion = "~-~";
            Modificador = "000";
            OpCode = "00000";
            LongitudOperacion = 0;

            CPU.Reset();
            CPU.Memoria.Cargar(
                CodigoMaquina.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Select(y => y == '1').ToArray()).ToArray());

            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(0));
            IP.EnableEscritura(false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Alu.Alu;
using static _8086VCPU.Registros.Registros;

namespace _8086VCPU.Auxiliares
{
    public class InstruccionEjecucion : VistaPrevia
    {
        private bool[] Operacion;
        private bool[] Modificador;
        private bool[] Operador1;
        private bool[] Operador2;
        private bool[] Instruccion;
        public int LongitudOperacion { get; private set; }

        private InstruccionEjecucion(bool[] Instruccion)
        {
            this.Operacion = new bool[5];
            this.Modificador = new bool[3];

            this.Operador1 = new bool[Palabra];
            this.Operador2 = new bool[Palabra];
            this.Instruccion = Instruccion;
        }
        public static InstruccionEjecucion Fetch()
        {
            IP.EnableLectura(true);
            bool[] Instruccion = CPU.Memoria.Leer(IP.Get());
            IP.EnableLectura(false);
            return new InstruccionEjecucion(Instruccion);
        }

        internal bool IsPrefetched()
        {
            return this.OpInstruccion == "~-~";
        }

        public InstruccionEjecucion Decode()
        {
            GetOpCode();
            GetModificador();
            GetOperadores();
            CalcularVistaPrevia();
            return this;
        }
        private void GetOpCode()
        {
            //OpCode
            Array.Copy(Instruccion, Palabra - 8, Operacion, 0, 5);
            GetOpCode(Operacion);
        }
        private void GetModificador()
        {
            Array.Copy(Instruccion, Palabra - 3, Modificador, 0, 3);
            GetModCode(Modificador);
            switch (this.ModCode)
            {
                case "011":
                case "010":
                case "001":
                case "100":
                case "101":
                    LongitudOperacion = 3;
                    break;
                case "110":
                case "111":
                    LongitudOperacion = 2;
                    break;
                case "000":
                    LongitudOperacion = 1;
                    break;
                default:
                    Debugger.Break();
                    break;
            }
        }

        internal bool EsEtiqueta()
        {
            return this.OpCode == "11010";
        }

        private void GetOperadores()
        {
            int ip_actual = IP.Decimal;

            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(IP.Decimal + 1));
            IP.EnableEscritura(false);

            IP.EnableLectura(true);
            Operador1 = CPU.Memoria.Leer(IP.Get());
            IP.EnableLectura(false);

            if (this.LongitudOperacion == 3)
            {
                IP.EnableEscritura(true);
                IP.Set(ConversorBinario.Palabra(IP.Decimal + 1));
                IP.EnableEscritura(false);

                IP.EnableLectura(true);
                Operador2 = CPU.Memoria.Leer(IP.Get());
                IP.EnableLectura(false);
            }
            IP.EnableEscritura(true);
            IP.Set(ConversorBinario.Palabra(ip_actual));
            IP.EnableEscritura(false);

            SetOperadores(Operador1, Operador2);
        }



        public bool EsFin()
        {
            return this.OpCode == "11111";
        }
        private bool EsNOP()
        {
            return this.Instruccion.All(x => !x);
        }

        public InstruccionEjecucion Execute()
        {
            CPU.Ejecutar(Operacion, Modificador, Operador1, Operador2);
            return this;
        }
        public override string ToString()
        {
            return this.OpInstruccion;
        }
    }
}

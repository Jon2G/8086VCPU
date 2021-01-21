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
        public bool[] Operador1 { get; private set; }
        private bool[] Operador2;
        public bool[] Instruccion { get; private set; }
        public int LongitudOperacion { get;  set; }

        private InstruccionEjecucion(bool[] Instruccion)
        {
            this.Operacion = new bool[6];
            this.Modificador = new bool[4];

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
            Operacion[0] = Instruccion[22];
            Operacion[1] = Instruccion[23];
            Operacion[2] = Instruccion[24];
            Operacion[3] = Instruccion[25];
            Operacion[4] = Instruccion[26];
            Operacion[5] = Instruccion[27];
            GetOpCode(Operacion);
        }
        private void GetModificador()
        {
            Modificador[0] = Instruccion[28];
            Modificador[1] = Instruccion[29];
            Modificador[2] = Instruccion[30];
            Modificador[3] = Instruccion[31];
            GetModCode(Modificador);
            switch (this.ModCode)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 9:
                case 10:
                case 11:
                    LongitudOperacion = 3;
                    break;
                case 6:
                case 7:
                    LongitudOperacion = 2;
                    break;
                case 0:
                    LongitudOperacion = 1;
                    break;
                default:
                   // Debugger.Break();
                   LongitudOperacion = 1;
                    break;
            }
            if (this.EsNOP())
            {
                LongitudOperacion = 1;
            }
        }

        internal bool EsEtiqueta()
        {
            return this.OpCode == 26;
        }

        private void GetOperadores()
        {
            //int ip_actual = IP.Decimal;

            IA.EnableEscritura(true);
            IA.Set(ConversorBinario.Palabra(IP.Decimal + 1));
            IA.EnableEscritura(false);

            IA.EnableLectura(true);
            Operador1 = CPU.Memoria.Leer(IA.Get());
            IA.EnableLectura(false);

            if (this.LongitudOperacion == 3)
            {
                IA.EnableEscritura(true);
                IA.Set(ConversorBinario.Palabra(IA.Decimal + 1));
                IA.EnableEscritura(false);

                IA.EnableLectura(true);
                Operador2 = CPU.Memoria.Leer(IA.Get());
                IA.EnableLectura(false);
            }
            //IA.EnableEscritura(true);
            //IA.Set(ConversorBinario.Palabra(ip_actual));
            //IA.EnableEscritura(false);

            SetOperadores(Operador1, Operador2);
        }

        public bool IsBegin()
        {
            if (this.OpCode == 28)
            {
                if (this.ModCode == 0)
                {
                    if (ConversorBinario.BinarioToString(this.Instruccion) == "00000000000000000000000111000000")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool EsFin()
        {
            return this.OpCode == 31;
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

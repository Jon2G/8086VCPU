using _8086VCPU;
using _8086VCPU.Alu;
using _8086VCPU.Auxiliares;
using _8086VCPU.Registros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador
{
    public class Ejecucion
    {
        public string CodigoMaquina { get; private set; }
        public string Modificador { get; private set; }
        public bool[] ValorActual { get; set; }
        public int LongitudOperacion { get; set; }
        public Ejecucion(string CodigoMaquina)
        {
            this.CodigoMaquina = CodigoMaquina;
            Redo();
        }
        public void Siguiente()
        {
            Registros.IP.EnableEscritura(true);
            Registros.IP.Set(ConversorBinario.Palabra(Registros.IP.Decimal + 1));
            Registros.IP.EnableEscritura(false);

            Registros.IP.EnableLectura(true);
            this.ValorActual = CPU.Memoria.Leer(Registros.IP.Get());
            Registros.IP.EnableLectura(false);

            this.Modificador = string.Concat(
                ValorActual[Alu.Palabra - 1] ? "1" : "0"
                , ValorActual[Alu.Palabra - 2] ? "1" : "0"
                , ValorActual[Alu.Palabra - 3] ? "1" : "0");

            switch (Modificador)
            {
                case "011":
                case "010":
                case "001":
                case "100":
                case "101":
                    this.LongitudOperacion = 3;
                    break;
                case "110":
                    this.LongitudOperacion = 2;
                    break;
            }


        }
        public void Redo()
        {
            LongitudOperacion = 0;

            CPU.Reset();
            CPU.Memoria.Cargar(
                CodigoMaquina.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Select(y => y == '1').ToArray()).ToArray());

            Registros.IP.EnableEscritura(true);
            Registros.IP.Set(ConversorBinario.Palabra(-1));
            Registros.IP.EnableEscritura(false);
        }
    }
}

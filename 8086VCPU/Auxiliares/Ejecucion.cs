using _8086VCPU;
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
        public bool[] ValorActual { get; set; }
        public Ejecucion(string CodigoMaquina)
        {
            this.CodigoMaquina = CodigoMaquina;
            CPU.Reset();
            CPU.Memoria.Cargar(
                CodigoMaquina.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Select(y => y == '1').ToArray()).ToArray());

            Registros.IP.EnableEscritura(true);
            Registros.IP.Set(ConversorBinario.Palabra(-1));
            Registros.IP.EnableEscritura(false);

        }

        public void Siguiente()
        {
            Registros.IP.EnableEscritura(true);
            Registros.IP.Set(ConversorBinario.Palabra(Registros.IP.Decimal + 1));
            Registros.IP.EnableEscritura(false);

            Registros.IP.EnableLectura(true);
            this.ValorActual = CPU.Memoria.Leer(Registros.IP.Get());
            Registros.IP.EnableLectura(false);
        }
    }
}

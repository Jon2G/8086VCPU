using _8086VCPU.Registros;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones.MOV
{
    public class Indexado : MOV
    {
        public readonly string NombreRegistroD;
        public readonly string NombreRegistroDesplazamiento;

        public Indexado(string NombreRegistroD, string NombreRegistroDesplazamiento, ResultadosCompilacion resultados, DocumentLine cs) : base(cs)
        {
            this.NombreRegistroDesplazamiento = NombreRegistroDesplazamiento;

            this.NombreRegistroD = NombreRegistroD;

            Destino = Registros.PorNombre(NombreRegistroD);
            TamañoDestino = TamañoRegistro(NombreRegistroD);

        }

    }
}

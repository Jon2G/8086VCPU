using _8086VCPU.Alu;
using _8086VCPU.Registros;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones.Modos
{
    public class Indexado : Direccionado
    {
        public readonly string NombreRegistroD;
        public readonly string NombreRegistroDesplazamiento;

        public Indexado(string NombreRegistroD, string NombreRegistroDesplazamiento,
            ResultadosCompilacion resultados, DocumentLine cs, TipoInstruccion tipo) : base(cs, tipo)
        {
            this.NombreRegistroDesplazamiento = NombreRegistroDesplazamiento;

            this.NombreRegistroD = NombreRegistroD;

            Destino = Registros.PorNombre(NombreRegistroD);
            TamañoDestino = TamañoRegistro(NombreRegistroD);

        }

        protected override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("101");
            sb.AppendLine(Registros.OpCode(NombreRegistroD));
            sb.AppendLine(Registros.OpCode(NombreRegistroDesplazamiento));
            return sb;


        }
    }
}

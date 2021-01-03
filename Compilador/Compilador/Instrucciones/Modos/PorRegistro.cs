using _8086VCPU.Alu;
using _8086VCPU.Registros;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Localidad;

namespace Gui.Compilador.Instrucciones.Modos
{
    public class PorRegistro : Direccionado
    {
        public readonly string NombreRegistroD;
        public readonly string NombreRegistroF;
        public PorRegistro(string NombreRegistroD, string NombreRegistroF, ResultadosCompilacion resultados,
            DocumentLine cs, TipoInstruccion tipo) : base(cs, tipo)
        {
            this.NombreRegistroF = NombreRegistroF;
            this.NombreRegistroD = NombreRegistroD;

            Destino = Registros.PorNombre(NombreRegistroD);
            TamañoDestino = TamañoRegistro(NombreRegistroD);

            Fuente = Registros.PorNombre(NombreRegistroF);
            TamañoFuente = TamañoRegistro(NombreRegistroF);


            if (TamañoFuente != TamañoDestino)
            {
                resultados.ResultadoCompilacion($"El tamaño de '{NombreRegistroF}' - {TamañoFuente} no conicide con el tamaño de '{NombreRegistroD.ToUpper()}' - {TamañoDestino}", LineaDocumento);
            }

        }

        protected override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Alu.Palabra - 3; i++)
            {
                sb.Append("0");
            }
            sb.AppendLine("001");
            sb.AppendLine(Registros.OpCode(NombreRegistroD));
            sb.AppendLine(Registros.OpCode(NombreRegistroF));
            return sb;


        }
    }
}

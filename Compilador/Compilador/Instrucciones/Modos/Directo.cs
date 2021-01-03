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
    public class Directo : Direccionado
    {
        public readonly string NombreRegistroD;
        public Directo(string NombreRegistro, Numero Fuente, ResultadosCompilacion resultados, DocumentLine cs, TipoInstruccion tipo) : base(cs, tipo)
        {
            NombreRegistroD = NombreRegistro;
            Destino = Registros.PorNombre(NombreRegistro);
            TamañoDestino = TamañoRegistro(NombreRegistroD);

            this.Fuente = Fuente;
            TamañoFuente = this.Fuente.Tamaño;

            //Validar que la dirección sea valdida?
            //if (TamañoFuente > TamañoDestino)
            //{
            //    resultados.ResultadoCompilacion($"El valor '{Fuente.Hex}' - {TamañoFuente} sobrepasa el tamaño del operando de destino '{NombreRegistro.ToUpper()}' - {TamañoDestino}", LineaDocumento);
            //}
        }

        protected override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Alu.Palabra - 3; i++)
            {
                sb.Append("0");
            }
            sb.AppendLine("010");
            sb.AppendLine(Registros.OpCode(this.NombreRegistroD));
            sb.AppendLine(this.Fuente.ToString());
            return sb;
        }
    }
}

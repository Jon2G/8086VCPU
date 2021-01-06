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
    public class Inmediato : Direccionado
    {
        public readonly string NombreRegistroD;
        public Inmediato(string NombreRegistroD, Numero Fuente,
            ResultadosCompilacion resultados, DocumentLine cs, TipoInstruccion tipo) : base(cs, tipo)
        {
            this.NombreRegistroD = NombreRegistroD.ToUpper();
            Destino = Registros.PorNombre(this.NombreRegistroD);
            TamañoDestino = TamañoRegistro(this.NombreRegistroD);


            this.Fuente = Fuente;
            TamañoFuente = this.Fuente.Tamaño;


            if (TamañoFuente > TamañoDestino)
            {
                resultados.ResultadoCompilacion($"El valor '{Fuente.Hex}' - {TamañoFuente} sobrepasa el tamaño del operando de destino '{this.NombreRegistroD}' - {TamañoDestino}", LineaDocumento);
            }
        }

        protected override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("011");
            sb.AppendLine(Registros.OpCode(NombreRegistroD));

            string fuente = this.Fuente.ToString();
            for (int i = 0; i < Alu.Palabra - fuente.Length; i++)
            {
                sb.Append("0");
            }

            sb.AppendLine(fuente);
            return sb;

        }
    }
}

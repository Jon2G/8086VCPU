using _8086VCPU;
using _8086VCPU.Registros;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Memoria;

namespace Gui.Compilador.Instrucciones.MOV
{
    public abstract class MOV : Instruccion
    {
        public Memoria Fuente { get; protected set; }
        public Memoria Destino { get; protected set; }
        public Tamaños TamañoDestino { get; protected set; }
        public Tamaños TamañoFuente { get; protected set; }

        public MOV(DocumentLine cs) : base(cs)
        {

        }



        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            //throw new NotImplementedException();
            //Revisar tamaños de operandos por ejemplo que no se haga: AX,(numero mayor de 16 bits)
            return !(TamañoFuente > TamañoDestino);
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(this.OpCode)
            //    .Append(" ")
            //    .Append(string.Join(string.Empty, Destino.Direccion)).Append(" ");
            //if (Origen is null)
            //{
            //    sb.AppendLine(string.Join(string.Empty, Valor));
            //}
            //else
            //{
            //    sb.AppendLine(string.Join(string.Empty, Origen.Direccion));
            //}
            return sb;
        }
    }
}

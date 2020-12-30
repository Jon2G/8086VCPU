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

namespace Gui.Compilador.Instrucciones.SUB
{
    public abstract class SUB : Instruccion
    {
        public Memoria Fuente { get; protected set; }
        public Memoria Destino { get; protected set; }
        public Tamaños TamañoDestino { get; protected set; }
        public Tamaños TamañoFuente { get; protected set; }
        public SUB(DocumentLine cs) : base(cs)
        {

        }
        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append(this.OpCode)
            //    .Append(" ")
            //    .Append(string.Join(string.Empty, Operador1.Direccion)).Append(" ");
            //if (Operador2 is null)
            //{
            //    sb.AppendLine(string.Join(string.Empty, Valor));
            //}
            //else
            //{
            //    sb.AppendLine(string.Join(string.Empty, Operador2.Direccion));
            //}
            return sb;
        }
    }
}

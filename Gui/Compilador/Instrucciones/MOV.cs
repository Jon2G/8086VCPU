using _8086VCPU;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones
{
    public class MOV : Instruccion
    {
        public IDireccion Origen { get; set; }
        public IDireccion Destino { get; set; }
        public byte[] Valor { get; set; }

        public MOV(DocumentLine cs) : base(ConversorBinario.OpCode(1), cs)
        {
            this.Valor = new byte[ALU.Bits];
        }
        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            //throw new NotImplementedException();
            //Revisar tamaños de operandos por ejemplo que no se haga: AX,(numero mayor de 16 bits)
            return true;
        }

        public override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.OpCode)
                .Append(" ")
                .Append(string.Join(string.Empty, Destino.Direccion)).Append(" ");
            if (Origen is null)
            {
                sb.AppendLine(string.Join(string.Empty, Valor));
            }
            else
            {
                sb.AppendLine(string.Join(string.Empty, Origen.Direccion));
            }
            return sb;
        }
    }
}

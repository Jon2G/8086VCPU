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
    public class Simple : Instruccion
    {
        public string NombreOperador { get; private set; }
        public Localidad Operador { get; protected set; }
        public Tamaños Tamaño { get; protected set; }

        public Simple(string NombreOperador,DocumentLine LineaDocumento, TipoInstruccion tipo) : base(LineaDocumento, tipo)
        {
            this.NombreOperador = NombreOperador;
            this.Tamaño = TamañoRegistro(NombreOperador);
            this.Operador = Registros.PorNombre(NombreOperador);
        }
   

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }

        protected override StringBuilder Traduccion()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("110");
            sb.AppendLine(Registros.OpCode(NombreOperador));
            return sb;

        }
    }
}

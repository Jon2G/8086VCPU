using _8086VCPU.Registros;
using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Memoria;

namespace Gui.Compilador.Instrucciones
{
    public class MUL : Instruccion
    {
        public string NombreOperador { get; private set; }
        public Memoria Operador { get; protected set; }
        public Tamaños Tamaño { get; protected set; }

        public MUL(string NombreOperador, DocumentLine LineaDocumento) : base(LineaDocumento)
        {
            this.NombreOperador = NombreOperador;
            this.Tamaño = TamañoRegistro(NombreOperador);
            this.Operador = Registros.PorNombre(NombreOperador);
        }

        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            throw new NotImplementedException();
        }

        public override StringBuilder Traduccion()
        {
            throw new NotImplementedException();
        }
    }
}

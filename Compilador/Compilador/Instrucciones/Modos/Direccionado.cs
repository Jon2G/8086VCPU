using _8086VCPU;
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
    public abstract class Direccionado : Instruccion
    {
        public Localidad Fuente { get; protected set; }
        public Localidad Destino { get; protected set; }
        public Tamaños TamañoDestino { get; protected set; }
        public Tamaños TamañoFuente { get; protected set; }
        public Direccionado(DocumentLine cs, TipoInstruccion tipo) : base(cs, tipo)
        {

        }
        public override bool RevisarSemantica(ResultadosCompilacion Errores)
        {
            return true;
        }
    }
}

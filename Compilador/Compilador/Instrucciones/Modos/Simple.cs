using _8086VCPU.Alu;
using _8086VCPU.Registros;
using Gui.Advertencias;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Localidad;

namespace Gui.Compilador.Instrucciones.Modos
{
    public class Simple : Direccionamiento
    {
        public string NombreOperador { get; private set; }
        public Localidad Operador { get; protected set; }
        public Tamaños Tamaño { get; protected set; }

        protected override Regex ExpresionRegular => throw new NotImplementedException();
        public Simple():base()
        {

        }
        public Simple(string NombreOperador, LineaLexica LineaDocumento, TipoInstruccion tipo) : base(LineaDocumento, tipo)
        {
            this.NombreOperador = NombreOperador;
            this.Tamaño = TamañoRegistro(NombreOperador);
            this.Operador = Registros.PorNombre(NombreOperador);
        }

        protected override Instruccion EsValida(LineaLexica linea, ResultadosCompilacion Errores, TipoInstruccion tipo)
        {
            throw new NotImplementedException();
        }

        protected override StringBuilder Traducir(CodeSegment code)
        {
            throw new NotImplementedException();
        }
    }
}

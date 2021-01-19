using _8086VCPU.Alu;
using Gui.Advertencias;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones
{
    internal class Etiqueta : Instruccion
    {
        public string Identificador;
        public Etiqueta(string Identificador, LineaLexica LineaDocumento, TipoInstruccion Tipo) : base(LineaDocumento, Tipo)
        {
            this.Identificador = Identificador;
        }
        //protected override StringBuilder Traduccion(CodeSegment code)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine($"11010000 ;{Identificador}");
        //    return sb;
        //}

        protected override StringBuilder Traducir(CodeSegment code)
        {
            return new StringBuilder();
        }
    }
}

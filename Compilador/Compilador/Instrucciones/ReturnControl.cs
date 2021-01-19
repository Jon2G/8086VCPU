using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones
{
    public class ReturnControl : Instruccion
    {
        public ReturnControl(LineaLexica LineaDocumento, TipoInstruccion Tipo) : base(LineaDocumento, Tipo)
        {
        }

        //protected override StringBuilder Traduccion(CodeSegment code)
        //{
        //    return new StringBuilder().AppendLine("11111000 ;RETURN CONTROL");
        //}

        protected override StringBuilder Traducir(CodeSegment code)
        {
            return new StringBuilder();
        }
    }
}

using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones
{
    public class Begin : Instruccion
    {
        public Begin(LineaLexica LineaDocumento, TipoInstruccion Tipo) : base(LineaDocumento, Tipo)
        {

        }

        protected override StringBuilder Traducir(CodeSegment code)
        {
            return new StringBuilder();
        }
    }
}

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
        public Begin(DocumentLine LineaDocumento, TipoInstruccion Tipo) : base(LineaDocumento, Tipo)
        {

        }

        protected override StringBuilder Traduccion(CodeSegment code)
        {
            return new StringBuilder().AppendLine("11100000 ;Inicia segmento de código");
        }
    }
}

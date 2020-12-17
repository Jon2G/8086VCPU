using Gui.Advertencias;
using Gui.Compilador.Fases;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Instrucciones
{
    public abstract class Instruccion
    {
        protected readonly string OpCode;
        //protected readonly CodeSegment CodeSegment;
        public readonly DocumentLine LineaDocumento;
        protected Instruccion(string OpCode, DocumentLine LineaDocumento)
        {
            this.OpCode = OpCode;
            //this.CodeSegment = Programa;
            this.LineaDocumento = LineaDocumento;
        }

        //protected void HacerReferencia(Token tk)
        //{
        //    //Variable variable = this.CodeSegment.SegmentoDeDatos.ObtenerVariable(tk.Lexema);
        //    //variable?.HacerReferencia();
        //}
        public abstract bool RevisarSemantica(ResultadosCompilacion Errores);
        public abstract StringBuilder Traduccion();
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.GetType().Name);
            return sb.ToString();
        }
    }
}

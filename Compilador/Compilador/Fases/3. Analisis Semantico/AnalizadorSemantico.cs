using Gui.Advertencias;
using Gui.Compilador.Fases._2._Analisis_Sintactico;
using Gui.Compilador.Instrucciones;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases._3._Analisis_Semantico
{
    public class AnalizadorSemantico : Analizador
    {
        internal readonly CodeSegment CodeSegment;
        public AnalizadorSemantico(AnalizadorSintactico Sintactico, TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.CodeSegment = Sintactico.CodeSegment;
        }

        public override void Analizar()
        {
            this.EsValido = true;

            for (int i = 0; i < CodeSegment.Instrucciones.Count; i++)
            {
                Instruccion Accion = CodeSegment.Instrucciones[i];
                if (!Accion.RevisarSemantica(this.Errores))
                {
                    Errores.ResultadoCompilacion("Error semantico", Accion.LineaDocumento);
                    this.EsValido = false;
                    break;
                }
            }
        }
    }
}

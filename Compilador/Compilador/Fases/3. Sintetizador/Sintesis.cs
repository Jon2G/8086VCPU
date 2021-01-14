using Gui.Advertencias;
using Gui.Compilador.Fases._2._Analisis_Sintactico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases._3._Sintetizador
{
    public class Sintesis : Analizador
    {
        public readonly CodeSegment CodeSegment;
        public Sintesis(AnalizadorSintactico Semantico, TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.CodeSegment = Semantico.CodeSegment;
        }
        public override void Analizar()
        {
            this.EsValido = true;
            //for (var index = 0; index < CodeSegment.SegmentoDeDatos.Variables.Count; index++)
            //{
            //    Variable variable = Programa.SegmentoDeDatos.Variables[index];
            //    if (variable.Referencias <= 0)
            //    {
            //        Errores.ResultadoCompilacion($"La variable '{variable.Nombre}' se declara pero nunca se utiliza.",
            //            variable.LineaDocumento, true);
            //        Programa.SegmentoDeDatos.Variables.Remove(variable);
            //    }
            //}
        }


    }
}

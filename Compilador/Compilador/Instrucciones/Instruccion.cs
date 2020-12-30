using Gui.Advertencias;
using Gui.Compilador.Fases;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _8086VCPU.Registros.Memoria;

namespace Gui.Compilador.Instrucciones
{
    public abstract class Instruccion
    {
        //protected readonly CodeSegment CodeSegment;
        public readonly DocumentLine LineaDocumento;
        protected Instruccion(DocumentLine LineaDocumento)
        {
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
        protected static Tamaños TamañoRegistro(string NombreRegistro)
        {
            if (NombreRegistro == "SI" || NombreRegistro == "DI" || NombreRegistro.EndsWith("X"))
            {
                return Tamaños.Palabra;
            }
            else
            {
                return Tamaños.Byte;
            }
        }
        public bool[] GetOpCode()
        {
            //switch (instruccion)
            //{
            //    case MOV _:
            //        return new bool[4] { false, false, false, true };
            //    case ADD _:
            //        return new bool[4] { false, false, true, false };
            //    case SUB _:
            //        return new bool[4] { false, false, true, true };
            //}
            return new bool[4];
        }

    }

}
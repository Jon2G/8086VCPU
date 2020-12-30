using Gui.Compilador.Fases;
using Gui.Compilador.Instrucciones;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador
{
    public class CodeSegment
    {
        //public SegmentoDeDatos SegmentoDeDatos;
        public readonly List<Instruccion> Instrucciones;
        public readonly ExpresionesRegulares Expresiones;
        public CodeSegment(ExpresionesRegulares Expresiones)
        {
            Instrucciones = new List<Instruccion>();
            this.Expresiones = Expresiones;
        }
        public void AgregarInstruccion(Instruccion accion)
        {
            Instrucciones.Add(accion);
        }

    }
}

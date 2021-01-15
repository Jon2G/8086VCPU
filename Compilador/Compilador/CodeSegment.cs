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
        public readonly Dictionary<string, string> Etiquetas;
        public bool TieneInicio { get; private set; }
        public bool TieneFin { get; private set; }
        public CodeSegment(ExpresionesRegulares Expresiones)
        {
            Instrucciones = new List<Instruccion>();
            this.Expresiones = Expresiones;
            this.Etiquetas = new Dictionary<string, string>();
        }
        public void AgregarInstruccion(Instruccion accion)
        {
            Instrucciones.Add(accion);
        }
        public void AgregarEtiqueta(string Etiqueta, string DireccionMemoria)
        {
            Etiquetas.Add(Etiqueta, DireccionMemoria);
        }

        internal void Validar()
        {
            this.TieneFin = this.Instrucciones.OfType<ReturnControl>().Any();
            this.TieneInicio = this.Instrucciones.OfType<Begin>().Any();
        }
    }
}

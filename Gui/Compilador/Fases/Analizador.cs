using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases
{
    public abstract class Analizador
    {
        protected readonly TextDocument Documento;
        public bool EsValido { get; protected set; }
        protected readonly ResultadosCompilacion Errores;
        protected Analizador(TextDocument Documento, ResultadosCompilacion Errores)
        {
            this.Documento = Documento;
            this.EsValido = false;
            this.Errores = Errores;
        }
        public abstract void Analizar();
        public virtual void Analizar(List<Tuple<string, DocumentLine>> Lineas)
        {

        }
    }
}

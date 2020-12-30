using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;

namespace Gui.Advertencias
{
    public class ErrorCompilacion : Exception
    {
        public bool EsAdvertencia { get; internal set; }
        public string Texto { get; internal set; }
        public DocumentLine Linea { get; internal set; }

        public ErrorCompilacion(bool EsAdvertencia, string Texto, DocumentLine Linea) : base(Texto)
        {
            this.EsAdvertencia = EsAdvertencia;
            this.Texto = Texto;
            this.Linea = Linea;

        }
    }
}

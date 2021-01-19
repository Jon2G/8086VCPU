using Gui.Compilador.Fases._1._Analisis_Lexico;
using ICSharpCode.AvalonEdit.Document;
using System.Text;
namespace Gui.Compilador.Instrucciones
{
    internal class Salto : Instruccion
    {
        public string Etiqueta { get; set; }
        public Salto(string Etiqueta, LineaLexica Linea, TipoInstruccion Tipo) : base(Linea, Tipo)
        {
            this.Etiqueta = Etiqueta;
        }

        protected override StringBuilder Traducir(CodeSegment code)
        {
            StringBuilder sb = new StringBuilder();
            if (!code.Etiquetas.ContainsKey(this.Etiqueta))
            {
                //Kit.Services.CustomMessageBox.Current.Show($"No se definio la etiqueta: [{this.Etiqueta}]", "Alerta", Kit.Enums.CustomMessageBoxButton.OK, Kit.Enums.CustomMessageBoxImage.Error);
                return sb.AppendLine(";ERROR");
            }
            sb.AppendLine(code.Etiquetas[this.Etiqueta]);
            return sb;
        }
    }
}

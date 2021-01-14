using ICSharpCode.AvalonEdit.Document;
using System.Text;
namespace Gui.Compilador.Instrucciones
{
    internal class Salto : Instruccion
    {
        public string Etiqueta { get; set; }
        public Salto(string Etiqueta, DocumentLine LineaDocumento, TipoInstruccion Tipo) : base(LineaDocumento, Tipo)
        {
            this.Etiqueta = Etiqueta;
        }
        protected override StringBuilder Traduccion(CodeSegment code)
        {
            StringBuilder sb = new StringBuilder();
            switch (this.Tipo)
            {
                case TipoInstruccion.JMP:
                    sb.Append("01110");
                    break;
                case TipoInstruccion.JZ:
                    sb.Append("01111");
                    break;
                case TipoInstruccion.JE:
                    sb.Append("10000");
                    break;
                case TipoInstruccion.JNZ:
                    sb.Append("10001");
                    break;
                case TipoInstruccion.JNE:
                    sb.Append("10010");
                    break;
                case TipoInstruccion.JC:
                    sb.Append("10011");
                    break;
                case TipoInstruccion.JA:
                    sb.Append("10100");
                    break;
                case TipoInstruccion.JAE:
                    sb.Append("10101");
                    break;
                case TipoInstruccion.JLE:
                    sb.Append("10110");
                    break;
                case TipoInstruccion.JO:
                    sb.Append("10111");
                    break;
                case TipoInstruccion.JNS:
                    sb.Append("11000");
                    break;
                case TipoInstruccion.JNO:
                    sb.Append("11001");
                    break;
                case TipoInstruccion.JL:
                    sb.Append("11011");
                    break;
            }
            sb.AppendLine($"111 ;{this.Tipo}");
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

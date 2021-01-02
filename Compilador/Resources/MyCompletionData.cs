using Gui.Views;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gui.Resources
{
    public class MyCompletionData : ICompletionData
    {
        public MyCompletionData(string Token, string Completado, string Descripcion, int Orden, string ImgSource, AutoCompletado AutoCompletado)
        {
            this.Text = Completado;
            this.Descripcion = Descripcion;
            this.Orden = (double)Orden;
            this.ImgSource = ImgSource;
            this.ElementoAutoCompletado = new ElementoAutoCompletado(Token);
            this.AutoCompletado = AutoCompletado;
        }

        private readonly ElementoAutoCompletado ElementoAutoCompletado;
        public readonly string ImgSource;
        public System.Windows.Media.ImageSource Image
        {
            get => null;

        }

        public string Text { get; private set; }

        // Use this property if you want to show a fancy UIElement in the drop down list.
        public object Content => ElementoAutoCompletado;

        private readonly string Descripcion;
        public object Description
        {
            get => Descripcion;
        }

        private readonly double Orden;
        public double Priority
        {
            get => (double)Orden;
        }
        private readonly AutoCompletado AutoCompletado;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            this.AutoCompletado.DeberiaAnalizar = true;
            textArea.Document.Replace(completionSegment, this.Text);
            this.AutoCompletado.AutoCompletar();

            //switch (this.ImgSource)
            //{
            //    case "Imgs\\AddVariable_16x.png":

            //        break;
            //    case "/Gui;component/Images/Method_left_16x.png":
            //        this.AutoCompletado.AutoCompletar(contexto);
            //        break;
            //}

            //switch (this.ImgSource)
            //{
            //    case "Imgs\\AddVariable_16x.png":
            //        contexto = textArea.Document.GetText(linea);
            //        tipo = this.AutoCompletado.Registros.FirstOrDefault(x => x.StartsWith(contexto.Trim(), StringComparison.OrdinalIgnoreCase));
            //        if (tipo != null)
            //        {
            //            textArea.Document.Replace(linea.Offset, linea.Length, Regex.Replace(contexto, tipo, tipo, RegexOptions.IgnoreCase));
            //        }
            //        break;
            //    case "/Gui;component/Images/Method_left_16x.png":
            //        contexto = textArea.Document.GetText(linea);
            //        tipo = this.AutoCompletado.OperacionesAritmeticas.FirstOrDefault(x => contexto.ToLower().Trim().Contains(x.ToLower()));
            //        if (tipo != null)
            //        {
            //            textArea.Document.Replace(linea.Offset, linea.Length, Regex.Replace(contexto, tipo, tipo, RegexOptions.IgnoreCase));
            //        }
            //        this.AutoCompletado.AutoCompletar();
            //        break;
            //}
            //GetText(completionSegment.Offset- this.Text.Length, this.Text.Length);
            //DocumentLine linea = this.Documento.GetLineByNumber((s as CompletionWindow).TextArea.Caret.Line);

        }
    }
}

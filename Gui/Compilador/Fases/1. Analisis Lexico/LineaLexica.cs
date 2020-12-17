using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases._1._Analisis_Lexico
{
    public class LineaLexica
    {
        public List<Token> Tokens { get; private set; }
        public int Elementos => Tokens.Count;
        public Token this[int index] => Tokens[index];
        public DocumentLine LineaDocumento { get; private set; }
        public readonly string Texto;
        public LineaLexica(DocumentLine LineaDocumento,string Texto)
        {
            this.Texto = Texto;
            this.Tokens = new List<Token>();
            this.LineaDocumento = LineaDocumento;
        }
        internal void Agregar(Token token)
        {
            this.Tokens.Add(token);
        }
        internal bool Remover(Token token)
        {
            return this.Tokens.Remove(token);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.Tokens.ForEach(tk =>
            {
                sb.Append(tk.Lexema);
                sb.Append(" ");
            });
            return sb.ToString();
        }
    }
}

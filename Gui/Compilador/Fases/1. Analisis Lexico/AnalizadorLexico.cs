using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Compilador.Fases._1._Analisis_Lexico
{
    public class AnalizadorLexico : Analizador
    {
        public readonly List<LineaLexica> LineasLexicas;
        public bool FinLineas { get; private set; }
        internal ExpresionesRegulares Expresiones;
        public readonly List<Token> Simbolos;
        public AnalizadorLexico(TextDocument Documento, ResultadosCompilacion Errores) : base(Documento, Errores)
        {
            this.FinLineas = false;
            this.LineasLexicas = new List<LineaLexica>();
            this.Simbolos = new List<Token>();
            this.Expresiones = new ExpresionesRegulares();
        }
        public override void Analizar()
        {
            Token token = null;
            this.EsValido = true;
            foreach (DocumentLine linea in this.Documento.Lines)
            {
                string texto = this.Documento.GetText(linea).Trim();
                //Remover comentarios
                texto = Expresiones.Comentarios.Replace(texto, "").Trim();
                if (string.IsNullOrEmpty(texto))
                {
                    continue;
                }
                LineaLexica LLex = new LineaLexica(linea, texto);
                string[] palabras =
                    Expresiones.Documento
                        .Split(texto)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                foreach (string palabra in palabras)
                {
                    token = Token.Identificar(palabra, linea, Expresiones);
                    if (token != null)
                    {
                        if (token.EsValido)
                        {
                            this.Simbolos.Add(token);
                            LLex.Agregar(token);
                        }
                        else
                        {
                            this.Errores.ResultadoCompilacion(
                                $"El nombre '{token.Lexema}' no existe en el contexto actual", linea);
                            this.EsValido = false;
                        }
                    }
                }
                this.LineasLexicas.Add(LLex);
            }
        }

        public override void Analizar(List<Tuple<string, DocumentLine>> Lineas)
        {
            Token token = null;
            this.EsValido = true;

            foreach (Tuple<string, DocumentLine> linea in Lineas)
            {
                string texto = linea.Item1;
                //Remover comentarios
                texto = Expresiones.Comentarios.Replace(texto, "").Trim();
                if (string.IsNullOrEmpty(texto))
                {
                    continue;
                }
                LineaLexica LLex = new LineaLexica(linea.Item2, texto);
                string[] palabras =
                    Expresiones.Documento
                        .Split(texto)
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                foreach (string palabra in palabras)
                {
                    token = Token.Identificar(palabra, linea.Item2, Expresiones);
                    if (token != null)
                    {
                        if (token.EsValido)
                        {
                            this.Simbolos.Add(token);
                            LLex.Agregar(token);
                        }
                        else
                        {
                            this.Errores.ResultadoCompilacion(
                                $"El nombre '{token.Lexema}' no existe en el contexto actual", linea.Item2);
                            this.EsValido = false;
                        }
                    }
                }
                this.LineasLexicas.Add(LLex);
            }



        }
    }
}

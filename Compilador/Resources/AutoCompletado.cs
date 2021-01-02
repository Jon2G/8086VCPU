using Gui.Advertencias;
using Gui.Compilador.Fases;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Threading;
using System.Collections.ObjectModel;
using Gui.Compilador.Fases._1._Analisis_Lexico;
using Gui.Compilador.Fases._2._Analisis_Sintactico;
using Gui.Compilador;
using System.Windows;
using _8086VCPU.Registros;

namespace Gui.Resources
{
    public class AutoCompletado
    {
        private readonly TextDocument Documento;
        private readonly TextArea Texto;
        public CompletionWindow CompletionWindow { get; private set; }
        private readonly ResultadosCompilacion Errores;
        private Analizador Analizador;
        public bool Analizando { get; private set; }
        public bool DeberiaAnalizar { get; set; }
        private CodeSegment CodeSegment;
        ITextMarkerService TextMarkerService;
        public readonly List<string> Registros;
        public readonly List<string> OperacionesAritmeticas;
        public readonly List<string> OperacionesLogicas;
        public AutoCompletado(TextArea Texto, ResultadosCompilacion Errores)
        {
            this.Registros = new List<string>() { "AX", "AH", "AL", "BX", "BH", "BL", "CX", "CH", "CL", "DX", "DH", "DL", "SI", "DI" };
            this.OperacionesAritmeticas = new List<string>() { "MOV", "ADD", "MUL", "SUB", "DIV" };
            this.OperacionesLogicas = new List<string>() { "NOT", "OR", "NOR", "XOR", "XNOR", "AND", "NAND" };

            this.Texto = Texto;
            this.Documento = this.Texto.Document;
            this.Errores = Errores;
            IniciarMarcadoDeErrores();
        }
        private void IniciarMarcadoDeErrores()
        {
            var textMarkerService = new TextMarkerService(Texto.Document);
            Texto.TextView.BackgroundRenderers.Add(textMarkerService);
            Texto.TextView.LineTransformers.Add(textMarkerService);
            IServiceContainer services = (IServiceContainer)Texto.Document.ServiceProvider.GetService(typeof(IServiceContainer));
            if (services != null)
                services.AddService(typeof(ITextMarkerService), textMarkerService);
            this.TextMarkerService = textMarkerService;
        }
        public async Task Analizar()
        {
            await Task.Yield();
            this.Analizando = true;
            if (this.Documento.TextLength > 0)
            {
                var lineas = this.Documento.Lines.Select(x => new Tuple<string, DocumentLine>(this.Documento.GetText(x), x)).ToList();
                //Thread th = new Thread(() =>
                //{

                this.Errores.Resultados.Clear();
                try
                {
                    this.Analizador =
                        new AnalizadorLexico(this.Documento, this.Errores);
                    this.Analizador.Analizar(lineas);
                    if (this.Analizador.EsValido)
                    {

                        this.Analizador =
                            new AnalizadorSintactico((AnalizadorLexico)this.Analizador, this.Documento, this.Errores);
                        this.Analizador.Analizar();
                        if (this.Analizador.EsValido)
                        {

                        }
                        CodeSegment = ((AnalizadorSintactico)this.Analizador).CodeSegment;
                    }

                    Application.Current?.Dispatcher?.Invoke(() =>
                    {
                        TextMarkerService.RemoveAll(m => true);
                        foreach (var ex in this.Errores.Resultados.Where(x => x.Linea != null && !x.Linea.IsDeleted && x.Linea.LineNumber >= 0))
                        {
                            if (ex.EsAdvertencia)
                            {
                                ResaltarError(ex.Linea, System.Windows.Media.Colors.Olive);
                            }
                            else
                            {
                                ResaltarError(ex.Linea, System.Windows.Media.Colors.Red);
                            }
                        }
                    });
                }
                catch (Exception) { }
                this.Analizando = false;
                //});
                //th.SetApartmentState(ApartmentState.STA);
                //th.Priority = ThreadPriority.Lowest;
                //th.Start();

            }
            else { this.Analizando = false; }

        }
        private void ResaltarError(DocumentLine Linea, System.Windows.Media.Color Color)
        {
            ITextMarker marker = this.TextMarkerService.Create(Linea.Offset, Linea.Length);
            marker.MarkerTypes = TextMarkerTypes.SquigglyUnderline;
            marker.MarkerColor = Color;
        }
        private CompletionWindow InicializarVentana()
        {
            // open code completion after the user has pressed dot:
            this.CompletionWindow = new CompletionWindow(this.Texto)
            {
                MinWidth = 200
            };
            this.CompletionWindow.Closed += delegate
            {
                this.CompletionWindow = null;
            };
            return this.CompletionWindow;
        }
        public void AutoCompletar()
        {
            DocumentLine linea = this.Documento.GetLineByNumber(this.Texto.Caret.Line);
            string contexto = this.Documento.GetText(linea);
            // open code completion after the user has pressed dot:
            InicializarVentana();
            AutoCompletar(contexto);
            if (CompletionWindow.CompletionList.CompletionData.Any())
            {
                this.CompletionWindow.Show();
            }
        }

        private void AutoCompletar(string contexto)
        {
            if (contexto.EndsWith(","))
            {
                foreach (string registro in this.Registros)
                {
                    Regex regex = new Regex(Regex.Escape(contexto), RegexOptions.IgnoreCase);
                    this.CompletionWindow.CompletionList.CompletionData.Add(new MyCompletionData(registro, regex.Replace(registro, "", 1) + " ", $"Registro :{registro}", 0, "/Gui;component/Images/AddVariable_16x.png", this));
                }
                return;
            }

            AutoCompletarRegistros(contexto, this.CompletionWindow.CompletionList.CompletionData);
            AutoCompletarFunciones(contexto, this.CompletionWindow.CompletionList.CompletionData);

            if (!CompletionWindow.CompletionList.CompletionData.Any())
            {
                AutoCompletarFunciones(contexto, this.CompletionWindow.CompletionList.CompletionData);
            }
            if (CompletionWindow.CompletionList.CompletionData.Any())
            {
                this.CompletionWindow.Show();
            }

            //if (this.CodeSegment is null) { return; }
            //if (
            //    contexto.EndsWith("*") ||
            //    contexto.EndsWith("/") ||
            //    contexto.EndsWith("+") ||
            //    contexto.EndsWith("-") ||
            //    contexto.EndsWith(":=")
            //    )
            //{
            //    foreach (Variable variable in this.CodeSegment.SegmentoDeDatos.Variables.Where(x => !x.EsAutomatica && x.TipoDato == TipoDato.Decimal || x.TipoDato == TipoDato.Entero))
            //    {
            //        data.Add(new MyCompletionData(variable.Nombre, variable.Nombre, $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png", this));
            //    }
            //    return;
            //}
            //if (contexto.Length > 2)
            //{
            //    contexto = contexto.Trim();
            //    contexto = contexto.Substring(0, contexto.Length - 1);
            //    contexto = contexto.Trim();
            //}
            //if (contexto.EndsWith("Imprime") || contexto.EndsWith("Lee"))
            //{
            //    foreach (Variable variable in this.CodeSegment.SegmentoDeDatos.Variables.Where(x => !x.EsAutomatica))
            //    {
            //        data.Add(new MyCompletionData(variable.Nombre, variable.Nombre + ");", $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png", this));
            //    }
            //}
            //if (contexto.EndsWith("Si"))
            //{
            //    foreach (Funciones.Variable variable in this.CodeSegment.SegmentoDeDatos.Variables.Where(x => !x.EsAutomatica))
            //    {
            //        if (variable.TipoDato == TipoDato.Entero || variable.TipoDato == TipoDato.Decimal)
            //        {
            //            foreach (string comparacion in new string[] { ":=", ">", "<", ">=", "<=" })
            //            {
            //                data.Add(new MyCompletionData(variable.Nombre + comparacion, variable.Nombre + comparacion, $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png", this));
            //            }
            //        }
            //        else
            //        {
            //            if (variable.TipoDato == TipoDato.Cadena)
            //            {
            //                data.Add(new MyCompletionData(variable.Nombre + "=", variable.Nombre + "=", $"Variable tipo:{variable.TipoDato}", 0, "Imgs\\Property_16x.png", this));
            //            }
            //        }

            //    }
            //}
        }
        private void AutoCompletarRegistros(string contexto, IList<ICompletionData> data)
        {
            contexto = contexto.ToLower();
            IEnumerable<string> registros = this.Registros.Where(x => x.StartsWith(contexto, StringComparison.OrdinalIgnoreCase));
            foreach (string registro in registros)
            {
                Regex regex = new Regex(Regex.Escape(contexto), RegexOptions.IgnoreCase);
                data.Add(new MyCompletionData(registro, regex.Replace(registro, "", 1) + " ", $"Registro :{registro}", 0, "/Gui;component/Images/AddVariable_16x.png", this));
            }

        }
        private void AutoCompletarFunciones(string contexto, IList<ICompletionData> data)
        {
            contexto = contexto.ToUpper();
            Regex regex = new Regex(Regex.Escape(contexto), RegexOptions.IgnoreCase);
            foreach (string tipo in this.OperacionesAritmeticas.Where(x => x.StartsWith(contexto, StringComparison.OrdinalIgnoreCase)))
            {
     
                if (tipo == "DIV" || tipo == "MUL")
                {
                    foreach (string registro in this.Registros)
                    {
                        data.Add(new MyCompletionData(tipo + $" { registro}", regex.Replace(tipo, "", 1) + $" { registro}", $"Función :{tipo}", 0, "/Gui;component/Images/Method_left_16x.png", this));
                    }
                    continue;
                }

                foreach (string registro in this.Registros)
                {
                    data.Add(new MyCompletionData(tipo + $" { registro},", regex.Replace(tipo, "", 1) + $" { registro},", $"Función :{tipo}", 0, "/Gui;component/Images/Method_left_16x.png", this));
                }
            }
            if (!data.Any())
            {
                foreach (string tipo in this.OperacionesAritmeticas.Where(x => x.StartsWith(contexto.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    if (tipo == "DIV" || tipo == "MUL")
                    {
                        foreach (string registro in this.Registros)
                        {
                            data.Add(new MyCompletionData(tipo + $" { registro}", regex.Replace(tipo, "", 1) + $" { registro}", $"Función :{tipo}", 0, "/Gui;component/Images/Method_left_16x.png", this));
                        }
                        continue;
                    }
                    foreach (string registro in this.Registros)
                    {
                        data.Add(new MyCompletionData(tipo + $" { registro},", regex.Replace(tipo, "", 1) + $"{registro},", $"Función :{tipo}", 0, "/Gui;component/Images/Method_left_16x.png", this));
                    }
                }
            }

            foreach (string tipo in this.OperacionesLogicas.Where(x => x.StartsWith(contexto, StringComparison.OrdinalIgnoreCase)))
            {
                if (tipo == "NOT")
                {
                    foreach (string registro in this.Registros)
                    {
                        data.Add(new MyCompletionData(tipo + $" { registro}", regex.Replace(tipo, "", 1) + $" { registro}", $"Función :{tipo}", 0, "/Gui;component/Images/Method_left_16x.png", this));
                    }
                    continue;
                }
                foreach (string registro in this.Registros)
                {
                    data.Add(new MyCompletionData(tipo + $" { registro},", regex.Replace(tipo, "", 1) + $" { registro},", $"Función :{tipo}", 0, "/Gui;component/Images/logical-thinking.png", this));
                }
            }
            if (!data.Any())
            {
                foreach (string tipo in this.OperacionesAritmeticas.Where(x => x.StartsWith(contexto.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    if (tipo == "NOT")
                    {
                        foreach (string registro in this.Registros)
                        {
                            data.Add(new MyCompletionData(tipo + $" { registro}", regex.Replace(tipo, "", 1) + $" { registro}", $"Función :{tipo}", 0, "/Gui;component/Images/Method_left_16x.png", this));
                        }
                        continue;
                    }
                    foreach (string registro in this.Registros)
                    {
                        data.Add(new MyCompletionData(tipo + $" { registro},", regex.Replace(tipo, "", 1) + $"{registro},", $"Función :{tipo}", 0, "/Gui;component/Images/Method_left_16x.png", this));
                    }
                }
            }
        }
    }
}

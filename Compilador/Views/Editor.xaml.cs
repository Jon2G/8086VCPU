using Gui.Advertencias;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using Kit;
using Microsoft.Win32;
using SQLHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gui.Views
{    /// <summary>
    /// Lógica de interacción para Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        //private readonly AutoCompletado AutoCompletado;
        readonly FoldingManager FoldingManager;
        //readonly BraceFoldingStrategy FoldingStrategy;
        private Gui.Compilador.Compilador Compilador;
        public ResultadosCompilacion Errores { get; set; }

        public Editor()
        {
            //this.FoldingStrategy = new BraceFoldingStrategy();
            this.Errores = new ResultadosCompilacion();

            InitializeComponent();
            this.CmbxEjemplos.ItemsSource = Ejemplos.Ejemplo.ListarEjemplos();
            this.CmbxEjemplos.SelectedItem = this.CmbxEjemplos.ItemsSource.OfType<Ejemplos.Ejemplo>().Last();


            this.FoldingManager = FoldingManager.Install(TxtMy.TextArea);

            //this.FoldingStrategy = new BraceFoldingStrategy();
            //this.FoldingStrategy.UpdateFoldings(this.FoldingManager, TxtMy.Document);


            //this.AutoCompletado = new AutoCompletado(this.TxtMy.TextArea, this.Errores);
            //this.AutoCompletado.Analizar();
            //propertyGridComboBox.SelectedIndex = 2;

            //TxtMy.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("My8086");
            //TxtAsm.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("ASM");


            TxtMy.Load($@"{Tools.Instance.LibraryPath}\..\..\Ejemplos\Movimientos.asm");
            TxtMy.Load($@"{Tools.Instance.LibraryPath}\..\..\Ejemplos\Sumas.asm");
            TxtArchivo.Text = TxtMy.Document.FileName;


            // initial highlighting now set by XAML
            TxtMy.TextArea.TextEntering += TextEditor_TextArea_TextEntering;
            TxtMy.TextArea.TextEntered += TextEditor_TextArea_TextEntered;

            DispatcherTimer foldingUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            foldingUpdateTimer.Tick += FoldingUpdateTimer_Tick;
            foldingUpdateTimer.Start();
            ///
            //TxtMy.TextArea.TextView.BackgroundRenderers.Add(new HighLight(TxtMy));
            /////

            this.Compilador = new Gui.Compilador.Compilador(TxtMy.TextArea.Document, this.Errores);
        }


        private void VerLinea(object sender, EventArgs e)
        {
            int linea = (sender as DocumentLine)?.LineNumber ?? -1;
            if (linea > 0)
            {
                var l = this.TxtMy.TextArea.Document.GetLineByNumber(linea);
                SelectText(l.Offset, l.Length);
                this.TxtMy.ScrollToLine(linea);
            }
        }
        void OpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { CheckFileExists = true };
            if (dlg.ShowDialog() ?? false)
            {
                this.Compilador.Compilado = false;
                TxtMy.Document.FileName =
                TxtArchivo.Text = dlg.FileName;
                TxtMy.Load(dlg.FileName);
                //TxtMy.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(dlg.FileName));
            }
        }
        void SaveFileClick(object sender, EventArgs e)
        {
            if (TxtMy.Document.FileName == null)
            {
                SaveFileDialog dlg = new SaveFileDialog()
                {
                    Filter = "Archivos de código (*.my86)|*.my86"
                };
                dlg.DefaultExt = ".my86";
                if (dlg.ShowDialog() ?? false)
                {
                    TxtArchivo.Text =
                    TxtMy.Document.FileName = dlg.FileName;
                }
                else
                {
                    return;
                }
            }

            try
            {
                TxtMy.Save(TxtMy.Document.FileName);

            }
            catch (Exception ex)
            {
                //Log.LogMe(ex);
            }
        }
        void TextEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            this.Compilador.Compilado = false;
            //if (e.Text == "(" || e.Text == "*" || e.Text == "/" || e.Text == "+" || e.Text == "-" || e.Text == "=")
            //{
            //    this.AutoCompletado.AutoCompletar();
            //}
            //else
            //{
            //    this.AutoCompletado.AutoCompletarPalabrasReservadas();
            //}
        }
        void TextEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            //if (e.Text.Length > 0 && AutoCompletado.CompletionWindow != null)
            //{
            //    if (!char.IsLetterOrDigit(e.Text[0]))
            //    {
            //        // Whenever a non-letter is typed while the completion window is open,
            //        // insert the currently selected element.
            //        AutoCompletado.CompletionWindow.CompletionList.RequestInsertion(e);
            //    }
            //}
            // do not set e.Handled=true - we still want to insert the character that was typed
        }
        void FoldingUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (TxtMy.Document != null)
            {
                this.ProgresoCompilacion.IsIndeterminate = true;
                //    FoldingStrategy.UpdateFoldings(FoldingManager, TxtMy.Document);
                //    if (!this.AutoCompletado.Analizando)
                //    {
                //        //Dispatcher.BeginInvoke(new Action(() =>
                //        //{
                //        this.AutoCompletado.Analizar();
                //        this.ErroresList.ItemsSource = this.Errores.Resultados;
                //        this.ErroresList.Items.Refresh();
                //        //}));
                //        this.ProgresoCompilacion.IsIndeterminate = false;
                //    }
            }

        }
        private void Ejecutar(object sender, RoutedEventArgs e)
        {
            if (!this.Compilador.Compilado)
            {
                if (MessageBox.Show("Debe compilar el código antes de poder ejecutarlo.\n¿Desea compilarlo ahora?",
                        "Compilar código", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Compilar(sender, e);
                    if (Compilador.Compilado)
                    {
                        Ejecutar(sender, e);
                    }
                }
            }
            else
            {
                this.ProgresoCompilacion.IsIndeterminate = true;
                Salida.Text = Compilador.Ejecutar();
            }
        }

        private void Compilar(object sender, RoutedEventArgs e)
        {
            if (TxtMy.Document.FileName != null)
            {
                SaveFileClick(sender, e);
            }

            this.Compilador = new Gui.Compilador.Compilador(this.TxtMy.TextArea.Document, this.Errores);
            Compilador.OnProgreso += (o, i) =>
            {
                //this.ProgresoCompilacion.SetPercent(Compilador.Progreso);
            };
            this.Salida.Text = Compilador.Compilar();
            // TxtAsm.Text = Compilador.CodigoMaquina?.ToString();

            if (!this.Errores.Resultados.Any())
            {
                TabErrores.SelectedIndex = 1;
            }
            else
            {
                TabErrores.SelectedIndex = 0;
            }
            this.Salida.Text += string.Join("\n", this.Errores.Resultados
                .Select(x => $"->[{x.Linea}] " + x.Texto));

        }
        private void SelectText(int offset, int length)
        {
            try
            {
                //Get the line number based off the offset.
                var line = TxtMy.Document.GetLineByOffset(offset);
                var lineNumber = line.LineNumber;

                //Select the text.
                TxtMy.SelectionStart = offset;
                TxtMy.SelectionLength = length;

                //Scroll the textEditor to the selected line.
                var visualTop = TxtMy.TextArea.TextView.GetVisualTopByDocumentLine(lineNumber);
                TxtMy.ScrollToVerticalOffset(visualTop);
            }
            catch (Exception ex)
            {
                Log.LogMe(ex);
            }
        }

        private void CompilarYEjecutar(object sender, RoutedEventArgs e)
        {
            Compilar(sender, e);
            if (Compilador.Compilado)
            {
                Ejecutar(sender, e);
            }
        }

        private void CmbxEjemplos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ejemplos.Ejemplo ex = (CmbxEjemplos.SelectedItem as Ejemplos.Ejemplo);
            TxtMy.Document.Text = ex.GetDocumento();
            TxtMy.Document.FileName = null;
            TxtArchivo.Text = ex.Nombre;

        }
    }
}

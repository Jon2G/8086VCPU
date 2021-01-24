using Gui.Advertencias;
using Gui.Resources;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using Kit;
using Microsoft.Win32;
using Prism.Regions;
using Kit.Sql;
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
    public partial class Editor : NavigationUserControl
    {
        private readonly AutoCompletado AutoCompletado;
        private Gui.Compilador.Compilador Compilador;
        public ResultadosCompilacion Errores { get; set; }
        public Editor(IRegionManager RegionManager) : base(RegionManager)
        {

            this.Errores = new ResultadosCompilacion();

            InitializeComponent();
            this.CmbxEjemplos.ItemsSource = Ejemplos.Ejemplo.ListarEjemplos();
            //this.CmbxEjemplos.SelectedItem = this.CmbxEjemplos.ItemsSource.OfType<Ejemplos.Ejemplo>().Last();




            //this.AutoCompletado = new AutoCompletado(this.TxtMy.TextArea, this.Errores);
            //this.AutoCompletado.Analizar();


            //TxtMy.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("My8086");
            //TxtAsm.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("ASM");


            ////TxtMy.Load($@"{Tools.Instance.LibraryPath}\..\..\Ejemplos\Movimientos.asm");
            //TxtMy.Load($@"{Tools.Instance.LibraryPath}\..\..\Ejemplos\Burbuja.asm");
            //TxtArchivo.Text = TxtMy.Document.FileName;


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
            TxtMy.TextArea.TextView.BackgroundRenderers.Add(new HighLight(TxtMy));
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
        void InfoClick(object sender, RoutedEventArgs e)
        {
            AboutUs aboutUs = new AboutUs()
            {
                Owner = Window.GetWindow(this)
            };
            aboutUs.ShowDialog();
        }
        void OpenFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { CheckFileExists = true };
            dlg.Filter = "Archivos de código (*.my86)|*.my86";
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
                Log.LogMe(ex);
            }
        }
        void TextEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            this.Compilador.Compilado = false;
            if (e.Text == "\n")
            {
                return;
            }
            if(this.AutoCompletado is null) { return; }
            this.AutoCompletado.AutoCompletar();
            AutoCompletado.DeberiaAnalizar = true;
        }
        void TextEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && AutoCompletado?.CompletionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    AutoCompletado.CompletionWindow.CompletionList.RequestInsertion(e);
                    AutoCompletado.DeberiaAnalizar = true;
                }
            }
            // do not set e.Handled=true - we still want to insert the character that was typed
        }
        async void FoldingUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (TxtMy.Document != null)
            {
                if(this.AutoCompletado is null) { return; }
                if (!this.AutoCompletado.Analizando && this.AutoCompletado.DeberiaAnalizar)
                {
                    await this.AutoCompletado.Analizar();
                    AutoCompletado.DeberiaAnalizar = false;
                }
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
                if (!this.Compilador.PuedeEjecutar())
                {
                    return;
                }

                this.Push<Ejecutar>(new NavigationParameters
                {
                    { "Ejecucion", this.Compilador.Ejecutar() }
                });
            }
        }

        private async void Compilar(object sender, RoutedEventArgs e)
        {
            await _Compilar();
        }
        private async Task _Compilar()
        {
            if (TxtMy.Document.FileName != null)
            {
                SaveFileClick(this, EventArgs.Empty);
            }

            this.Compilador = new Gui.Compilador.Compilador(this.TxtMy.TextArea.Document, this.Errores);

            this.Salida.Text = await Compilador.Compilar();
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

        private async void CompilarYEjecutar(object sender, RoutedEventArgs e)
        {
            await _Compilar();
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
            if (this.AutoCompletado != null)
            {
                this.AutoCompletado.DeberiaAnalizar = true;
            }

        }
    }
}

using _8086VCPU.Alu;
using Compilador;
using Gui.Advertencias;
using Gui.Compilador;
using Gui.Resources;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using Kit;
using Microsoft.Win32;
using Prism.Regions;
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
    public partial class Ejecutar : NavigationUserControl
    {
        private Ejecucion Ejecucion;
        private int Linea;
        private DispatcherTimer Timer;
        public Ejecutar(IRegionManager RegionManager) : base(RegionManager)
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
            Linea = 1;
        }
        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            this.Ejecucion = this.GetParameter<Ejecucion>("Ejecucion");
            TxtMy.Text = Ejecucion.CodigoMaquina;
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

        private void Redo_Click(object sender, MouseButtonEventArgs e)
        {
            Timer.Stop();
            Linea = 1;
            Ejecucion.LongitudOperacion = 0;
            Ejecucion.Redo();
            Next_Click(sender, e);
        }
        private void Next_Click(object sender, EventArgs e)
        {
            //POR REGISTRO
            if (Ejecucion.LongitudOperacion > 0)
            {
                Linea += Ejecucion.LongitudOperacion;
            }
            Ejecucion.Siguiente();
            Seleccionar(Ejecucion.LongitudOperacion);


        }
        private void Seleccionar(int LongitudOperacion)
        {
            ICSharpCode.AvalonEdit.Document.DocumentLine line = TxtMy.Document.GetLineByNumber(Linea);

            TxtMy.SelectionStart = line.Offset;

            ICSharpCode.AvalonEdit.Document.DocumentLine lineafin = TxtMy.Document.GetLineByNumber(Linea + LongitudOperacion);
            TxtMy.SelectionLength = lineafin.EndOffset - line.Offset;

            double vertOffset = (TxtMy.TextArea.TextView.DefaultLineHeight) * (line.LineNumber-4);
            if (vertOffset < 0)
            {
                vertOffset = 0;
            }
            TxtMy.ScrollToVerticalOffset(vertOffset);
        }

        private void Run_Click(object sender, MouseButtonEventArgs e)
        {
            Timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Next_Click(sender, e);
        }
    }
}

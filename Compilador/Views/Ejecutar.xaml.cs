using _8086VCPU.Alu;
using _8086VCPU.Auxiliares;
using _8086VCPU.Registros;
using Compilador;
using Gui.Advertencias;
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
        private Ejecucion _Ejecucion;
        public Ejecucion Ejecucion { get => _Ejecucion; set { _Ejecucion = value; OnPropertyChanged(); } }
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

        private void Redo_Click(object sender, EventArgs e)
        {
            Timer.Stop();
            Linea = 1;
            Ejecucion.LongitudOperacion = 0;
            Ejecucion.Redo();
            if (!(sender is bool))
            {
                Next_Click(sender, e);
            }
        }
        private void Volver_Click(object sender, EventArgs e)
        {
            Redo_Click(sender, e);
            this.Push<Editor>();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (Ejecucion.Reiniciar)
            {
                Redo_Click(false, e);
                return;
            }
            //POR REGISTRO
            if (Ejecucion.LongitudOperacion > 0)
            {
                Linea += Ejecucion.LongitudOperacion;
            }
            if (!Ejecucion.Siguiente())
            {
                TxtMy.SelectionStart = 0;
                TxtMy.SelectionLength = 0;
                TxtMy.ScrollToVerticalOffset(0);
                return;
            }
            Seleccionar(Ejecucion.LongitudOperacion);
        }
        private void Seleccionar(int LongitudOperacion)
        {
            ICSharpCode.AvalonEdit.Document.DocumentLine line = TxtMy.Document.GetLineByNumber(Linea);

            TxtMy.SelectionStart = line.Offset;

            ICSharpCode.AvalonEdit.Document.DocumentLine lineafin = TxtMy.Document.GetLineByNumber(Linea + LongitudOperacion - 1);
            TxtMy.SelectionLength = lineafin.EndOffset - line.Offset;

            double vertOffset = (TxtMy.TextArea.TextView.DefaultLineHeight) * (line.LineNumber - 4);
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

        private void VistaRegistro_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Registro registro)
            {
                (new VistaRegistro(registro)
                {
                    Owner = App.Current.MainWindow
                }).ShowDialog();

            }
        }
    }
}

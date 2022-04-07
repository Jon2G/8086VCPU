using _8086VCPU.Auxiliares;
using _8086VCPU.Registros;
using Kit;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Gui.Views
{    /// <summary>
     /// Lógica de interacción para Editor.xaml
     /// </summary>
    public partial class Ejecutar : NavigationUserControl
    {
        private Ejecucion _Ejecucion;
        public Ejecucion Ejecucion { get => _Ejecucion; set { _Ejecucion = value; OnPropertyChanged(); } }

        private DispatcherTimer Timer;
        public Ejecutar(IRegionManager RegionManager) : base(RegionManager)
        {
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(0);
            Timer.Tick += Timer_Tick;

        }
        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
            this.Ejecucion = this.GetParameter<Ejecucion>("Ejecucion");
            TxtMy.Text = Ejecucion.CodigoMaquina;
            Redo_Click(true, EventArgs.Empty);
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            Timer.Stop();
            Ejecucion.Redo();
            Seleccionar(Ejecucion.InstruccionSiguiente.LongitudOperacion);
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

            if (!Ejecucion.Siguiente())
            {
                TxtMy.SelectionStart = 0;
                TxtMy.SelectionLength = 0;
                TxtMy.ScrollToVerticalOffset(0);
                return;
            }
            Seleccionar(this.Ejecucion.InstruccionSiguiente.LongitudOperacion);
        }
        private void Seleccionar(int LongitudOperacion)
        {
            try
            {
                if (this.TxtMy.Document.LineCount < this.Ejecucion.Linea)
                {
                    return;
                }
                ICSharpCode.AvalonEdit.Document.DocumentLine line = TxtMy.Document.GetLineByNumber(this.Ejecucion.Linea);
                if (TxtMy.Document.TextLength < line.Offset)
                {
                    return;
                }

                TxtMy.SelectionLength = 0;
                TxtMy.SelectionStart = line.Offset;

                ICSharpCode.AvalonEdit.Document.DocumentLine lineafin;
                if (LongitudOperacion > 0)
                {
                    lineafin = TxtMy.Document.GetLineByNumber(this.Ejecucion.Linea + LongitudOperacion - 1);
                    TxtMy.SelectionLength = lineafin.EndOffset - line.Offset;
                }
                else
                {
                    TxtMy.SelectionStart = line.Offset;
                    TxtMy.SelectionLength = line.Length;
                }


                double vertOffset = (TxtMy.TextArea.TextView.DefaultLineHeight) * (line.LineNumber - 4);
                if (vertOffset < 0)
                {
                    vertOffset = 0;
                }
                TxtMy.ScrollToVerticalOffset(vertOffset);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Seleccionar");
            }
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
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            this.Timer.Stop();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Timer is null)
            {
                return;
            }
            bool running = this.Timer.IsEnabled;
            if (running)
            {
                this.Timer.Stop();
            }
            this.Timer.Interval = TimeSpan.FromSeconds((sender as Slider).Value);
            if (running)
            {
                this.Timer.Start();
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (Memoria.SelectedItem != null)
            //{
            //    Memoria.UpdateLayout();
            //    Memoria.ScrollIntoView(Memoria.SelectedItem);
            //}
        }
    }
}

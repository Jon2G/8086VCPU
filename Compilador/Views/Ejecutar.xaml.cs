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
        public Ejecutar(IRegionManager RegionManager) : base(RegionManager)
        {
            InitializeComponent();
            TxtMy.TextArea.TextView.BackgroundRenderers.Add(new HighLight(TxtMy));
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

        }
        private void Next_Click(object sender, MouseButtonEventArgs e)
        {
            Ejecucion.Siguiente();


        }
        private void Run_Click(object sender, MouseButtonEventArgs e)
        {

        }


    }
}

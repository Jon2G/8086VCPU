using Gui.Resources;
using SQLHelper;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Gui.Views
{
    /// <summary>
    /// Lógica de interacción para ElementoAutoCompletado.xaml
    /// </summary>
    public partial class ElementoAutoCompletado : UserControl
    {
        public ElementoAutoCompletado(string Texto)
        {
            InitializeComponent();
            this.TxtName.Text = Texto;
            this.InvalidateVisual();

            this.Loaded += ElementoAutoCompletado_Loaded;
        }

        private void ElementoAutoCompletado_Loaded(object sender, RoutedEventArgs e)
        {
            MyCompletionData data = (MyCompletionData)this.DataContext;


            this.Imagen.Source = new BitmapImage(new Uri(data.ImgSource, UriKind.Relative));

        }
        public static BitmapImage CreateImage(string path)
        {
            if (File.Exists(path))
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(path);
                myBitmapImage.EndInit();
                return myBitmapImage;
            }
            Log.LogMe($"Imagen no encontrada:{path}");
            return new BitmapImage();
        }
    }
}

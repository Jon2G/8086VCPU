using Gui.Resources;
using Kit;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            Log.Logger.Warning($"Imagen no encontrada:{path}");
            return new BitmapImage();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui.Advertencias
{
    /// <summary>
    /// Lógica de interacción para WarningError.xaml
    /// </summary>
    public partial class WarningError : UserControl
    {
        public WarningError()
        {
            InitializeComponent();
        }
        public event EventHandler VerLinea;

        public static readonly DependencyProperty ExcepcionProperty =
            DependencyProperty.Register(
                "Excepcion", typeof(ErrorCompilacion), typeof(WarningError),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    (o, e) => ((WarningError)o).AuxCambio(e.NewValue as ErrorCompilacion)));
        public ErrorCompilacion Excepcion
        {
            get => GetValue(ExcepcionProperty) as ErrorCompilacion;
            set => SetValue(ExcepcionProperty, value);
        }
        private ErrorCompilacion AuxCambio(ErrorCompilacion value)
        {
            ErrorCompilacion old = Excepcion;
            Excepcion = value;
            OnPropertyChanged(new DependencyPropertyChangedEventArgs(ExcepcionProperty,
                old, Excepcion));
            Cargar();
            return value;
        }
        private void Cargar()
        {
            if (Excepcion.Linea?.IsDeleted ?? false)
            {
                return;
            }
            //this.Img.Source = new BitmapImage(new Uri(Excepcion.EsAdvertencia ? @"/Gui;component/Images/StatusWarning_16x.png" : @"/Gui;component/Images/bug.png"));
            Error.Text = Excepcion.Texto;
            Linea.Text = (Excepcion?.Linea?.LineNumber ?? -1).ToString();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            VerLinea?.Invoke(Excepcion.Linea, e);
        }
    }
}

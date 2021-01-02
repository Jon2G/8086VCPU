using Compilador;
using HandyControl.Data;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Gui.Views
{
    public partial class MainWindow
    {
        private readonly IRegionManager RegionManager;
        public MainWindow(IRegionManager RegionManager)
        {
            this.RegionManager = RegionManager;
            InitializeComponent();
            if (this.RegionManager == null)
            {
                throw new ArgumentNullException(nameof(this.RegionManager));
            }

           Navegar(typeof(Editor));


        }
        public void Navegar(Type ventana)
        {
            this.RegionManager.RegisterViewWithRegion("ContentRegion", ventana);
        }

        #region Change Skin
        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is SkinType tag)
            {
                PopupConfig.IsOpen = false;
                ((App)Application.Current).UpdateSkin(tag);
            }
        }
        #endregion
    }
}

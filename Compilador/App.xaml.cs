using Gui.Resources;
using Gui.Views;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using Prism.Ioc;
using System;
using System.Windows;

namespace Gui
{
    public partial class App
    {


        protected override Window CreateShell()
        {
            ((App)Application.Current).UpdateSkin(SkinType.Dark);
            return Container.Resolve<MainWindow>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            Kit.WPF.Tools.Init();
            Highlighting.Init();
            base.OnStartup(e);
            //var boot = new Bootstrapper();
            //boot.Run();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Editor>(nameof(Editor));
            containerRegistry.RegisterForNavigation<Ejecutar>(nameof(Ejecutar));

        }
        internal void UpdateSkin(SkinType skin)
        {
            SharedResourceDictionary.SharedDictionaries.Clear();
            Resources.MergedDictionaries.Add(ResourceHelper.GetSkin(skin));
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            });
            Current.MainWindow?.OnApplyTemplate();
        }
    }
}

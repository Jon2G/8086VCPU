using Gui.Resources;
using Gui.Views;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using Prism.Ioc;
using System;
using System.Diagnostics;
using System.Windows;

namespace Compilador
{
    public partial class App 
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            Kit.WPF.Tools.Init().InitLoggin(AlertAfterCritical: true);
            SQLHelper.SQLHelper.Init(Kit.Tools.Instance.LibraryPath, Debugger.IsAttached);
            Highlighting.Init();
            base.OnStartup(e);
            //var boot = new Bootstrapper();
            //boot.Run();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Editor>(nameof(Editor));

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

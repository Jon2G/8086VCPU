using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Prism.Ioc;
using System.Windows;
using Prism.DryIoc;
using HandyControl.Themes;
using HandyControl.Tools;
using HandyControl.Data;
using Gui.Resources;

namespace Gui
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Kit.WPF.Tools.Init().InitLoggin(AlertAfterCritical: true);
            SQLHelper.SQLHelper.Init(Kit.Tools.Instance.LibraryPath, Debugger.IsAttached);
            Highlighting.Init();
            base.OnStartup(e);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<LogIn>(nameof(LogIn));
            //containerRegistry.RegisterForNavigation<TestConfirmation>(nameof(TestConfirmation));
            //containerRegistry.RegisterForNavigation<ApplicantRegister>(nameof(ApplicantRegister));
            //containerRegistry.RegisterForNavigation<AdminView>(nameof(AdminView));
            //containerRegistry.RegisterForNavigation<MainTest>(nameof(MainTest));
            //containerRegistry.RegisterForNavigation<TestEnd>(nameof(TestEnd));
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

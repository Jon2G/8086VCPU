using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Gui.Views
{
    public class NavigationUserControl : UserControl, INavigationAware, INotifyPropertyChanged, IJournalAware
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, args);
        }
        #endregion
        private readonly IRegionManager RegionManager;
        private NavigationContext NavigationContext;
        public NavigationUserControl() { }

        protected void Push<T>(Dictionary<string, object> parameters)
        {
            NavigationParameters parames = new NavigationParameters();
            foreach (var value in parameters)
            {
                parames.Add(value.Key, value.Value);
            }
            Push<T>(parames);
        }
        protected void Push<T>(NavigationParameters parameters = null)
        {
            string Name = typeof(T).Name;
            this.RegionManager.Regions["ContentRegion"].NavigationService.Journal.NavigationTarget.
                RequestNavigate(Name, parameters);
        }
        protected bool Pop()
        {
            if (this.RegionManager.Regions["ContentRegion"].NavigationService.Journal.CanGoBack)
            {
                this.RegionManager.Regions["ContentRegion"].NavigationService.Journal.GoBack();
                return true;
            }
            return false;
        }
        protected void PopAll()
        {
            while (this.Pop())
            {
                //;)
            }
        }

        public NavigationUserControl(IRegionManager IRegionManager)
        {
            this.RegionManager = IRegionManager;
        }
        public bool IsNavigationTarget(NavigationContext NavigationContext)
        {
            this.NavigationContext = NavigationContext;
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext NavigationContext)
        {
            this.NavigationContext = NavigationContext;
        }

        public void OnNavigatedTo(NavigationContext NavigationContext)
        {
            this.NavigationContext = NavigationContext;
            OnNavigatedTo();
        }
        protected virtual void OnNavigatedTo() { }
        protected NavigationParameters GetParameters()
        {
            return this.NavigationContext.Parameters;
        }
        protected T GetParameter<T>(string Name)
        {
            return (T)this.NavigationContext.Parameters[Name];
        }

        public bool PersistInHistory()
        {
            return true;
        }
    }
}

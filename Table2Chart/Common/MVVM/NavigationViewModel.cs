using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Table2Chart.Extensions;

namespace Table2Chart.Common.MVVM
{
    public class NavigationViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        public readonly IEventAggregator eventAggregator;

        public NavigationViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        private bool _KeepAlive = false;

        public bool KeepAlive
        {
            get { return _KeepAlive; }
            internal set { _KeepAlive = value; }
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public void UpdateLoading(bool IsOpen)
        {
            eventAggregator.UpdataLoading(new Events.UpdateModel()
            {
                IsOpen = IsOpen
            });
        }
    }
}
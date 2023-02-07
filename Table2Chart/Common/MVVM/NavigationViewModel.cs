using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Table2Chart.Extensions;

namespace Table2Chart.Common.MVVM
{
    public class NavigationViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        public readonly IEventAggregator aggregator;

        public NavigationViewModel(IContainerProvider containerProvider)
        {
            aggregator = containerProvider.Resolve<IEventAggregator>();
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
            aggregator.UpdataLoading(new Events.UpdateModel()
            {
                IsOpen = IsOpen
            });
        }
    }
}
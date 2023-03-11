using Prism.Events;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using Table2Chart.Common;
using Table2Chart.Common.Models;
using Table2Chart.Common.MVVM;

namespace Table2Chart.ViewModels.Settings
{
    /// <summary>
    /// 设定界面的ViewModel
    /// </summary>
    public class SettingsViewModel : NavigationViewModel
    {
        public SettingsViewModel(IEventAggregator eventAggregator, IRegionManager regionManager) : base(eventAggregator)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            CreateMenuBar();
            this.regionManager = regionManager;
            //regionManager.Regions[PrismManager.SettingsRegionName].RequestNavigate("SkinView");
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            SelectedMenuBar ??= _MenuBars.First();

        }

        private ObservableCollection<MenuBar> _MenuBars;
        private readonly IRegionManager regionManager;

        //private DelegateCommand<MenuBar> _NavigateCommand;
        private MenuBar _SelectedMenuBar;

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return _MenuBars; }
            set { _MenuBars = value; RaisePropertyChanged(); }
        }

        public MenuBar SelectedMenuBar
        {
            get { return _SelectedMenuBar; }
            set { SetProperty(ref _SelectedMenuBar, value); NavigateAction(); }
        }

        private void NavigateAction()
        {
            if (SelectedMenuBar == null || string.IsNullOrWhiteSpace(SelectedMenuBar.NameSpace))
                return;
            regionManager.Regions[PrismManager.SettingsRegionName].RequestNavigate(SelectedMenuBar.NameSpace);
        }

        private void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { IconKind = "Cog", Title = "系统设置", NameSpace = "SystemSettingView" });
            MenuBars.Add(new MenuBar() { IconKind = "Palette", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar() { IconKind = "InformationOutline", Title = "更多", NameSpace = "AboutView" });
        }

        //public DelegateCommand<MenuBar> NavigateCommand => _NavigateCommand = _NavigateCommand = new DelegateCommand<MenuBar>((o) =>
        //{
        //    if (o == null || string.IsNullOrWhiteSpace(o.NameSpace))
        //        return;
        //    regionManager.Regions[PrismManager.SettingsRegionName].RequestNavigate(o.NameSpace);
        //});
    }
}
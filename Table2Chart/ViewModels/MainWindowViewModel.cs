using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Table2Chart.Common;
using Table2Chart.Common.Models;

namespace Table2Chart.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        /// <summary>
        /// 左侧抽屉是否打开
        /// </summary>
        private bool _IsLeftDrawerOpen;

        /// <summary>
        /// 左侧导航菜单
        /// </summary>
        private ObservableCollection<MenuBar> _MenuBars;

        /// <summary>
        /// 选中的导航
        /// </summary>
        private MenuBar _SelectedMenuBar;

        /// <summary>
        /// 区域导航日志
        /// </summary>
        private IRegionNavigationJournal journal;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="regionManager"></param>
        public MainWindowViewModel(IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            this.regionManager = regionManager;
            //Configure();
        }

        /// <summary>
        /// 多命令执行
        /// </summary>
        public ICommand ExecuteCommand => new DelegateCommand<string>(parameter =>
        {
            switch (parameter)
            {
                case "GoReaderView":
                    GoReaderView(); break;
                case "GoPlotsView":
                    GoPlotsView(); break;
                case "MenuToggleButtonClick":
                    MenuToggleButtonClick(); break;
                default:
                    break;
            }
        });

        /// <summary>
        /// 向后命令
        /// </summary>
        public ICommand GoBackCommand => new DelegateCommand(() =>
        {
            if (journal != null && journal.CanGoBack)
                journal.GoBack();
        });

        /// <summary>
        /// 向前命令
        /// </summary>
        public ICommand GoForwardCommand => new DelegateCommand(() =>
        {
            if (journal != null && journal.CanGoForward)
                journal.GoForward();
        });

        public bool IsLeftDrawerOpen
        {
            get { return _IsLeftDrawerOpen; }
            set { SetProperty(ref _IsLeftDrawerOpen, value); }
        }

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

        ////对应导航
        //private DelegateCommand<MenuBar> _NavigateCommand;

        //public DelegateCommand<MenuBar> NavigateCommand => _NavigateCommand ?? (_NavigateCommand = new DelegateCommand<MenuBar>((o) =>
        //{
        //    if (o == null || string.IsNullOrWhiteSpace(o.NameSpace))
        //        return;
        //    regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(o.NameSpace, back =>
        //    {
        //        journal = back.Context.NavigationService.Journal;
        //    });
        //}));

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            MenuBars.Add(new MenuBar() { IconKind = "Database", Title = "读取表格", NameSpace = PrismManager.ExcelReaderView });
            MenuBars.Add(new MenuBar() { IconKind = "ChartScatterPlot", Title = "图表", NameSpace = PrismManager.PlotsView });
            MenuBars.Add(new MenuBar() { IconKind = "Cog", Title = "设置", NameSpace = PrismManager.SettingsView });

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(PrismManager.ExcelReaderView, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        /// <summary>
        /// 导航到 图 页面
        /// </summary>
        private void GoPlotsView()
        {
            var v = regionManager.Regions[PrismManager.MainViewRegionName].ActiveViews.FirstOrDefault();
            var t = v.GetType();
            if (t.Name == PrismManager.PlotsView)
                return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(PrismManager.PlotsView, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        /// <summary>
        /// 导航至 读表格页面
        /// </summary>
        private void GoReaderView()
        {
            var v = regionManager.Regions[PrismManager.MainViewRegionName].ActiveViews.FirstOrDefault();
            var t = v.GetType();
            if (t.Name == PrismManager.ExcelReaderView)
                return;

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(PrismManager.ExcelReaderView, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        /// <summary>
        /// 当按钮点击时，检测活动的View，并更新的RadioButton选中状态
        /// </summary>
        private void MenuToggleButtonClick()
        {
            try
            {
                var v = regionManager.Regions[PrismManager.MainViewRegionName].ActiveViews.FirstOrDefault();
                var t = v.GetType();
                foreach (var item in MenuBars)
                {
                    if (item.NameSpace == t.Name)
                    {
                        _SelectedMenuBar = item;//防止触发NavigatAction
                        RaisePropertyChanged(nameof(SelectedMenuBar));
                        break;
                    }
                }
            }
            catch (System.Exception)
            {
            }
            IsLeftDrawerOpen = true;
        }

        /// <summary>
        /// 执行导航
        /// </summary>
        private void NavigateAction()
        {
            if (SelectedMenuBar == null || string.IsNullOrWhiteSpace(SelectedMenuBar.NameSpace))
                return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(SelectedMenuBar.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }
    }
}
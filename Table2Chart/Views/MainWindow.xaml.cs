using MaterialDesignThemes.Wpf;
using Prism.Events;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Table2Chart.Common;
using Table2Chart.Common.Services;
using Table2Chart.Extensions;

namespace Table2Chart.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IEventAggregator aggregator, IDialogHostService dialogHostService, IVariableService variableService, IRegionManager regionManager)
        {
            InitializeComponent();

            //注册等待转圈窗口
            aggregator.Register(arg =>
            {
                //dialogHostService.Dispatcher.BeginInvoke(new Action(() =>
                //{
                dialogHost.IsOpen = arg.IsOpen;//
                if (dialogHost.IsOpen)
                {
                    dialogHost.DialogContent = new ProgressView();
                }
                //}));
            });
            //注册提示消息
            aggregator.RegisterMessage(arg =>
            {
                string message = "error";
                if (arg.Message != null)
                    message = arg.Message;
                snackbar.MessageQueue.Enqueue(message);
                //snackbar.Dispatcher.BeginInvoke(new Action(() => snackbar.MessageQueue.Enqueue(message)));
            }, "Main");

            //最小化
            btnMin.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            //最大化
            btnMax.Click += (s, e) =>
            {
                this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            };
            //退出？
            btnClose.Click += (s, e) =>
            {

                //关闭window
                this.Close();
            };
            //拖动窗口
            borderZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            int clickTimes = 0;//点击两下放大
            borderZone.MouseDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    clickTimes += 1;
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                    timer.Tick += (sender1, e1) => { timer.IsEnabled = false; clickTimes = 0; };
                    timer.IsEnabled = true;
                    if (clickTimes == 2)  //2击就是2下，
                    {
                        clickTimes = 0;
                        this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                    }
                }
            };
            menuBar.SelectionChanged += (s, e) =>
            {
                drawewHost.IsLeftDrawerOpen = false;
            };
            this.Closing += async (s, e) =>
            {
                e.Cancel = true;
                var dialogResult = await dialogHostService.Question("温馨提示", "确认退出？");
                if (dialogResult?.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(PrismManager.SettingsView);
                //退出保存json
                PaletteHelper _paletteHelper = new PaletteHelper();
                var theme = _paletteHelper.GetTheme() as Theme;
                //主题
                JsonConfigHelper.SaveConfig(theme, JsonConfigHelper.ConfigFile.SkinConfig);
                //服务内容
                if (variableService is VariableService variableConfig)
                    JsonConfigHelper.SaveConfig(variableConfig, JsonConfigHelper.ConfigFile.VariableConfig);
                Environment.Exit(0);


            };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            btnMax.Content = this.WindowState == WindowState.Maximized ? new PackIcon() { Kind = PackIconKind.DockWindow } : new PackIcon() { Kind = PackIconKind.WindowMaximize };
        }
    }
}
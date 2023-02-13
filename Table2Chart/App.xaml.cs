using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Table2Chart.Common;
using Table2Chart.Common.Services;
using Table2Chart.ViewModels;
using Table2Chart.ViewModels.Dialogs;
using Table2Chart.ViewModels.OxyProperties;
using Table2Chart.ViewModels.Settings;
using Table2Chart.Views;
using Table2Chart.Views.Dialogs;
using Table2Chart.Views.OxyProperties;
using Table2Chart.Views.Settings;
using static ControlzEx.Standard.NativeMethods;

namespace Table2Chart
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        private string inputPassword = string.Empty;
        private string realPassword = string.Empty;
        private static Mutex appMutex;

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            foreach (var item in e.Args)
            {
                inputPassword = item;
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            realPassword = DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
            CheckMutex(e);
        }

        private void CheckMutex(StartupEventArgs e)
        {

            //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            ////string name = assembly.GetName().Version.ToString();
            //string assemblyName = assembly.GetName().PropertyName.ToString();
            //string filePath = System.Windows.Forms.Application.ExecutablePath;
            //var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(filePath);
            //string ProductName = versionInfo.ProductName;

            Process currentProc = Process.GetCurrentProcess();
            appMutex = new System.Threading.Mutex(true, currentProc.ProcessName, out bool createdNew);
            //if (appMutex.WaitOne(0, false))//判断软件是否已经开启
            if (createdNew)//判断软件是否已经开启
            {
                base.OnStartup(e);
            }
            else
            {
                foreach (Process proc in Process.GetProcessesByName(currentProc.ProcessName))
                {
                    if (proc.Id != currentProc.Id)
                    {
                        //这里不作用。PID是正确的
                        //IntPtr handle = proc.Handle;
                        //这里也能起作用，也是用的是主窗口名称
                        IntPtr handle = proc.MainWindowHandle;
                        //var ffi = Create_FLASHWINFO(handle, FlashWindowFlag.FLASHW_TIMERNOFG, 500, 5000);
                        //FlashWindowEx(ref ffi);
                        ShowWindow(handle, 9);
                        SetForegroundWindow(handle);
                        break;
                    }
                }
                //var hwnd = FindWindow(null, currentProc.ProcessName);//找string的窗口
                ////var fi = User32Api.Create_FLASHWINFO(hwnd, FlashWindowFlag.FLASHW_TIMERNOFG, 1, 2000);
                ////FlashWindowEx(ref fi);
                //// FlashWindow(hwnd, true);//Flash 会有点慢
                //ShowWindow(hwnd, 9);
                //SetForegroundWindow(hwnd);//使用的窗口名称
                //Process.GetCurrentProcess().Kill();
                //App.Current.Shutdown();
                Environment.Exit(0);
            }
        }

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, uint nCmdShow);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("User32.dll", CharSet = CharSet.Unicode, EntryPoint = "FlashWindow")]
        public static extern void FlashWindow(IntPtr hwnd, bool bInvert);

        protected override void OnInitialized()
        {
            PaletteHelper _paletteHelper = new PaletteHelper();
            Theme theme = _paletteHelper.GetTheme() as Theme;//读取设置设置主题
            JsonConfigHelper.ReadConfig(ref theme, JsonConfigHelper.ConfigFile.SkinConfig);

            if (theme != null)
            {
                _paletteHelper.SetTheme(theme);
            }
            else
            {
                //_paletteHelper.ChangePrimaryColor( new System.Windows.Media.Color()) ;
            }

            if (!realPassword.Equals(inputPassword))
            {
                var loginView = Container.Resolve<LoginView>();
                var result = loginView.ShowDialog();
                if (result.Value != true)
                {
                    Application.Current.Shutdown();
                    Environment.Exit(0);
                }
            }
            //将MainWindow上下文作为IConfigureService服务
            var service = Current.MainWindow.DataContext as MainWindowViewModel;
            service?.Configure();
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //注册导航
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<PlotsView, PlotsViewModel>();
            containerRegistry.RegisterForNavigation<ExcelReaderView, ExcelReaderViewModel>();

            containerRegistry.RegisterForNavigation<AboutView, AboutViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<SystemSettingView, SystemSettingViewModel>();
            //注册弹窗，也是导航
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();

            containerRegistry.RegisterForNavigation<AddBarSeriesView, AddBarSeriesViewModel>();
            containerRegistry.RegisterForNavigation<AddPieSeriesView, AddPieSeriesViewModel>();
            containerRegistry.RegisterForNavigation<AddLineSeriesView, AddLineSeriesViewModel>();
            //containerRegistry.RegisterForNavigation<LoginView>();
            //注册对话窗口
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<CalculatorDialogView, CalculatorDialogViewModel>();

            containerRegistry.RegisterDialog<BarSeriesView, BarSeriesViewModel>();
            containerRegistry.RegisterDialog<PieSeriesView, PieSeriesViewModel>();
            containerRegistry.RegisterDialog<ThreeColorLineSeriesView, ThreeColorLineSeriesViewModel>();
            //containerRegistry.RegisterForNavigation<ScatterSeriesView, ScatterSeriesViewModel>();
            //containerRegistry.RegisterForNavigation<LineSeriesDetailPointsView, LineSeriesDetailPointsViewModel>();

            //注册服务
            containerRegistry.Register<IDialogHostService, DialogHostService>();
            //containerRegistry.RegisterSingleton<IVariableService, VariableService>();

            VariableService variableConfig = null;//读取全局变量json
            JsonConfigHelper.ReadConfig(ref variableConfig, JsonConfigHelper.ConfigFile.VariableConfig);
            if (variableConfig != null)
            {
                containerRegistry.RegisterInstance<IVariableService>(variableConfig);
            }
            else
            {
                containerRegistry.RegisterInstance<IVariableService>(new VariableService());
            }
        }

    
        protected override IContainerExtension CreateContainerExtension()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(configure =>
            {
                configure.ClearProviders();
                configure.SetMinimumLevel(LogLevel.Trace);
                configure.AddNLog();
            });
            return new DryIocContainerExtension(new Container(CreateContainerRules())
                .WithDependencyInjectionAdapter(serviceCollection));
        }
    }
}
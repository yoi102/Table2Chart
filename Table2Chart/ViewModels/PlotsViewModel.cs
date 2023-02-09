using DryIoc;
using Gma.System.MouseKeyHook;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using Table2Chart.Common.Enum;
using Table2Chart.Common.Models;
using Table2Chart.Common.Models.MyDataSet;
using Table2Chart.Common.MVVM;
using Table2Chart.Common.Services;
using Table2Chart.Extensions;
using Table2Chart.Common.Models.OxyModels.Series;
using Table2Chart.Views;
using Table2Chart.Views.OxyProperties;
using System.Threading.Tasks;

namespace Table2Chart.ViewModels
{
    public class PlotsViewModel : NavigationViewModel
    {
        #region Constructors

        public PlotsViewModel(Logger<PlotsView> logger,
            IContainerProvider containerProvider,
            IVariableService variableService,
            IDialogService dialogService) : base(containerProvider)
        {
            globalHook = Hook.GlobalEvents();
            globalHook.KeyDown += GlobalHook_KeyDown;
            globalHook.KeyUp += GlobalHook_KeyUp;
            KeepAlive = true;
            dialogHost = containerProvider.Resolve<IDialogHostService>();
            _DataTableInfos = variableService.DataTableInfos;

            this.logger = logger;
            this.variableService = variableService;
            this.dialogService = dialogService;
            if (variableService.TimerIntervalSeconds <= 0 && variableService.TimerIntervalSeconds >= int.MinValue)
            {
                variableService.TimerIntervalSeconds = 5;
            }
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            MyPlotModels = variableService.PlotModels;
        }

        /// <summary>
        ///导航离开，更新服务数据；关闭软件不会触发
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            //variableService.LineSeriesPointsCountLimit = LineSeriesPointsCountLimit;
            dispatcherTimer.Stop();
        }

        /// <summary>
        ///  导航进来，刷新数据。
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            Task.Run(() => UpdatePage());
        }

        private void UpdatePage()
        {
            UpdateLoading(true);

            DispatcherTimer_Tick(null, null);
            if (variableService.TimerIntervalOn)
            {
                dispatcherTimer.Interval = TimeSpan.FromSeconds(variableService.TimerIntervalSeconds);
                dispatcherTimer.Start();
            }
            UpdateLoading(false);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 所读取的表格
        /// </summary>
        private readonly ObservableCollection<DataTableInfo> _DataTableInfos;

        /// <summary>
        /// IDialogHostService 用于 Md 的弹窗服务
        /// </summary>
        private readonly IDialogHostService dialogHost;

        /// <summary>
        /// Prism 的弹窗服务
        /// </summary>
        private readonly IDialogService dialogService;

        /// <summary>
        /// 更新图表用的定时器
        /// </summary>
        private readonly DispatcherTimer dispatcherTimer;

        /// <summary>
        /// 全局键鼠钩子，用于 Ctrl + wheel  缩放
        /// </summary>
        private readonly IKeyboardMouseEvents globalHook;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly Logger<PlotsView> logger;

        /// <summary>
        /// 全局变量
        /// </summary>
        private readonly IVariableService variableService;

        //private int _TimerIntervalSeconds = 5;
        /// <summary>
        /// 左侧抽屉是否弹出
        /// </summary>
        private bool _IsLeftDrawerOpen;

        /// <summary>
        /// 所有的 PlotModel
        /// </summary>
        private ObservableCollection<MyPlotModel> _MyPlotModels = new ObservableCollection<MyPlotModel>();

        /// <summary>
        /// 是否允许滚轮缩放 PlotView
        /// </summary>
        private bool _PlotViewWheelEnabled;

        /// <summary>
        /// 定时器运行时，是否重置 PlotView 的缩放
        /// </summary>
        public bool CanTimerResetPlot
        {
            get { return variableService.CanTimerResetPlot; }
            set
            {
                if (value != variableService.CanTimerResetPlot)
                {
                    variableService.CanTimerResetPlot = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// 左侧抽屉出否弹出
        /// </summary>
        public bool IsLeftDrawerOpen
        {
            get { return _IsLeftDrawerOpen; }
            set
            {
                SetProperty(ref _IsLeftDrawerOpen, value);
            }
        }

        /// <summary>
        /// 图表集合
        /// </summary>
        public ObservableCollection<MyPlotModel> MyPlotModels
        {
            get { return _MyPlotModels; }
            set
            {
                SetProperty(ref _MyPlotModels, value);
                try
                {
                    //UpdateTables();
                    foreach (var plotModel in MyPlotModels)
                    {
                        ResetPlotModel(plotModel);
                        plotModel.ResetMyModel();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "MyPlotModels-Error!");
                }
            }
        }

        /// <summary>
        /// 高
        /// </summary>
        public double PlotViewHeight
        {
            get { return variableService.PlotViewHeight; }
            set
            {
                variableService.PlotViewHeight = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 布局列数量
        /// </summary>
        public int PlotViewsColumnsCount
        {
            get { return variableService.PlotViewsColumnsCount; }
            set
            {
                variableService.PlotViewsColumnsCount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 是否允许滚轮缩 PlotView
        /// </summary>
        public bool PlotViewWheelEnabled
        {
            get { return _PlotViewWheelEnabled; }
            set { SetProperty(ref _PlotViewWheelEnabled, value); }
        }

        /// <summary>
        /// 宽
        /// </summary>
        public double PlotViewWidth
        {
            get { return variableService.PlotViewWidth; }
            set
            {
                variableService.PlotViewWidth = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 是否允许定时器更新图
        /// </summary>
        public bool TimerIntervalOn
        {
            get { return variableService.TimerIntervalOn; }
            set
            {
                if (value != variableService.CanTimerResetPlot)
                {
                    variableService.TimerIntervalOn = value;
                    RaisePropertyChanged();
                    if (variableService.TimerIntervalOn)
                    {
                        dispatcherTimer.Interval = TimeSpan.FromSeconds(variableService.TimerIntervalSeconds);
                        dispatcherTimer.Start();
                    }
                    else
                    {
                        dispatcherTimer.Stop();
                    }
                }
            }
        }

        //public int LineSeriesPointsCountLimit
        //{
        //    get { return _LineSeriesPointsCountLimit; }
        //    set
        //    {
        //        if (value < 50)
        //        {
        //            value = 50;
        //        }
        //        SetProperty(ref _LineSeriesPointsCountLimit, value);
        //    }
        //}
        /// <summary>
        /// 定时器间隔秒数
        /// </summary>
        public int TimerIntervalSeconds
        {
            get { return variableService.TimerIntervalSeconds; }
            set
            {
                variableService.TimerIntervalSeconds = value;
                if (variableService.TimerIntervalOn)
                {
                    dispatcherTimer.Stop();
                    dispatcherTimer.Interval = TimeSpan.FromSeconds(variableService.TimerIntervalSeconds);
                    dispatcherTimer.Start();
                }

                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// 添加 Plot 命令
        /// </summary>
        public ICommand AddPlotModelCommand => new DelegateCommand<string>(async (parameter) =>
        {
            dispatcherTimer.Stop();

            string view = string.Empty;
            switch (parameter)
            {
                case "Line":
                    view = nameof(AddLineSeriesView);
                    break;

                case "Pie":
                    view = nameof(AddPieSeriesView);
                    break;

                case "Bar":
                    view = nameof(AddBarSeriesView);
                    break;

                default:
                    break;
            }
            if (!string.IsNullOrEmpty(view))
            {
                var result = await dialogHost.ShowDialog(view, null);

                if (result.Result == ButtonResult.OK &&
                    result.Parameters.ContainsKey("Model"))
                {
                    var pltModel = result.Parameters.GetValue<MyPlotModel>("Model");
                    MyPlotModels.Add(pltModel);
                    ResetPlotModel(pltModel);
                    //加进来后应该Update一下
                }
            }
            dispatcherTimer.Start();
        });

        /// <summary>
        /// 删除当前Plot命令
        /// </summary>
        public ICommand DeleteCommand => new DelegateCommand<MyPlotModel>(async (parameter) =>
        {
            var dialogResult = await dialogHost.Question("温馨提示", "确认删除？");
            if (dialogResult.Result != ButtonResult.OK) return;

            MyPlotModels.Remove(parameter);
        });

        /// <summary>
        /// 编辑当前的 Plot 命令
        /// </summary>
        public ICommand EditDetailPropertiesCommand => new DelegateCommand<MyPlotModel>(ExecuteEditDetailPropertiesCommand);

        /// <summary>
        /// 通用执行命令
        /// </summary>
        public DelegateCommand<string> ExecuteCommand => new DelegateCommand<string>((parameter) =>
        {
            switch (parameter)
            {
                case "Setting": SettingButtonClick(); break;
            }
        });

        [JsonIgnore]
        public ICommand PlotViewMouseDoubleClickCommand => new DelegateCommand<MyPlotModel>(ExecuteEditDetailPropertiesCommand);

        /// <summary>
        /// 重设ViewPolt的 位置，适应图像
        /// </summary>
        [JsonIgnore]
        public DelegateCommand<MyPlotModel> ResetCommand => new DelegateCommand<MyPlotModel>((parameter) =>
        {
            parameter.ResetMyModel();
        });

        /// <summary>
        /// 执行编辑 Plot 弹出编辑
        /// </summary>
        /// <param name="parameter"></param>
        private void ExecuteEditDetailPropertiesCommand(MyPlotModel parameter)
        {
            if (variableService.TimerIntervalOn)
            {
                dispatcherTimer.Stop();
            }
            string viewName = string.Empty;
            switch (parameter.SeriesType)
            {
                case SeriesType.BarSeries:
                    viewName = nameof(BarSeriesView);
                    break;

                case SeriesType.PieSeries:
                    viewName = nameof(PieSeriesView);
                    break;

                case SeriesType.ThreeColorLineSeries:
                    viewName = nameof(ThreeColorLineSeriesView);
                    break;

                default:
                    break;
            }
            if (!string.IsNullOrEmpty(viewName))
            {
                DialogParameters dialogParameters = new DialogParameters { { "Model", parameter } };
                dialogService.ShowDialog(viewName, dialogParameters, (rs) =>
                {
                    //关闭窗口后，回调
                    ResetPlotModel(parameter);
                    if (variableService.TimerIntervalOn)
                    {
                        dispatcherTimer.Start();
                    }
                });
            }
        }

        /// <summary>
        /// 打开左侧抽屉
        /// </summary>
        private void SettingButtonClick()
        {
            IsLeftDrawerOpen = true;
        }

        #endregion Commands

        #region Methods

        /// <summary>
        /// 定时读表，更新图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            //读取表格
            try
            {
                UpdateTables();
                foreach (var plotModel in MyPlotModels)
                {
                    UpdateSeriesProperty(plotModel);
                    if (variableService.CanTimerResetPlot)
                    {
                        plotModel.MyModel.ResetAllAxes();
                    }
                    plotModel.MyModel.InvalidatePlot(true);
                }
            }
            catch (Exception ex)
            {
                aggregator.SendMessage(ex.Message);
                logger.LogError(ex, "DispatcherTimer_Tick-Error!");
            }
        }

        /// <summary>
        /// 全局钩子按键按下，监听Control按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey ||
                e.KeyCode == Keys.RControlKey ||
                e.KeyCode == Keys.ControlKey ||
                e.KeyCode == Keys.Control)
            {
                PlotViewWheelEnabled = true;
            }
        }

        /// <summary>
        /// 全局钩子按键松开，监听Control按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalHook_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey ||
                e.KeyCode == Keys.RControlKey ||
                e.KeyCode == Keys.ControlKey ||
                e.KeyCode == Keys.Control)
            {
                PlotViewWheelEnabled = false;
            }
        }

        /// <summary>
        /// 更新输入的PlotModel
        /// </summary>
        /// <param name="myPlotModel"></param>
        private void ResetPlotModel(MyPlotModel myPlotModel)
        {
            try
            {
                UpdateSeriesProperty(myPlotModel);
                var PlotModel = MyPlotModels.IndexOf(myPlotModel);
                myPlotModel.UpdateModel();
                if (PlotModel == -1)
                {
                    MyPlotModels.Add(myPlotModel);
                }
            }
            catch (Exception ex)
            {
                myPlotModel.ErrorMessage = ex.Message;

                logger.LogError(ex, "ResetPlotModel-Error!");
            }
        }

        /// <summary>
        /// 设置条形图信息
        /// </summary>
        private void UpdateBarSeriesProperty(MyPlotModel myPlotModel)
        {
            var tableName = myPlotModel.BarSeriesProperty.TableName;
            var dataTableInfo = this.variableService.DataTableInfos.FirstOrDefault(x => x.DataTableName == tableName);
            if (dataTableInfo == null)
            {
                myPlotModel.ErrorMessage = $"找不到表：{tableName}";
                return;
            }
            var columnList = dataTableInfo.DataTable.AsEnumerable().Select(r => r.Field<object>(myPlotModel.BarSeriesProperty.ColumnName)).ToList();//转为obj是可以的
            var doubleList = ReaderExtensions.ListObj2Double(columnList);//当前列
            myPlotModel.BarSeriesProperty.UpdateBarItem(doubleList);
            myPlotModel.ErrorMessage = null;
        }

        /// <summary>
        /// 设置饼图信息
        /// </summary>
        private void UpdatePieSeriesProperty(MyPlotModel myPlotModel)
        {
            var tableName = myPlotModel.PieSeriesProperty.TableName;
            var dataTableInfo = this.variableService.DataTableInfos.FirstOrDefault(x => x.DataTableName == tableName);
            if (dataTableInfo == null)
            {
                myPlotModel.ErrorMessage = $"找不到表：{tableName}";
                return;
            }
            var columnList = dataTableInfo.DataTable.AsEnumerable().Select(r => r.Field<object>(myPlotModel.PieSeriesProperty.ColumnName)).ToList();//转为obj是可以的
            var doubleList = ReaderExtensions.ListObj2Double(columnList);//当前列
            myPlotModel.PieSeriesProperty.SetPieLice(doubleList);
            myPlotModel.ErrorMessage = null;
        }

        /// <summary>
        /// 根据 SeriesType 类型 更新输入的 PlotModel
        /// </summary>
        /// <param name="myPlotModel"></param>
        private void UpdateSeriesProperty(MyPlotModel myPlotModel)
        {
            try
            {
                switch (myPlotModel.SeriesType)
                {
                    case SeriesType.BarSeries:
                        UpdateBarSeriesProperty(myPlotModel);
                        break;

                    case SeriesType.PieSeries:
                        UpdatePieSeriesProperty(myPlotModel);
                        break;

                    case SeriesType.ThreeColorLineSeries:
                        UpdateThreeColorLineSeriesProperty(myPlotModel);
                        break;
                    //case SeriesType.ScatterSeries:
                    //    SetScatterSeriesProperty(CurrentPlotModel);
                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                myPlotModel.ErrorMessage = ex.Message;
                logger.LogError(ex, "UpdateSeriesProperty-Error!");
            }
        }

        /// <summary>
        /// 所有更新Tables
        /// </summary>
        private void UpdateTables()
        {
            foreach (var dataTableInfos in variableService.DataTableInfos)
            {
                try
                {
                    var table = ReaderExtensions.ReadExcelAsDataTable(dataTableInfos.FilePath, out string message);
                    dataTableInfos.DataTable = table;
                }
                catch (Exception ex)
                {
                    aggregator.SendMessage(ex.Message);
                    logger.LogError(ex, "UpdateTables-Error");
                }
            }
        }
        /// <summary>
        /// 设置更新三色线，折线图信息
        /// </summary>
        private void UpdateThreeColorLineSeriesProperty(MyPlotModel myPlotModel)
        {
            //var countLimit = LineSeriesPointsCountLimit;
            foreach (var lineSeries in myPlotModel.ThreeColorLineSeriesProperty)
            {
                var dataTableInfo = this.variableService.DataTableInfos.FirstOrDefault(x => x.DataTableName == lineSeries.TableName);
                if (dataTableInfo == null)
                {
                    myPlotModel.ErrorMessage = $"找不到表：{lineSeries.TableName}";
                    return;
                }
                lineSeries.Points.Clear();
                var EnumerableTables = dataTableInfo.DataTable.AsEnumerable();
                var columnYList = EnumerableTables.Select(r => r.Field<object>(lineSeries.ColumnNameY)).ToList();//转为obj是可以的
                var doubleYList = ReaderExtensions.ListObj2Double(columnYList);//当前列

                if (!string.IsNullOrEmpty(lineSeries.ColumnNameX))
                {
                    var columnXList = dataTableInfo.DataTable.AsEnumerable().Select(r => r.Field<object>(lineSeries.ColumnNameX)).ToList();//转为obj是可以的

                    var doubleXList = ReaderExtensions.ListObj2Double(columnXList);//当前列

                    for (int i = 0; i < doubleYList.Count; i++)
                    {
                        lineSeries.Points.Add(new TDataPoint(doubleXList[i], doubleYList[i]));
                    }
                }
                else
                {
                    for (int i = 0; i < doubleYList.Count; i++)
                    {
                        lineSeries.Points.Add(new TDataPoint(i, doubleYList[i]));
                    }
                }
            }
            myPlotModel.ErrorMessage = null;
        }

        #endregion Methods
    }
}
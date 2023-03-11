using OxyPlot;
using OxyPlot.Series;
using Prism.Events;
using Table2Chart.Common.MVVM;
using Table2Chart.Common.Services;

namespace Table2Chart.ViewModels.Settings
{
    /// <summary>
    /// 系统设置的ViewModel
    /// </summary>
    public class SystemSettingViewModel : NavigationViewModel
    {
        private readonly IVariableService variableService;

        private PlotModel _PlotModel;

        public SystemSettingViewModel(IEventAggregator eventAggregator,
                    IVariableService variableService) : base(eventAggregator)
        {
            this.variableService = variableService;

            // Create the plot model
            var tmp = new PlotModel { Title = "Simple example", Subtitle = "using OxyPlot" };
            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));
            var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));
            // Add the series to the plot model
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);
            // Axes are created automatically if they are not defined
            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            this.PlotModel = tmp;//添加示例 PlotModel
        }

        /// <summary>
        /// 定时器运行时，是否可重置 Plot 的缩放
        /// </summary>
        public bool CanTimerResetPlot
        {
            get
            {
                return variableService.CanTimerResetPlot;
            }
            set
            {
                variableService.CanTimerResetPlot = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 用于展示的 PlotModel
        /// </summary>
        public PlotModel PlotModel
        {
            get { return _PlotModel; }
            set { SetProperty(ref _PlotModel, value); }
        }

        /// <summary>
        /// 高
        /// </summary>
        public double PlotViewHeight
        {
            get
            {
                return variableService.PlotViewHeight;
            }
            set
            {
                variableService.PlotViewHeight = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 布局的列数
        /// </summary>
        public int PlotViewsColumnsCount
        {
            get
            {
                return variableService.PlotViewsColumnsCount;
            }
            set
            {
                variableService.PlotViewsColumnsCount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 宽
        /// </summary>
        public double PlotViewWidth
        {
            get
            {
                return variableService.PlotViewWidth;
            }
            set
            {
                variableService.PlotViewWidth = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 是否启动定时器
        /// </summary>
        public bool TimerIntervalOn
        {
            get
            {
                return variableService.TimerIntervalOn;
            }
            set
            {
                variableService.TimerIntervalOn = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 定时器间隔时间
        /// </summary>
        public int TimerIntervalSeconds
        {
            get
            {
                return variableService.TimerIntervalSeconds;
            }
            set
            {
                variableService.TimerIntervalSeconds = value;
                RaisePropertyChanged();
            }
        }
    }
}
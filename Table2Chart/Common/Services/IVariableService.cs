using System.Collections.ObjectModel;
using Table2Chart.Common.Models;
using Table2Chart.Common.Models.MyDataSet;

namespace Table2Chart.Common.Services
{
    /// <summary>
    /// 全局变量服务
    /// </summary>
    public interface IVariableService
    {
        bool CanTimerResetPlot { get; set; }

        int PlotViewsColumnsCount { get; set; }
        double PlotViewWidth { get; set; }
        double PlotViewHeight { get; set; }

        //int LineSeriesPointsCountLimit { get; set; }
        ObservableCollection<DataTableInfo> DataTableInfos { get; set; }

        bool TimerIntervalOn { get; set; }
        int TimerIntervalSeconds { get; set; }
        ObservableCollection<MyPlotModel> PlotModels { get; set; }
    }
}
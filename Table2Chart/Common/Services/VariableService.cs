using System.Collections.ObjectModel;
using Table2Chart.Common.Models;
using Table2Chart.Common.Models.MyDataSet;

namespace Table2Chart.Common.Services
{
    /// <summary>
    /// 全局变量服务
    /// </summary>
    public class VariableService : IVariableService
    {
        private int _TimerIntervalSeconds = 300;
        private double _PlotViewWidth = 600;
        private double _PlotViewHeight = 300;
        private int _PlotViewsColumnsCount = 2;

        /// <summary>
        /// 间隔多少毫秒更新
        /// </summary>
        public int TimerIntervalSeconds
        {
            get => _TimerIntervalSeconds;
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _TimerIntervalSeconds = value;
            }
        }

        /// <summary>
        /// 图表宽
        /// </summary>
        public double PlotViewWidth
        {
            get => _PlotViewWidth;
            set
            {
                if (value < 20)
                {
                    value = 20;
                }
                else if (value > 5000)
                {
                    value = 5000;
                }
                _PlotViewWidth = value;
            }
        }

        /// <summary>
        /// 图表的高
        /// </summary>
        public double PlotViewHeight
        {
            get => _PlotViewHeight;
            set
            {
                if (value < 20)
                {
                    value = 20;
                }
                else if (value > 5000)
                {
                    value = 5000;
                }
                _PlotViewHeight = value;
            }
        }

        /// <summary>
        /// 图表的列排布
        /// </summary>
        public int PlotViewsColumnsCount
        {
            get => _PlotViewsColumnsCount;
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                _PlotViewsColumnsCount = value;
            }
        }

        /// <summary>
        /// 更新图表时是否重置适应大小，重置轴缩放
        /// </summary>
        public bool CanTimerResetPlot { get; set; } = false;

        /// <summary>
        /// 是否启动定时器
        /// </summary>
        public bool TimerIntervalOn { get; set; } = false;

        /// <summary>
        /// 全局的表信息
        /// </summary>
        public ObservableCollection<DataTableInfo> DataTableInfos { get; set; } = new ObservableCollection<DataTableInfo>();

        /// <summary>
        /// 用于图表界面，显示于UI的图表
        /// </summary>
        public ObservableCollection<MyPlotModel> PlotModels { get; set; } = new ObservableCollection<MyPlotModel>();
    }
}
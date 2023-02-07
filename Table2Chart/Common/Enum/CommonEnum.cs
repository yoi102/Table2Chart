using System.ComponentModel;

namespace Table2Chart.Common.Enum
{
    /// <summary>
    /// 图表类型
    /// </summary>
    public enum SeriesType
    {
        [Description("条形图")]
        BarSeries,

        //LineSeries,
        [Description("饼图")]
        PieSeries,

        [Description("折线图")]
        ThreeColorLineSeries,

        //ScatterSeries,
    }

    /// <summary>
    /// 坐标轴类型
    /// </summary>
    public enum AxisType
    {
        [Description("线轴")]
        LinearAxis,

        [Description("时间轴")]
        DateTimeAxis,

        [Description("类别轴")]
        CategoryAxis,
    }
}
using Newtonsoft.Json;
using OxyPlot;
using System.Collections.Generic;
using Table2Chart.Common.Models.OxyModels.Color;

namespace Table2Chart.Common.Models.OxyModels.Series
{
    public class LineSeriesProperty : SeriesPropertyBase
    {
        private MarkerType _MarkerType = MarkerType.Circle;
        private double _MarkerSize = 3.0;
        private double _MarkerStrokeThickness = 1;
        private double _StrokeThickness = 2;
        private OxyColor _MarkerStroke = MyColors.Automatic;
        private OxyColor _MarkerFill = MyColors.Automatic;
        private OxyColor _Color = OxyColors.Automatic;
        private LineStyle _LineStyle = LineStyle.Automatic;
        private bool _CanTrackerInterpolatePoints = true;
        private List<TDataPoint> _Points = new List<TDataPoint>();
        private string _TableName;
        private string _ColumnNameX;
        private string _ColumnNameY;
        private IEnumerable<string> _XColumnNameSource;
        private IEnumerable<string> _YColumnNameSource;

        /// <summary>
        /// 能存在空选项
        /// </summary>
        [JsonIgnore]
        public IEnumerable<string> XColumnNameSource
        {
            get { return _XColumnNameSource; }
            set { SetProperty(ref _XColumnNameSource, value); }
        }

        /// <summary>
        /// 不能存在空选项
        /// </summary>
        [JsonIgnore]
        public IEnumerable<string> YColumnNameSource
        {
            get { return _YColumnNameSource; }
            set { SetProperty(ref _YColumnNameSource, value); }
        }

        /// <summary>
        /// 不能为空，Changed后修改xy的集合，传入一个实例？
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { SetProperty(ref _TableName, value); }
        }

        /// <summary>
        /// 能为空
        /// </summary>
        public string ColumnNameX
        {
            get { return _ColumnNameX; }
            set { SetProperty(ref _ColumnNameX, value); }
        }

        /// <summary>
        /// 不能为空
        /// </summary>
        public string ColumnNameY
        {
            get { return _ColumnNameY; }
            set
            {
                SetProperty(ref _ColumnNameY, value);
                Title = value;
            }
        }

        /// <summary>
        /// 线图的节点类型
        /// </summary>
        public MarkerType MarkerType
        {
            get { return _MarkerType; }
            set { SetProperty(ref _MarkerType, value); }
        }

        /// <summary>
        /// 线图的节点尺寸
        /// </summary>
        public double MarkerSize
        {
            get { return _MarkerSize; }
            set { SetProperty(ref _MarkerSize, value); }
        }

        /// <summary>
        /// 线图的节点边框的粗细
        /// </summary>
        public double MarkerStrokeThickness
        {
            get { return _MarkerStrokeThickness; }
            set { SetProperty(ref _MarkerStrokeThickness, value); }
        }

        /// <summary>
        /// 线的粗细
        /// </summary>
        public double StrokeThickness
        {
            get { return _StrokeThickness; }
            set { SetProperty(ref _StrokeThickness, value); }
        }

        /// <summary>
        /// 线图的节点边框颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor MarkerStroke
        {
            get { return _MarkerStroke; }
            set { SetProperty(ref _MarkerStroke, value); }
        }

        [JsonProperty("MarkerStroke")]
        public uint MarkerStrokeUint
        {
            get { return MarkerStroke.ToUint(); }
            set { MarkerStroke = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 线图的节点填充颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor MarkerFill
        {
            get { return _MarkerFill; }
            set { SetProperty(ref _MarkerFill, value); }
        }

        [JsonProperty("MarkerFill")]
        public uint MarkerFillUint
        {
            get { return MarkerFill.ToUint(); }
            set { MarkerFill = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 线图的线颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor Color
        {
            get { return _Color; }
            set { SetProperty(ref _Color, value); }
        }

        [JsonProperty("Color")]
        public uint ColorUint
        {
            get { return Color.ToUint(); }
            set { Color = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 线图的线风格
        /// </summary>
        public LineStyle LineStyle
        {
            get { return _LineStyle; }
            set { SetProperty(ref _LineStyle, value); }
        }

        /// <summary>
        /// 线图中是否可Tracker非真实点
        /// </summary>
        public bool CanTrackerInterpolatePoints
        {
            get { return _CanTrackerInterpolatePoints; }
            set { SetProperty(ref _CanTrackerInterpolatePoints, value); }
        }

        /// <summary>
        /// 点集合
        /// </summary>
        public List<TDataPoint> Points
        {
            get { return _Points; }
            set { SetProperty(ref _Points, value); }
        }
    }

    /// <summary>
    /// 用于传递给 Plot 的点
    /// </summary>
    public class TDataPoint
    {
        public TDataPoint(double x, double y)
        {
            DataX = x;
            DataY = y;
        }

        public double DataX { get; set; }

        //public double DataX
        //{
        //    get { return _DataX; }
        //    set { SetProperty(ref _DataX, value); }
        //}

        public double DataY { get; set; }

        //public double DataY
        //{
        //    get { return _DataY; }
        //    set { SetProperty(ref _DataY, value); }
        //}
    }
}
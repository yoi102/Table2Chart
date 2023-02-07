using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Table2Chart.Common.Enum;
using Table2Chart.Common.Models.OxyModels.Axis;
using Table2Chart.Common.Models.OxyModels.Color;
using Table2Chart.Common.Models.OxyModels.Legend;
using Table2Chart.Common.Models.OxyModels.Series;
using Table2Chart.Common.Models.OxyModels.Axis;

namespace Table2Chart.Common.Models
{
    /// <summary>
    /// 图表
    /// </summary>
    public class MyPlotModel : BindableBase
    {
        public MyPlotModel()
        {
            SeriesType = SeriesType.ThreeColorLineSeries;
            if (_MyModel == null)
            {
                _MyModel = new PlotModel();
            }
        }

        private PlotModel _MyModel;
        private string _PlotModelSubTitle = string.Empty;
        private string _PlotModelTitle = string.Empty;
        private bool _IsBarSeriesStacked = false;
        private bool _SetAxis = true;
        private AxisProperty _LeftAxisProperty = new AxisProperty();
        private AxisProperty _BottomAxisProperty = new AxisProperty();
        private LegendProperty _LegendProperty = new LegendProperty();
        private BarSeriesProperty _BarSeriesProperty;

        //private ObservableCollection<ScatterSeriesProperty> _ScatterSeriesProperty;
        private ObservableCollection<ThreeColorLineSeriesProperty> _ThreeColorLineSeriesProperty;

        private PieSeriesProperty _PieSeriesProperty;
        private SeriesType _SeriesType;

        private string _ErrorMessage = null;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { SetProperty(ref _ErrorMessage, value); }
        }

        /// <summary>
        /// 设置使用哪种图表，限制只能用一种类型图表
        /// </summary>
        public SeriesType SeriesType
        {
            get { return _SeriesType; }
            set
            {
                BarSeriesProperty = null;
                PieSeriesProperty = null;
                ThreeColorLineSeriesProperty = null;
                //ScatterSeriesProperty = null;
                LegendProperty.IsLegendVisible = false;
                switch (value)
                {
                    case SeriesType.BarSeries:
                        BarSeriesProperty = new BarSeriesProperty();
                        break;

                    case SeriesType.PieSeries:
                        PieSeriesProperty = new PieSeriesProperty();
                        break;

                    case SeriesType.ThreeColorLineSeries:
                        ThreeColorLineSeriesProperty = new ObservableCollection<ThreeColorLineSeriesProperty>();
                        LegendProperty.IsLegendVisible = true;
                        break;
                    //case SeriesType.ScatterSeries:
                    //    ScatterSeriesProperty = new ObservableCollection<ScatterSeriesProperty>();
                    //    break;
                    default:
                        break;
                }
                SetProperty(ref _SeriesType, value);
                SetAxis = value != SeriesType.PieSeries;

                LeftAxisProperty.AxisType = value == SeriesType.BarSeries ? AxisType.CategoryAxis : AxisType.LinearAxis;
            }
        }

        public bool IsBarSeriesStacked
        {
            get { return _IsBarSeriesStacked; }
            set { SetProperty(ref _IsBarSeriesStacked, value); }
        }

        /// <summary>
        /// PlotModels
        /// </summary>
        [JsonIgnore]
        public PlotModel MyModel
        {
            get { return _MyModel; }
            private set { SetProperty(ref _MyModel, value); }
        }

        public bool SetAxis
        {
            get { return _SetAxis; }
            set { SetProperty(ref _SetAxis, value); }
        }

        /// <summary>
        /// Plot上方的标题
        /// </summary>
        public string PlotModelTitle
        {
            get { return _PlotModelTitle; }
            set { SetProperty(ref _PlotModelTitle, value); }
        }

        public string PlotModelSubTitle
        {
            get { return _PlotModelSubTitle; }
            set { SetProperty(ref _PlotModelSubTitle, value); }
        }

        public AxisProperty LeftAxisProperty
        {
            get { return _LeftAxisProperty; }
            set { SetProperty(ref _LeftAxisProperty, value); }
        }

        public AxisProperty BottomAxisProperty
        {
            get { return _BottomAxisProperty; }
            set { SetProperty(ref _BottomAxisProperty, value); }
        }

        public LegendProperty LegendProperty
        {
            get { return _LegendProperty; }
            set { SetProperty(ref _LegendProperty, value); }
        }

        public BarSeriesProperty BarSeriesProperty
        {
            get { return _BarSeriesProperty; }
            set { SetProperty(ref _BarSeriesProperty, value); }
        }

        //public ObservableCollection<ScatterSeriesProperty> ScatterSeriesProperty
        //{
        //    get { return _ScatterSeriesProperty; }
        //    set { SetProperty(ref _ScatterSeriesProperty, value); }
        //}

        public ObservableCollection<ThreeColorLineSeriesProperty> ThreeColorLineSeriesProperty
        {
            get { return _ThreeColorLineSeriesProperty; }
            set { SetProperty(ref _ThreeColorLineSeriesProperty, value); }
        }

        public PieSeriesProperty PieSeriesProperty
        {
            get { return _PieSeriesProperty; }
            set { SetProperty(ref _PieSeriesProperty, value); }
        }

        //Methods

        public void UpdateModel()
        {
            MyModel.Title = PlotModelTitle;
            MyModel.Subtitle = PlotModelSubTitle;

            UpdateAxis(MyModel);
            UpdateLegend(MyModel);
            MyModel.Series.Clear();

            switch (SeriesType)
            {
                case SeriesType.BarSeries:
                    UpdateBarSeries(MyModel);
                    break;

                case SeriesType.PieSeries:
                    UpdatePieSeries(MyModel);
                    break;

                case SeriesType.ThreeColorLineSeries:
                    UpdateThreeColorLineSeries(MyModel);
                    break;
                //case SeriesType.ScatterSeries:
                //    UpdateScatterSeries(MyModel);
                //    break;
                default:
                    break;
            }
            ResetMyModel();
        }

        /// <summary>
        /// 更新轴
        /// </summary>
        private void UpdateAxis(PlotModel model)
        {
            model.Axes.Clear();

            if (SeriesType == SeriesType.ThreeColorLineSeries)
            {
                var extraGridlines = LeftAxisProperty.ExtraGridlines.ToList();
                LeftAxisProperty.ExtraGridlines.Clear();
                foreach (var item in ThreeColorLineSeriesProperty)
                {
                    extraGridlines.RemoveAll(x => x.Value == item.LimitHi || x.Value == item.LimitLo);
                    if (item.IsLimitLineVisibility)
                    {
                        LeftAxisProperty.ExtraGridlines.Add(new Mydouble() { Value = item.LimitHi });
                        LeftAxisProperty.ExtraGridlines.Add(new Mydouble() { Value = item.LimitLo });
                    }
                }
                LeftAxisProperty.ExtraGridlines.AddRange(extraGridlines);
            }

            if (SetAxis)
            {
                model.Axes.Add(GetAxis(LeftAxisProperty, AxisPosition.Left));
                model.Axes.Add(GetAxis(BottomAxisProperty, AxisPosition.Bottom));
            }
        }

        private Axis GetAxis(AxisProperty axisProperty, AxisPosition axisPosition)
        {
            Axis axis = null;
            //if (BarSeriesProperty.Count > 0 && axisPosition == AxisPosition.Left)
            //{
            //    axisProperty.AxisType = AxisType.CategoryAxis;

            //}
            switch (axisProperty.AxisType)
            {
                case AxisType.LinearAxis:
                    axis = new LinearAxis();
                    break;

                case AxisType.DateTimeAxis:
                    axis = new DateTimeAxis();
                    break;

                case AxisType.CategoryAxis:
                    var categoryAxis = new CategoryAxis() { GapWidth = axisProperty.CategoryAxisGapWidth };
                    if (BarSeriesProperty != null)
                    {
                        categoryAxis.Labels.AddRange(BarSeriesProperty.BarItem.Select(b => b.Label));
                    }
                    //categoryAxis.ItemsSource = BarSeriesProperty.BarItem;
                    //categoryAxis.LabelField = nameof(BarItemProperty.Label);
                    axis = categoryAxis;
                    break;

                default:
                    break;
            }
            axis.Position = axisPosition;
            axis.Title = axisProperty.AxisTitle;
            axis.TickStyle = axisProperty.TickStyle;
            axis.TicklineColor = axisProperty.TicklineColor;
            axis.MajorGridlineStyle = axisProperty.MajorGridlineStyle;
            axis.MinorGridlineColor = axisProperty.MinorTicklineColor;
            axis.MinorGridlineStyle = axisProperty.MinorGridlineStyle;
            axis.PositionAtZeroCrossing = axisProperty.PositionAtZeroCrossing;
            List<double> ExtraGridlines = new List<double>();
            foreach (var item in axisProperty.ExtraGridlines)
            {
                ExtraGridlines.Add(item.Value);
            }
            axis.ExtraGridlines = ExtraGridlines.ToArray();
            axis.ExtraGridlineThickness = axisProperty.ExtraGridlineThickness;
            axis.ExtraGridlineColor = axisProperty.ExtraGridlineColor;
            axis.MajorGridlineColor = axisProperty.MajorGridlineColor;
            axis.IsAxisVisible = axisProperty.IsAxisVisibility;
            return axis;
        }

        /// <summary>
        /// 更新标识
        /// </summary>
        private void UpdateLegend(PlotModel model)
        {
            model.Legends.Clear();
            model.Legends.Add(CreateLegend(LegendProperty));
        }

        private static Legend CreateLegend(LegendProperty legendProperty)
        {
            Legend legend = new Legend
            {
                LegendPlacement = legendProperty.LegendPlacement,
                LegendPosition = legendProperty.LegendPosition,
                LegendOrientation = legendProperty.LegendOrientation,
                LegendBackground = legendProperty.LegendBackground,
                LegendBorder = legendProperty.LegendBorder,
                IsLegendVisible = legendProperty.IsLegendVisible
            };
            return legend;
        }

        /// <summary>
        /// 更新条状图
        /// </summary>
        private void UpdateBarSeries(PlotModel model)
        {
            model.Series.Add(CreateBarSeries(BarSeriesProperty, IsBarSeriesStacked));
        }

        /// <summary>
        /// 更新散点图
        /// </summary>
        /// <param name="model"></param>
        //private void UpdateScatterSeries(PlotModel model)
        //{
        //    foreach (var item in ScatterSeriesProperty)
        //    {
        //        model.Series.Add(CreateScatterSeries(item));
        //    }
        //}

        /// <summary>
        /// 更新饼图
        /// </summary>
        private void UpdatePieSeries(PlotModel model)
        {
            model.Series.Add(CreatePieSeries(PieSeriesProperty));
        }

        /// <summary>
        /// 更新三颜色线图
        /// </summary>
        private void UpdateThreeColorLineSeries(PlotModel model)
        {
            foreach (var item in ThreeColorLineSeriesProperty)
            {
                model.Series.Add(CreateThreeColorLineSeries(item));
            }
        }

        /// <summary>
        /// 生成条状
        /// </summary>
        /// <param name="barSeriesProperty"></param>
        /// <returns></returns>
        private static BarSeries CreateBarSeries(BarSeriesProperty barSeriesProperty, bool isStacked)
        {
            BarSeries barSeries = new BarSeries
            {
                Title = barSeriesProperty.Title,
                IsStacked = isStacked,
                ItemsSource = barSeriesProperty.BarItem,
                ColorField = nameof(BarItemProperty.Color),
                ValueField = nameof(BarItemProperty.Value),
            };
            return barSeries;
        }

        /// <summary>
        /// 生成线
        /// </summary>
        /// <param name="lineSeriesProperty"></param>
        /// <returns></returns>
        private static LineSeries CreateLineSeries(LineSeriesProperty lineSeriesProperty)
        {
            LineSeries lineSeries = new LineSeries
            {
                Title = lineSeriesProperty.Title,
                MarkerType = lineSeriesProperty.MarkerType,
                MarkerSize = lineSeriesProperty.MarkerSize,
                MarkerStrokeThickness = lineSeriesProperty.MarkerStrokeThickness,
                StrokeThickness = lineSeriesProperty.StrokeThickness,
                MarkerStroke = lineSeriesProperty.MarkerStroke,
                MarkerFill = lineSeriesProperty.MarkerFill,
                Color = lineSeriesProperty.Color,
                LineStyle = lineSeriesProperty.LineStyle,
                CanTrackerInterpolatePoints = lineSeriesProperty.CanTrackerInterpolatePoints,
                ItemsSource = lineSeriesProperty.Points,
                DataFieldX = nameof(TDataPoint.DataX),
                DataFieldY = nameof(TDataPoint.DataY),
            };

            return lineSeries;
        }

        ///// <summary>
        ///// 生成散点
        ///// </summary>
        ///// <param name="scatterSeriesProperty"></param>
        ///// <returns></returns>
        //private static ScatterSeries CreateScatterSeries(ScatterSeriesProperty scatterSeriesProperty)
        //{
        //    ScatterSeries scatterSeries = new ScatterSeries
        //    {
        //        Title = scatterSeriesProperty.Title,
        //        MarkerType = scatterSeriesProperty.MarkerType,
        //        MarkerSize = scatterSeriesProperty.MarkerSize,
        //        MarkerStrokeThickness = scatterSeriesProperty.MarkerStrokeThickness,
        //        MarkerStroke = scatterSeriesProperty.MarkerStroke,
        //        MarkerFill = scatterSeriesProperty.MarkerFill,
        //        ItemsSource = scatterSeriesProperty.Points,
        //        DataFieldX = nameof(TDataPoint.DataX),
        //        DataFieldY = nameof(TDataPoint.DataY),
        //    };
        //    return scatterSeries;
        //}

        /// <summary>
        /// 生成饼
        /// </summary>
        /// <param name="pieSeriesProperty"></param>
        /// <returns></returns>
        private static PieSeries CreatePieSeries(PieSeriesProperty pieSeriesProperty)
        {
            PieSeries pieSeries = new PieSeries
            {
                StrokeThickness = pieSeriesProperty.StrokeThickness,
                InsideLabelPosition = pieSeriesProperty.InsideLabelPosition,
                AngleSpan = pieSeriesProperty.AngleSpan,
                StartAngle = pieSeriesProperty.StartAngle,
                ItemsSource = pieSeriesProperty.PieSlice,
                LabelField = nameof(PieSliceProperty.Label),
                ValueField = nameof(PieSliceProperty.Value),
                ColorField = nameof(PieSliceProperty.Fill),
                IsExplodedField = nameof(PieSliceProperty.IsExploded)
            };
            return pieSeries;
        }

        /// <summary>
        /// 生成三色线
        /// </summary>
        /// <param name="threeColorLineSeriesProperty"></param>
        /// <returns></returns>
        private static ThreeColorLineSeries CreateThreeColorLineSeries(ThreeColorLineSeriesProperty threeColorLineSeriesProperty)
        {
            ThreeColorLineSeries threeColorLineSeries = new ThreeColorLineSeries
            {
                Title = threeColorLineSeriesProperty.Title,
                MarkerType = threeColorLineSeriesProperty.MarkerType,
                MarkerSize = threeColorLineSeriesProperty.MarkerSize,
                MarkerStrokeThickness = threeColorLineSeriesProperty.MarkerStrokeThickness,
                StrokeThickness = threeColorLineSeriesProperty.StrokeThickness,
                MarkerStroke = threeColorLineSeriesProperty.MarkerStroke,
                MarkerFill = threeColorLineSeriesProperty.MarkerFill,
                Color = threeColorLineSeriesProperty.Color,
                LineStyle = threeColorLineSeriesProperty.LineStyle,
                CanTrackerInterpolatePoints = threeColorLineSeriesProperty.CanTrackerInterpolatePoints,
                ItemsSource = threeColorLineSeriesProperty.Points,
                DataFieldX = nameof(TDataPoint.DataX),
                DataFieldY = nameof(TDataPoint.DataY),
                //以下不同部分
                LimitHi = threeColorLineSeriesProperty.LimitHi,
                LimitLo = threeColorLineSeriesProperty.LimitLo,
                LineStyleHi = threeColorLineSeriesProperty.LineStyleHi,
                LineStyleLo = threeColorLineSeriesProperty.LineStyleLo,
                ColorHi = threeColorLineSeriesProperty.ColorHi,
                ColorLo = threeColorLineSeriesProperty.ColorLo,
            };
            return threeColorLineSeries;
        }

        /// <summary>
        /// 重新设置Plot，更新了的内容会更新到ui
        /// </summary>
        public void ResetMyModel()
        {
            MyModel.ResetAllAxes();
            MyModel.InvalidatePlot(true);//会重置大小。。。。
        }

        private DelegateCommand<ThreeColorLineSeriesProperty> _RemoveThreeColorLineSeriesPropertyCommand;
        private DelegateCommand<BarItemProperty> _RemoveBarSeriesItemPropertyCommand;

        //private DelegateCommand<ScatterSeriesProperty> _RemoveScatterSeriesPropertyCommand;
        private DelegateCommand<PieSliceProperty> _RemovePieSeriesPropertyPieSliceCommand;

        private DelegateCommand<string> _ExecuteCommand;

        /// <summary>
        /// 删除当前三色线
        /// </summary>
        [JsonIgnore]
        public DelegateCommand<ThreeColorLineSeriesProperty> RemoveThreeColorLineSeriesPropertyCommand =>
            _RemoveThreeColorLineSeriesPropertyCommand ?? (_RemoveThreeColorLineSeriesPropertyCommand = new DelegateCommand<ThreeColorLineSeriesProperty>(ExecuteRemoveThreeColorLineSeriesPropertyCommand));

        private void ExecuteRemoveThreeColorLineSeriesPropertyCommand(ThreeColorLineSeriesProperty parameter)
        {
            ThreeColorLineSeriesProperty.Remove(parameter);
        }

        /// <summary>
        /// 删除当前的条形图
        /// </summary>
        /// <param name="parameter"></param>
        [JsonIgnore]
        public DelegateCommand<BarItemProperty> RemoveBarSeriesItemPropertyCommand =>
            _RemoveBarSeriesItemPropertyCommand ?? (_RemoveBarSeriesItemPropertyCommand = new DelegateCommand<BarItemProperty>(ExecuteRemoveBarSeriesItemPropertyCommand));

        private void ExecuteRemoveBarSeriesItemPropertyCommand(BarItemProperty parameter)
        {
            BarSeriesProperty.BarItem.Remove(parameter);
        }

        ///// <summary>
        ///// 删除当前散点
        ///// </summary>
        //[JsonIgnore]
        //public DelegateCommand<ScatterSeriesProperty> RemoveScatterSeriesPropertyCommand =>
        //    _RemoveScatterSeriesPropertyCommand ?? (_RemoveScatterSeriesPropertyCommand = new DelegateCommand<ScatterSeriesProperty>(ExecuteRemoveScatterSeriesPropertyCommand));

        //private void ExecuteRemoveScatterSeriesPropertyCommand(ScatterSeriesProperty parameter)
        //{
        //    ScatterSeriesProperty.Remove(parameter);
        //}

        /// <summary>
        /// 删除当前饼片
        /// </summary>
        [JsonIgnore]
        public DelegateCommand<PieSliceProperty> RemovePieSeriesPropertyPieSliceCommand =>
            _RemovePieSeriesPropertyPieSliceCommand ?? (_RemovePieSeriesPropertyPieSliceCommand = new DelegateCommand<PieSliceProperty>(ExecuteRemovePieSeriesPropertyPieSliceCommand));

        private void ExecuteRemovePieSeriesPropertyPieSliceCommand(PieSliceProperty parameter)
        {
            PieSeriesProperty.PieSlice.Remove(parameter);
        }

        [JsonIgnore]
        public DelegateCommand<string> ExecuteCommand =>
            _ExecuteCommand ?? (_ExecuteCommand = new DelegateCommand<string>(ExecuteExecuteCommand));

        private void ExecuteExecuteCommand(string parameter)
        {
            switch (parameter)
            {
                case "AddThreeColorLineSeriesProperty": AddThreeColorLineSeriesProperty(); break;
                case "AddBarSeriesItemProperty": AddBarSeriesItemProperty(); break;
                //case "AddScatterSeriesProperty": AddScatterProperty(); break;
                case "AddPieSeriesPieSliceProperty": AddPieSeriesPieSliceProperty(); break;
                case "BarPieConvert": BarPieConvert(); break;
            }
        }

        private void AddThreeColorLineSeriesProperty()
        {
            ThreeColorLineSeriesProperty.Add(new ThreeColorLineSeriesProperty());
        }

        //private void AddScatterProperty()
        //{
        //    ScatterSeriesProperty.Add(new ScatterSeriesProperty());
        //}

        private void AddBarSeriesItemProperty()
        {
            BarSeriesProperty.BarItem.Add(new BarItemProperty());
        }

        private void AddPieSeriesPieSliceProperty()
        {
            PieSeriesProperty.PieSlice.Add(new PieSliceProperty());
        }

        private void BarPieConvert()
        {
            bool bar2pie = PieSeriesProperty == null;
            //Bar2Pie
            if (bar2pie)
            {
                var tembar = BarSeriesProperty;
                SeriesType = SeriesType.PieSeries;
                PieSeriesProperty.TableName = tembar.TableName;
                PieSeriesProperty.Title = tembar.Title;
                PieSeriesProperty.ColumnName = tembar.ColumnName;

                foreach (var item in tembar.BarItem)
                {
                    PieSeriesProperty.PieSlice.Add(new PieSliceProperty
                    {
                        Label = item.Label,
                        Fill = item.Color,
                        PieLimitLoValue = item.BarItemLimitLoValue,
                        PieLimitHiValue = item.BarItemLimitHiValue,
                        Value = item.Value,
                    });
                }
                var properties = typeof(MyColors).GetProperties();
            }
            //Pie2Bae
            else
            {
                var tempie = PieSeriesProperty;
                SeriesType = SeriesType.BarSeries;
                BarSeriesProperty.TableName = tempie.TableName;
                BarSeriesProperty.Title = tempie.Title;
                BarSeriesProperty.ColumnName = tempie.ColumnName;
                foreach (var item in tempie.PieSlice)
                {
                    if (item.Fill == MyColors.Automatic)
                        item.Fill = MyColors.Random;

                    BarSeriesProperty.BarItem.Add(new BarItemProperty
                    {
                        Label = item.Label,
                        Color = item.Fill,
                        BarItemLimitLoValue = item.PieLimitLoValue,
                        BarItemLimitHiValue = item.PieLimitHiValue,
                        Value = item.Value,
                    });
                }
            }

            UpdateModel();
        }
    }
}
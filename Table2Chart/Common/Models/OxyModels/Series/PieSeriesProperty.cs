using Newtonsoft.Json;
using OxyPlot;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Table2Chart.Common.Models.OxyModels.Series
{
    /// <summary>
    /// 饼图设置的
    /// </summary>
    public class PieSeriesProperty : SeriesPropertyBase
    {
        private double _StrokeThickness = 0.8;
        private double _InsideLabelPosition = 0.8;
        private double _AngleSpan = 360;
        private double _StartAngle = 0;
        private ObservableCollection<PieSliceProperty> _PieSlice = new ObservableCollection<PieSliceProperty>();
        private string _ColumnName = string.Empty;
        private string _TableName;

        /// <summary>
        /// 使用的表的名字
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { SetProperty(ref _TableName, value); }
        }

        /// <summary>
        /// 使用的列的名字
        /// </summary>
        public string ColumnName
        {
            get { return _ColumnName; }
            set { SetProperty(ref _ColumnName, value); }
        }

        /// <summary>
        /// 饼图的轮廓粗细
        /// </summary>
        public double StrokeThickness
        {
            get { return _StrokeThickness; }
            set { SetProperty(ref _StrokeThickness, value); }
        }

        /// <summary>
        /// 标识位于饼图内什么位置
        /// </summary>
        public double InsideLabelPosition
        {
            get { return _InsideLabelPosition; }
            set { SetProperty(ref _InsideLabelPosition, value); }
        }

        /// <summary>
        /// 饼图角度跨度
        /// </summary>
        public double AngleSpan
        {
            get { return _AngleSpan; }
            set { SetProperty(ref _AngleSpan, value); }
        }

        /// <summary>
        /// 饼图开始角度
        /// </summary>
        public double StartAngle
        {
            get { return _StartAngle; }
            set { SetProperty(ref _StartAngle, value); }
        }

        /// <summary>
        /// 饼图的item
        /// </summary>
        public ObservableCollection<PieSliceProperty> PieSlice
        {
            get { return _PieSlice; }
            set { SetProperty(ref _PieSlice, value); }
        }

        /// <summary>
        /// 设置饼片
        /// </summary>
        /// <param name="doubles"></param>
        public void SetPieLice(IEnumerable<double> doubles)
        {
            foreach (var item in PieSlice)
            {
                item.Value = doubles.Count(x => x >= item.PieLimitLoValue && x <= item.PieLimitHiValue);
            }
        }
    }

    /// <summary>
    /// 饼图的饼片的设置
    /// </summary>
    public class PieSliceProperty : BindableBase
    {
        private string _Label = string.Empty;
        private double _Value = 0;
        private OxyColor _Fill = OxyColors.Automatic;
        private bool _IsExploded = true;
        private double _PieLimitHiValue = 999;
        private double _PieLimitLoValue = -999;

        /// <summary>
        /// 当为pie图时，区域上限
        /// </summary>
        public double PieLimitHiValue
        {
            get { return _PieLimitHiValue; }
            set
            {
                if (value > _PieLimitLoValue)
                {
                    SetProperty(ref _PieLimitHiValue, value);
                    //value = _PieLimitLoValue;
                }
            }
        }

        /// <summary>
        /// 当为pie图时，区域下限
        /// </summary>
        public double PieLimitLoValue
        {
            get { return _PieLimitLoValue; }
            set
            {
                if (value < _PieLimitHiValue)
                {
                    SetProperty(ref _PieLimitLoValue, value);
                    //value = _PieLimitHiValue;
                }
            }
        }

        /// <summary>
        /// 饼图item的标签
        /// </summary>
        public string Label
        {
            get { return _Label; }
            set { SetProperty(ref _Label, value); }
        }

        /// <summary>
        /// 饼图item的值
        /// </summary>
        public double Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }

        /// <summary>
        /// 饼图item的填充颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor Fill
        {
            get { return _Fill; }
            set { SetProperty(ref _Fill, value); }
        }

        [JsonProperty("Fill")]
        public uint FillUint
        {
            get { return Fill.ToUint(); }
            set { Fill = OxyColor.FromUInt32(value); }
        }

        public bool IsExploded
        {
            get { return _IsExploded; }
            set { SetProperty(ref _IsExploded, value); }
        }
    }
}
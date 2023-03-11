using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Series;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Table2Chart.Common.Models.OxyModels.Color;

namespace Table2Chart.Common.Models.OxyModels.Series
{
    /// <summary>
    /// 条形图的设置
    /// </summary>
    public class BarSeriesProperty : SeriesPropertyBase
    {
        private ObservableCollection<BarItemProperty> _BarItem = new ObservableCollection<BarItemProperty>();
        private string _ColumnName = string.Empty;
        private string _TableName;
        private LabelPlacement _LabelPlacement = LabelPlacement.Base;
        public LabelPlacement LabelPlacement
        {
            get { return _LabelPlacement; }
            set { SetProperty(ref _LabelPlacement, value); }
        }
        /// <summary>
        /// 使用的表
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { SetProperty(ref _TableName, value); }
        }

        /// <summary>
        /// 使用的列
        /// </summary>
        public string ColumnName
        {
            get { return _ColumnName; }
            set { SetProperty(ref _ColumnName, value); }
        }

        /// <summary>
        /// 条状图的item
        /// </summary>
        public ObservableCollection<BarItemProperty> BarItem
        {
            get { return _BarItem; }
            set { SetProperty(ref _BarItem, value); }
        }

        /// <summary>
        /// 更新每个BarItem
        /// </summary>
        /// <param name="doubles"></param>
        public void UpdateBarItem(IEnumerable<double> doubles)
        {
            foreach (var item in BarItem)
            {
                item.Value = doubles.Count(x => x >= item.BarItemLimitLoValue && x <= item.BarItemLimitHiValue);
            }
        }
    }

    /// <summary>
    /// 条形图的 BarItem 的设置信息
    /// </summary>
    public class BarItemProperty : BindableBase
    {
        private double _Value = 0;
        private OxyColor _Color = MyColors.Random;
        private string _Label = string.Empty;
        private double _BarItemLimitHiValue = 999;
        private double _BarItemLimitLoValue = -999;

        /// <summary>
        /// 区域上限
        /// </summary>
        public double BarItemLimitHiValue
        {
            get { return _BarItemLimitHiValue; }
            set
            {
                if (value > BarItemLimitLoValue)
                {
                    SetProperty(ref _BarItemLimitHiValue, value);
                }
            }
        }

        /// <summary>
        /// 区域下限
        /// </summary>
        public double BarItemLimitLoValue
        {
            get { return _BarItemLimitLoValue; }
            set
            {
                if (value < BarItemLimitHiValue)
                {
                    SetProperty(ref _BarItemLimitLoValue, value);
                }
            }
        }

        public double Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }

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

        public string Label
        {
            get { return _Label; }
            set { SetProperty(ref _Label, value); }
        }
    }
}
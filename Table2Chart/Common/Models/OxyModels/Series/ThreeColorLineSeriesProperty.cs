using Newtonsoft.Json;
using OxyPlot;

namespace Table2Chart.Common.Models.OxyModels.Series
{
    /// <summary>
    /// 三色折线图的设置，即现在用的
    /// </summary>
    public class ThreeColorLineSeriesProperty : LineSeriesProperty
    {
        private double _LimitLo = -999999;
        private OxyColor _ColorLo = OxyColors.Blue;
        private LineStyle _LineStyleLo = LineStyle.Automatic;
        private double _LimitHi = 999999;
        private OxyColor _ColorHi = OxyColors.Red;
        private LineStyle _LineStyleHi = LineStyle.Automatic;

        private bool _IsLimitLineVisibility = true;

        public bool IsLimitLineVisibility
        {
            get { return _IsLimitLineVisibility; }
            set { SetProperty(ref _IsLimitLineVisibility, value); }
        }

        /// <summary>
        /// 低限度
        /// </summary>
        public double LimitLo
        {
            get { return _LimitLo; }
            set
            {
                value = value > _LimitHi ? _LimitHi : value;
                SetProperty(ref _LimitLo, value);
            }
        }

        /// <summary>
        /// 低限度线颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor ColorLo
        {
            get { return _ColorLo; }
            set { SetProperty(ref _ColorLo, value); }
        }

        [JsonProperty("ColorLo")]
        public uint ColorLoUint
        {
            get { return ColorLo.ToUint(); }
            set { ColorLo = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 低限度线风格
        /// </summary>
        public LineStyle LineStyleLo
        {
            get { return _LineStyleLo; }
            set { SetProperty(ref _LineStyleLo, value); }
        }

        /// <summary>
        /// 高限度
        /// </summary>
        public double LimitHi
        {
            get { return _LimitHi; }
            set
            {
                value = value < _LimitLo ? _LimitLo : value;

                SetProperty(ref _LimitHi, value);
            }
        }

        /// <summary>
        /// 高限度线颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor ColorHi
        {
            get { return _ColorHi; }
            set { SetProperty(ref _ColorHi, value); }
        }

        [JsonProperty("ColorHi")]
        public uint ColorHiUint
        {
            get { return ColorHi.ToUint(); }
            set { ColorHi = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 高限度线风格
        /// </summary>
        public LineStyle LineStyleHi
        {
            get { return _LineStyleHi; }
            set { SetProperty(ref _LineStyleHi, value); }
        }
    }
}
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Legends;
using Prism.Mvvm;

namespace Table2Chart.Common.Models.OxyModels.Legend
{
    /// <summary>
    /// 用于设置Plot的图例
    /// </summary>
    public class LegendProperty : BindableBase
    {
        private LegendPlacement _LegendPlacement = LegendPlacement.Inside;
        private LegendPosition _LegendPosition = LegendPosition.TopRight;
        private LegendOrientation _LegendOrientation = LegendOrientation.Vertical;
        private OxyColor _LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
        private OxyColor _LegendBorder = OxyColors.Transparent;
        private bool _IsLegendVisible = false;

        /// <summary>
        /// 放在图表内还是图表外
        /// </summary>
        public LegendPlacement LegendPlacement
        {
            get { return _LegendPlacement; }
            set { SetProperty(ref _LegendPlacement, value); }
        }

        /// <summary>
        /// 放在图表的上下左右位置
        /// </summary>
        public LegendPosition LegendPosition
        {
            get { return _LegendPosition; }
            set { SetProperty(ref _LegendPosition, value); }
        }

        /// <summary>
        /// 项的横纵排序
        /// </summary>
        public LegendOrientation LegendOrientation
        {
            get { return _LegendOrientation; }
            set { SetProperty(ref _LegendOrientation, value); }
        }

        /// <summary>
        /// 背景颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor LegendBackground
        {
            get { return _LegendBackground; }
            set { SetProperty(ref _LegendBackground, value); }
        }

        [JsonProperty("LegendBackground")]
        public uint LegendBackgroundUint
        {
            get { return LegendBackground.ToUint(); }
            set { LegendBackground = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor LegendBorder
        {
            get { return _LegendBorder; }
            set { SetProperty(ref _LegendBorder, value); }
        }

        [JsonProperty("LegendBorder")]
        public uint LegendBorderUint
        {
            get { return LegendBorder.ToUint(); }
            set { LegendBorder = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsLegendVisible
        {
            get { return _IsLegendVisible; }
            set { SetProperty(ref _IsLegendVisible, value); }
        }
    }
}
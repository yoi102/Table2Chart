using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Table2Chart.Common.Models.OxyModels.Axis
{
    /// <summary>
    /// 用于设置Plot的轴
    /// </summary>
    public class AxisBaseProperty : BindableBase
    {
        private TickStyle _TickStyle = TickStyle.Crossing;
        private OxyColor _TicklineColor = OxyColors.Black;
        private LineStyle _MajorGridlineStyle = LineStyle.None;
        private OxyColor _MajorGridlineColor = OxyColors.Black;
        private OxyColor _MinorTicklineColor = OxyColors.Black;
        private LineStyle _MinorGridlineStyle = LineStyle.None;
        private bool _PositionAtZeroCrossing = false;
        private ObservableCollection<Mydouble> _ExtraGridlines = new ObservableCollection<Mydouble>();//换为数组？
        private double _ExtraGridlineThickness = 2;
        private OxyColor _ExtraGridlineColor = OxyColors.Red;
        private bool _IsAxisVisibility = true;
        private double _CategoryAxisGapWidth = 0.3;

        /// <summary>
        /// CategoryAxis 的间隙
        /// </summary>
        public double CategoryAxisGapWidth
        {
            get { return _CategoryAxisGapWidth; }
            set { SetProperty(ref _CategoryAxisGapWidth, value); }
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsAxisVisibility
        {
            get { return _IsAxisVisibility; }
            set { SetProperty(ref _IsAxisVisibility, value); }
        }

        /// <summary>
        /// 轴刻度风格
        /// </summary>
        public TickStyle TickStyle
        {
            get { return _TickStyle; }
            set { SetProperty(ref _TickStyle, value); }
        }

        /// <summary>
        /// 轴主刻度颜色，有数字的刻度
        /// </summary>
        [JsonIgnore]
        public OxyColor TicklineColor
        {
            get { return _TicklineColor; }
            set { SetProperty(ref _TicklineColor, value); }
        }

        [JsonProperty("TicklineColor")]
        public uint TicklineColorUint
        {
            get { return TicklineColor.ToUint(); }
            set { TicklineColor = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 主刻度颜色，不带数字的
        /// </summary>
        [JsonIgnore]
        public OxyColor MajorGridlineColor
        {
            get { return _MajorGridlineColor; }
            set { SetProperty(ref _MajorGridlineColor, value); }
        }

        [JsonProperty("MajorGridlineColor")]
        public uint MajorGridlineColorUint
        {
            get { return MajorGridlineColor.ToUint(); }
            set { MajorGridlineColor = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 主刻度网格线风格
        /// </summary>
        public LineStyle MajorGridlineStyle
        {
            get { return _MajorGridlineStyle; }
            set { SetProperty(ref _MajorGridlineStyle, value); }
        }

        /// <summary>
        /// 副刻度颜色，不带数字的
        /// </summary>
        [JsonIgnore]
        public OxyColor MinorTicklineColor
        {
            get { return _MinorTicklineColor; }
            set { SetProperty(ref _MinorTicklineColor, value); }
        }

        [JsonProperty("MinorTicklineColor")]
        public uint MinorTicklineColorUint
        {
            get { return MinorTicklineColor.ToUint(); }
            set { MinorTicklineColor = OxyColor.FromUInt32(value); }
        }

        /// <summary>
        /// 副刻度网格线风格
        /// </summary>
        public LineStyle MinorGridlineStyle
        {
            get { return _MinorGridlineStyle; }
            set { SetProperty(ref _MinorGridlineStyle, value); }
        }

        /// <summary>
        /// 是否显为经过O原点的十字的坐标轴，只有两坐标轴都设为true时有效
        /// </summary>
        public bool PositionAtZeroCrossing
        {
            get { return _PositionAtZeroCrossing; }
            set { SetProperty(ref _PositionAtZeroCrossing, value); }
        }

        /// <summary>
        /// 线轴的额外网格线
        /// </summary>
        public ObservableCollection<Mydouble> ExtraGridlines
        {
            get { return _ExtraGridlines; }
            set { SetProperty(ref _ExtraGridlines, value); }
        }

        /// <summary>
        /// 线轴的额外网格线的粗细
        /// </summary>
        public double ExtraGridlineThickness
        {
            get { return _ExtraGridlineThickness; }
            set { SetProperty(ref _ExtraGridlineThickness, value); }
        }

        /// <summary>
        /// 线轴的额外网格线的颜色
        /// </summary>
        [JsonIgnore]
        public OxyColor ExtraGridlineColor
        {
            get { return _ExtraGridlineColor; }
            set { SetProperty(ref _ExtraGridlineColor, value); }
        }

        [JsonProperty("ExtraGridlineColor")]
        public uint ExtraGridlineColorUint
        {
            get { return ExtraGridlineColor.ToUint(); }
            set { ExtraGridlineColor = OxyColor.FromUInt32(value); }
        }

        private DelegateCommand<Mydouble> _RemoveExtraGridlinesCommand;

        [JsonIgnore]
        public DelegateCommand<Mydouble> RemoveExtraGridlinesCommand =>
            _RemoveExtraGridlinesCommand ?? (_RemoveExtraGridlinesCommand = new DelegateCommand<Mydouble>(ExecuteRemoveExtraGridlinesCommand));

        private void ExecuteRemoveExtraGridlinesCommand(Mydouble parameter)
        {
            ExtraGridlines.Remove(parameter);
        }

        private DelegateCommand _AddExtraGridlinesCommand;

        public DelegateCommand AddExtraGridlinesCommand =>
            _AddExtraGridlinesCommand ?? (_AddExtraGridlinesCommand = new DelegateCommand(ExecuteAddExtraGridlinesCommand));

        private void ExecuteAddExtraGridlinesCommand()
        {
            ExtraGridlines.Add(new Mydouble());
        }
    }

    public class Mydouble : BindableBase
    {
        private double _Value;

        public double Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }
    }
}
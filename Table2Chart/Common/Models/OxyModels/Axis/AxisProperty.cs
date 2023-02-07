using OxyPlot.Axes;
using Table2Chart.Common.Enum;

namespace Table2Chart.Common.Models.OxyModels.Axis
{
    /// <summary>
    /// 用于设置Plot的轴
    /// </summary>
    public class AxisProperty : AxisBaseProperty
    {
        private AxisType _AxisType = AxisType.LinearAxis;
        private string _AxisTitle = "";
        private AxisPosition _AxisPosition = AxisPosition.None;

        public AxisType AxisType
        {
            get { return _AxisType; }
            set { SetProperty(ref _AxisType, value); }
        }

        public string AxisTitle
        {
            get { return _AxisTitle; }
            set { SetProperty(ref _AxisTitle, value); }
        }

        public AxisPosition AxisPosition
        {
            get { return _AxisPosition; }
            set { SetProperty(ref _AxisPosition, value); }
        }
    }
}
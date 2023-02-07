using Newtonsoft.Json;
using OxyPlot;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;

namespace Table2Chart.Common.Models.OxyModels.Series
{
    [Obsolete("未使用")]
    public class ScatterSeriesProperty : SeriesPropertyBase
    {
        private bool _IsVisible = false;
        private MarkerType _MarkerType = MarkerType.Square;
        private OxyColor _MarkerStroke = OxyColors.Black;
        private double _MarkerSize = 5;
        private OxyColor _MarkerFill = OxyColors.Automatic;
        private ObservableCollection<TDataPoint> _Points = new ObservableCollection<TDataPoint>();
        private DelegateCommand<TDataPoint> _AddPointCommand;
        private DelegateCommand<TDataPoint> _RemovePointCommand;
        private double _MarkerStrokeThickness = 1;
        private string _ColumnNameX = string.Empty;
        private string _ColumnNameY = string.Empty;

        public string ColumnNameX
        {
            get { return _ColumnNameX; }
            set { SetProperty(ref _ColumnNameX, value); }
        }

        public string ColumnNameY
        {
            get { return _ColumnNameY; }
            set { SetProperty(ref _ColumnNameY, value); }
        }

        public bool IsVisible
        {
            get { return _IsVisible; }
            set { SetProperty(ref _IsVisible, value); }
        }

        public MarkerType MarkerType
        {
            get { return _MarkerType; }
            set { SetProperty(ref _MarkerType, value); }
        }

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

        public double MarkerSize
        {
            get { return _MarkerSize; }
            set { SetProperty(ref _MarkerSize, value); }
        }

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

        public double MarkerStrokeThickness
        {
            get { return _MarkerStrokeThickness; }
            set { SetProperty(ref _MarkerStrokeThickness, value); }
        }

        public ObservableCollection<TDataPoint> Points
        {
            get { return _Points; }
            set { SetProperty(ref _Points, value); }
        }

        public DelegateCommand<TDataPoint> AddPointCommand =>
            _AddPointCommand ?? (_AddPointCommand = new DelegateCommand<TDataPoint>(ExecuteAddPointCommand));

        private void ExecuteAddPointCommand(TDataPoint parameter)
        {
            Points.Add(parameter);
        }

        public DelegateCommand<TDataPoint> RemovePointCommand =>
            _RemovePointCommand ?? (_RemovePointCommand = new DelegateCommand<TDataPoint>(ExecuteRemovePointCommand));

        private void ExecuteRemovePointCommand(TDataPoint parameter)
        {
            Points.Remove(parameter);
        }
    }
}
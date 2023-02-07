using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Table2Chart.Common.Models.MyDataSet
{
    //TableName 的作为对比的列

    public class CompareColumnInfo : BindableBase
    {
        private ObservableCollection<ColumnInfo> _ColumnInfos;
        private bool _IsVisible;
        private string _TableName = "";

        public string TableName
        {
            get { return _TableName; }
            set { SetProperty(ref _TableName, value); }
        }

        /// <summary>
        /// TableName 以外的表的列
        /// </summary>
        public ObservableCollection<ColumnInfo> ColumnInfos
        {
            get { return _ColumnInfos; }
            set { SetProperty(ref _ColumnInfos, value); }
        }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible
        {
            get { return _IsVisible; }
            set { SetProperty(ref _IsVisible, value); }
        }
    }
}
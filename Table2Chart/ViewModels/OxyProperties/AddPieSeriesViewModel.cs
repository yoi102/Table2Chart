using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Table2Chart.Common.Enum;
using Table2Chart.Common.Models;
using Table2Chart.Common.Models.MyDataSet;
using Table2Chart.Common.Models.OxyModels.Series;
using Table2Chart.Common.Services;
using Table2Chart.Extensions;

namespace Table2Chart.ViewModels.OxyProperties
{
    /// <summary>
    /// 添加饼图时的的 md 弹窗
    /// </summary>
    public class AddPieSeriesViewModel : BindableBase, IDialogHostAware, IRegionMemberLifetime
    {
        /// <summary>
        /// 获取的全局变量
        /// </summary>
        private readonly IVariableService variableService;

        /// <summary>
        /// 要添加的 PlotModel
        /// </summary>

        private MyPlotModel _MyPlotModel = new MyPlotModel() { SeriesType = SeriesType.PieSeries };

        /// <summary>
        /// 选中表的列信息
        /// </summary>
        private ObservableCollection<ColumnInfo> _ColumnInfos;

        /// <summary>
        /// 由服务提供，用于关闭 md 窗口
        /// </summary>
        public string DialogHostName { get; set; }

        /// <summary>
        /// 表的数据信息
        /// </summary>
        public ObservableCollection<DataTableInfo> DataTableInfos
        {
            get { return variableService.DataTableInfos; }
        }

        /// <summary>
        /// 选中表的列信息
        /// </summary>
        public ObservableCollection<ColumnInfo> ColumnInfos
        {
            get { return _ColumnInfos; }
            set { SetProperty(ref _ColumnInfos, value); }
        }

        /// <summary>
        /// 要添加的 PlotModel
        /// </summary>
        public MyPlotModel MyPlotModel
        {
            get { return _MyPlotModel; }
            //set { SetProperty(ref _MyPlotModel, value); }
        }

        /// <summary>
        /// 生命周期，关闭后不可持续
        /// </summary>
        public bool KeepAlive => false;

        /// <summary>
        /// 保存按钮命令
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// 取消按钮命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        /// <summary>
        /// 移除 DataGrid 行
        /// </summary>
        public ICommand RemovePieSeriesPieSliceCommand => new DelegateCommand<PieSliceProperty>((parameter) =>
        {
            MyPlotModel.PieSeriesProperty.PieSlice.Remove(parameter);
        });

        /// <summary>
        /// 添加 DataGrid 行
        /// </summary>
        public ICommand AddPieSeriesPieSliceCommand => new DelegateCommand(() =>
        {
            AddPieSeriesPieSlice();
        });

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="variableService"></param>
        public AddPieSeriesViewModel(IVariableService variableService)
        {
            this.variableService = variableService;
            SaveCommand = new DelegateCommand(ExecuteSave);
            CancelCommand = new DelegateCommand(ExecuteCancel);
            MyPlotModel.PieSeriesProperty.PropertyChanged += PieSeriesProperty_PropertyChanged;
            if (DataTableInfos.Count() != 0)
            {
                MyPlotModel.PieSeriesProperty.TableName = DataTableInfos[0].DataTableName;
            }
        }

        /// <summary>
        /// 添加饼片
        /// </summary>
        private void AddPieSeriesPieSlice()
        {
            int i = 1;
            string temName = ReaderExtensions.ExcelColumnFromNumber(i);
            while (MyPlotModel.PieSeriesProperty.PieSlice.FirstOrDefault(x => x.Label == "标签" + temName) != null &&
                !string.IsNullOrEmpty(temName))
            {
                i++;
                temName = ReaderExtensions.ExcelColumnFromNumber(i);
            }
            var p = new PieSliceProperty() { Label = "标签" + temName };

            MyPlotModel.PieSeriesProperty.PieSlice.Add(p);
        }

        /// <summary>
        /// 属性变化时，TableName 变化时，改变 ColumnInfos 即修改 combobox 的选项源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PieSeriesProperty_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PieSeriesProperty.TableName))
            {
                var pieSeries = (PieSeriesProperty)sender;
                var table = variableService.DataTableInfos.FirstOrDefault(x => x.DataTableName == pieSeries.TableName);
                var name = pieSeries.ColumnName;
                ColumnInfos = table.ColumnInfos;
                if (ColumnInfos.FirstOrDefault(x => x.Name == name) == null)
                {
                    pieSeries.ColumnName = ColumnInfos[0].Name;
                }
                else
                {
                    pieSeries.ColumnName = name;
                }
            }
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            AddPieSeriesPieSlice();
        }

        private void ExecuteCancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                MyPlotModel.PieSeriesProperty.PropertyChanged -= PieSeriesProperty_PropertyChanged;

                _MyPlotModel = null;
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, null));
            }
        }

        private void ExecuteSave()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                param.Add("Model", MyPlotModel);

                MyPlotModel.PieSeriesProperty.PropertyChanged -= PieSeriesProperty_PropertyChanged;
                _MyPlotModel = null;
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }
    }
}
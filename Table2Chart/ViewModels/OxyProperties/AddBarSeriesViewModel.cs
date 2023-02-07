using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
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
    /// 添加条形图时 md 的弹窗
    /// </summary>
    public class AddBarSeriesViewModel : BindableBase, IDialogHostAware, IRegionMemberLifetime
    {
        private readonly IVariableService variableService;
        private ObservableCollection<ColumnInfo> _ColumnInfos;
        private MyPlotModel _MyPlotModel = new MyPlotModel() { SeriesType = SeriesType.BarSeries };
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="variableService"></param>
        public AddBarSeriesViewModel(IVariableService variableService)
        {
            this.variableService = variableService;
            SaveCommand = new DelegateCommand(ExecuteSave);
            CancelCommand = new DelegateCommand(ExecuteCancel);
            MyPlotModel.BarSeriesProperty.PropertyChanged += MyPlotModelBarSeriesProperty_PropertyChanged;
            if (DataTableInfos.Count() != 0)
            {
                MyPlotModel.BarSeriesProperty.TableName = DataTableInfos[0].DataTableName;
            }
        }

        /// <summary>
        /// 添加行命令
        /// </summary>
        public ICommand AddBarSeriesBarItemCommand => new DelegateCommand(() =>
        {
            AddBarSeriesBarItem();
        });

        /// <summary>
        /// 取消命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        /// <summary>
        /// 所选中表的列的信息数据，提供给 combobox 选项
        /// </summary>
        public ObservableCollection<ColumnInfo> ColumnInfos
        {
            get { return _ColumnInfos; }
            set { SetProperty(ref _ColumnInfos, value); }
        }

        /// <summary>
        /// 全局变量的 表数据，提供给 combobox 选项
        /// </summary>
        public ObservableCollection<DataTableInfo> DataTableInfos
        {
            get { return variableService.DataTableInfos; }
        }

        /// <summary>
        /// 此 DialogHostName 由 Service 提供，用于关闭弹窗用
        /// </summary>
        public string DialogHostName { get; set; }

        /// <summary>
        /// 此生命周期，关闭后不可持续
        /// </summary>
        public bool KeepAlive => false;
        /// <summary>
        /// 要添加进去的 PlotModel
        /// </summary>
        public MyPlotModel MyPlotModel
        {
            get { return _MyPlotModel; }
            //set { SetProperty(ref _MyPlotModel, value); }
        }

        /// <summary>
        /// 移除行命令
        /// </summary>
        public ICommand RemoveBarSeriesBarItemCommand => new DelegateCommand<BarItemProperty>((parameter) =>
        {
            MyPlotModel.BarSeriesProperty.BarItem.Remove(parameter);
        });

        /// <summary>
        /// 保存命令
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }
        /// <summary>
        /// 弹窗开启进来时
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            AddBarSeriesBarItem();
        }

        /// <summary>
        /// 添加 DataGrid 时执行
        /// </summary>
        private void AddBarSeriesBarItem()
        {
            int i = 1;
            string temName = ReaderExtensions.ExcelColumnFromNumber(i);
            while (MyPlotModel.BarSeriesProperty.BarItem.FirstOrDefault(x => x.Label == "标签" + temName) != null &&
                !string.IsNullOrEmpty(temName))
            {
                i++;
                temName = ReaderExtensions.ExcelColumnFromNumber(i);
            }
            var p = new BarItemProperty() { Label = "标签" + temName };
            MyPlotModel.BarSeriesProperty.BarItem.Add(p);
        }

        /// <summary>
        /// 取消按钮关闭窗口
        /// </summary>
        private void ExecuteCancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                MyPlotModel.BarSeriesProperty.PropertyChanged -= MyPlotModelBarSeriesProperty_PropertyChanged;
                _MyPlotModel = null;
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, null));
            }
        }

        /// <summary>
        /// 保存按钮关闭窗口
        /// </summary>
        private void ExecuteSave()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                param.Add("Model", MyPlotModel);
                MyPlotModel.BarSeriesProperty.PropertyChanged -= MyPlotModelBarSeriesProperty_PropertyChanged;
                _MyPlotModel = null;
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }

        /// <summary>
        /// 当属性变化时，TableName 变化时，改变 ColumnInfos 即改变选项源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPlotModelBarSeriesProperty_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BarSeriesProperty.TableName))
            {
                var barSeries = (BarSeriesProperty)sender;
                var table = variableService.DataTableInfos.FirstOrDefault(x => x.DataTableName == barSeries.TableName);
                var name = barSeries.ColumnName;
                ColumnInfos = table.ColumnInfos;
                if (ColumnInfos.FirstOrDefault(x => x.Name == name) == null)
                {
                    barSeries.ColumnName = ColumnInfos[0].Name;
                }
                else
                {
                    barSeries.ColumnName = name;
                }
            }
        }
    }
}
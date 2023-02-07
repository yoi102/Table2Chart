using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Table2Chart.Common.Enum;
using Table2Chart.Common.Models;
using Table2Chart.Common.Models.MyDataSet;
using Table2Chart.Common.Models.OxyModels.Series;
using Table2Chart.Common.Services;

namespace Table2Chart.ViewModels.OxyProperties
{
    /// <summary>
    /// 添加折线图即三色折线图的md的弹窗
    /// </summary>
    public class AddLineSeriesViewModel : BindableBase, IDialogHostAware, IRegionMemberLifetime
    {
        private readonly IVariableService variableService;

        /// <summary>
        /// 作为添加的实例
        /// </summary>
        private MyPlotModel _MyPlotModel = new MyPlotModel() { SeriesType = SeriesType.ThreeColorLineSeries };

        /// <summary>
        /// 由服务提供，用于关闭 md 的弹窗
        /// </summary>
        public string DialogHostName { get; set; }

        /// <summary>
        /// 此实例的生命周期，关闭后不可持续
        /// </summary>
        public bool KeepAlive => false;

        /// <summary>
        /// 表数据
        /// </summary>
        public ObservableCollection<DataTableInfo> DataTableInfos
        {
            get { return variableService.DataTableInfos; }
        }

        /// <summary>
        /// 作为添加的实例
        /// </summary>
        public MyPlotModel MyPlotModel
        {
            get { return _MyPlotModel; }
            //set { SetProperty(ref _MyPlotModel, value); }
        }

        /// <summary>
        /// 保存命令
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// 取消命令
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        /// <summary>
        /// 移除 DataGrid 行命令
        /// </summary>
        public ICommand RemoveLineSeriesCommand => new DelegateCommand<ThreeColorLineSeriesProperty>((parameter) =>
        {
            if (parameter != null)
            {
                parameter.PropertyChanged -= LineSeries_PropertyChanged;
                MyPlotModel.ThreeColorLineSeriesProperty.Remove(parameter);
            }
        });

        /// <summary>
        /// 添加 DataGrid 行命令
        /// </summary>
        public ICommand AddLineSeriesCommand => new DelegateCommand(() =>
        {
            AddLineSeries();
        });

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="variableService"></param>
        public AddLineSeriesViewModel(IVariableService variableService)
        {
            this.variableService = variableService;
            SaveCommand = new DelegateCommand(ExecuteSave);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }

        /// <summary>
        /// 弹窗启动进来时
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            AddLineSeries();
        }

        /// <summary>
        /// 取消按钮执行
        /// </summary>
        private void ExecuteCancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                foreach (var item in MyPlotModel.ThreeColorLineSeriesProperty)
                {
                    item.PropertyChanged -= LineSeries_PropertyChanged;
                }
                _MyPlotModel = null;
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No, null));
            }
        }

        /// <summary>
        /// 保存按钮执行
        /// </summary>
        private void ExecuteSave()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                param.Add("Model", MyPlotModel);
                foreach (var item in MyPlotModel.ThreeColorLineSeriesProperty)
                {
                    item.PropertyChanged -= LineSeries_PropertyChanged;
                }
                _MyPlotModel = null;
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }

        /// <summary>
        /// 添加 DataGrid 行
        /// </summary>
        private void AddLineSeries()
        {
            var lineSeries = new ThreeColorLineSeriesProperty();
            if (DataTableInfos.Count() != 0)
            {
                lineSeries.PropertyChanged += LineSeries_PropertyChanged;
                lineSeries.TableName = DataTableInfos[0].DataTableName;
            }
            MyPlotModel.ThreeColorLineSeriesProperty.Add(lineSeries);
        }

        /// <summary>
        /// 属性变化时，TableName 变化时，改变列名选项源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineSeries_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //当TableName变更时，更新对应的x和y的选择项和当前选项值
            if (e.PropertyName == nameof(LineSeriesProperty.TableName))
            {
                var lineSeries = (ThreeColorLineSeriesProperty)sender;
                var tableName = lineSeries.TableName;
                //table不会为空
                var table = variableService.DataTableInfos.FirstOrDefault(x => x.DataTableName == tableName);

                var columnsName = table.ColumnInfos.Select(x => x.Name);
                //Y选项部分
                lineSeries.YColumnNameSource = columnsName;
                if (columnsName.Count() != 0)
                {
                    if (!columnsName.Contains(lineSeries.ColumnNameY))
                    {
                        lineSeries.ColumnNameY = columnsName.ToList()[0];
                    }
                }
                else
                {
                    lineSeries.ColumnNameY = string.Empty;
                }
                //X选项部分
                var columnsNameList = columnsName.ToList();
                columnsNameList.Insert(0, string.Empty);
                lineSeries.XColumnNameSource = columnsNameList;
                if (!columnsName.Contains(lineSeries.ColumnNameX))
                {
                    lineSeries.ColumnNameX = string.Empty;
                }
            }
        }
    }
}
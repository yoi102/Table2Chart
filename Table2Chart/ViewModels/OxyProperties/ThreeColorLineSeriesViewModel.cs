using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Table2Chart.Common.Models;
using Table2Chart.Common.Models.MyDataSet;
using Table2Chart.Common.Models.OxyModels.Series;
using Table2Chart.Common.Services;

namespace Table2Chart.ViewModels.OxyProperties
{
    /// <summary>
    /// 折线图即三色折线图，编辑时弹窗
    /// </summary>
    public class ThreeColorLineSeriesViewModel : BindableBase, IDialogAware
    {
        /// <summary>
        /// 全局变量
        /// </summary>
        private readonly IVariableService variableService;
        /// <summary>
        /// 当前编辑的 PlotModel
        /// </summary>
        private MyPlotModel _Model;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="variableService"></param>
        public ThreeColorLineSeriesViewModel(IVariableService variableService)
        {
            this.variableService = variableService;
        }
        /// <summary>
        /// 关闭窗口请求
        /// </summary>
        public event Action<IDialogResult> RequestClose;
        /// <summary>
        /// 添加命令
        /// </summary>
        public ICommand AddLineSeriesCommand => new DelegateCommand(() =>
        {
            AddLineSeries();
        });
        /// <summary>
        /// 表选项源
        /// </summary>
        public ObservableCollection<DataTableInfo> DataTableInfos
        {
            get { return variableService.DataTableInfos; }
        }
        /// <summary>
        /// 当前编辑的 PlotModel
        /// </summary>
        public MyPlotModel Model
        {
            get { return _Model; }
            set { SetProperty(ref _Model, value); }
        }
        /// <summary>
        /// 移除命令
        /// </summary>
        public ICommand RemoveLineSeriesCommand => new DelegateCommand<ThreeColorLineSeriesProperty>((parameter) =>
        {
            if (parameter != null)
            {
                parameter.PropertyChanged -= LineSeries_PropertyChanged;
                Model.ThreeColorLineSeriesProperty.Remove(parameter);
            }
        });
        /// <summary>
        /// 当前窗口的标题
        /// </summary>
        public string Title => "编辑折线图";
        /// <summary>
        /// 是否可关闭窗口
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }
        /// <summary>
        /// 关闭窗口后
        /// </summary>
        public void OnDialogClosed()
        {
            foreach (var item in Model.ThreeColorLineSeriesProperty)
            {
                item.PropertyChanged -= LineSeries_PropertyChanged;
            }
        }
        /// <summary>
        /// 窗口启动进入后
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Model = parameters.GetValue<MyPlotModel>("Model");

            foreach (var item in Model.ThreeColorLineSeriesProperty)
            {
                item.PropertyChanged += LineSeries_PropertyChanged;
                LineSeries_PropertyChanged(item, new PropertyChangedEventArgs(nameof(LineSeriesProperty.TableName)));
            }
        }
        /// <summary>
        /// 添加折线
        /// </summary>
        private void AddLineSeries()
        {
            var lineSeries = new ThreeColorLineSeriesProperty();
            if (DataTableInfos.Count() != 0)
            {
                lineSeries.PropertyChanged += LineSeries_PropertyChanged;
                lineSeries.TableName = DataTableInfos[0].DataTableName;
            }
            Model.ThreeColorLineSeriesProperty.Add(lineSeries);
        }
        /// <summary>
        /// 属性变化时，TableName 选项改变时，修改X Y 选项源
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
                if (table == null) return;
                var columnsName = table.ColumnInfos.Select(x => x.Name);
                //Y选项部分
                lineSeries.YColumnNameSource = columnsName;
                if (columnsName.Count() != 0)
                {
                    if (!columnsName.Contains(lineSeries.ColumnNameY))
                    {
                        lineSeries.ColumnNameY = columnsName.First();
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
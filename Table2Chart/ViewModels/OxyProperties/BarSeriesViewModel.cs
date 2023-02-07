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
using Table2Chart.Extensions;

namespace Table2Chart.ViewModels.OxyProperties
{
    /// <summary>
    /// 编辑条形图时的普通弹窗
    /// </summary>
    public class BarSeriesViewModel : BindableBase, IDialogAware
    {
        /// <summary>
        /// 获取的全局变量
        /// </summary>
        private readonly IVariableService variableService;

        /// <summary>
        /// 列选项源
        /// </summary>
        private ObservableCollection<ColumnInfo> _ColumnInfos;

        /// <summary>
        /// 当前 PlotModel
        /// </summary>
        private MyPlotModel _Model;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="variableService"></param>
        public BarSeriesViewModel(IVariableService variableService)
        {
            this.variableService = variableService;
        }

        /// <summary>
        /// 用于请求关闭窗口
        /// </summary>
        public event Action<IDialogResult> RequestClose;
        /// <summary>
        /// 添加 DataGrid 行
        /// </summary>
        public ICommand AddBarSeriesBarItemCommand => new DelegateCommand(() =>
        {
            AddBarSeriesBarItem();
        });

        /// <summary>
        /// 列选项源
        /// </summary>
        public ObservableCollection<ColumnInfo> ColumnInfos
        {
            get { return _ColumnInfos; }
            set { SetProperty(ref _ColumnInfos, value); }
        }

        /// <summary>
        /// 表的选项源
        /// </summary>
        public ObservableCollection<DataTableInfo> DataTableInfos
        {
            get { return variableService.DataTableInfos; }
        }

        /// <summary>
        /// 当前的 PlotModel
        /// </summary>
        public MyPlotModel Model
        {
            get { return _Model; }
            set { SetProperty(ref _Model, value); }
        }

        /// <summary>
        /// 移除 DataGrid 行
        /// </summary>
        public ICommand RemoveBarSeriesBarItemCommand => new DelegateCommand<BarItemProperty>((parameter) =>
        {
            Model.BarSeriesProperty.BarItem.Remove(parameter);
        });

        /// <summary>
        /// 窗口头标题
        /// </summary>
        public string Title => "编辑条形图信息";
        /// <summary>
        /// 是否允许关闭窗口
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
            Model.BarSeriesProperty.PropertyChanged -= BarSeriesProperty_PropertyChanged;
        }

        /// <summary>
        /// 窗口启动进来后
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Model = parameters.GetValue<MyPlotModel>("Model");

            Model.BarSeriesProperty.PropertyChanged += BarSeriesProperty_PropertyChanged;

            //var dataTableInfo = DataTableInfos.FirstOrDefault(x => x.DataTableName == Model.BarSeriesProperty.TableName);
            //if (dataTableInfo == null && DataTableInfos.Count() > 0)
            //{
            //    Model.BarSeriesProperty.TableName = DataTableInfos[0].DataTableName;
            //}

            BarSeriesProperty_PropertyChanged(Model.BarSeriesProperty, new PropertyChangedEventArgs(nameof(BarSeriesProperty.TableName)));
        }

        /// <summary>
        /// 添加条
        /// </summary>
        private void AddBarSeriesBarItem()
        {
            int i = 1;
            string temName = ReaderExtensions.ExcelColumnFromNumber(i);
            while (Model.BarSeriesProperty.BarItem.FirstOrDefault(x => x.Label == "标签" + temName) != null &&
                !string.IsNullOrEmpty(temName))
            {
                i++;
                temName = ReaderExtensions.ExcelColumnFromNumber(i);
            }
            var p = new BarItemProperty() { Label = "标签" + temName };
            Model.BarSeriesProperty.BarItem.Add(p);
        }

        /// <summary>
        /// 属性变化时，TableName 选择的表变化时，修改 ColumnName 选项源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarSeriesProperty_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BarSeriesProperty.TableName))
            {
                var barSeries = (BarSeriesProperty)sender;
                var table = variableService.DataTableInfos.FirstOrDefault(x => x.DataTableName == barSeries.TableName);
                if (table == null) return;
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
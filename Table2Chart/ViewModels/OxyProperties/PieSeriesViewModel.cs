using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
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
    /// 饼图编辑时，普通弹窗
    /// </summary>
    public class PieSeriesViewModel : BindableBase, IDialogAware
    {   /// <summary>
        /// 选中表中的列信息 用作列的选项源
        /// </summary>
        private ObservableCollection<ColumnInfo> _ColumnInfos;
        /// <summary>
        /// 当前编辑的 PlotModel
        /// </summary>
        private MyPlotModel _Model;
        /// <summary>
        /// 全局变量
        /// </summary>
        private IVariableService variableService;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="variableService"></param>
        public PieSeriesViewModel(IVariableService variableService)
        {
            this.variableService = variableService;
        }
        /// <summary>
        /// 请求关闭窗口用
        /// </summary>
        public event Action<IDialogResult> RequestClose;
        /// <summary>
        /// 添加饼片命令
        /// </summary>
        public ICommand AddPieSeriesPieSliceCommand => new DelegateCommand(() =>
        {
            int i = 1;
            string temName = ReaderExtensions.ExcelColumnFromNumber(i);
            while (Model.PieSeriesProperty.PieSlice.FirstOrDefault(x => x.Label == "标签" + temName) != null &&
                !string.IsNullOrEmpty(temName))
            {
                i++;
                temName = ReaderExtensions.ExcelColumnFromNumber(i);
            }
            var p = new PieSliceProperty() { Label = "标签" + temName };

            Model.PieSeriesProperty.PieSlice.Add(p);
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
        /// 移除饼片，DataGrid 行
        /// </summary>
        public ICommand RemovePieSeriesPieSliceCommand => new DelegateCommand<PieSliceProperty>((parameter) =>
        {
            Model.PieSeriesProperty.PieSlice.Remove(parameter);
        });
        /// <summary>
        /// 当前窗口的头标题
        /// </summary>
        public string Title => "编辑饼图";
        /// <summary>
        /// 是否允许关闭弹窗
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }
        /// <summary>
        /// 关闭弹窗后
        /// </summary>
        public void OnDialogClosed()
        {
            Model.PieSeriesProperty.PropertyChanged -= PieSeriesProperty_PropertyChanged; ;
        }
        /// <summary>
        /// 弹窗启动进来后
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Model = parameters.GetValue<MyPlotModel>("Model");

            Model.PieSeriesProperty.PropertyChanged += PieSeriesProperty_PropertyChanged; ;

            //var dataTableInfo = DataTableInfos.FirstOrDefault(x => x.DataTableName == Model.PieSeriesProperty.TableName);
            //if (dataTableInfo == null && DataTableInfos.Count() > 0)
            //{
            //    Model.PieSeriesProperty.TableName = DataTableInfos[0].DataTableName;
            //}

            PieSeriesProperty_PropertyChanged(Model.PieSeriesProperty, new PropertyChangedEventArgs(nameof(PieSeriesProperty.TableName)));
        }
        /// <summary>
        /// 属性变化时，TableName 所选中的表变化时， 修改列选项源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PieSeriesProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PieSeriesProperty.TableName))
            {
                var pieSeries = (PieSeriesProperty)sender;
                var table = variableService.DataTableInfos.FirstOrDefault(x => x.DataTableName == pieSeries.TableName);
                if (table != null)
                {
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
        }
    }
}
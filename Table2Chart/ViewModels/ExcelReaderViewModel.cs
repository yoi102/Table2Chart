using DataGridExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Table2Chart.Common.Models.MyDataSet;
using Table2Chart.Common.MVVM;
using Table2Chart.Common.Services;
using Table2Chart.Controls;
using Table2Chart.Extensions;
using Table2Chart.Views;
using Table2Chart.Views.Dialogs;

namespace Table2Chart.ViewModels
{
    public class ExcelReaderViewModel : NavigationViewModel
    {
        #region Constructors

        public ExcelReaderViewModel(Logger<ExcelReaderView> logger, IDialogService dialogService, IEventAggregator eventAggregator,
            IDialogHostService dialogHostService, IVariableService variableService) : base(eventAggregator)
        {
            KeepAlive = true;
            this.logger = logger;
            this.dialogService = dialogService;
            this.variableService = variableService;
            this.dialogHostService = dialogHostService;

            _DataTableInfos = variableService.DataTableInfos;
            DataTableInfos.CollectionChanged += DataTables_CollectionChanged;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        /// <summary>
        ///导航离开，更新服务数据；关闭软件不会触发
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            dispatcherTimer.Stop();
        }

        /// <summary>
        ///  导航进来，刷新数据。
        /// </summary>
        /// <param name="navigationContext"></param>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            Task.Run(() => UpdatePage());
        }

        private void UpdatePage()
        {
            UpdateLoading(true);
            //进来时刷新表格
            foreach (var item in DataTableInfos)//更新了
            {
                item.SetDataTableInfos(DataTableInfos);
                UpdateTable(item.FilePath);
            }
            foreach (var item in DataTableInfos)
            {
                item.UpdateCompareColumnInfos();
            }

            if (variableService.TimerIntervalOn)
            {
                dispatcherTimer.Interval = TimeSpan.FromSeconds(variableService.TimerIntervalSeconds);
                dispatcherTimer.Start();
            }
            UpdateLoading(false);
        }

        /// <summary>
        ///  DataTables_CollectionChanged 变化后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataTables_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //如果是添加，
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)// NewItems 就一个项
                {
                    if (item is DataTableInfo dataTableInfo)
                    {
                        //这里只为同步列的 IsVisible
                        foreach (var columnInfo in dataTableInfo.ColumnInfos)
                        {
                            foreach (var data in DataTableInfos)
                            {
                                var c = data.ColumnInfos.FirstOrDefault(x => x.Name == columnInfo.Name);
                                if (c != null)
                                {
                                    columnInfo.SetIsVisible(c.IsVisible);
                                }
                            }
                        }
                        //更新一下当前的列信息
                        dataTableInfo.UpdateColumnInfos();
                    }
                }
            }
            //更新一下所有对比表
            foreach (var item in DataTableInfos)
            {
                item.UpdateCompareColumnInfos();
            }
        }

        /// <summary>
        /// 定时器订阅者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateTables();
        }

        #endregion Constructors

        #region Properties

        private readonly ObservableCollection<DataTableInfo> _DataTableInfos;
        private readonly IDialogHostService dialogHostService;
        private readonly IDialogService dialogService;
        private readonly DispatcherTimer dispatcherTimer;
        private readonly Logger<ExcelReaderView> logger;
        private readonly IVariableService variableService;

        /// <summary>
        /// 用于显示的Table,有过滤功能
        /// </summary>
        private ListCollectionView _DisplayDataTableView;

        private DataTableInfo _SelectedDataTableInfo;

        /// <summary>
        /// Table集合，左侧的表的选项
        /// </summary>
        public ObservableCollection<DataTableInfo> DataTableInfos
        {
            get { return _DataTableInfos; }
            //private set { SetProperty(ref _DataTableInfos, value); }
        }

        /// <summary>
        /// 这个类可用于筛选，用于装 SelectedDataTableInfo
        /// </summary>
        public ListCollectionView DisplayDataTableView
        {
            get { return _DisplayDataTableView; }
            set { SetProperty(ref _DisplayDataTableView, value); }
        }

        /// <summary>
        /// 所选择的DataTable,当前为使用SelectedItem该更新view，也可通过行为触发器更新
        /// </summary>
        public DataTableInfo SelectedDataTableInfo
        {
            get { return _SelectedDataTableInfo; }
            set
            {
                SetProperty(ref _SelectedDataTableInfo, value);
                if (value == null || value.DataTable == null)
                {
                    DisplayDataTableView = null;
                }
                else
                {
                    DisplayDataTableView = new ListCollectionView(value.DataTable.DefaultView);
                }
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// 添加用于显示的列的基础信息--最大值、平均、、、、
        /// </summary>
        public ICommand AddColumnBaseInfosCommand => new DelegateCommand<object>(parameter =>
        {
            //需要命令传递过来的propertyName 为属性明
            var obj = (object[])parameter;
            ColumnInfo columnInfo = (ColumnInfo)obj[0];
            string propertyName = (string)obj[1];
            if (columnInfo.ColumnBaseInfos.FirstOrDefault(x => x.PropertyName == propertyName) != null)
                return;
            //遍历属性，是否有对应特性名称的属性，
            //需要确保前端传递的command字符和对应属性名字一样
            var property = columnInfo.GetType().GetProperty(propertyName);
            var descriptions = (DescriptionAttribute[])property.GetCustomAttributes(typeof(DescriptionAttribute), false);
            ColumnBaseInfo columnBaseInfo = new ColumnBaseInfo(propertyName);
            columnBaseInfo.Description = descriptions[0].Description;
            columnBaseInfo.Value = (double)property.GetValue(columnInfo);
            //foreach (var item in columnInfo.GetType().GetProperties())
            //{
            //    var descriptions = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //    if (item.Name == propertyName)
            //    {
            //        columnBaseInfo = new ColumnBaseInfo();
            //        columnBaseInfo.PropertyName = descriptions[0].Description;
            //        columnBaseInfo.Description = descriptions[0].Description;
            //        columnBaseInfo.Value = (double)item.GetValue(columnInfo);
            //    }
            //}
            //如果为null则不添加
            columnInfo.ColumnBaseInfos.Add(columnBaseInfo);
        });

        /// <summary>
        /// 为自定义列信息中，添加计算器
        /// </summary>
        public ICommand AddColumnInfoCommand => new DelegateCommand<ColumnInfo>(parameter =>
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("Model", parameter);
            parameters.Add("IsEdit", false);
            dialogService.ShowDialog(nameof(CalculatorDialogView), parameters, (rs) =>
            {
            });
        });

        /// <summary>
        /// 清除所有过滤条件命令,暂未使用
        /// </summary>
        public ICommand ClearAllFiltersCommand => new DelegateCommand<DataGrid>(obj =>
        {
            //var values = (object[])obj;
            //var dataGrid = (DataGrid)values[0];
            //var textBox = ((TextBox)values[1]);
            //textBox.Text = string.Empty;

            ClearDataGridFilters(obj);
        });

        /// <summary>
        /// 编辑自定义列信息中计算器
        /// </summary>
        public ICommand EditExpressionCalculatorCommand => new DelegateCommand<ExpressionCalculator>(parameter =>
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("Model", parameter);
            parameters.Add("IsEdit", true);
            dialogService.Show(nameof(CalculatorDialogView), parameters, (rs) =>
            {//不需要回调
            });
        });

        /// <summary>
        /// 多指令执行命令
        /// </summary>
        public ICommand ExecuteCommand => new DelegateCommand<string>(parameter =>
        {
            switch (parameter)
            {
                case "OpenExcelFile": OpenExcelFile(); break;//打开文件
                //case "CleanExcelData": CleanDataTables(); break;//清除所有的Table
                default:
                    break;
            }
        });

        /// <summary>
        /// 移除用于显示的列的基础信息--最大值、平均、、、、
        /// </summary>
        public ICommand RemoveColumnBaseInfosCommand => new DelegateCommand<object>(parameter =>
        {
            var obj = (object[])parameter;
            var ColumnBaseInfos = (Collection<ColumnBaseInfo>)obj[0];
            var ColumnBaseInfo = (ColumnBaseInfo)obj[1];
            ColumnBaseInfos.Remove(ColumnBaseInfo);
        });

        /// <summary>
        /// 移除当前的Table命令
        /// </summary>
        public ICommand RemoveCurrentDataTableInfoCommand => new DelegateCommand<DataTableInfo>(parameter =>
        {
            DataTableInfos.Remove(parameter);
        });

        /// <summary>
        /// 移除列信息中的计算器
        /// </summary>
        public ICommand RemoveExpressionCalculatorCommand => new DelegateCommand<object>(parameter =>
        {
            var obj = (object[])parameter;
            var expressionCalculators = (Collection<ExpressionCalculator>)obj[0];
            var expressionCalculator = (ExpressionCalculator)obj[1];
            expressionCalculators.Remove(expressionCalculator);
        });

        ///// <summary>
        ///// 清除数据集 ， 暂未使用
        ///// </summary>
        //private async void CleanDataTables()
        //{
        //    var dialogResult = await dialogHostService.Question("温馨提示", "确认清理所读取的数据？");
        //    if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
        //    DataTableInfos.Clear();
        //    SelectedDataTableInfo = null;
        //    eventAggregator.SendMessage("数据清理完成");
        //}

        /// <summary>
        /// 执行打开Excel文件夹
        /// </summary>
        private void OpenExcelFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;      //该值确定是否可以选择多个文件
            dialog.Title = "请选择CSV或XLSX文件";     //弹窗的标题
            //dialog.InitialDirectory = "D:\\";       //默认打开的文件夹的位置
            dialog.Filter = "XLSX、CSV文件|*.xlsx;*.csv*|CSV文件(*.csv*)|*.csv*|XLSX文件(*.xlsx*)|*.xlsx*"; //筛选文件

            if (dialog.ShowDialog().Value)
            {
                UpdateLoading(true);//转圈等待
                UpdateTable(dialog.FileName);//选中表格后，更新或添加表
                UpdateLoading(false);//退出转圈
            }
            else
            {
                eventAggregator.SendMessage("已取消操作");
            }
        }

        #endregion Commands

        #region Methods

        /// <summary>
        /// 清除所有过滤条件，未使用
        /// </summary>
        /// <param name="dataGrid"></param>
        private static void ClearDataGridFilters(DataGrid dataGrid)
        {
            foreach (var column in dataGrid.Columns.Where(column => column.GetDataGridFilterColumnControl() != null))
            {
                var dataGridFilterColumnControl = column.GetDataGridFilterColumnControl();
                if (dataGridFilterColumnControl.FilterControl is FilterWithPopupControl filterWithPopupControl)
                {
                    filterWithPopupControl.Maximum = 0;
                    filterWithPopupControl.Minimum = 0;
                }
            }
            dataGrid?.GetFilter().Clear();
        }

        /// <summary>
        /// 读取或更新Table
        /// </summary>
        private void UpdateTable(string path)
        {
            try
            {
                var table = ReaderExtensions.ReadExcelAsDataTable(path, out string message);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);// 没有扩展名的文件名 "default"
                var dt = DataTableInfos.FirstOrDefault(x => x.DataTableName == fileNameWithoutExtension);
                if (table != null)
                {
                    DataTableInfo dataTableInfo;
                    if (dt != null)//dt不为 Null 则为更新，即存在相同名字表
                    {
                        dt.DataTable = table;
                        dataTableInfo = dt;
                    }
                    else
                    {
                        dataTableInfo = new DataTableInfo(DataTableInfos) { DataTable = table, FilePath = path };
                        dataTableInfo.DataTableName = table.TableName;
                        DataTableInfos.Add(dataTableInfo);
                    }

                    SelectedDataTableInfo = dataTableInfo;
                }
                else//如果表 读的表为 null
                {
                    if (dt != null)//且 存在同名的
                    {
                        dt.DataTable = null;
                    }
                    eventAggregator.SendMessage(message);
                }
            }
            catch (Exception ex)
            {
                eventAggregator.SendMessage(ex.Message);
                logger.LogError(ex, "UpdateTable");
            }
        }

        /// <summary>
        /// 所有更新Tables
        /// </summary>
        private void UpdateTables()
        {
            foreach (var dataTableInfos in variableService.DataTableInfos)
            {
                try
                {
                    var table = ReaderExtensions.ReadExcelAsDataTable(dataTableInfos.FilePath, out string message);
                    dataTableInfos.DataTable = table;
                }
                catch (Exception ex)
                {
                    eventAggregator.SendMessage(ex.Message);
                    logger.LogError(ex, "UpdateTables-Error");
                }
            }
        }

        #endregion Methods
    }
}
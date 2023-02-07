using Newtonsoft.Json;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Table2Chart.Extensions;

namespace Table2Chart.Common.Models.MyDataSet
{
    public class DataTableInfo : BindableBase
    {
        public DataTableInfo(IList<DataTableInfo> dataTableInfos)
        {
            this.dataTableInfos = dataTableInfos;
        }

        [JsonConstructor]
        public DataTableInfo(ObservableCollection<ColumnInfo> columnInfos,
            ObservableCollection<CompareColumnInfo> compareColumnInfos)
        {
            _ColumnInfos = columnInfos;
            _CompareColumnInfos = compareColumnInfos;
        }

        /// <summary>
        /// 当前实例所在的集合
        /// </summary>
        [JsonIgnore]//应该序列化不了
        private IList<DataTableInfo> dataTableInfos;

        private string _FilePath;
        private ObservableCollection<ColumnInfo> _ColumnInfos;
        private ObservableCollection<CompareColumnInfo> _CompareColumnInfos;
        private DataTable _DataTable;
        private string _DataTableName;

        /// <summary>
        /// 表的数据
        /// </summary>
        [JsonIgnore]
        public DataTable DataTable
        {
            get { return _DataTable; }
            set { SetProperty(ref _DataTable, value); UpdateColumnInfos(); }
        }

        /// <summary>
        /// 表名字
        /// </summary>
        public string DataTableName
        {
            get { return _DataTableName; }
            set { SetProperty(ref _DataTableName, value); }
        }

        /// <summary>
        /// 当前表的文件地址
        /// </summary>
        public string FilePath
        {
            get { return _FilePath; }
            set { SetProperty(ref _FilePath, value); }
        }

        /// <summary>
        /// 当前表所有列
        /// </summary>
        public ObservableCollection<ColumnInfo> ColumnInfos
        {
            get { return _ColumnInfos; }
            //set { SetProperty(ref _ColumnInfos, value); }
        }

        /// <summary>
        /// 其余表的列，作为对比
        /// </summary>
        public ObservableCollection<CompareColumnInfo> CompareColumnInfos
        {
            get { return _CompareColumnInfos; }
            //set { SetProperty(ref _CompareColumnInfo, value); }
        }

        /// <summary>
        /// 将包含此实例的上级，传到这里，用来查找
        /// </summary>
        /// <param name="dataTableInfos"></param>
        public void SetDataTableInfos(IList<DataTableInfo> dataTableInfos)
        {
            this.dataTableInfos = dataTableInfos;
        }

        /// <summary>
        /// 更新当前表的列信息
        /// </summary>
        public void UpdateColumnInfos()
        {
            //同名字列IsVisible共享问题
            //和 表达式共享问题-这个可能不好解决

            if (ColumnInfos == null)
                _ColumnInfos = new ObservableCollection<ColumnInfo>();

            if (dataTableInfos == null || _DataTable == null)
            {
                ColumnInfos.Clear();
                return;
            }
            List<string> strings = new List<string>();
            for (int i = 0; i < _DataTable.Columns.Count; i++)
            {
                var columnItems = _DataTable.AsEnumerable().Select(r => r.Field<object>(i)).ToList();//转为obj是可以的
                var dataList = ReaderExtensions.ListObj2Double(columnItems);//当前列
                var columnInfo = _ColumnInfos.FirstOrDefault(x => x.Name == _DataTable.Columns[i].ColumnName);
                strings.Add(_DataTable.Columns[i].ColumnName);
                //如果存在就暂存，后面清理同一add？也一样要循环两次
                if (columnInfo != null)//如果存在就更新
                {
                    columnInfo.CalculateBaseInfo(dataList);
                    //问题这里？
                    columnInfo.IsVisibleChanged -= ColumnInfo_IsVisibleChanged;
                    columnInfo.IsVisibleChanged += ColumnInfo_IsVisibleChanged;
                }
                else//不过不存在就加进去
                {
                    ColumnInfo c = new ColumnInfo();
                    c.Name = _DataTable.Columns[i].ColumnName;
                    c.CalculateBaseInfo(dataList);
                    ColumnInfos.Add(c);
                    c.IsVisibleChanged += ColumnInfo_IsVisibleChanged;
                }
            }
            //删除表中不存在的列
            for (int i = 0; i < ColumnInfos.Count; i++)
            {
                if (!strings.Contains(ColumnInfos[i].Name))
                {
                    ColumnInfos.Remove(ColumnInfos[i]);
                    i--;
                }
            }
        }

        /// <summary>
        /// 当列可视变换时
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void ColumnInfo_IsVisibleChanged(string name, bool value)
        {
            //IsVisibleChangedUnSubscribe();

            foreach (var dataTableInfo in dataTableInfos)
            {
                var c = dataTableInfo.ColumnInfos.Where(x => x.Name == name);
                foreach (var item in c)
                {
                    item.SetIsVisible(value);
                }
            }
            //IsVisibleChangedSubscribe();
        }

        /// <summary>
        /// 更新作为对比的列
        /// </summary>
        public void UpdateCompareColumnInfos()
        {
            if (CompareColumnInfos == null)
                _CompareColumnInfos = new ObservableCollection<CompareColumnInfo>();
            if (dataTableInfos == null || _DataTable == null)
            {
                CompareColumnInfos.Clear();
                return;
            }
            List<string> strings = new List<string>();

            foreach (var item in dataTableInfos)
            {
                if (item.DataTable != _DataTable && item.DataTable != null)
                {
                    var compareColumnInfo = CompareColumnInfos.FirstOrDefault(x => x.TableName == item.DataTable.TableName);
                    strings.Add(item.DataTable.TableName);
                    if (compareColumnInfo != null)
                    {
                        compareColumnInfo.ColumnInfos = item.ColumnInfos;
                    }
                    else
                    {
                        CompareColumnInfos.Add(new CompareColumnInfo { ColumnInfos = item.ColumnInfos, TableName = item.DataTable.TableName });
                    }
                }
            }
            //删除表中不存在的列
            for (int i = 0; i < CompareColumnInfos.Count; i++)
            {
                if (!strings.Contains(CompareColumnInfos[i].TableName))
                {
                    CompareColumnInfos.Remove(CompareColumnInfos[i]);
                    i--;
                }
            }

            ////当某表格被移除时，删除对应
            //if (CompareColumnInfos.Count >= dataTableInfos.Count)
            //{
            //    List<CompareColumnInfo> toRemove = new List<CompareColumnInfo>();
            //    foreach (var item in CompareColumnInfos)
            //    {
            //        var d = dataTableInfos.FirstOrDefault(x => x.DataTable.TableName == item.TableName);

            //        if (d == null)
            //        {
            //            toRemove.Add(item);
            //        }
            //    }
            //    foreach (var item in toRemove)
            //    {
            //        CompareColumnInfos.Remove(item);
            //    }
            //}
        }

        //public void UpdateCompareColumnInfos_WhentDataTableInfosRemove(DataTableInfo removeInfo)
        //{
        //    var r = CompareColumnInfos.FirstOrDefault(x => x.TableName == removeInfo.DataTable.TableName);
        //    CompareColumnInfos.Remove(r);
        //}
    }
}
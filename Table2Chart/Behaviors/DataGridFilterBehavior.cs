using DataGridExtensions;
using Microsoft.Xaml.Behaviors;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Table2Chart.Controls;

namespace Table2Chart.Behaviors
{
    /// <summary>
    /// 用于获取Textbox 的文本，作用于DataGrid 搜索包含的文字
    /// </summary>
    public class DataGridFilterBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
        }

        public string GrobalFilterText
        {
            get { return (string)GetValue(GrobalFilterTextProperty); }
            set { SetValue(GrobalFilterTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GrobalFilterText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GrobalFilterTextProperty =
            DependencyProperty.Register("GrobalFilterText", typeof(string),
                                           typeof(DataGridFilterBehavior),
                                           new PropertyMetadata(string.Empty,
                                           GrobalFilterText_Changed));

        private static void GrobalFilterText_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (DataGridFilterBehavior)d;
            if (behavior == null)
                return;
            DataGrid dataGrid = behavior.AssociatedObject;
            //ClearDataGridFilters(dataGrid);//触发后，将DataGrid的过滤器都清除

            Predicate<object> predicate = ee =>
            {
                bool result = false;
                var dataRowView = (DataRowView)ee;
                //先查找包含字符串的
                for (int i = 0; i < dataRowView.Row.ItemArray.Length; i++)
                {
                    string cellValue = dataRowView.Row.ItemArray[i].ToString();
                    result |= cellValue.Contains(e.NewValue.ToString());
                }
                return result;
            };
            DataGridFilter.SetGlobalFilter(dataGrid, predicate);

            //if (dataGrid.ItemsSource is ICollectionView collectionView)
            //{
            //    //能获取最大最小值
            //    List<double[]> popupFilter = new List<double[]>();
            //    for (int i = 0; i < dataGrid.Columns.Count; i++)
            //    {
            //        var column = dataGrid.Columns[i];
            //        var dataGridFilterColumnControl = column.GetDataGridFilterColumnControl();
            //        //var activeFilter = column.GetActiveFilter();
            //        //activeFilter.IsMatch();
            //        double[] doubles = null;
            //        if (dataGridFilterColumnControl?.FilterControl is FilterWithPopupControl filterWithPopupControl
            //            && filterWithPopupControl.Filter != null)
            //        {
            //            doubles = new double[] { filterWithPopupControl.Minimum, filterWithPopupControl.Maximum };
            //        }
            //        popupFilter.Add(doubles);
            //    }

            //    collectionView.Filter = ee =>
            //    {
            //        bool result = false;
            //        var dataRowView = (DataRowView)ee;
            //        //先查找包含字符串的
            //        for (int i = 0; i < dataRowView.Row.ItemArray.Length; i++)
            //        {
            //            string cellValue = dataRowView.Row.ItemArray[i].ToString();
            //            result |= cellValue.Contains(e.NewValue.ToString());
            //        }
            //        //若存在筛选
            //        //再查找符合筛选条件的，但只支持上下限的，如果是其他类型将不太好搞
            //        if (popupFilter.Any(x => x != null))
            //        {
            //            for (int i = 0; i < dataRowView.Row.ItemArray.Length; i++)
            //            {
            //                string cellValue = dataRowView.Row.ItemArray[i].ToString();

            //                if (double.TryParse(cellValue, out double currentValue) &&
            //                    popupFilter[i] != null &&
            //                    !(currentValue >= popupFilter[i][0]
            //                     && currentValue < popupFilter[i][1]))
            //                {
            //                    result &= false;
            //                }
            //            }
            //        }
            //        return result;
            //    };
            //}
        }

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
    }
}
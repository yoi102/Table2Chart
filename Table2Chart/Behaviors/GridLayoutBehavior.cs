using Microsoft.Xaml.Behaviors;
using OxyPlot.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Table2Chart.Common.Models;
using Table2Chart.ViewModels;
using EventTrigger = Microsoft.Xaml.Behaviors.EventTrigger;

namespace Table2Chart.Behaviors
{
    [Obsolete("未使用")]
    public class GridLayoutBehavior : Behavior<Grid>
    {
        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.Register("ColumnsCount", typeof(int), typeof(GridLayoutBehavior), new PropertyMetadata(3, ColumnCountChanged));

        private static void ColumnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < 0)
            {
                return;
            }
            var gridLayoutBehavior = (GridLayoutBehavior)d;
            gridLayoutBehavior.PlotModels.CollectionChanged -= gridLayoutBehavior.PlotModels_CollectionChanged;
            gridLayoutBehavior.PlotModels.CollectionChanged += gridLayoutBehavior.PlotModels_CollectionChanged;
            gridLayoutBehavior.ResetPlotViewLayout();
        }

        public ObservableCollection<MyPlotModel> PlotModels
        {
            get { return (ObservableCollection<MyPlotModel>)GetValue(PlotModelPropertys); }
            set { SetValue(PlotModelPropertys, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlotModelPropertys =
            DependencyProperty.Register(nameof(PlotModels), typeof(ObservableCollection<MyPlotModel>), typeof(GridLayoutBehavior), new PropertyMetadata(null, PlotModelsChanged));

        private static void PlotModelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;
            var gridLayoutBehavior = (GridLayoutBehavior)d;
            gridLayoutBehavior.PlotModels.CollectionChanged -= gridLayoutBehavior.PlotModels_CollectionChanged;
            gridLayoutBehavior.PlotModels.CollectionChanged += gridLayoutBehavior.PlotModels_CollectionChanged;
            gridLayoutBehavior.ResetPlotViewLayout();
        }

        private void ResetPlotViewLayout()
        {
            AssociatedObject.ColumnDefinitions.Clear();
            AssociatedObject.RowDefinitions.Clear();
            AssociatedObject.Children.Clear();
            if (PlotModels == null || PlotModels.Count == 0)
                return;
            int rowCount = PlotModels.Count / ColumnCount;
            if (PlotModels.Count % ColumnCount != 0)
            {
                rowCount++;
            }

            int spanColumnCount = ColumnCount - 1;//需要插入的列数
            int spanRowCount = rowCount - 1;//需要插入的行数

            //添加列GridSplitter
            for (int i = 0; i < ColumnCount + spanColumnCount; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                AssociatedObject.ColumnDefinitions.Add(columnDefinition);
                if (i % 2 != 0)//当前列为奇数列时插入
                {
                    columnDefinition.Width = GridLength.Auto;

                    GridSplitter gridSplitter = new GridSplitter();
                    gridSplitter.Width = 5;
                    gridSplitter.Background = Brushes.Black;
                    gridSplitter.HorizontalAlignment = HorizontalAlignment.Stretch;
                    gridSplitter.VerticalAlignment = VerticalAlignment.Stretch;
                    Grid.SetRowSpan(gridSplitter, rowCount + spanRowCount);
                    Grid.SetColumn(gridSplitter, i);
                    Grid.SetRow(gridSplitter, 0);
                    AssociatedObject.Children.Add(gridSplitter);
                }
            }
            //添加行GridSplitter
            for (int i = 0; i < rowCount + spanRowCount; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                AssociatedObject.RowDefinitions.Add(rowDefinition);
                if (i % 2 != 0)//当前行为奇数行时插入
                {
                    rowDefinition.Height = GridLength.Auto;
                    GridSplitter gridSplitter = new GridSplitter();
                    gridSplitter.Height = 5;
                    gridSplitter.Background = Brushes.Black;
                    gridSplitter.HorizontalAlignment = HorizontalAlignment.Stretch;
                    gridSplitter.VerticalAlignment = VerticalAlignment.Stretch;
                    Grid.SetColumnSpan(gridSplitter, ColumnCount + spanColumnCount);
                    Grid.SetRow(gridSplitter, i);
                    Grid.SetColumn(gridSplitter, 0);
                    AssociatedObject.Children.Add(gridSplitter);
                }
            }

            for (int i = 0; i < PlotModels.Count; i++)
            {
                PlotView plotView;
                if (PlotModels[i].MyModel.PlotView != null)
                {
                    plotView = (PlotView)PlotModels[i].MyModel.PlotView;
                }
                else
                {
                    plotView = new PlotView();
                    //不是集合。。。不能
                    //DragDrop.SetDropTargetAdornerBrush(plotView, Brushes.Orange);
                    //DragDrop.SetIsDragSource(plotView, true);
                    //DragDrop.SetIsDropTarget(plotView, true);
                    //DragDrop.SetUseDefaultEffectDataTemplate(plotView, true);
                    //DragDrop.SetUseDefaultDragAdorner(plotView, true);
                    //这触发器，不知后台怎么弄
                    var triggerCollection = Interaction.GetTriggers(plotView);
                    //triggerCollection.Add();
                    EventTrigger eventTrigger = new EventTrigger("MouseDoubleClick");
                    InvokeCommandAction invokeCommandAction = new InvokeCommandAction();
                    var model = (PlotsViewModel)AssociatedObject.DataContext;
                    invokeCommandAction.Command = model.PlotViewMouseDoubleClickCommand;
                    invokeCommandAction.CommandParameter = PlotModels[i].MyModel;
                    //还有一些ContextMenu菜单呢

                    plotView.Model = PlotModels[i].MyModel;
                }

                PlotModelColumnRow(i, out int rowIndex, out int columnIndex);
                Grid.SetRow(plotView, rowIndex * 2);
                Grid.SetColumn(plotView, columnIndex * 2);
                AssociatedObject.Children.Add(plotView);
            }
        }

        private void PlotModelColumnRow(int index, out int rowIndex, out int columnIndex)
        {
            rowIndex = index / ColumnCount;
            columnIndex = index - rowIndex * ColumnCount;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (PlotModels == null)
            {
                return;
            }
            PlotModels.CollectionChanged += PlotModels_CollectionChanged;
            ResetPlotViewLayout();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            PlotModels.CollectionChanged -= PlotModels_CollectionChanged;
        }

        private void PlotModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ResetPlotViewLayout();
        }
    }
}
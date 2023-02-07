using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Table2Chart.Controls
{
    /// <summary>
    /// 用于显示图表的 ItemsControl 的面板
    /// </summary>
    public class VirtualizingGridPanel : Panel
    {
        public int ColumnsCount
        {
            get { return (int)GetValue(ColumnsCountProperty); }
            set { SetValue(ColumnsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsCountProperty =
            DependencyProperty.Register("ColumnsCount", typeof(int), typeof(VirtualizingGridPanel),
                new PropertyMetadata(3, ColumnCountChanged), ColumnCountValidateValueCallback);

        private static bool ColumnCountValidateValueCallback(object value)
        {
            var v = (int)value;
            return v >= 1;
        }

        private static void ColumnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var virtualizingGrid = (VirtualizingGridPanel)d;
            virtualizingGrid.InvalidateMeasure();
        }

        private int RowCount
        {
            get
            {
                int rowCount = InternalChildren.Count / ColumnsCount;
                if (InternalChildren.Count % ColumnsCount != 0)
                {
                    rowCount++;
                }
                return rowCount;
            }
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(VirtualizingGridPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), IsWidthHeightValid);

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(VirtualizingGridPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), IsWidthHeightValid);

        private static bool IsWidthHeightValid(object value)
        {
            double num = (double)value;
            return !double.IsNaN(num) ? num >= 0.0 ? !double.IsPositiveInfinity(num) : false : true;
        }

        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get
            {
                return (double)GetValue(ItemWidthProperty);
            }
            set
            {
                SetValue(ItemWidthProperty, value);
            }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get
            {
                return (double)GetValue(ItemHeightProperty);
            }
            set
            {
                SetValue(ItemHeightProperty, value);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (double.IsNaN(ItemWidth) || double.IsNaN(ItemHeight))
            {
                return base.MeasureOverride(availableSize);
            }
            var childSize = new Size(ItemWidth, ItemHeight);//根据固定宽高
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(childSize);
            }

            double width = ItemWidth * ColumnsCount;
            double height = ItemHeight * RowCount;
            var desiredSize = new Size(width, height);

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (double.IsNaN(ItemWidth) || double.IsNaN(ItemHeight))
            {
                return base.ArrangeOverride(finalSize);
            }
            double x = 0;
            double y = 0;
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                InternalChildren[i].Arrange(new Rect(x, y, ItemWidth, ItemHeight));
                x += ItemWidth;
                if ((i + 1) % ColumnsCount == 0)
                {
                    x = 0;
                    y += ItemHeight;
                }
            }

            return finalSize;
        }
    }
}
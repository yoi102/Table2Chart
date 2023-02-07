﻿using DataGridExtensions;
using System.Windows;

namespace Table2Chart.Controls
{
    /// <summary>
    /// Interaction logic for FilterWithPopupControl.xaml
    /// </summary>
    public partial class FilterWithPopupControl
    {
        public FilterWithPopupControl()
        {
            InitializeComponent();
        }

        public bool IsFilterEnable
        {
            get { return (bool)GetValue(IsFilterEnableProperty); }
            set { SetValue(IsFilterEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFilterEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFilterEnableProperty =
            DependencyProperty.Register("IsFilterEnable", typeof(bool), typeof(FilterWithPopupControl), new PropertyMetadata(false, (sender, _) => ((FilterWithPopupControl)sender).IsFilterEnable_Changed()));

        private void IsFilterEnable_Changed()
        {
            if (IsFilterEnable)
            {
                Range_Changed();
            }
            else
            {
                Filter = null;
            }
        }

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        /// <summary>
        /// Identifies the Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(FilterWithPopupControl)
                , new FrameworkPropertyMetadata("Enter the limits:", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Identifies the Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(FilterWithPopupControl)
                , new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => ((FilterWithPopupControl)sender).Range_Changed()));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Identifies the Maximum dependency property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(FilterWithPopupControl)
                , new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => ((FilterWithPopupControl)sender).Range_Changed()));

        //public bool IsPopupVisible
        //{
        //    get => (bool)GetValue(IsPopupVisibleProperty);
        //    set => SetValue(IsPopupVisibleProperty, value);
        //}
        ///// <summary>
        ///// Identifies the IsPopupVisible dependency property
        ///// </summary>
        //public static readonly DependencyProperty IsPopupVisibleProperty =
        //    DependencyProperty.Register("IsPopupVisible", typeof(bool), typeof(FilterWithPopupControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private void Range_Changed()
        {
            if (IsFilterEnable)
            {
                Filter = Maximum > Minimum ? new ContentFilter(Minimum, Maximum) : null;
                if (Filter == null)
                {
                    SetValue(IsFilterEnableProperty, false);
                }
            }
        }

        public IContentFilter Filter
        {
            get => (IContentFilter)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        /// <summary>
        /// Identifies the Filter dependency property
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(IContentFilter), typeof(FilterWithPopupControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => ((FilterWithPopupControl)sender).Filter_Changed()));

        private void Filter_Changed()
        {
            if (Filter is ContentFilter filter)
            {
                Minimum = filter.Min;
                Maximum = filter.Max;
            }
        }
    }

    public class ContentFilter : IContentFilter
    {
        public ContentFilter(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Min { get; }

        public double Max { get; }

        public bool IsMatch(object value)
        {
            if (value == null)
                return false;

            if (!double.TryParse(value.ToString(), out var number))
            {
                return false;
            }

            return (number >= Min) && (number <= Max);
        }
    }
}
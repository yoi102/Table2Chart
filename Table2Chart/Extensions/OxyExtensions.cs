using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Table2Chart.Extensions
{
    public static class OxyExtensions
    {
        private const string XAXIS_KEY = "x";
        private const string YAXIS_KEY = "y";

        /// <summary>
        /// Transposes a PlotModels. The given PlotModels is mutated and returned for convenience.
        /// </summary>
        /// <param name="model">The PlotModels.</param>
        /// <returns>The transposed PlotModels.</returns>
        public static PlotModel Transpose(this PlotModel model)
        {
            if (!string.IsNullOrEmpty(model.Title))
            {
                model.Title += " (transposed)";
            }

            // Update plot to generate default axes etc.
            ((IPlotModel)model).Update(false);

            foreach (var axis in model.Axes)
            {
                switch (axis.Position)
                {
                    case AxisPosition.Bottom:
                        axis.Position = AxisPosition.Left;
                        break;

                    case AxisPosition.Left:
                        axis.Position = AxisPosition.Bottom;
                        break;

                    case AxisPosition.Right:
                        axis.Position = AxisPosition.Top;
                        break;

                    case AxisPosition.Top:
                        axis.Position = AxisPosition.Right;
                        break;

                    case AxisPosition.None:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var annotation in model.Annotations)
            {
                if (annotation.XAxis != null && annotation.XAxisKey == null)
                {
                    if (annotation.XAxis.Key == null)
                    {
                        annotation.XAxis.Key = XAXIS_KEY;
                    }

                    annotation.XAxisKey = annotation.XAxis.Key;
                }

                if (annotation.YAxis != null && annotation.YAxisKey == null)
                {
                    if (annotation.YAxis.Key == null)
                    {
                        annotation.YAxis.Key = YAXIS_KEY;
                    }

                    annotation.YAxisKey = annotation.YAxis.Key;
                }
            }

            foreach (var series in model.Series.OfType<XYAxisSeries>())
            {
                if (series.XAxisKey == null)
                {
                    if (series.XAxis == null) // this can happen if the series is invisible initially
                    {
                        series.XAxisKey = XAXIS_KEY;
                    }
                    else
                    {
                        if (series.XAxis.Key == null)
                        {
                            series.XAxis.Key = XAXIS_KEY;
                        }

                        series.XAxisKey = series.XAxis.Key;
                    }
                }

                if (series.YAxisKey == null)
                {
                    if (series.YAxis == null)
                    {
                        series.YAxisKey = YAXIS_KEY;
                    }
                    else
                    {
                        if (series.YAxis.Key == null)
                        {
                            series.YAxis.Key = YAXIS_KEY;
                        }

                        series.YAxisKey = series.YAxis.Key;
                    }
                }
            }

            return model;
        }

        #region StaticMethods

        /// <summary>
        /// The time origin.
        /// </summary>
        /// <remarks>This gives the same numeric date values as Excel</remarks>
        private static readonly DateTime TimeOrigin = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a DateTime to days after the time origin.
        /// </summary>
        /// <param name="value">The date/time structure.</param>
        /// <returns>The number of days after the time origin.</returns>
        public static double DateTimeToDouble(DateTime value)
        {
            var span = value - TimeOrigin;
            return span.TotalDays + 1;
        }

        /// <summary>
        /// 线性回归计算
        /// </summary>
        /// <param name="points"></param>
        /// <param name="slope"></param>
        /// <param name="intercept"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        private static void CalculateLinearRegressionParameters(IEnumerable<ScatterPoint> points, out double slope, out double intercept)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            if (points.Count() < 2)
            {
                throw new ArgumentException("at least two points required", nameof(points));
            }

            var meanX = points.Select(p => p.X).Average();
            var meanY = points.Select(p => p.Y).Average();

            var cov = Covariance(points, meanX, meanY);
            var var2_x = Variance2(points.Select(p => p.X));

            slope = cov / var2_x;
            intercept = meanY - slope * meanX;
        }

        /// <summary>
        /// Returns the covariance between the points x and y values.
        /// </summary>
        private static double Covariance(IEnumerable<ScatterPoint> points, double meanX, double meanY)
        {
            var res = points.Sum(p => p.X * p.Y);

            res -= points.Count() * meanX * meanY;
            res /= points.Count() - 1;

            return res;
        }

        /// <summary>
        /// Returns the squared variance of a quantity.
        /// </summary>
        private static double Variance2(IEnumerable<double> values)
        {
            var mean = values.Average();

            var res = values.Sum(x => x * x);

            res -= values.Count() * mean * mean;
            res /= values.Count() - 1;

            return res;
        }

        /// <summary>
        /// Calculates the Least squares fit of a list of DataPoints.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="a">The slope.</param>
        /// <param name="b">The intercept.</param>
        public static void LeastSquaresFit(IEnumerable<ScatterPoint> points, out double a, out double b)
        {
            // http://en.wikipedia.org/wiki/Least_squares
            // http://mathworld.wolfram.com/LeastSquaresFitting.html
            // http://web.cecs.pdx.edu/~gerry/nmm/course/slides/ch09Slides4up.pdf

            double Sx = 0;
            double Sy = 0;
            double Sxy = 0;
            double Sxx = 0;
            int m = 0;
            foreach (var p in points)
            {
                Sx += p.X;
                Sy += p.Y;
                Sxy += p.X * p.Y;
                Sxx += p.X * p.X;
                m++;
            }

            double d = Sx * Sx - m * Sxx;
            a = 1 / d * (Sx * Sy - m * Sxy);
            b = 1 / d * (Sx * Sxy - Sxx * Sy);
        }

        #endregion StaticMethods
    }
}
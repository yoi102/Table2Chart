using Microsoft.Win32;
using OxyPlot.Wpf;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using PdfExporter = OxyPlot.SkiaSharp.PdfExporter;

namespace Table2Chart.Views
{
    /// <summary>
    /// PlotsView.xaml 的交互逻辑
    /// </summary>
    public partial class PlotsView : UserControl
    {
        public PlotsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存表格为图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePlotViewImage_Click(object sender, RoutedEventArgs e)
        {
            var plot = ContextMenuService.GetPlacementTarget(LogicalTreeHelper.GetParent((DependencyObject)e.Source)) as PlotView; ;
            var dlg = new SaveFileDialog
            {
                Title = "Save Plot",
                Filter = ".png files|*.png|.jpg files|*.jpg|.svg files|*.svg|.pdf files|*.pdf|.xaml files|*.xaml",
                DefaultExt = ".png"
            };
            if (dlg.ShowDialog().Value)
            {
                var ext = Path.GetExtension(dlg.FileName).ToLower();
                switch (ext)
                {
                    case ".jpg":
                    case ".png":
                        plot.SaveBitmap(dlg.FileName, 0, 0);
                        break;

                    case ".svg":
                        var rc = new CanvasRenderContext(new Canvas());
                        var svg = OxyPlot.SvgExporter.ExportToString(plot.Model, plot.ActualWidth, plot.ActualHeight, false, rc);
                        File.WriteAllText(dlg.FileName, svg);
                        break;

                    case ".pdf":
                        PdfExporter.Export(plot.Model, dlg.FileName, (float)plot.ActualWidth, (float)plot.ActualHeight);
                        break;

                    case ".xaml":
                        plot.SaveXaml(dlg.FileName);
                        break;
                }
                this.OpenContainingFolder(dlg.FileName);
            }
        }

        private void OpenContainingFolder(string fileName)
        {
            // var folder = Path.GetDirectoryName(fileName);
            var psi = new ProcessStartInfo("Explorer.exe", "/select," + fileName);
            Process.Start(psi);
        }
    }
}
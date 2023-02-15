using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Table2Chart.Extensions
{
    public static class ReaderExtensions
    {
        public static string ExcelColumnFromNumber(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString.ToLower();
        }

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
        /// 将List<string>转为List<string>
        /// </summary>
        /// <param name="listString"></param>
        /// <returns></returns>
        public static List<double> ListString2Double(List<string> listString)
        {
            List<double> result = new List<double>();
            foreach (var item in listString)
            {
                double value;
                if (double.TryParse(item, out double d))
                {
                    value = d;
                }
                else
                {
                    value = DateTime.TryParse(item, out DateTime dateTime)
                        ? DateTimeToDouble(dateTime)
                        : bool.TryParse(item, out bool b) ? Convert.ToDouble(b) : double.NaN;
                }
                result.Add(value);
            }
            return result;
        }

        public static IEnumerable<double> ListObj2Double(IEnumerable<object> listObj)
        {
            List<double> result = new List<double>();
            foreach (var item in listObj)
            {
                double value;
                if (double.TryParse(item?.ToString(), out double d))
                {
                    value = d;
                }
                else
                {
                    value = DateTime.TryParse(item?.ToString(), out DateTime dateTime)
                        ? DateTimeToDouble(dateTime)
                        : bool.TryParse(item?.ToString(), out bool b) ? Convert.ToDouble(b) : double.NaN;
                }
                result.Add(value);
            }
            return result;
        }

        /// <summary>
        /// 当前行是否为空
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static bool IsEmptyRow(IExcelDataReader reader)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var value = reader.GetValue(i);
                if (value != null)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 读取表格,当出现问题，返回null
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="myDataSet"></param>
        /// <returns></returns>
        public static DataTable ReadExcelAsDataTable(string fullPath, out string message)
        {
            //文件有问题直接返回null

            string filename = Path.GetFileName(fullPath);//返回带扩展名的文件名 "default.avi"
            string extension = Path.GetExtension(fullPath);//扩展名 ".avi"
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);// 没有扩展名的文件名 "default"
            if (!File.Exists(fullPath))
            {
                message = $"{fileNameWithoutExtension}\n文件不存在";
                return null;
            }

            #region 读取规则

            //Reader配置
            ExcelReaderConfiguration excelReaderConfiguration = new ExcelReaderConfiguration()
            {
                // Gets or sets the encoding to use when the input XLS lacks a CodePage
                // record, or when the input CSV lacks a BOM and does not parse as UTF8.
                // Default: cp1252 (XLS BIFF2-5 and CSV only)
                //FallbackEncoding = Encoding.GetEncoding(1252),
                FallbackEncoding = Encoding.GetEncoding(936),//中文编码
                                                             // Gets or sets the inputPasswordpassword used to open inputPasswordpassword protected workbooks.
                Password = "inputPasswordpassword",
                // Gets or sets an array of CSV separator candidates. The reader
                // autodetects which best fits the input data. Default: , ; TAB | #
                // (CSV only)
                AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },
                // Gets or sets a value indicating whether to leave the stream open after
                // the IExcelDataReader object is disposed. Default: false
                LeaveOpen = false,
                // Gets or sets a value indicating the number of rows to analyze for
                // encoding, separator and field count in a CSV. When set, this option
                // causes the IExcelDataReader.RowCount property to throw an exception.
                // Default: 0 - analyzes the entire file (CSV only, has no effect on other
                // formats)
                AnalyzeInitialCsvRows = 0,
            };
            int progress = 0;
            //读DataSet配置
            ExcelDataSetConfiguration excelDataSetConfiguration = new ExcelDataSetConfiguration()
            {
                // Gets or sets a value indicating whether to set the DataColumn.DataType
                // property in a second pass.
                UseColumnDataType = true,

                // Gets or sets a callback to determine whether to include the current sheet
                // in the MyTable. Called once per sheet before ConfigureDataTable.
                FilterSheet = (tableReader, sheetIndex) => true,

                // Gets or sets a callback to obtain configuration options for a SelectedDataTableInfo.
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    // Gets or sets a value indicating the prefix of generated column names.
                    EmptyColumnNamePrefix = "EmptyColumn",

                    // Gets or sets a value indicating whether to use a row from the
                    // data as column names.
                    UseHeaderRow = true,
                    // Gets or sets a callback to determine which row is the header row.
                    // Only called when UseHeaderRow = true.
                    ReadHeaderRow = (rowReader) =>
                    {
                        // F.ex skip the first row and use the 2nd row as column headers:
                        //rowReader.Read();
                    },

                    // Gets or sets a callback to determine whether to include the
                    // current row in the SelectedDataTableInfo.
                    FilterRow = (rowReader) =>
                    {
                        progress = (int)Math.Ceiling(rowReader.Depth / (decimal)rowReader.RowCount * 100);
                        // progress is in the range 0..100
                        return true;
                    },

                    // Gets or sets a callback to determine whether to include the specific
                    // column in the SelectedDataTableInfo. Called once per column after reading the
                    // headers.
                    FilterColumn = (rowReader, columnIndex) =>
                    {
                        return true;
                    }
                }
            };

            #endregion 读取规则

            try
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //using var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read);
                    IExcelDataReader reader;
                    switch (extension.ToLower())
                    {
                        case ".csv": reader = ExcelReaderFactory.CreateCsvReader(stream, excelReaderConfiguration); break;
                        case ".xlsx": reader = ExcelReaderFactory.CreateReader(stream, excelReaderConfiguration); break;
                        default:
                            message = "读表失败";
                            return null;
                    }

                    var dataSet = reader.AsDataSet(excelDataSetConfiguration);
                    if (dataSet.Tables.Count > 0)
                    {
                        var table = dataSet.Tables[0];
                        table.TableName = fileNameWithoutExtension;
                        message = "读表成功";
                        return table;
                    }
                    message = "文件不存在表格";
                    return null;
                }
            }
            catch
            {
                message = "读表失败";
                return null;
            }
        }
    }
}
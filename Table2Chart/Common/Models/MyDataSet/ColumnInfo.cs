using ImTools;
using Newtonsoft.Json;
using Prism.Mvvm;
using StringMath;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Table2Chart.Common.Collection;

namespace Table2Chart.Common.Models.MyDataSet
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class ColumnInfo : BindableBase
    {
        public ColumnInfo()
        {
            Init();
        }

        private void Init()
        {
            //将当前实例加入进去，以方便计算
            ExpressionCalculators.WhenAdded(x =>
            {
                x.SetColumnInfo(this);
                foreach (var item in x.Inputs)
                {
                    item.SetExpressionCalculator(x);
                }
            });
        }

        /// <summary>
        /// JSON构造器，json加载数据进入
        /// </summary>
        /// <param name="isVisible"></param>
        /// <param name="name"></param>
        /// <param name="expressionCalculators"></param>
        /// <param name="columnBaseInfos"></param>
        [JsonConstructor]
        public ColumnInfo(bool isVisible, string name,
            NodifyObservableCollection<ExpressionCalculator> expressionCalculators,
            ObservableCollection<ColumnBaseInfo> columnBaseInfos)
        {
            _IsVisible = isVisible;
            _Name = name;
            _ExpressionCalculators = expressionCalculators;
            _ColumnBaseInfos = columnBaseInfos;
            foreach (var item in _ExpressionCalculators)
            {
                item.SetColumnInfo(this);
            }
            Init();
        }

        /// <summary>
        /// 当此列信息变为可见时
        /// </summary>
        public event Action<string, bool> IsVisibleChanged;

        private bool _IsVisible;
        private string _Name;
        private double _Max;
        private double _Min;
        private double _Median;
        private double _Sum;
        private double _Average;
        private double _Variance;
        private double _StandardDeviation;
        private readonly NodifyObservableCollection<ExpressionCalculator> _ExpressionCalculators = new NodifyObservableCollection<ExpressionCalculator>();
        private readonly ObservableCollection<ColumnBaseInfo> _ColumnBaseInfos = new ObservableCollection<ColumnBaseInfo>();

        /// <summary>
        /// 是否可显示于界面
        /// </summary>
        public bool IsVisible
        {
            get { return _IsVisible; }
            set { SetProperty(ref _IsVisible, value); IsVisibleChanged?.Invoke(_Name, value); }
        }

        public void SetIsVisible(bool isVisible)
        {
            _IsVisible = isVisible;
            RaisePropertyChanged(nameof(IsVisible));
        }

        /// <summary>
        /// 当前列的名字
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        [JsonIgnore]
        [Description("最大值")]
        public double Max
        {
            get { return _Max; }
            private set { SetProperty(ref _Max, value); }
        }

        [JsonIgnore]
        [Description("最小值")]
        public double Min
        {
            get { return _Min; }
            private set { SetProperty(ref _Min, value); }
        }

        [JsonIgnore]
        [Description("中位数")]
        public double Median
        {
            get { return _Median; }
            private set { SetProperty(ref _Median, value); }
        }

        [JsonIgnore]
        [Description("总和")]
        public double Sum
        {
            get { return _Sum; }
            private set { SetProperty(ref _Sum, value); }
        }

        [JsonIgnore]
        [Description("平均值")]
        public double Average
        {
            get { return _Average; }
            private set { SetProperty(ref _Average, value); }
        }

        [JsonIgnore]
        [Description("方差")]
        public double Variance
        {
            get { return _Variance; }
            private set { SetProperty(ref _Variance, value); }
        }

        [JsonIgnore]
        [Description("标准差")]
        public double StandardDeviation
        {
            get { return _StandardDeviation; }
            private set { SetProperty(ref _StandardDeviation, value); }
        }

        /// <summary>
        /// 表达式计算器
        /// </summary>
        public NodifyObservableCollection<ExpressionCalculator> ExpressionCalculators
        {
            get { return _ExpressionCalculators; }
            //set { SetProperty(ref _ExpressionCalculators, value); }
        }

        /// <summary>
        /// 用于显示于界面的4列基本信息
        /// </summary>
        public ObservableCollection<ColumnBaseInfo> ColumnBaseInfos
        {
            get { return _ColumnBaseInfos; }
            //set { SetProperty(ref _ColumnBaseInfos, value); }
        }

        /// <summary>
        /// 更新 ColumnBaseInfos 里的内容
        /// </summary>
        private void UpdateColumnBaseInfos()
        {
            foreach (var item in ColumnBaseInfos)
            {
                item.Value = (double)GetType().GetProperty(item.PropertyName).GetValue(this);
            }

            //ColumnBaseInfo columnBaseInfo = null;
            ////遍历属性，是否有对应特性名称的属性，
            ////需要确保前端传递的command字符和对应属性名字一样
            //foreach (var item in columnInfo.GetType().GetProperties())
            //{
            //    var descriptions = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //    if (item.PropertyName == command)
            //    {
            //        columnBaseInfo = new ColumnBaseInfo();
            //        columnBaseInfo.PropertyName = descriptions[0].Description;
            //        columnBaseInfo.Value = (double)item.GetValue(columnInfo);
            //    }
            //}
        }

        /// <summary>
        /// 列信息变化，更新所有的计算
        /// </summary>
        private void UpdateExpressionCalculators()
        {
            foreach (var calculator in _ExpressionCalculators)
            {
                try
                {
                    foreach (var input in calculator.Inputs)
                    {
                        input.OnStringValueChange();
                    }//input赋值后
                    MathExpr _expr = calculator.Expression.ToMathExpr();
                    calculator.OnInputValueChanged(_expr);
                }
                catch { }
            }
        }

        /// <summary>
        /// 计算更新基础信息
        /// </summary>
        /// <param name="data"></param>
        public void CalculateBaseInfo(IEnumerable<double> data)
        {
            Max = data.Max();
            Min = data.Min();
            Average = data.Average();
            Sum = data.Sum();
            Variance = CalculateVariance(data);
            StandardDeviation = Math.Sqrt(Variance);
            Median = CalculateMedian(data);
            UpdateColumnBaseInfos();
            UpdateExpressionCalculators();
        }

        /// <summary>
        /// 计算方差
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public double CalculateVariance(IEnumerable<double> data)
        {
            double ave = data.Average();

            double sum = 0;
            foreach (var item in data)
            {
                sum += Math.Pow(item - ave, 2);
            }
            return sum / ave;
        }

        /// <summary>
        /// 计算中位数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public double CalculateMedian(IEnumerable<double> data)
        {
            var lis = data.ToList();
            lis.Sort();
            var len = lis.Count;
            double med = len % 2 == 0 ? (lis[len / 2] + lis[len / 2 - 1]) / 2 : lis[len / 2];
            return med;
        }
    }

    /// <summary>
    /// 列的基本信息，用于显示于界面的内容
    /// </summary>
    public class ColumnBaseInfo : BindableBase
    {
        public ColumnBaseInfo(string propertyName)
        {
            PropertyName = propertyName;
        }

        public readonly string PropertyName;
        ///// <summary>
        ///// 用于分辨
        ///// </summary>
        //public string PropertyName
        //{
        //    get { return _PropertyName; }
        //    set { SetProperty(ref _PropertyName, value); }
        //}

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }

        private double _Value;

        public double Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }
    }

    /// <summary>
    /// 表达式计算器
    /// </summary>
    public class ExpressionCalculator : BindableBase
    {
        public ExpressionCalculator()
        {
            Init();
        }

        private void Init()
        {       //Inputs新成员加入时
            Inputs.WhenAdded(x =>
            {
                x.SetExpressionCalculator(this);
                x.PropertyChanged += OnInputValueChanged;
            })
            .WhenRemoved(x =>
            {
                x.PropertyChanged -= OnInputValueChanged;
            });
        }

        /// <summary>
        /// 当input的Value变化时，运行计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInputValueChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ExpressionInput.Value))
            {
                try
                {
                    MathExpr _expr = Expression.ToMathExpr();
                    OnInputValueChanged(_expr);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// json构造器，
        /// </summary>
        /// <param name="name"></param>
        /// <param name="outputValue"></param>
        /// <param name="inputs"></param>
        /// <param name="expression"></param>
        [JsonConstructor]
        public ExpressionCalculator(string name, double outputValue,
            NodifyObservableCollection<ExpressionInput> inputs, string expression)
        {
            _Title = name;
            _OutputValue = outputValue;
            _Inputs = inputs;
            _Expression = expression;
            foreach (var item in _Inputs)
            {
                item.SetExpressionCalculator(this);
                item.PropertyChanged += OnInputValueChanged;
            }
            Init();
        }

        private string _Title = "××值";
        private double _OutputValue = 0;
        private readonly NodifyObservableCollection<ExpressionInput> _Inputs = new NodifyObservableCollection<ExpressionInput>();
        private ColumnInfo _ColumnInfo;
        private string _Expression;

        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        public double OutputValue
        {
            get { return _OutputValue; }
            private set { SetProperty(ref _OutputValue, value); }
        }

        public NodifyObservableCollection<ExpressionInput> Inputs
        {
            get { return _Inputs; }
            //set { SetProperty(ref _Inputs, value); }
        }

        [JsonIgnore]
        public ColumnInfo ColumnInfo
        {
            get { return _ColumnInfo; }
            //set { SetProperty(ref _ColumnInfo, value); }
        }

        public void SetColumnInfo(ColumnInfo columnInfo)
        {
            _ColumnInfo = columnInfo;
        }

        public string Expression
        {
            get { return _Expression; }
            set { SetProperty(ref _Expression, value); GenerateInput(); }//表达式被修改后，更新Inputs
        }

        private void GenerateInput()
        {
            try
            {
                MathExpr expr;
                expr = Expression.ToMathExpr();
                //移除Inputs中所有不存在于expr表达式的变量。
                ExpressionInput[] toRemove = Inputs.Where(i => !expr.LocalVariables.Contains(i.Name)).ToArray();
                toRemove.ForEach(i => Inputs.Remove(i));

                //这里为，添加表达式中存在变量到Inputs里，如果有重复则不添加
                HashSet<string> existingVars = Inputs.Select(i => i.Name).Where(n => n != null).ToHashSet();
                foreach (string variable in expr.LocalVariables.Except(existingVars))
                {
                    Inputs.Add(new ExpressionInput
                    {
                        Name = variable,
                    });
                }

                OnInputValueChanged(expr);
            }
            catch
            {
                //Inputs.Clear();
                OutputValue = 0;
            }
        }

        public void OnInputValueChanged(MathExpr expr)
        {
            try
            {
                var v = expr.Variables;//这个可以获取对应变量，a,b,c,d
                var inputsName = Inputs.Select(x => x.Name);
                var variables = expr.Variables;

                #region 修改部分

                ////如果Inputs 存在对应变量则替换，否则替换为0。
                //foreach (var variable in variables)
                //{
                //    //如果Inputs中不存在这个变量，则将其赋值为0；
                //    if (!inputsName.Contains(variable))
                //    {
                //        expr.Substitute(variable, 0);
                //    }
                //    else
                //    {
                //        var input = Inputs.FirstOrDefault((x => x.Name == variable));
                //        expr.Substitute(input.Name, input.Value);
                //    }
                //}

                #endregion 修改部分

                //将Inpus的参数全部替换到表达式中
                foreach (var item in Inputs)
                {
                    expr.Substitute(item.Name, item.Value);
                }

                OutputValue = expr.Result;
            }
            catch
            {
                OutputValue = 0;
            }
        }
    }

    /// <summary>
    /// Input的变量
    /// </summary>
    public class ExpressionInput : BindableBase
    {
        private string _Name;
        private double _Value = 0;
        private string _StringValue = "0";
        private ExpressionCalculator _ExpressionCalculator;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (!string.IsNullOrEmpty(value) &&
                    _ExpressionCalculator?.Inputs?.FirstOrDefault(x => x.Name == value) == null &&
                    !double.TryParse(value, out _))
                {
                    SetProperty(ref _Name, value);
                }
            }
        }

        public double Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }

        public string StringValue
        {
            get { return _StringValue; }
            set
            {
                SetProperty(ref _StringValue, value);
                OnStringValueChange();
            }
        }

        /// <summary>
        /// 当前的位置的ExpressionCalculator
        /// </summary>
        [JsonIgnore]
        public ExpressionCalculator ExpressionCalculator
        {
            get { return _ExpressionCalculator; }
            //set { SetProperty(ref _ExpressionCalculator, value); }
        }

        public void SetExpressionCalculator(ExpressionCalculator expressionCalculator)
        {
            _ExpressionCalculator = expressionCalculator;
        }

        public void OnStringValueChange()
        {
            if (ExpressionCalculator?.ColumnInfo == null) return;
            if (_StringValue == null) return;
            var columnInfo = ExpressionCalculator.ColumnInfo;

            if (_StringValue == "Pi")
            {
                Value = Math.PI;
            }
            else if (_StringValue == "E")
            {
                Value = Math.E;
            }
            else if (double.TryParse(_StringValue, out double d))
            {
                Value = d;
            }
            else
            {
                Value = 0;
                //如果为选择的属性
                foreach (var item in columnInfo.GetType().GetProperties())
                {
                    var o = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (o?.Length > 0 &&
                        _StringValue.Equals(o[0].Description))
                    {
                        Value = (double)item.GetValue(columnInfo);
                        break;
                    }
                }
            }
        }
    }
}
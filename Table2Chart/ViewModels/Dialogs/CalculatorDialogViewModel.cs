using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using StringMath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Table2Chart.Common.Models.MyDataSet;
using Table2Chart.Extensions;

namespace Table2Chart.ViewModels.Dialogs
{
    /// <summary>
    /// 计算器的对话窗口
    /// </summary>
    public class CalculatorDialogViewModel : BindableBase, IDialogAware
    {
        public readonly IEventAggregator aggregator;

        private IEnumerable<string> _ColumnInfoProperties;

        private ExpressionCalculator _ExpressionCalculator = new ExpressionCalculator();

        private bool _IsEdit;

        private ColumnInfo _Model;

        private string _Title = "_Title";

        public CalculatorDialogViewModel(IContainerProvider containerProvider)
        {
            aggregator = containerProvider.Resolve<IEventAggregator>();
        }

        public event Action<IDialogResult> RequestClose;
        public IEnumerable<string> ColumnInfoProperties
        {
            get { return _ColumnInfoProperties; }
            set { SetProperty(ref _ColumnInfoProperties, value); }
        }

        public ICommand ExecuteCommand => new DelegateCommand<string>((o) =>
        {
            switch (o)
            {
                case "RunMathExpr":
                    RunMathExpr();
                    break;

                case "AddInput":
                    AddInput();
                    break;

                default:
                    break;
            }
        });

        public ExpressionCalculator ExpressionCalculator
        {
            get { return _ExpressionCalculator; }
            set { SetProperty(ref _ExpressionCalculator, value); }
        }

        //public IEnumerable<string> ColumnInfoProperties;
        public ColumnInfo Model
        {
            get { return _Model; }
            set { SetProperty(ref _Model, value); }
        }

        public ICommand RemoveInputCommand => new DelegateCommand<ExpressionInput>((o) =>
        {
            ExpressionCalculator.Inputs.Remove(o);
        });

        /// <summary>
        /// 对话窗口的名字
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            if (!_IsEdit)
            {
                Model.ExpressionCalculators.Add(ExpressionCalculator);
            }
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("IsEdit"))
                _IsEdit = parameters.GetValue<bool>("IsEdit");

            if (_IsEdit)
            {
                if (parameters.ContainsKey("Model"))
                    this.ExpressionCalculator = parameters.GetValue<ExpressionCalculator>("Model");
                Model = ExpressionCalculator.ColumnInfo;
            }
            else
            {
                if (parameters.ContainsKey("Model"))
                    Model = parameters.GetValue<ColumnInfo>("Model");
                ExpressionCalculator.SetColumnInfo(Model);
            }

            Title = "列名: " + Model.Name;
            List<string> PropertiesName = new List<string>();
            foreach (var item in Model.GetType().GetProperties())
            {
                var o = (DescriptionAttribute[])item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (o?.Length > 0)
                {
                    PropertiesName.Add(o[0].Description);
                }
            }
            ColumnInfoProperties = PropertiesName;
        }

        private void AddInput()
        {
            string inputName = "var";

            inputName = GetUniquename();

            ExpressionInput expressionInput = new ExpressionInput();
            //expressionInput.SetExpressionCalculator(ExpressionCalculator);
            expressionInput.Value = 0;
            expressionInput.Name = inputName;
            ExpressionCalculator.Inputs.Add(expressionInput);
        }

        private string GetUniquename()
        {
            //ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
            //List<byte> ints = new List<byte>();
            //byte count = 97;
            //ints.Add(count);
            //string temName = aSCIIEncoding.GetString(ints.ToArray());
            ////a--97
            ////z--122

            //while (ExpressionCalculator.Inputs.FirstOrDefault(x => x.PropertyName == temName) != null)
            //{
            //    count++;
            //    ints[ints.Count - 1] = count;
            //    temName = aSCIIEncoding.GetString(ints.ToArray());
            //    if (count >= 122)
            //    {
            //        count = 96;
            //        ints.RemoveAt(ints.Count - 1);
            //        ints.Add(97);
            //        ints.Add(count);
            //    }
            //}
            int i = 1;
            string temName = ReaderExtensions.ExcelColumnFromNumber(i);
            while (ExpressionCalculator.Inputs.FirstOrDefault(x => x.Name == temName) != null &&
                !string.IsNullOrEmpty(temName))
            {
                i++;
                temName = ReaderExtensions.ExcelColumnFromNumber(i);
            }

            return temName;
        }

        private void RunMathExpr()
        {
            try
            {
                MathExpr expr;
                expr = ExpressionCalculator.Expression.ToMathExpr();
                ExpressionCalculator.OnInputValueChanged(expr);
                aggregator.SendMessage("已运行");
            }
            catch
            {
                aggregator.SendMessage("表达式存在问题！！");
            }
        }
        //void ExecuteCommandName()
        //{
        //    DialogParameters p = new DialogParameters();
        //    RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        //}
    }
}
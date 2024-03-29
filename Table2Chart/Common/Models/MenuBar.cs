﻿using Prism.Mvvm;

namespace Table2Chart.Common.Models
{
    /// <summary>
    /// 系统导航菜单实体类
    /// </summary>
    public class MenuBar : BindableBase
    {
        private string _IconKind;
        private string _Title;
        private string _NameSpace;

        public string IconKind
        {
            get { return _IconKind; }
            set { _IconKind = value; }
        }

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public string NameSpace
        {
            get { return _NameSpace; }
            set { _NameSpace = value; }
        }
    }
}
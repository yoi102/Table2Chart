using System;

namespace Table2Chart.Attributes
{
    [Obsolete("未使用")]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FilterPropertyAttribute : Attribute
    {
        public FilterPropertyAttribute()
        {
        }
    }
}
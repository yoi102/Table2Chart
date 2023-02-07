using Prism.Events;

namespace Table2Chart.Common.Events
{
    public class UpdateModel
    {
        public bool IsOpen { get; set; }
    }

    /// <summary>
    /// 用于导航转换页面时，转圈加载
    /// </summary>
    public class UpdateLoadingEvent : PubSubEvent<UpdateModel>
    {
    }
}
using Prism.Events;

namespace Table2Chart.Common.Events
{
    public class MessageModel
    {
        public string Filter { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 右下角的消息
    /// </summary>
    public class MessageEvent : PubSubEvent<MessageModel>
    {
    }
}
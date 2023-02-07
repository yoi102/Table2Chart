using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace Table2Chart.Common.Collection
{
    /// <summary>
    /// 如何使的 ObservableCollection 线程安全的？(How to make ObservableCollection thread-safe?)
    /// 弃用，有问题
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete()]
    public class MTObservableCollection<T> : ObservableCollection<T>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler CollectionChanged = this.CollectionChanged;

            if (CollectionChanged != null)
                foreach (NotifyCollectionChangedEventHandler nh in CollectionChanged.GetInvocationList())
                {
                    DispatcherObject dispObj = nh.Target as DispatcherObject;
                    if (dispObj != null)
                    {
                        Dispatcher dispatcher = dispObj.Dispatcher;
                        //if (dispatcher != null && !dispatcher.CheckAccess())
                        //{
                        //    dispatcher.BeginInvoke(
                        //        () => nh.Invoke(this,
                        //            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)),
                        //        DispatcherPriority.DataBind);
                        //    continue;
                        //}
                    }
                    nh.Invoke(this, e);
                }
        }
    }
}
using System;

namespace Numbers.Web.Generic
{
    public enum NotifyCollectionChangedAction { Add, Remove, Reset }

    public class NotifyCollectionChangedEventArgs : EventArgs
    {
        public NotifyCollectionChangedAction Action { get; private set; }

        public object Item { get; private set; }

        public int Index { get; private set; }

        private NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object item, int index)
        {
            this.Action = action;
            this.Item = item;
            this.Index = index;
        }

        public static NotifyCollectionChangedEventArgs CreateAdd(object item, int index)
        {
            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
        }

        public static NotifyCollectionChangedEventArgs CreateRemove(object item, int index)
        {
            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
        }

        public static NotifyCollectionChangedEventArgs CreateReset()
        {
            return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, null, -1);
        }
    }

    public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);

    public interface INotifyCollectionChanged
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}

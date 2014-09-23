using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Numbers.Web.Generic
{
    public interface IObservableEnumerable<T> : IEnumerable<T>, INotifyCollectionChanged
    {
        //
    }

    public interface IObservableCollection<T> : IObservableEnumerable<T>, IList<T>
    {
        //
    }

    [DefaultMemberReflectability(MemberReflectability.None)]
    public class ObservableCollection<T> : IObservableCollection<T>, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs("Count");

        public int Count { get { return items.Count; } }

        public T this[int index]
        {
            get { return items[index]; }
            set
            {
                RaiseCollectionChanged(NotifyCollectionChangedEventArgs.CreateRemove(items[index], index));
                items[index] = value;
                RaiseCollectionChanged(NotifyCollectionChangedEventArgs.CreateAdd(value, index));
            }
        }

        private List<T> items;

        public ObservableCollection()
        {
            items = new List<T>();
        }

        public void Add(T item)
        {
            items.Add(item);
            RaiseCollectionChanged(NotifyCollectionChangedEventArgs.CreateAdd(item, Count - 1));
            RaisePropertyChanged(CountPropertyChangedEventArgs);
        }

        public void Insert(int index, T item)
        {
            if (index > items.Count)
            {
                Add(item);
            }
            else
            {
                if (index < 0)
                {
                    index = 0;
                }

                items.Insert(index, item);
                RaiseCollectionChanged(NotifyCollectionChangedEventArgs.CreateAdd(item, index));
                RaisePropertyChanged(CountPropertyChangedEventArgs);
            }
        }

        public bool Remove(T item)
        {
            int index = items.IndexOf(item);

            if (index == -1)
            {
                return false;
            }

            items.Remove(item);
            RaiseCollectionChanged(NotifyCollectionChangedEventArgs.CreateRemove(item, index));
            RaisePropertyChanged(CountPropertyChangedEventArgs);

            return true;
        }

        public void RemoveAt(int index)
        {
            T item = items[index];
            items.RemoveAt(index);
            RaiseCollectionChanged(NotifyCollectionChangedEventArgs.CreateRemove(item, index));
            RaisePropertyChanged(CountPropertyChangedEventArgs);
        }

        public void Clear()
        {
            if (Count > 0)
            {
                items.Clear();
                RaiseCollectionChanged(NotifyCollectionChangedEventArgs.CreateReset());
                RaisePropertyChanged(CountPropertyChangedEventArgs);
            }
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }
    }
}

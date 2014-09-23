using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Numbers.Web.Generic
{
    [DefaultMemberReflectability(MemberReflectability.None)]
    public class ConvertedObservableCollection<S, T> : IObservableEnumerable<T>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly IEnumerable<S> sourceCollection;
        private readonly ObservableCollection<T> convertedCollection;
        private readonly Func<S, T> convertItem;
        private readonly Action<T> detachItem;

        public ConvertedObservableCollection(IObservableEnumerable<S> sourceCollection, Func<S, T> convertItem, Action<T> detachItem = null)
        {
            this.sourceCollection = sourceCollection;
            this.convertedCollection = new ObservableCollection<T>();

            this.convertItem = convertItem;
            this.detachItem = detachItem;

            Build();

            sourceCollection.CollectionChanged += SourceCollectionChanged;
            convertedCollection.CollectionChanged += ConvertedCollectionChanged;
        }

        private void Build()
        {
            foreach (S sourceItem in sourceCollection)
            {
                convertedCollection.Add(convertItem(sourceItem));
            }
        }

        private void Clear()
        {
            if (detachItem != null)
            {
                foreach (T convertedItem in convertedCollection)
                {
                    detachItem(convertedItem);
                }
            }

            convertedCollection.Clear();
        }

        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (eventArgs.Action == NotifyCollectionChangedAction.Add)
            {
                convertedCollection.Insert(eventArgs.Index, convertItem((S)eventArgs.Item));
            }

            if (eventArgs.Action == NotifyCollectionChangedAction.Remove)
            {
                if (detachItem != null)
                {
                    detachItem(convertedCollection[eventArgs.Index]);
                }

                convertedCollection.RemoveAt(eventArgs.Index);
            }

            if (eventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                Clear();
                Build();
            }
        }

        private void ConvertedCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, eventArgs);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return convertedCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return convertedCollection.GetEnumerator();
        }
    }
}

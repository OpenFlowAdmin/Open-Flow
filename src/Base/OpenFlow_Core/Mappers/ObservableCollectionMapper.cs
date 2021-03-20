using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core
{
    public class ObservableCollectionMapper<TIn, TOut>
    {
        private readonly ITypeMapper<TIn, TOut> mapper;
        private readonly ObservableCollection<TOut> mapTo;

        public static ReadOnlyObservableCollection<TOut> Create(INotifyCollectionChanged collection, ITypeMapper<TIn, TOut> mapper)
        {
            ObservableCollection<TOut> output = new ObservableCollection<TOut>();
            _ = new ObservableCollectionMapper<TIn, TOut>(output, collection, mapper);
            return new ReadOnlyObservableCollection<TOut>(output);
        }

        private ObservableCollectionMapper(ObservableCollection<TOut> mapTo, INotifyCollectionChanged mapFrom, ITypeMapper<TIn, TOut> mapper)
        {
            this.mapper = mapper;
            this.mapTo = mapTo;
            foreach (object item in (mapFrom as IList))
            {
                if (item is TIn itemOut)
                {
                    mapTo.Add(mapper.MapType(itemOut));
                }
            }

            mapFrom.CollectionChanged += Collection_CollectionChanged;
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    InsertRange(e.NewStartingIndex, e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveRange(e.OldStartingIndex, e.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveRange(e.OldStartingIndex, e.OldItems.Count);
                    InsertRange(e.NewStartingIndex, e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemoveRange(0, mapTo.Count);
                    break;
                case NotifyCollectionChangedAction.Move:
                    RemoveRange(e.OldStartingIndex, e.OldItems.Count);
                    InsertRange(e.NewStartingIndex, e.NewItems);
                    break;
            }
        }

        private void InsertRange(int index, IList list)
        {
            index = index == -1 ? mapTo.Count - 1 : index;
            int j = 0;
            foreach (object item in list)
            {
                if (item is TIn itemIn)
                {
                    mapTo.Insert(index + j, mapper.MapType(itemIn));
                    j++;
                }
            }
        }

        private void RemoveRange(int index, int count)
        {
            index = index == -1 ? mapTo.Count - 1 : index;
            for (int i = 0; i < count; i++)
            {
                mapTo.RemoveAt(i + index);
            }
        }
    }
}

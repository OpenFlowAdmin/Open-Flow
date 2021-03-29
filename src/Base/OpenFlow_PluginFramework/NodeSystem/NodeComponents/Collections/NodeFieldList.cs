using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    public class NodeFieldList : INotifyCollectionChanged, IList, IList<VisualNodeComponent>
    {
        private readonly ObservableCollection<NodeComponent> _components;

        public NodeFieldList(ObservableCollection<NodeComponent> nodeComponents)
        {
            nodeComponents.CollectionChanged += Components_CollectionChanged;
            _components = nodeComponents;
            ComponentsAdded(nodeComponents);
        }

        public int Count { get; private set; } = 0;

        public bool IsFixedSize => false;

        public bool IsReadOnly => true;

        public bool IsSynchronized => false;

        public object SyncRoot => false;

        VisualNodeComponent IList<VisualNodeComponent>.this[int index] { get => this[index] as VisualNodeComponent; set => this[index] = value; }

        public object this[int index] 
        { 
            get
            {
                Debug.WriteLine("Slow indexer called, don't like, bad");
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                IEnumerator<VisualNodeComponent> enumer = GetEnumerator();
                for (int i = 0; i < index; i++)
                {
                    enumer.MoveNext();
                }

                return enumer.Current;
            }
            set => throw new NotSupportedException();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<VisualNodeComponent> GetEnumerator()
        {
            foreach (NodeComponent component in _components)
            {
                if (!component.IsVisible)
                {
                    continue;
                }

                foreach (VisualNodeComponent field in component.VisualComponentList)
                {
                    yield return field;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(object value)
        {
            if (value is not VisualNodeComponent)
            {
                throw new InvalidCastException($"{nameof(NodeFieldList)} only stores valus of type {nameof(VisualNodeComponent)}");
            }
            return Contains(value as VisualNodeComponent);
        }

        public bool Contains(VisualNodeComponent item) => IndexOf(item) != -1;

        public int IndexOf(object value) => GetIndexOf(value);

        public int IndexOf(VisualNodeComponent item) => GetIndexOf(item);

        public void CopyTo(Array array, int index) => CopyTo(array as VisualNodeComponent[], index);

        public void CopyTo(VisualNodeComponent[] array, int arrayIndex)
        {
            IEnumerator<VisualNodeComponent> enumer = GetEnumerator();
            int i = 0;
            while (enumer.MoveNext())
            {
                array[i + arrayIndex] = enumer.Current;
                i++;
            }
        }

        public bool Remove(VisualNodeComponent item) => throw new NotSupportedException();

        public void Insert(int index, object value) => throw new NotSupportedException();

        public void Remove(object value) => throw new NotSupportedException();

        public void RemoveAt(int index) => throw new NotSupportedException();

        public int Add(object value) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public void Insert(int index, VisualNodeComponent item) => throw new NotSupportedException();

        public void Add(VisualNodeComponent item) => throw new NotSupportedException();

        private int GetLinearIndexFromNested(int nested)
        {
            int output = 0;
            for (int i = 0; i < nested; i++)
            {
                output += _components[i].VisualComponentList.Count;
            }
            return output;
        }

        private void Components_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ComponentsAdded(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ComponentsRemoved(e.OldItems, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ComponentsRemoved(e.OldItems, e.OldStartingIndex);
                    ComponentsAdded(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Count = 0;
                    CollectionChanged?.Invoke(this, e);
                    break;
                case NotifyCollectionChangedAction.Move:
                    ComponentsRemoved(e.OldItems, e.OldStartingIndex);
                    ComponentsAdded(e.NewItems, e.NewStartingIndex);
                    break;
            }
        }

        private void ComponentsRemoved(IList components, int index)
        {
            foreach (NodeComponent component in components)
            {
                if (component is NodeComponentCollection componentCollection)
                {
                    componentCollection.VisualComponentList.CollectionChanged -= Child_NodeFieldsList_Changed;
                }

                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, component.VisualComponentList, GetLinearIndexFromNested(index)));
                Count -= component.VisualComponentList.Count;
                component.VisibilityChanged -= NodePart_VisibilityChanged;
            }
        }

        private void ComponentsAdded(IList components, int index = -1)
        {
            foreach (NodeComponent component in components)
            {
                if (component is NodeComponentCollection componentCollection)
                {
                    componentCollection.VisualComponentList.CollectionChanged += Child_NodeFieldsList_Changed;
                }

                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, component.VisualComponentList, GetLinearIndexFromNested(index)));
                Count += component.VisualComponentList.Count;
                component.VisibilityChanged += NodePart_VisibilityChanged;
            }
        }

        private void NodePart_VisibilityChanged(object sender, bool e)
        {
            if (sender is NodeComponent component)
            {
                if (e)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, component.VisualComponentList, GetLinearIndexFromNested(_components.IndexOf(component))));
                    Count += component.VisualComponentList.Count;
                }
                else
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, component.VisualComponentList, GetLinearIndexFromNested(_components.IndexOf(component))));
                    Count -= component.VisualComponentList.Count;
                }
            }
        }

        private void Child_NodeFieldsList_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Count += e.NewItems.Count;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Count -= e.OldItems.Count;
                    break;
                case NotifyCollectionChangedAction.Replace:
                    Count = Count - e.OldItems.Count + e.NewItems.Count;
                    break;
            }

            int i = 0;
            int startingIndex = 0;
            while (i < _components.Count && (_components[i] as NodeComponentCollection)?.VisualComponentList != sender)
            {
                startingIndex += _components[i].VisualComponentList.Count;
                i++;
            }

            CollectionChanged?.Invoke(this, ChangeCollectionChangedIndex(e, startingIndex, (sender as IList).Count));
        }

        private static NotifyCollectionChangedEventArgs ChangeCollectionChangedIndex(NotifyCollectionChangedEventArgs e, int offset, int listLength) 
        {
            int newStartingIndex = (e.NewStartingIndex == -1 ? listLength - 1 : e.NewStartingIndex) + offset;
            int oldStartingIndex = (e.OldStartingIndex == -1 ? listLength - 1 : e.OldStartingIndex) + offset;
            return (e.Action) switch
            {
                NotifyCollectionChangedAction.Add => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewItems, newStartingIndex),
                NotifyCollectionChangedAction.Remove => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldItems, oldStartingIndex),
                NotifyCollectionChangedAction.Replace => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewItems, e.OldItems, newStartingIndex),
                NotifyCollectionChangedAction.Move => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, newStartingIndex, oldStartingIndex),
                _ => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
            };
        }

        private int GetIndexOf(object value)
        {
            IEnumerator<VisualNodeComponent> enumer = GetEnumerator();
            for (int i = 0; i < Count; i++)
            {
                enumer.MoveNext();
                if (enumer.Current == value)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}

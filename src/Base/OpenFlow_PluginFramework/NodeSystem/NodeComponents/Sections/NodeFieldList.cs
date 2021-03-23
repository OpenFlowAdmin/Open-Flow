﻿using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
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
    public class NodeFieldList : INotifyCollectionChanged, IList, IList<NodeField>
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

        NodeField IList<NodeField>.this[int index] { get => this[index] as NodeField; set => this[index] = value; }

        public object this[int index] 
        { 
            get
            {
                Debug.WriteLine("Slow indexer called, don't like, bad");
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                IEnumerator<NodeField> enumer = GetEnumerator();
                for (int i = 0; i < index; i++)
                {
                    enumer.MoveNext();
                }

                return enumer.Current;
            }
            set => throw new NotSupportedException();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<NodeField> GetEnumerator()
        {
            foreach (NodeComponent component in _components)
            {
                if (!component.IsVisible)
                {
                    continue;
                }

                foreach (NodeField field in component.NodeFields)
                {
                    yield return field;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(object value)
        {
            return IndexOf(value) != -1;
        }

        public bool Contains(NodeField item) => Contains(item);

        public int IndexOf(object value)
        {
            IEnumerator<NodeField> enumer = GetEnumerator();
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

        public int IndexOf(NodeField item) => IndexOf(item);

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(NodeField[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(NodeField item) => throw new NotSupportedException();

        public void Insert(int index, object value) => throw new NotSupportedException();

        public void Remove(object value) => throw new NotSupportedException();

        public void RemoveAt(int index) => throw new NotSupportedException();

        public int Add(object value) => throw new NotSupportedException();

        public void Clear() => throw new NotSupportedException();

        public void Insert(int index, NodeField item) => throw new NotSupportedException();

        public void Add(NodeField item) => throw new NotSupportedException();

        private int GetLinearIndexFromNested(int nested)
        {
            int output = 0;
            for (int i = 0; i < nested; i++)
            {
                output += _components[i].NodeFields.Count;
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
                    componentCollection.NodeFields.CollectionChanged -= Child_NodeFieldsList_Changed;
                }

                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, component.NodeFields, GetLinearIndexFromNested(index)));
                Count -= component.NodeFields.Count;
                component.VisibilityChanged -= NodePart_VisibilityChanged;
            }
        }

        private void ComponentsAdded(IList components, int index = -1)
        {
            foreach (NodeComponent component in components)
            {
                if (component is NodeComponentCollection componentCollection)
                {
                    componentCollection.NodeFields.CollectionChanged += Child_NodeFieldsList_Changed;
                }

                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, component.NodeFields, GetLinearIndexFromNested(index)));
                Count += component.NodeFields.Count;
                component.VisibilityChanged += NodePart_VisibilityChanged;
            }
        }

        private void NodePart_VisibilityChanged(object sender, bool e)
        {
            if (sender is NodeComponent component)
            {
                if (e)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, component.NodeFields, GetLinearIndexFromNested(_components.IndexOf(component))));
                    Count += component.NodeFields.Count;
                }
                else
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, component.NodeFields, GetLinearIndexFromNested(_components.IndexOf(component))));
                    Count -= component.NodeFields.Count;
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
            while (i < _components.Count && (_components[i] as NodeComponentCollection)?.NodeFields != sender)
            {
                startingIndex += _components[i].NodeFields.Count;
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
    }
}

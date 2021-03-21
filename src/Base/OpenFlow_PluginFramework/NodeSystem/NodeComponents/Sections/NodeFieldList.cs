using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
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
        private readonly ObservableCollection<NodeComponent> parts;

        public NodeFieldList(ObservableCollection<NodeComponent> nodeParts)
        {
            nodeParts.CollectionChanged += NodeParts_CollectionChanged;
            parts = nodeParts;
            NodePartsAdded(nodeParts);
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
            foreach (NodeComponent part in parts)
            {
                if (!part.IsVisible)
                {
                    continue;
                }

                if (part is NodeField nodeField)
                {
                    yield return nodeField;
                }
                else if (part is NodeComponentCollection nodeSection)
                {
                    foreach (NodeField field in nodeSection.AllFields)
                    {
                        yield return field;
                    }
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
                output += parts[i].FieldCount;
            }
            return output;
        }

        private void NodeParts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("Something changed with the NodeComponentCollection I'm looking at");
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    NodePartsAdded(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    NodePartsRemoved(e.OldItems, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    NodePartsRemoved(e.OldItems, e.OldStartingIndex);
                    NodePartsAdded(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Count = 0;
                    CollectionChanged?.Invoke(this, e);
                    break;
                case NotifyCollectionChangedAction.Move:
                    NodePartsRemoved(e.OldItems, e.OldStartingIndex);
                    NodePartsAdded(e.NewItems, e.NewStartingIndex);
                    break;
            }
        }

        private void NodePartsRemoved(IList parts, int index)
        {
            foreach (object part in parts)
            {
                if (part is NodeComponentCollection componentCollection)
                {
                    componentCollection.AllFields.CollectionChanged -= Child_NodeFieldsList_Changed;
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, componentCollection.AllFields, GetLinearIndexFromNested(index)));
                    Count -= componentCollection.FieldCount;
                }
                else
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, parts, GetLinearIndexFromNested(index)));
                    Count -= 1;
                }
            }
        }

        private void NodePartsAdded(IList parts, int index = -1)
        {
            foreach (object part in parts)
            {
                if (part is NodeComponentCollection componentCollection)
                {
                    componentCollection.AllFields.CollectionChanged += Child_NodeFieldsList_Changed;
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, componentCollection.AllFields, GetLinearIndexFromNested(index)));
                    Count += componentCollection.FieldCount;
                }
                else
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, parts, GetLinearIndexFromNested(index)));
                    Count += 1;
                }
            }
        }

        private void Child_NodeFieldsList_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.WriteLine("One of my children's ComponentCollecitons changed");

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
            while (i < parts.Count && (parts[i] as NodeComponentCollection)?.AllFields != sender)
            {
                startingIndex += parts[i].FieldCount;
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

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    /// <summary>
    /// Defines a read only collection of NodeComponents, with protected list methods for children to make public
    /// </summary>
    public class NodeComponentCollection : NodeComponent, IEnumerable<NodeComponent>, INotifyCollectionChanged
    {
        private INode parentNode;
        private readonly ObservableCollection<NodeComponent> _subParts;

        public NodeComponentCollection() : this(Enumerable.Empty<NodeComponent>()) { }

        public NodeComponentCollection(IEnumerable<NodeComponent> subParts)
        {
            _subParts = new ObservableCollection<NodeComponent>(subParts);
            AllFields = new NodeFieldList(_subParts);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                _subParts.CollectionChanged += value;
            }
            
            remove
            {
                _subParts.CollectionChanged -= value;
            }
        }

        public NodeFieldList AllFields { get; }

        public override int FieldCount => AllFields.Count;

        public override INode ParentNode
        {
            get => parentNode;
            set
            {
                parentNode = value;
                foreach (NodeComponent part in _subParts)
                {
                    part.ParentNode = parentNode;
                }
            }
        }

        public int ComponentCount => _subParts.Count;

        protected NodeComponent this[Index index]
        {
            get => _subParts[index];
            set 
            {
                if (GetIndex(index) >= _subParts.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                _subParts[index] = value;
            }
        }

        public IEnumerator<NodeComponent> GetEnumerator() => _subParts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(NodeComponent component) => _subParts.IndexOf(component);

        public bool Contains(NodeComponent component) => _subParts.Contains(component);

        protected virtual void ProtectedAdd(NodeComponent newField)
        {
            _subParts.Add(newField);
            newField.ParentNode = ParentNode;
        }

        protected virtual void ProtectedInsert(int index, NodeComponent newField)
        {
            _subParts.Insert(index, newField);
            newField.ParentNode = ParentNode;
        }

        protected virtual void ProtectedRemoveAt(int index)
        {
            if (GetIndex(index) < _subParts.Count)
            {
                _subParts.RemoveAt(index);
            }
        }

        protected virtual bool ProtectedRemove(NodeComponent component)
        {
            return _subParts.Remove(component);
        }

        protected virtual void ProtectedReset()
        {
            for (int i = _subParts.Count - 1; i >= 0; i--)
            {
                ProtectedRemoveAt(i);
            }
        }

        protected virtual void ProtectedClear()
        {
            _subParts.Clear();
        }

        private int GetIndex(Index index) => index.IsFromEnd ? _subParts.Count - index.Value : index.Value;
    }
}

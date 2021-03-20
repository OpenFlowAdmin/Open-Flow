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

    public class NodeComponentCollection : NodeComponent, IEnumerable<NodeComponent>, INotifyCollectionChanged
    {
        private INode parentNode;
        private ObservableCollection<NodeComponent> _subParts;

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

        protected virtual void Add(NodeComponent newField)
        {
            _subParts.Add(newField);
            newField.ParentNode = ParentNode;
        }

        protected virtual void Insert(int index, NodeComponent newField)
        {
            _subParts.Insert(index, newField);
            newField.ParentNode = ParentNode;
        }

        protected virtual void RemoveAt(int index)
        {
            if (GetIndex(index) < _subParts.Count)
            {
                _subParts.RemoveAt(index);
            }
        }

        protected virtual bool Remove(NodeComponent component)
        {
            return _subParts.Remove(component);
        }

        protected virtual void Reset()
        {
            for (int i = 0; i < _subParts.Count; i++)
            {
                RemoveAt(i);
            }
        }

        protected virtual bool Contains(NodeComponent component) => _subParts.Contains(component);

        private int GetIndex(Index index) => index.IsFromEnd ? _subParts.Count - index.Value : index.Value;
    }
}

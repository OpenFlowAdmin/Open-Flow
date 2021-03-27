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
        private readonly ObservableCollection<NodeComponent> _childComponents;

        public NodeComponentCollection() : this(Enumerable.Empty<NodeComponent>()) { }

        public NodeComponentCollection(params NodeComponent[] childComponents) : this(childComponents.AsEnumerable()) { }

        public NodeComponentCollection(IEnumerable<NodeComponent> childComponents)
        {
            foreach (NodeComponent component in childComponents)
            {
                component.Opacity.AddOpacityFactor(Opacity);
            }

            _childComponents = new ObservableCollection<NodeComponent>(childComponents);
            VisualComponentList = new NodeFieldList(_childComponents);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                _childComponents.CollectionChanged += value;
            }
            
            remove
            {
                _childComponents.CollectionChanged -= value;
            }
        }

        public override NodeFieldList VisualComponentList { get; }

        public override INode ParentNode
        {
            get => parentNode;
            set
            {
                parentNode = value;
                foreach (NodeComponent component in _childComponents)
                {
                    component.ParentNode = parentNode;
                }
            }
        }

        public int ComponentCount => _childComponents.Count;

        protected NodeComponent this[Index index]
        {
            get => _childComponents[index];
            set 
            {
                if (GetIndex(index) >= _childComponents.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                _childComponents[index] = value;
            }
        }

        public IEnumerator<NodeComponent> GetEnumerator() => _childComponents.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(NodeComponent component) => _childComponents.IndexOf(component);

        public bool Contains(NodeComponent component) => _childComponents.Contains(component);

        public override NodeComponent Clone() => CloneTo(new NodeComponentCollection(_childComponents.Select(x => x.Clone())));

        protected virtual void ProtectedAdd(NodeComponent newComponent)
        {
            ProtectedInsert(_childComponents.Count, newComponent);
        }

        protected virtual void ProtectedInsert(int index, NodeComponent newComponent)
        {
            _childComponents.Insert(index, newComponent);
            newComponent.Opacity.AddOpacityFactor(Opacity);
            newComponent.ParentNode = ParentNode;
        }

        protected virtual void ProtectedRemoveAt(int index)
        {
            if (GetIndex(index) < _childComponents.Count)
            {
                _childComponents.RemoveAt(index);
            }
        }

        protected virtual bool ProtectedRemove(NodeComponent component)
        {
            component.Opacity.RemoveOpacityFactor(Opacity);
            return _childComponents.Remove(component);
        }

        protected virtual void ProtectedReset()
        {
            for (int i = _childComponents.Count - 1; i >= 0; i--)
            {
                ProtectedRemoveAt(i);
            }
        }

        private int GetIndex(Index index) => index.IsFromEnd ? _childComponents.Count - index.Value : index.Value;
    }
}

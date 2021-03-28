namespace OpenFlow_Core.Nodes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_Core.Nodes.VisualNodeComponentDisplays;

    public class NodeBase
    {
        private readonly INode _baseNode;
        private readonly NodeComponentCollection _fieldSection;
        private bool _errorState;
        private bool _evaluating;

        public NodeBase(INode baseNode)
        {
            _baseNode = baseNode;
            _fieldSection = new NodeComponentCollection(baseNode.Fields)
            {
                ParentNode = baseNode
            };

            Fields = ObservableCollectionMapper<VisualNodeComponent, IVisualNodeComponentDisplay>.Create(_fieldSection.VisualComponentList, new VisualNodeComponentMapper(this));

            baseNode.SubscribeToEvaluate(TryEvaluate);

            TryEvaluate();
        }

        public event EventHandler<bool> ErrorStateChanged;

        public bool ErrorState
        {
            get => _errorState;
            private set
            {
                if (value != _errorState)
                {
                    _errorState = value;
                    ErrorStateChanged?.Invoke(this, _errorState);
                }
            }
        }

        public double X { get; set; }

        public double Y { get; set; }

        public object Tag { get; set; }

        public string Name => _baseNode.NodeName;

        public ReadOnlyObservableCollection<IVisualNodeComponentDisplay> Fields { get; }

        public NodeBase DuplicateNode() => new((INode)Activator.CreateInstance(_baseNode.GetType()));

        public bool TryGetSpecialField(SpecialFieldFlags flag, out NodeFieldDisplay field)
        {
            if (_baseNode.TryGetSpecialField(flag, out NodeField baseField) && _fieldSection.Contains(baseField))
            {
                field = Fields[_fieldSection.VisualComponentList.IndexOf(baseField)] as NodeFieldDisplay;
                return true;
            }
            else
            {
                field = default;
                return false;
            }
        }

        public void TryEvaluate()
        {
            if (!_evaluating)
            {
                _evaluating = true;
                try
                {
                    _baseNode.Evaluate();
                    ErrorState = false;
                }
                catch
                {
                    ErrorState = true;
                }

                _evaluating = false;
            }
        }

        public void DeepUpdate()
        {
            foreach (IVisualNodeComponentDisplay field in Fields)
            {
                if (field is NodeFieldDisplay nodeField && nodeField.Input?.ExclusiveConnection != null)
                {
                    nodeField.Input.ExclusiveConnection.Parent.DeepUpdate();
                }
            }
            TryEvaluate();
        }
    }
}

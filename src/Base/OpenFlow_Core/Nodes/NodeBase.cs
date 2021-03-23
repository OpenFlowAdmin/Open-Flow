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
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeBase
    {
        private readonly INode baseNode;
        private readonly NodeComponentCollection fieldSection;
        private bool errorState;
        private bool evaluating;

        public NodeBase(INode baseNode)
        {
            this.baseNode = baseNode;
            fieldSection = new NodeComponentCollection(baseNode.Fields)
            {
                ParentNode = baseNode
            };

            Fields = ObservableCollectionMapper<NodeField, DisplayNodeField>.Create(fieldSection.NodeFields, new NodeFieldMapper());

            baseNode.SubscribeToEvaluate(TryEvaluate);

            TryEvaluate();
        }

        public event EventHandler<bool> ErrorStateChanged;

        public bool ErrorState
        {
            get => errorState;
            private set
            {
                if (value != errorState)
                {
                    errorState = value;
                    ErrorStateChanged?.Invoke(this, errorState);
                }
            }
        }

        public double X { get; set; }

        public double Y { get; set; }

        public object Tag { get; set; }

        public string Name => baseNode.NodeName;

        public ReadOnlyObservableCollection<DisplayNodeField> Fields { get; }

        public NodeBase DuplicateNode() => new((INode)Activator.CreateInstance(baseNode.GetType()));

        public bool TryGetSpecialField(SpecialFieldFlags flag, out DisplayNodeField field)
        {
            if (baseNode.TryGetSpecialField(flag, out NodeField baseField))
            {
                field = Fields[fieldSection.NodeFields.IndexOf(baseField)];
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
            if (!evaluating)
            {
                evaluating = true;
                try
                {
                    baseNode.Evaluate();
                    ErrorState = false;
                }
                catch
                {
                    ErrorState = true;
                }

                evaluating = false;
            }
        }

        public void DeepUpdate()
        {
            foreach (DisplayNodeField field in Fields)
            {
                if (field.Input?.ExclusiveConnection != null)
                {
                    field.Input.ExclusiveConnection.Parent.DeepUpdate();
                }
            }
        }
    }
}

namespace OpenFlow_Core.Nodes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using OpenFlow_Core.Nodes.Connectors;
    using OpenFlow_Core.Nodes.VisualNodeComponentDisplays;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class FlowSourceNode : INode
    {
        private readonly NodeField _sourceField = new NodeField() { Name = "Manual Trigger" }.WithValue<Action>("Displayed", false).WithFlowOutput() as NodeField;
        private NodeBase _parentNodeBase;

        public FlowSourceNode()
        {
            this.SetSpecialField(SpecialFieldFlags.FlowOutput, _sourceField);
        }

        public VisualNodeComponent FlowOutField => _sourceField;

        public string NodeName => "Flow Source";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return _sourceField;
            }
        }

        public void Evaluate()
        {
        }

        public void SetParentNode(NodeBase parent)
        {
            _parentNodeBase = parent;
            _sourceField["Displayed"] = (Action)(() =>
            {
                if (_parentNodeBase.TryGetSpecialField(SpecialFieldFlags.FlowOutput, out NodeFieldDisplay field))
                {
                    (field.Output as FlowConnector).Activate();
                }
            });
        }
    }
}

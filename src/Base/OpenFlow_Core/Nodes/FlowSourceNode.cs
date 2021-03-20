namespace OpenFlow_Core.Nodes
{
    using System;
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class FlowSourceNode : IFlowNode
    {
        private readonly ValueField sourceField = new("Manual Trigger", "Trigger Button");

        public FlowSourceNode()
        {
            // sourceField["Trigger Button"] = (Action)FlowOutput.Activate;
        }

        public NodeField FlowOutField => sourceField;

        public string NodeName => "Flow Source";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return sourceField;
            }
        }

        public void Evaluate()
        {
        }
    }
}

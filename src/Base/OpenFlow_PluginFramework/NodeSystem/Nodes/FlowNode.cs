namespace OpenFlow_PluginFramework.NodeSystem.Nodes
{
    using System;
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;

    public abstract class FlowNode : IFlowNode
    {
        private readonly NodeField flowField = new NodeField("Flow").WithFlowInput().WithFlowOutput();

        public NodeField FlowOutField => flowField;

        public abstract string NodeName { get; }

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return flowField;
                foreach (NodeComponent field in FlowNodeFields)
                {
                    yield return field;
                }
            }
        }

        protected abstract IEnumerable<NodeComponent> FlowNodeFields { get; }

        public abstract void Evaluate();
    }
}

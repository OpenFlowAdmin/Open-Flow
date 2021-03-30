namespace OpenFlow_PluginFramework.NodeSystem.Nodes
{
    using System;
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;

    public abstract class FlowNode : INode
    {
        private readonly VisualNodeComponent _flowField = new NodeLabel("Flow").WithFlowInput().WithFlowOutput();

        public FlowNode()
        {
            this.SetFlowOutput(_flowField);
        }

        public abstract string NodeName { get; }

        public IEnumerable<INodeComponent> Fields
        {
            get
            {
                yield return _flowField;
                foreach (INodeComponent field in FlowNodeFields)
                {
                    yield return field;
                }
            }
        }

        protected abstract IEnumerable<INodeComponent> FlowNodeFields { get; }

        public abstract void Evaluate();
    }
}

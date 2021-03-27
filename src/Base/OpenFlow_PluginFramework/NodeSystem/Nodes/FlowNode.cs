namespace OpenFlow_PluginFramework.NodeSystem.Nodes
{
    using System;
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;

    public abstract class FlowNode : INode
    {
        private readonly NodeField _flowField = new NodeField() { Name = "Flow" }.WithFlowInput().WithFlowOutput();

        public FlowNode()
        {
            this.SetSpecialField(SpecialFieldFlags.FlowOutput, _flowField);
        }

        public abstract string NodeName { get; }

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return _flowField;
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

﻿namespace OpenFlow_PluginFramework.NodeSystem.Nodes
{
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;

    public interface IFlowNode : INode
    {
        public VisualNodeComponent FlowOutField { get; }
    }
}

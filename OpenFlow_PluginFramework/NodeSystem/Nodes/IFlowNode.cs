namespace OpenFlow_PluginFramework.NodeSystem.Nodes
{
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;

    public interface IFlowNode : INode
    {
        public NodeField FlowOutField { get; }
    }
}

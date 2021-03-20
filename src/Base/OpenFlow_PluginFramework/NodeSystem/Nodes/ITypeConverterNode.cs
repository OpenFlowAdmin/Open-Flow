namespace OpenFlow_PluginFramework.NodeSystem.Nodes
{
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;

    public interface ITypeConverterNode : INode
    {
        NodeField ConvertInput { get; }

        NodeField ConvertOutput { get; }
    }
}

namespace OpenFlow_PluginFramework.NodeSystem.Nodes
{
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;

    public interface ITypeConverterNode : INode
    {
        VisualNodeComponent ConvertInput { get; }

        VisualNodeComponent ConvertOutput { get; }
    }
}

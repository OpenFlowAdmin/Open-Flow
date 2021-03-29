using OpenFlow_Core.Nodes.Connectors;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.Primitives;

namespace OpenFlow_Core.Nodes.VisualNodeComponentDisplays
{
    public interface IVisualNodeComponentDisplay
    {
        public Opacity Opacity { get; }

        public ObservableValue<IConnector> InputConnector { get; }

        public ObservableValue<IConnector> OutputConnector { get; }
    }
}

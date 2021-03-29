using OpenFlow_Core.Nodes.Connectors;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.Primitives;
using System.ComponentModel;

namespace OpenFlow_Core.Nodes.VisualNodeComponentDisplays
{
    public abstract class VisualNodeComponentDisplay<T> : IVisualNodeComponentDisplay
        where T : VisualNodeComponent
    {
        public VisualNodeComponentDisplay(NodeBase parentNode, T childComponent)
        {
            ParentNode = parentNode;
            ChildComponent = childComponent;
            ChildComponent.GetFlowInput().PropertyChanged += (o, e) => UpdateInput();
            ChildComponent.GetFlowOutput().PropertyChanged += (o, e) => UpdateOutput();

            UpdateInput();
            UpdateOutput();
        }

        public Opacity Opacity => ChildComponent.Opacity;

        public virtual ObservableValue<IConnector> InputConnector { get; } = new(null);

        public ObservableValue<HorizontalAlignment> Alignment { get; } = new(HorizontalAlignment.Middle);

        public virtual ObservableValue<IConnector> OutputConnector { get; } = new(null);

        protected T ChildComponent { get; }

        protected NodeBase ParentNode { get; }

        protected virtual bool TryUpdateConnector(IConnector connector, ConnectionType connectionType, out IConnector newConnector)
        {
            if (GetFlowFor(connectionType) && connector is not FlowConnector)
            {
                newConnector = new FlowConnector(ParentNode, connectionType);
                return true;
            }

            newConnector = default;
            return false;
        }

        protected void UpdateInput()
        {
            if (TryUpdateConnector(InputConnector.Value, ConnectionType.Input, out IConnector newInput))
            {
                InputConnector.Value = newInput;
            }
            Alignment.Value = CalculateAlignment();
        }

        protected void UpdateOutput()
        {
            if (TryUpdateConnector(OutputConnector.Value, ConnectionType.Output, out IConnector newOutput))
            {
                OutputConnector.Value = newOutput;
            }
            Alignment.Value = CalculateAlignment();
        }

        protected virtual HorizontalAlignment CalculateAlignment() => HorizontalAlignment.Middle;

        private bool GetFlowFor(ConnectionType connectionType) => (connectionType) switch
        {
            ConnectionType.Input => ChildComponent.GetFlowInput(),
            ConnectionType.Output => ChildComponent.GetFlowOutput(),
            _ => false,
        };
    }
}

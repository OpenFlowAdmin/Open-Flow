namespace OpenFlow_Core.Nodes.Connectors
{
    using OpenFlow_Core.Nodes;
    using OpenFlow_Core.Nodes.VisualNodeComponentDisplays;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using System.Diagnostics;

    public class FlowConnector : Connector
    {
        public FlowConnector(NodeBase parent,  ConnectionTypes connectionType)
            : base(parent, connectionType)
        {
        }

        public override bool IsExclusiveConnection => ConnectionType == ConnectionTypes.Output;

        public override string ColourHex => "#800080";

        public void Activate()
        {
            Debug.WriteLine("A flow was chained");
            Parent.DeepUpdate();
            if (Parent.TryGetSpecialField(SpecialFieldFlags.FlowOutput, out NodeFieldDisplay field) && field.Output is FlowConnector flowOutput)
            {
                (flowOutput.ExclusiveConnection as FlowConnector)?.Activate();
            }
        }
    }
}

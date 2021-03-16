namespace OpenFlow_Core.Nodes.Connectors
{
    using OpenFlow_Core.Nodes;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class FlowConnector : Connector
    {
        public FlowConnector(ConnectionTypes connectionType)
            : base(connectionType)
        {
        }

        public override bool IsExclusiveConnection => ConnectionType == ConnectionTypes.Output;

        public override string ColourHex => "#800080";

        public void Activate()
        {
            Parent.DeepUpdate();
            if (Parent.TryGetSpecialField(SpecialFieldFlags.FlowOutput, out DisplayNodeField field) && field.Output is FlowConnector flowOutput)
            {
                (flowOutput.ExclusiveConnection as FlowConnector)?.Activate();
            }
        }
    }
}

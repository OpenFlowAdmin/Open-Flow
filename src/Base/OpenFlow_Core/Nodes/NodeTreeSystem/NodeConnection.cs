namespace OpenFlow_Core.Nodes.NodeTreeSystem
{
    using OpenFlow_Core.Nodes.Connectors;

    public record NodeConnection(Connector Output, Connector Input)
    {
        public static bool Construct(Connector connector1, Connector connector2, out NodeConnection connection)
        {
            static bool CheckCompat(Connector output, Connector input) => input.ConnectionType == ConnectionTypes.Input && output.ConnectionType == ConnectionTypes.Output;

            if (CheckCompat(connector1, connector2))
            {
                connection = new NodeConnection(connector1, connector2);
                return true;
            }
            else if (CheckCompat(connector2, connector1))
            {
                connection = new NodeConnection(connector2, connector1);
                return true;
            }
            else
            {
                connection = default;
                return false;
            }
        }
    }
}

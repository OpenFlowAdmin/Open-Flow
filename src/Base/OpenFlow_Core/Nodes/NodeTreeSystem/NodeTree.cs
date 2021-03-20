namespace OpenFlow_Core.Nodes.NodeTreeSystem
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using OpenFlow_Core.Nodes.Connectors;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeTree
    {
        private readonly Dictionary<Connector, Connector> connections = new();

        public ObservableCollection<NodeBase> Nodes { get; private set; } = new();

        public bool TryConnectFields(Connector field1, Connector field2)
        {
            if (NodeConnection.Construct(field1, field2, out NodeConnection newConnection))
            {
                if (SimpleConnect(newConnection))
                {
                    return true;
                }

                if (TypeConverter.TryGetConverter(
                    (newConnection.Output as ValueConnector)?.DisplayValue.TypeDefinition.ValueType,
                    (newConnection.Input as ValueConnector)?.DisplayValue.TypeDefinition.ValueType,
                    out Type converterType))
                {
                    NodeBase newNode = new NodeBase((INode)Activator.CreateInstance(converterType));
                    if (newNode.TryGetSpecialField(SpecialFieldFlags.ConvertInput, out DisplayNodeField convertInput) && newNode.TryGetSpecialField(SpecialFieldFlags.ConvertOutput, out DisplayNodeField convertOutput) &&
                        NodeConnection.Construct(newConnection.Output, convertInput.Input, out NodeConnection firstConnection) && SimpleConnect(firstConnection) &&
                        NodeConnection.Construct(convertOutput.Output, newConnection.Input, out NodeConnection secondConnection) && SimpleConnect(secondConnection))
                    {
                        newNode.X = (field1.Parent.X + field2.Parent.X) / 2;
                        newNode.Y = (field1.Parent.Y + field2.Parent.Y) / 2;

                        AddNode(newNode);
                    }

                    return true;
                }
            }

            return false;
        }

        public Connector ConnectionChanged(Connector interacted)
        {
            if (connections.TryGetValue(interacted, out Connector value))
            {
                connections.Remove(interacted);
                interacted.TryRemoveConnection(value);
                return value;
            }

            return interacted;
        }

        public void AddNode(NodeBase newNode)
        {
            Nodes.Add(newNode);
        }

        public IEnumerable<NodeConnection> GetConnections()
        {
            Dictionary<Connector, Connector>.Enumerator enumerator = connections.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (NodeConnection.Construct(enumerator.Current.Key, enumerator.Current.Value, out NodeConnection connection))
                {
                    yield return connection;
                }
            }
        }

        private bool SimpleConnect(NodeConnection connection)
        {
            if (connection.Input.TryAddConnection(connection.Output) || connection.Output.TryAddConnection(connection.Input))
            {
                if (connection.Input.IsExclusiveConnection)
                {
                    connections[connection.Input] = connection.Output;
                }
                else if (connection.Output.IsExclusiveConnection)
                {
                    connections[connection.Output] = connection.Input;
                }

                return true;
            }

            return false;
        }
    }
}

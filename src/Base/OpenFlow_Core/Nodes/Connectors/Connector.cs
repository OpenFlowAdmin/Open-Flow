namespace OpenFlow_Core.Nodes.Connectors
{
    using System.Collections.Generic;
    using OpenFlow_Core.Nodes;

    public abstract class Connector
    {
        protected Connector(ConnectionTypes connectionType)
        {
            ConnectionType = connectionType;
        }

        private enum ConnectionStatus
        {
            NeutralStatus,
            MyUpdateTurn,
            UpdatesComplete,
            Cleanup,
        }

        public bool ConnectionDirty { get; private set; }

        public NodeBase Parent { get; set; }

        public object Tag { get; set; }

        public abstract bool IsExclusiveConnection { get; }

        public abstract string ColourHex { get; }

        public Connector ExclusiveConnection
        {
            get => IsExclusiveConnection && Connections.Count > 0 ? Connections[0] : null;
            set => Connections = new List<Connector>() { value };
        }

        public ConnectionTypes ConnectionType { get; }

        protected List<Connector> Connections { get; private set; } = new List<Connector>();

        public bool TryAddConnection(Connector toAdd)
        {
            switch (GetConnectionStatus(toAdd))
            {
                case ConnectionStatus.NeutralStatus:
                    if (CanAddConnection(toAdd))
                    {
                        goto case ConnectionStatus.MyUpdateTurn;
                    }

                    break;
                case ConnectionStatus.MyUpdateTurn:
                    AddConnection(toAdd);
                    ConnectorAdded(toAdd);
                    ConnectionDirty = true;
                    toAdd.TryAddConnection(this);
                    return true;
                case ConnectionStatus.UpdatesComplete:
                    ConnectionDirty = false;
                    toAdd.TryAddConnection(this);
                    break;
                case ConnectionStatus.Cleanup:
                    ConnectionDirty = false;
                    break;
            }

            return false;
        }

        public bool TryRemoveConnection(Connector toRemove)
        {
            switch (GetConnectionStatus(toRemove))
            {
                case ConnectionStatus.NeutralStatus:
                    if (Connections.Contains(toRemove))
                    {
                        goto case ConnectionStatus.MyUpdateTurn;
                    }

                    break;
                case ConnectionStatus.MyUpdateTurn:
                    Connections.Remove(toRemove);
                    ConnectorRemoved(toRemove);
                    ConnectionDirty = true;
                    toRemove.TryRemoveConnection(this);
                    return true;
                case ConnectionStatus.UpdatesComplete:
                    ConnectionDirty = false;
                    toRemove.TryRemoveConnection(this);
                    return true;
                case ConnectionStatus.Cleanup:
                    ConnectionDirty = false;
                    return true;
            }

            return false;
        }

        protected virtual bool CanAddConnection(Connector toConnect)
        {
            return (int)toConnect.ConnectionType + (int)ConnectionType == 3 && toConnect.GetType() == GetType();
        }

        protected virtual void ConnectorRemoved(Connector e)
        {
        }

        protected virtual void ConnectorAdded(Connector e)
        {
        }

        private ConnectionStatus GetConnectionStatus(Connector toConnect) => (ConnectionDirty, toConnect.ConnectionDirty) switch
        {
            (true, true) => ConnectionStatus.UpdatesComplete,
            (false, true) => ConnectionStatus.MyUpdateTurn,
            (true, false) => ConnectionStatus.Cleanup,
            (false, false) => ConnectionStatus.NeutralStatus,
        };

        private void AddConnection(Connector connector)
        {
            if (IsExclusiveConnection)
            {
                ExclusiveConnection = connector;
            }
            else
            {
                Connections.Add(connector);
            }
        }
    }
}

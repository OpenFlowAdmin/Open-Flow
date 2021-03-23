namespace OpenFlow_Core.Nodes.Connectors
{
    using System.ComponentModel;
    using OpenFlow_PluginFramework.NodeSystem;
    using OpenFlow_PluginFramework.Primitives;

    public class ValueConnector : Connector, INotifyPropertyChanged
    {
        public ValueConnector(OpenFlowValue displayValue, ConnectionTypes connectionType)
            : base(connectionType)
        {
            DisplayValue = displayValue;
            DisplayValue.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(DisplayValue.TypeDefinition))
                {
                    PropertyChanged?.Invoke(o, new PropertyChangedEventArgs(nameof(ColourHex)));
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OpenFlowValue DisplayValue { get; }

        public override bool IsExclusiveConnection => ConnectionType == ConnectionTypes.Input;

        public override string ColourHex {
            get 
            {
                if (DisplayValue.TypeDefinition is null)
                {
                    return "#FFFFFF";
                }

                return Instance.Current.TypeInfo[DisplayValue.TypeDefinition.ValueType].HexColour;
            }
        }

        protected override bool CanAddConnection(Connector connector)
            => base.CanAddConnection(connector) &&
            connector is ValueConnector valueConnector &&
            valueConnector.DisplayValue.TypeDefinition != null &&
            DisplayValue.CanSetValue(valueConnector.DisplayValue.Value);

        protected override void ConnectorAdded(Connector e)
        {
            if (ConnectionType == ConnectionTypes.Input)
            {
                DisplayValue.Driver = (e as ValueConnector).DisplayValue;
                Parent?.TryEvaluate();
            }
        }

        protected override void ConnectorRemoved(Connector e)
        {
            if (ConnectionType == ConnectionTypes.Input)
            {
                DisplayValue.Driver = null;
                Parent?.TryEvaluate();
            }
        }
    }
}

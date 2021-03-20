namespace OpenFlow_PluginFramework.Primitives
{
    using OpenFlow_PluginFramework.Primitives.TypeDefinition;
    using OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// Stores a well-constrained value by managing a list of <see cref="ITypeDefinition"/>
    /// </summary>
    public class OpenFlowValue : INotifyPropertyChanged
    {
        private object value;
        private bool isEditable;
        private OpenFlowValue driver; 
        private ITypeDefinitionProvider typeDefinitionProvider;
        private ITypeDefinition currentTypeDefinition;
        private string name;

        /// <summary>
        /// Creates a new instance of the OpenFlowValue class
        /// </summary>
        /// <param name="typeDefinitions">A list of possible <see cref="ITypeDefinition"/> which defines what values are allowed</param>
        public OpenFlowValue(ITypeDefinitionProvider typeDefinitionProvider)
        {
            this.typeDefinitionProvider = typeDefinitionProvider;
            TypeDefinition = typeDefinitionProvider.DefaultTypeDefiniton;
        }

        public OpenFlowValue()
        {
            typeDefinitionProvider = new AutoTypeDefinitionProvider();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The <see cref="ITypeDefinition"/> which currently governs the OpenFlowValue
        /// </summary>
        public ITypeDefinition TypeDefinition
        {
            get => currentTypeDefinition;
            private set
            {
                if (value != currentTypeDefinition)
                {
                    currentTypeDefinition = value;
                    this.value = currentTypeDefinition.DefaultValue;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditable)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypeDefinition)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        /// <summary>
        /// The name of the OpenFlowValue, to be used by UI displays
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        /// <summary>
        /// The value the OpenFlowValue currently has
        /// </summary>
        public object Value
        {
            get => driver == null ? value : driver.Value;
            set
            {
                currentTypeDefinition ??= typeDefinitionProvider.TryGetTypeDefinitionFor(value, out ITypeDefinition typeDefinition) ? typeDefinition : null;

                if (currentTypeDefinition != null && currentTypeDefinition.TryConstraintValue(value, out object outputVal) && !outputVal.Equals(Value))
                {
                    this.value = outputVal;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        /// <summary>
        /// Whether the OpenFlowValue should be edited by the user
        /// </summary>
        public bool IsEditable
        {
            get => isEditable;
            set
            {
                if (isEditable != value)
                {
                    isEditable = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditable)));
                }
            }
        }

        /// <summary>
        /// If not null, the <see cref="Value"/> of the Driver will determine the value of this OpenFlowValue
        /// </summary>
        public OpenFlowValue Driver
        {
            get => driver;
            set
            {
                if (driver != null)
                {
                    driver.PropertyChanged -= DriverPropertyChanged;
                }

                driver = value;

                if (driver != null)
                {
                    driver.PropertyChanged += DriverPropertyChanged;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                    IsEditable = false;
                }
                else
                {
                    IsEditable = true;
                }
            }
        }

        /// <summary>
        /// Determines whether this OpenFlowValue can take a value. Will change <see cref="TypeDefinition"/> if required
        /// </summary>
        /// <param name="value">The value to be checked</param>
        /// <returns>True if the value can be set, false if the value cannot</returns>
        public bool CanSetValue(object value)
        {
            if (typeDefinitionProvider.TryGetTypeDefinitionFor(value, out ITypeDefinition typeDefinition))
            {
                TypeDefinition = typeDefinition;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Clones this OpenFlowValue
        /// </summary>
        /// <returns>A new OpenFlowValue with the same properties as this one</returns>
        public OpenFlowValue Clone() => new OpenFlowValue(typeDefinitionProvider)
        {
            Value = Value,
            IsEditable = IsEditable,
            Name = Name,
        };

        /// <summary>
        /// An event which is called when a property changes on the <see cref="Driver"/>, and relays this change forward
        /// </summary>
        /// <param name="sender">The driver that sent the event</param>
        /// <param name="e">The name of the property</param>
        private void DriverPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}

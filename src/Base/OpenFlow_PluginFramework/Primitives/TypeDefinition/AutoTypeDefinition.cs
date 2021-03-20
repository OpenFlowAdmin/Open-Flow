 namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Defines how a value should be changed and edited. Constrains itself based on an object input
    /// </summary>
    public class AutoTypeDefinition : ITypeDefinition
    {
        /// <summary>
        /// Makes a new instance of the AutoTypeDefinition Class
        /// </summary>
        /// <param name="defaultValue">The default value the TypeDefinition is built around</param>
        public AutoTypeDefinition(object defaultValue)
        {
            DefaultValue = defaultValue;
            ValueType = defaultValue.GetType();
        }

        ///<inheritdoc/>
        public object DefaultValue { get; }

        ///<inheritdoc/>
        public Type ValueType { get; }

        ///<inheritdoc/>
        public string EditorName { get; init; }

        ///<inheritdoc/>
        public string DisplayName { get; init; }

        public ITypeDefinition DefaultTypeDefiniton => this;

        public bool CanAcceptValue(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (ValueType.IsAssignableFrom(value.GetType()) || TypeDescriptor.GetConverter(value.GetType()).CanConvertTo(ValueType))
            {
                return true;
            }

            return false;
        }

        ///<inheritdoc/>
        public bool TryConstraintValue(object inputValue, out object outputValue)
        {
            if (inputValue != null && inputValue.GetType() == ValueType)
            {
                outputValue = inputValue;
                return true;
            }

            outputValue = default;
            return false;
        }

        public bool TryGetTypeDefinitionFor(object value, out ITypeDefinition typeDefinition)
        {
            throw new NotImplementedException();
        }
    }
}

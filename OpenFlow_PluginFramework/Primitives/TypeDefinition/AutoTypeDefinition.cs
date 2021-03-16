namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    using System;

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

        ///<inheritdoc/>
        public bool TrySetValue(object inputValue, out object outputValue)
        {
            if (inputValue != null && inputValue.GetType() == ValueType)
            {
                outputValue = inputValue;
                return true;
            }

            outputValue = default;
            return false;
        }
    }
}

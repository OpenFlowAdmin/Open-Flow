 namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Defines how a value should be changed and edited. Constrains itself based on an object input
    /// </summary>
    public class AutoTypeDefinition : TypeDefinition
    {
        /// <summary>
        /// Makes a new instance of the AutoTypeDefinition Class
        /// </summary>
        /// <param name="defaultValue">The default value the TypeDefinition is built around</param>
        public AutoTypeDefinition(object defaultValue)
        {
            DefaultValue = defaultValue;
        }

        ///<inheritdoc/>
        public override object DefaultValue { get; init; }

        ///<inheritdoc/>
        public override Type ValueType => DefaultValue.GetType();

        ///<inheritdoc/>
        public override string EditorName { get; init; }

        ///<inheritdoc/>
        public override string DisplayName { get; init; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    public abstract class TypeDefinition : ITypeDefinition
    {
        /// <inheritdoc/>
        public abstract Type ValueType { get; }

        /// <inheritdoc/>
        public abstract object DefaultValue { get; init; }

        /// <inheritdoc/>
        public abstract string EditorName { get; init; }

        /// <inheritdoc/>
        public abstract string DisplayName { get; init; }

        /// <inheritdoc/>
        public ITypeDefinition DefaultTypeDefiniton => this;

        /// <inheritdoc/>
        public virtual bool CanAcceptValue(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (ValueType.IsAssignableFrom(value.GetType()))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool TryConstraintValue(object inputValue, out object outputValue)
        {
            if (CanAcceptValue(inputValue))
            {
                outputValue = ConstraintValue(inputValue);
                return true;
            }

            outputValue = default;
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetTypeDefinitionFor(object value, out ITypeDefinition typeDefinition)
        {
            if (CanAcceptValue(value))
            {
                typeDefinition = this;
                return true;
            }

            typeDefinition = default;
            return false;
        }

        protected virtual object ConstraintValue(object value) => value;
    }
}

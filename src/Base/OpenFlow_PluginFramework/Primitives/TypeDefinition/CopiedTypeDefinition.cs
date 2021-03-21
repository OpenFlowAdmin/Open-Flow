using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    public class CopiedTypeDefinition : ITypeDefinition
    {
        public CopiedTypeDefinition(ITypeDefinition copyFrom)
        {
            ValueType = copyFrom.ValueType;
            DefaultValue = copyFrom.DefaultValue;
            EditorName = copyFrom.EditorName;
            DisplayName = copyFrom.DisplayName;
            DefaultTypeDefiniton = this;
        }


        public Type ValueType { get; init; }

        public object DefaultValue { get; init; }

        public string EditorName { get; init; }

        public string DisplayName { get; init; }

        public ITypeDefinition DefaultTypeDefiniton { get; }

        public bool CanAcceptValue(object value)
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

        ///<inheritdoc/>
        public bool TryConstraintValue(object inputValue, out object outputValue)
        {
            if (CanAcceptValue(inputValue))
            {
                outputValue = inputValue;
                return true;
            }

            outputValue = default;
            return false;
        }

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
    }
}

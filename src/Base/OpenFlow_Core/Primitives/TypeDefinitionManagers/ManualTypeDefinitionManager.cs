using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using OpenFlow_PluginFramework.Primitives.ValueConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Primitives.TypeDefinitionManagers
{
    public class ManualTypeDefinitionManager : IManualTypeDefinitionManager
    {
        private readonly List<ITypeDefinition> _typeDefinitions = new();

        public ITypeDefinition DefaultDefinition { get; private set; }

        public void RegisterTypeDefinition<T>(T defaultValue, string editorName, string displayName, ValueConstraintChain<T> constraints)
        {
            RegisterTypeDefinition(new TypeDefinition<T>()
            {
                DefaultValue = defaultValue,
                EditorName = editorName,
                DisplayName = displayName,
                Constraints = constraints
            });
        }

        public void RegisterTypeDefinition(ITypeDefinition typeDefinition)
        {
            _typeDefinitions.Add(typeDefinition);

            if (_typeDefinitions.Count == 1)
            {
                DefaultDefinition = _typeDefinitions[0];
            }
        }

        public bool TryGetDefinitionFor(object value, out ITypeDefinition typeDefinition)
        {
            foreach (ITypeDefinition myTypeDefinition in _typeDefinitions)
            {
                if (myTypeDefinition.CanAcceptValue(value))
                {
                    typeDefinition = myTypeDefinition;
                    return true;
                }
            }

            typeDefinition = default;
            return false;
        }

        private class TypeDefinition<T> : ITypeDefinition
        {
            public Dictionary<string, object> ConstraintValues => Constraints.ConstraintValues;

            public Type ValueType { get; } = typeof(T);

            public string EditorName { get; init; }

            public string DisplayName { get; init; }

            public object DefaultValue { get; init; }

            public ValueConstraintChain<T> Constraints { get; init; }

            public bool CanAcceptValue(object value) => value != null && typeof(T).IsAssignableFrom(value.GetType());

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

            public TypeDefinition<T> WithConstraint(ValueConstraint<T> constraint)
            {
                Constraints.AddConstraint(constraint);
                return this;
            }

            public TypeDefinition<T> WithConstraint(Func<T, T> func)
            {
                Constraints.AddConstraint(func);
                return this;
            }

            protected object ConstraintValue(object value) => Constraints != null ? Constraints.TotalConstraint((T)value) : value;
        }
    }
}

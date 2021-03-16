namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    using OpenFlow_PluginFramework.Primitives.ValueConstraints;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// Defines how a value should be edited and displayed
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    public class TypeDefinition<T> : ITypeDefinition
    {
        private ValueConstraintChain<T> constraints = new();

        /// <summary>
        /// A dictionary of values which describe how the value is constrained. Used to display additional information in UIs
        /// </summary>
        public Dictionary<string, object> ConstraintValues => constraints.ConstraintValues;

        /// <inheritdoc/>
        public Type ValueType { get; protected set; } = typeof(T);

        /// <inheritdoc/>
        public string EditorName { get; init; }

        /// <inheritdoc/>
        public string DisplayName { get; init; }

        /// <inheritdoc/>
        public object DefaultValue { get; init; }

        /// <inheritdoc/>
        public bool TrySetValue(object inputValue, out object outputValue)
        {
            if (inputValue == null)
            {
                outputValue = default;
                return false;
            }

            if (ValueType.IsAssignableFrom(inputValue.GetType())) 
            {
                outputValue = constraints.TotalConstraint((T)inputValue);
                return true;
            }

            if  (TypeDescriptor.GetConverter(inputValue.GetType()).CanConvertTo(ValueType))
            {
                outputValue = constraints.TotalConstraint((T)TypeDescriptor.GetConverter(inputValue.GetType()).ConvertTo(inputValue, ValueType));
                return true;
            }

            outputValue = default;
            return false;
        }

        /// <summary>
        /// Adds a constraint to this type definition that will be applied whenever the value is set
        /// </summary>
        /// <param name="constraint">The constraint to add</param>
        /// <returns>The same TypeDefinition for fluent coding</returns>
        public TypeDefinition<T> WithConstraint(ValueConstraint<T> constraint)
        {
            constraints.AddConstraint(constraint);
            return this;
        }

        /// <summary>
        /// Adds a constraint automatically through a delegate
        /// </summary>
        /// <param name="func">The constraint to be applied</param>
        /// <returns></returns>
        public TypeDefinition<T> WithConstraint(Func<T, T> func)
        {
            constraints.AddConstraint(func);
            return this;
        }
    }
}

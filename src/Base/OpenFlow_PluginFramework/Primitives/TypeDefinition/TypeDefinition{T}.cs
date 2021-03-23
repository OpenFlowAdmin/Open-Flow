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
    public class TypeDefinition<T> : TypeDefinition
    {
        private readonly ValueConstraintChain<T> constraints = new();

        /// <summary>
        /// A dictionary of values which describe how the value is constrained. Used to display additional information in UIs
        /// </summary>
        public Dictionary<string, object> ConstraintValues => constraints.ConstraintValues;

        /// <inheritdoc/>
        public override Type ValueType { get; } = typeof(T);

        /// <inheritdoc/>
        public override string EditorName { get; init; }

        /// <inheritdoc/>
        public override string DisplayName { get; init; }

        /// <inheritdoc/>
        public override object DefaultValue { get; init; }

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

        protected override object ConstraintValue(object value) => constraints.TotalConstraint((T)value);
    }
}

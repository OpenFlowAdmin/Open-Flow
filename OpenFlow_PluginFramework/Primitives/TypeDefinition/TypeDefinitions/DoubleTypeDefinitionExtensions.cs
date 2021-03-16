namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    using OpenFlow_PluginFramework.Primitives.ValueConstraints;
    using System;

    /// <summary>
    /// A static class that defines fluent ways of constraining a <see cref="TypeDefinition{double}"/> of type double
    /// </summary>
    public static class DoubleTypeDefinitionExtensions
    {
        /// <summary>
        /// Defines the maximum value of the <see cref="TypeDefinition{double}"/>
        /// </summary>
        /// <param name="td">The <see cref="TypeDefinition{T}"/> to be constrained</param>
        /// <param name="max">The maximum value of the <see cref="TypeDefinition{T}"/></param>
        /// <returns>The input TypeDefinition to allow for fluent coding</returns>
        public static TypeDefinition<double> MaximumValue(this TypeDefinition<double> td, double max) => td.WithConstraint(new ValueConstraint<double>("Max", max, (x) => Math.Min(max, x)));

        /// <summary>
        /// Defines the minimum value of the <see cref="TypeDefinition{double}"/>
        /// </summary>
        /// <param name="td">The <see cref="TypeDefinition{T}"/> to be constrained</param>
        /// <param name="max">The minimum value of the <see cref="TypeDefinition{T}"/></param>
        /// <returns>The input TypeDefinition to allow for fluent coding</returns>
        public static TypeDefinition<double> MinimumValue(this TypeDefinition<double> td, double min) => td.WithConstraint(new ValueConstraint<double>("Min", min, (x) => Math.Max(min, x)));
    }
}

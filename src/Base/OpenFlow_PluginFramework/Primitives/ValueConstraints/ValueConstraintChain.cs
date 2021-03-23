using System;
using System.Collections.Generic;

namespace OpenFlow_PluginFramework.Primitives.ValueConstraints
{
    /// <summary>
    /// A chain of constraints which can be added to, and applied all at once
    /// </summary>
    /// <typeparam name="T">The type which will be constrained</typeparam>
    public class ValueConstraintChain<T>
    {
        private readonly List<ValueConstraint<T>> _constraints = new();

        /// <summary>
        /// Values registered to be associated with certain constraints
        /// </summary>
        public Dictionary<string, object> ConstraintValues { get; } = new();

        /// <summary>
        /// The total constraint which respresents the entire chain
        /// </summary>
        public Func<T, T> TotalConstraint { get; private set; } = (x) => x;

        /// <summary>
        /// Adds a constraint to the chain
        /// </summary>
        /// <param name="constraint">The constraint to add</param>
        public void AddConstraint(ValueConstraint<T> constraint)
        {
            if (constraint.Name != null)
            {
                ConstraintValues.Add(constraint.Name, constraint.Value);
            }

            constraint.AddToEndOfChain(_constraints);
            TotalConstraint = constraint.TotalFunc;
        }

        /// <summary>
        /// Adds a delegate to the chain
        /// </summary>
        /// <param name="constraint">The function to add</param>
        public void AddConstraint(Func<T, T> constraint) => AddConstraint(new ValueConstraint<T>(null, null, constraint));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives.ValueConstraints
{
    /// <summary>
    /// Defines a constraint which is part of a chain, with a name and a value
    /// </summary>
    public class ValueConstraint<T>
    {
        public ValueConstraint(string name, object value, Func<T, T> myFunc)
        {
            Name = name;
            Value = value;
            MyFunc = myFunc;
            TotalFunc = MyFunc;
        }
        
        /// <summary>
        /// The name of the constraint
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The value of the constraint
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// The function which represents the constraint
        /// </summary>
        public Func<T, T> MyFunc { get; }

        /// <summary>
        /// The total function which represents the entire chain up until this point
        /// </summary>
        public Func<T, T> TotalFunc { get; private set; }

        /// <summary>
        /// Adds this constraint to the end of a chain of constraints
        /// </summary>
        /// <param name="constraints">The chain of constraints to add this to</param>
        public void AddToEndOfChain(List<ValueConstraint<T>> constraints)
        {
            if (constraints.Count > 0)
            {
                Func<T, T> PreviousTotal = constraints[constraints.Count - 1].TotalFunc;
                TotalFunc = (x) => MyFunc(PreviousTotal(x));
            }

            constraints.Add(this);
        }
    }
}

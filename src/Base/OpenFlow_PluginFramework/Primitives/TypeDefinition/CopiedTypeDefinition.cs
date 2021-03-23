using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    public class CopiedTypeDefinition : TypeDefinition
    {
        public CopiedTypeDefinition(ITypeDefinition copyFrom)
        {
            DefaultValue = copyFrom.DefaultValue;
            EditorName = copyFrom.EditorName;
            DisplayName = copyFrom.DisplayName;
        }

        /// <inheritdoc/>
        public override Type ValueType => DefaultValue.GetType();

        /// <inheritdoc/>
        public override object DefaultValue { get; init; }

        /// <inheritdoc/>
        public override string EditorName { get; init; }

        /// <inheritdoc/>
        public override string DisplayName { get; init; }
    }
}

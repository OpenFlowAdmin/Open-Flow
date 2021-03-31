using OpenFlow_PluginFramework.Primitives.ValueConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives.TypeDefinition
{
    public interface ITypeDefinitionManager
    {
        public ITypeDefinition DefaultDefinition { get; }

        public bool TryGetDefinitionFor(object value, out ITypeDefinition typeDefinition);
    }
}

using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider
{
    public class AcceptsAllTypeDefinitionProvider : ITypeDefinitionProvider
    {
        public ITypeDefinition DefaultTypeDefiniton => null;

        public bool TryGetTypeDefinitionFor(object value, out ITypeDefinition typeDefinition)
        {
            typeDefinition = new AutoTypeDefinition(value);
            return true;
        }
    }
}

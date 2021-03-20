using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider
{
    public class AutoTypeDefinitionProvider : ITypeDefinitionProvider
    {
        private ITypeDefinition definitionToProvide;

        public ITypeDefinition DefaultTypeDefiniton => null;

        public bool TryGetTypeDefinitionFor(object value, out ITypeDefinition typeDefinition)
        {
            if (definitionToProvide == null)
            {
                definitionToProvide = new AutoTypeDefinition(value);
                typeDefinition = definitionToProvide;
                return true;
            }

            if (definitionToProvide.CanAcceptValue(value))
            {
                typeDefinition = definitionToProvide;
                return true;
            }

            typeDefinition = default;
            return false;
        }
    }
}

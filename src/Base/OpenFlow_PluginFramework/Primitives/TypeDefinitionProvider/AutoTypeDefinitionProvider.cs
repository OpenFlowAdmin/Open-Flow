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
        private ITypeDefinition _definitionToProvide;

        public ITypeDefinition DefaultTypeDefiniton => _definitionToProvide;

        public bool TryGetTypeDefinitionFor(object value, out ITypeDefinition typeDefinition)
        {
            if (_definitionToProvide == null)
            {
                _definitionToProvide = new AutoTypeDefinition(value);
                typeDefinition = _definitionToProvide;
                return true;
            }

            if (_definitionToProvide.CanAcceptValue(value))
            {
                typeDefinition = _definitionToProvide;
                return true;
            }

            typeDefinition = default;
            return false;
        }
    }
}

using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider
{
    public class CompoundTypeDefinitionProvider : ITypeDefinitionProvider
    {
        private readonly ITypeDefinition[] _acceptedTypeDefinitions;

        public CompoundTypeDefinitionProvider(params ITypeDefinition[] typeDefinitions)
        {
            if (typeDefinitions is null)
            {
                throw new ArgumentNullException(nameof(typeDefinitions));
            }

            if (typeDefinitions.Length < 1)
            {
                throw new ArgumentException("Length of typeDefinitions must be at least one");
            }

            _acceptedTypeDefinitions = typeDefinitions;
        }


        public ITypeDefinition DefaultTypeDefiniton { get; private set; }

        public bool TryGetTypeDefinitionFor(object value, out ITypeDefinition typeDefinition)
        {
            foreach (ITypeDefinition typeDef in _acceptedTypeDefinitions)
            {
                if (typeDef.CanAcceptValue(value))
                {
                    typeDefinition = typeDef;
                    return true;
                }
            }

            typeDefinition = default;
            return false;
        }
    }
}

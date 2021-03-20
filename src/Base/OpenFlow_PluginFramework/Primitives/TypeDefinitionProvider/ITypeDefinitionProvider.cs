using OpenFlow_PluginFramework.Primitives.TypeDefinition;

namespace OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider
{
    /// <summary>
    /// A class that can provide an ITypeDefintion for a specific value
    /// </summary>
    public interface ITypeDefinitionProvider
    {
        /// <summary>
        /// Given an input value, try to get an ITypeDefinition which can take that value
        /// </summary>
        /// <param name="value">The value which is being tested against</param>
        /// <param name="typeDefinition">The ITypeDefiniton which can accept the value</param>
        /// <returns>True if an ITypeDefinitoin could be provided, false otherwise</returns>
        public bool TryGetTypeDefinitionFor(object value, out ITypeDefinition typeDefinition);

        public ITypeDefinition DefaultTypeDefiniton { get; }
    }
}

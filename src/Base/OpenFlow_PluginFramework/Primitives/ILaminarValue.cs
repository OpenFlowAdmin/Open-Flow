using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System.ComponentModel;

namespace OpenFlow_PluginFramework.Primitives
{
    public interface ILaminarValue : IObservableValue<object>
    {
        string Name { get; set; }

        ILaminarValue Driver { get; set; }

        bool IsUserEditable { get; set; }

        ITypeDefinitionManager TypeDefinitionManager { get; set; }

        ITypeDefinition TypeDefinition { get; }

        bool CanSetValue(object value);

        ILaminarValue Clone();
    }
}
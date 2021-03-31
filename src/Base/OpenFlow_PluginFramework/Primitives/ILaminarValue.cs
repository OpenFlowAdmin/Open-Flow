using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System.ComponentModel;

namespace OpenFlow_PluginFramework.Primitives
{
    public interface ILaminarValue
    {
        ILaminarValue Driver { get; set; }
        bool IsUserEditable { get; set; }
        string Name { get; set; }

        ITypeDefinitionManager TypeDefinitionManager { get; set; }

        ITypeDefinition TypeDefinition { get; }
        object Value { get; set; }

        event PropertyChangedEventHandler PropertyChanged;

        bool CanSetValue(object value);
        ILaminarValue Clone();
    }
}
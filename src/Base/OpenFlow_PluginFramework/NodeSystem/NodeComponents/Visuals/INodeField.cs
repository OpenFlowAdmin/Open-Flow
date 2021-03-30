using OpenFlow_PluginFramework.Primitives;
using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider;
using System;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    public interface INodeField : IVisualNodeComponent
    {
        public const string InputKey = "Input";
        public const string OutputKey = "Output";

        event EventHandler<object> AnyValueChanged;
        event EventHandler<object> ValueStoreChanged;

        object this[object key] { get; set; }

        LaminarValue DisplayedValue { get; }

        object Input { get; set; }

        object Output { get; set; }

        void AddValue(object key, LaminarValue newVal);

        void AddValue(object key, ITypeDefinitionProvider typeDef, bool isUserEditable);

        LaminarValue GetDisplayValue(object key);
    }
}
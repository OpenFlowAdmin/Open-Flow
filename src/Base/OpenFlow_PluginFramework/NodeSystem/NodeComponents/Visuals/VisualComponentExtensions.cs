using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    public static class VisualComponentExtensions
    {
        public static T WithFlowInput<T>(this T component, bool HasFlowInput = true) where T : IVisualNodeComponent
        {
            component.SetFlowInput(HasFlowInput);
            return component;
        }

        public static T WithFlowOutput<T>(this T component, bool HasFlowOutput = true) where T : IVisualNodeComponent
        {
            component.SetFlowOutput(HasFlowOutput);
            return component;
        }

        public static TComponent WithValue<TComponent, TValue>(this TComponent nodeField, string valueKey, TValue defaultValue, bool isUserEditable = false) where TComponent : INodeField
        {
            nodeField.AddValue(valueKey, Constructor.ManualTypeDefinitionManager().WithAcceptedDefinition(defaultValue), isUserEditable);
            return nodeField;
        }

        public static TComponent WithValue<TComponent>(this TComponent nodeField, string valueKey, ITypeDefinitionManager typeDefinition, bool isUserEditable = false) where TComponent : INodeField
        {
            nodeField.AddValue(valueKey, typeDefinition, isUserEditable);
            return nodeField;
        }

        public static TComponent WithValue<TComponent>(this TComponent nodeField, string valueKey, ITypeDefinition typeDefinition, bool isUserEditable = false) where TComponent : INodeField
        {
            nodeField.AddValue(valueKey, Constructor.ManualTypeDefinitionManager().WithAcceptedDefinition(typeDefinition), isUserEditable);
            return nodeField;
        }

        public static TComponent WithInput<TComponent, TValue>(this TComponent nodeField, TValue defaultValue) where TComponent : INodeField
            => nodeField.WithValue<TComponent, TValue>(INodeField.InputKey, defaultValue, true);

        public static TComponent WithInput<TComponent>(this TComponent nodeField, ITypeDefinitionManager typeDefinition) where TComponent : INodeField
            => nodeField.WithValue<TComponent>(INodeField.InputKey, typeDefinition, true);

        public static TComponent WithInput<TComponent>(this TComponent nodeField, ITypeDefinition typeDefinition) where TComponent : INodeField
            => nodeField.WithValue<TComponent>(INodeField.InputKey, typeDefinition, true);

        public static TComponent WithOutput<TComponent, TValue>(this TComponent nodeField, TValue defaultValue) where TComponent : INodeField
            => nodeField.WithValue<TComponent, TValue>(INodeField.OutputKey, defaultValue, false);

        public static TComponent WithOutput<TComponent>(this TComponent nodeField, ITypeDefinitionManager typeDefinition) where TComponent : INodeField
            => nodeField.WithValue<TComponent>(INodeField.OutputKey, typeDefinition, false);

        public static TComponent WithOutput<TComponent>(this TComponent nodeField, ITypeDefinition typeDefinition) where TComponent : INodeField
            => nodeField.WithValue(INodeField.OutputKey, typeDefinition, true);
    }
}

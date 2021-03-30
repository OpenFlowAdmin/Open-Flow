using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.Primitives;
using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents
{
    public static class NodeComponentBuilder
    {
        public static NodeComponentFactory Factory { get; set; } = new();

        public static NodeComponentBuilderInstance<INodeField> NodeField(string fieldName) 
        {
            NodeComponentBuilderInstance<INodeField> output = new(Factory);

            output.Build.Name = fieldName;

            return output;
        }

        public static NodeComponentBuilderInstance<INodeLabel> NodeLabel(string labelText)
        {
            NodeComponentBuilderInstance<INodeLabel> output = new(Factory);

            output.Build.Name = labelText;

            return output;
        }

        public static NodeComponentBuilderInstance<T> WithFlowInput<T>(this NodeComponentBuilderInstance<T> builder, bool HasFlowInput = true) where T : IVisualNodeComponent
        {
            builder.Build.SetFlowInput(HasFlowInput);
            return builder;
        }

        public static NodeComponentBuilderInstance<T> WithFlowOutput<T>(this NodeComponentBuilderInstance<T> builder, bool HasFlowOutput = true) where T : IVisualNodeComponent
        {
            builder.Build.SetFlowOutput(HasFlowOutput);
            return builder;
        }

        public static NodeComponentBuilderInstance<TComponent> WithValue<TComponent, TValue>(this NodeComponentBuilderInstance<TComponent> builder, string valueKey, TValue defaultValue, bool isUserEditable = false) where TComponent : INodeField
        {
            builder.Build.AddValue(valueKey, new TypeDefinition<TValue>() { DefaultValue = defaultValue }, isUserEditable);
            return builder;
        }

        public static NodeComponentBuilderInstance<TComponent> WithValueTypeProvider<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, string valueKey, ITypeDefinitionProvider typeDefinition, bool isUserEditable = false) where TComponent : INodeField
        {
            builder.Build.AddValue(valueKey, typeDefinition, isUserEditable);
            return builder;
        }

        public static NodeComponentBuilderInstance<TComponent> WithInput<TComponent, TValue>(this NodeComponentBuilderInstance<TComponent> builder, TValue defaultValue) where TComponent : INodeField
            => builder.WithValue<TComponent, TValue>(INodeField.InputKey, defaultValue, true);

        public static NodeComponentBuilderInstance<TComponent> WithInputTypeProvider<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, ITypeDefinitionProvider typeDefinition) where TComponent : INodeField
            => builder.WithValueTypeProvider<TComponent>(INodeField.InputKey, typeDefinition, true);

        public static NodeComponentBuilderInstance<TComponent> WithOutput<TComponent, TValue>(this NodeComponentBuilderInstance<TComponent> builder, TValue defaultValue) where TComponent : INodeField
            => builder.WithValue<TComponent, TValue>(INodeField.OutputKey, defaultValue, false);

        public static NodeComponentBuilderInstance<TComponent> WithOutputTypeProvider<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, ITypeDefinitionProvider typeDefinition) where TComponent : INodeField
            => builder.WithValueTypeProvider<TComponent>(INodeField.OutputKey, typeDefinition, false);

        public class NodeComponentBuilderInstance<T> where T : INodeComponent
        {
            public NodeComponentBuilderInstance(NodeComponentFactory factory)
            {
                Build = factory.GetImplementation<T>();
            }

            public T Build { get; }
        }
    }
}

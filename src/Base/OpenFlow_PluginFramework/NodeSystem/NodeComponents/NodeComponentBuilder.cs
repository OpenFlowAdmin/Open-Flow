using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Collections;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.Primitives;
using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using OpenFlow_PluginFramework.Primitives.ValueConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents
{
    public static class NodeComponentBuilder
    {
        public static IObjectFactory Factory { get; set; }

        public static NodeComponentBuilderInstance<T> GetInstance<T>() => new(Factory);

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

        public static NodeComponentBuilderInstance<INodeDecorator> NodeDecorator(NodeDecoratorType DecoratorType)
        {
            NodeComponentBuilderInstance<INodeDecorator> output = new(Factory);

            output.Build.DecoratorType = DecoratorType;

            return output;
        }

        public static NodeComponentBuilderInstance<INodeComponentAutoCloner> NodeComponentAutoCloner(INodeComponent originalClone, int minimumFieldCount, Func<int, string> nameRule)
        {
            NodeComponentBuilderInstance<INodeComponentAutoCloner> output = new(Factory);

            output.Build.ResetWith(originalClone, minimumFieldCount, nameRule);

            return output;
        }

        public static NodeComponentBuilderInstance<INodeComponentList> NodeComponentList(params INodeComponent[] components) => NodeComponentList(components.AsEnumerable());

        public static NodeComponentBuilderInstance<INodeComponentList> NodeComponentList(IEnumerable<INodeComponent> components)
        {
            NodeComponentBuilderInstance<INodeComponentList> output = new(Factory);

            foreach (INodeComponent component in components)
            {
                output.Build.Add(component);
            }

            return output;
        }

        public static NodeComponentBuilderInstance<ITypeDefinitionManager> TypeDefinitionManager()
        {
            return new(Factory);
        }

        public static NodeComponentBuilderInstance<IRigidTypeDefinitionManager> RigidTypeDefinitionManager(object value, string editorName = null, string displayName = null)
        {
            NodeComponentBuilderInstance<IRigidTypeDefinitionManager> output = new(Factory);

            output.Build.RegisterTypeDefinition(value, editorName, displayName);

            return output;
        }

        public static NodeComponentBuilderInstance<IManualTypeDefinitionManager> ManualTypeDefinitionManager() => new(Factory);

        public static NodeComponentBuilderInstance<INodeComponentDictionary> NodeComponentDictionary() => new(Factory);

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
            builder.Build.AddValue(valueKey, ManualTypeDefinitionManager().AddAcceptedDefinition<TValue>(defaultValue).Build, isUserEditable);
            return builder;
        }

        public static NodeComponentBuilderInstance<TComponent> WithValueTypeProvider<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, string valueKey, ITypeDefinitionManager typeDefinition, bool isUserEditable = false) where TComponent : INodeField
        {
            builder.Build.AddValue(valueKey, typeDefinition, isUserEditable);
            return builder;
        }

        public static NodeComponentBuilderInstance<TComponent> WithValueType<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, string valueKey, ITypeDefinition typeDefinition, bool isUserEditable = false) where TComponent : INodeField
        {
            builder.Build.AddValue(valueKey, ManualTypeDefinitionManager().AddAcceptedDefinition(typeDefinition).Build, isUserEditable);
            return builder;
        }

        public static NodeComponentBuilderInstance<TComponent> WithInput<TComponent, TValue>(this NodeComponentBuilderInstance<TComponent> builder, TValue defaultValue) where TComponent : INodeField
            => builder.WithValue<TComponent, TValue>(INodeField.InputKey, defaultValue, true);

        public static NodeComponentBuilderInstance<TComponent> WithInputTypeProvider<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, ITypeDefinitionManager typeDefinition) where TComponent : INodeField
            => builder.WithValueTypeProvider<TComponent>(INodeField.InputKey, typeDefinition, true);

        public static NodeComponentBuilderInstance<TComponent> WithInputType<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, ITypeDefinition typeDefinition) where TComponent : INodeField
    => builder.WithValueType<TComponent>(INodeField.InputKey, typeDefinition, true);

        public static NodeComponentBuilderInstance<TComponent> WithOutput<TComponent, TValue>(this NodeComponentBuilderInstance<TComponent> builder, TValue defaultValue) where TComponent : INodeField
            => builder.WithValue<TComponent, TValue>(INodeField.OutputKey, defaultValue, false);

        public static NodeComponentBuilderInstance<TComponent> WithOutputTypeProvider<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, ITypeDefinitionManager typeDefinition) where TComponent : INodeField
            => builder.WithValueTypeProvider<TComponent>(INodeField.OutputKey, typeDefinition, false);

        public static NodeComponentBuilderInstance<TComponent> WithOutputType<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, ITypeDefinition typeDefinition) where TComponent : INodeField
            => builder.WithValueType<TComponent>(INodeField.OutputKey, typeDefinition, true);

        public static NodeComponentBuilderInstance<TComponent> Add<TComponent>(this NodeComponentBuilderInstance<TComponent> builder, object key, INodeComponent value) where TComponent : INodeComponentDictionary
        {
            builder.Build.Add(key, value);
            return builder;
        }

        public static NodeComponentBuilderInstance<IManualTypeDefinitionManager> AddAcceptedDefinition<T>(this NodeComponentBuilderInstance<IManualTypeDefinitionManager> builder, T defaultValue, string editorName = null, string displayName = null, ValueConstraintChain<T> constraints = null)
        {
            builder.Build.RegisterTypeDefinition(defaultValue, editorName, displayName, constraints);
            return builder;
        }

        public static NodeComponentBuilderInstance<IManualTypeDefinitionManager> AddAcceptedDefinition(this NodeComponentBuilderInstance<IManualTypeDefinitionManager> builder, ITypeDefinition typeDefinition)
        {
            builder.Build.RegisterTypeDefinition(typeDefinition);
            return builder;
        }

        public class NodeComponentBuilderInstance<T>
        {
            public NodeComponentBuilderInstance(IObjectFactory factory)
            {
                Build = factory.GetImplementation<T>();
            }

            public T Build { get; }
        }
    }
}

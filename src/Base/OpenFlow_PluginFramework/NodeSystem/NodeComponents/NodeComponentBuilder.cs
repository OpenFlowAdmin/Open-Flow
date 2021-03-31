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

        public static Instance<T> GetInstance<T>() => new(Factory);

        public static Instance<INodeField> NodeField(string fieldName) 
        {
            Instance<INodeField> output = new(Factory);

            output.Build.Name = fieldName;

            return output;
        }

        public static Instance<INodeLabel> NodeLabel(string labelText)
        {
            Instance<INodeLabel> output = new(Factory);

            output.Build.Name = labelText;

            return output;
        }

        public static Instance<INodeDecorator> NodeDecorator(NodeDecoratorType DecoratorType)
        {
            Instance<INodeDecorator> output = new(Factory);

            output.Build.DecoratorType = DecoratorType;

            return output;
        }

        public static Instance<INodeComponentAutoCloner> NodeComponentAutoCloner(INodeComponent originalClone, int minimumFieldCount, Func<int, string> nameRule)
        {
            Instance<INodeComponentAutoCloner> output = new(Factory);

            output.Build.ResetWith(originalClone, minimumFieldCount, nameRule);

            return output;
        }

        public static Instance<INodeComponentList> NodeComponentList(params INodeComponent[] components) => NodeComponentList(components.AsEnumerable());

        public static Instance<INodeComponentList> NodeComponentList(IEnumerable<INodeComponent> components)
        {
            Instance<INodeComponentList> output = new(Factory);

            foreach (INodeComponent component in components)
            {
                output.Build.Add(component);
            }

            return output;
        }

        public static Instance<ITypeDefinitionManager> TypeDefinitionManager()
        {
            return new(Factory);
        }

        public static Instance<IRigidTypeDefinitionManager> RigidTypeDefinitionManager(object value, string editorName = null, string displayName = null)
        {
            Instance<IRigidTypeDefinitionManager> output = new(Factory);

            output.Build.RegisterTypeDefinition(value, editorName, displayName);

            return output;
        }

        public static Instance<IManualTypeDefinitionManager> ManualTypeDefinitionManager() => new(Factory);

        public static Instance<INodeComponentDictionary> NodeComponentDictionary() => new(Factory);

        public static Instance<T> WithFlowInput<T>(this Instance<T> builder, bool HasFlowInput = true) where T : IVisualNodeComponent
        {
            builder.Build.SetFlowInput(HasFlowInput);
            return builder;
        }

        public static Instance<T> WithFlowOutput<T>(this Instance<T> builder, bool HasFlowOutput = true) where T : IVisualNodeComponent
        {
            builder.Build.SetFlowOutput(HasFlowOutput);
            return builder;
        }

        public static Instance<TComponent> WithValue<TComponent, TValue>(this Instance<TComponent> builder, string valueKey, TValue defaultValue, bool isUserEditable = false) where TComponent : INodeField
        {
            builder.Build.AddValue(valueKey, ManualTypeDefinitionManager().AddAcceptedDefinition<TValue>(defaultValue).Build, isUserEditable);
            return builder;
        }

        public static Instance<TComponent> WithValueTypeProvider<TComponent>(this Instance<TComponent> builder, string valueKey, ITypeDefinitionManager typeDefinition, bool isUserEditable = false) where TComponent : INodeField
        {
            builder.Build.AddValue(valueKey, typeDefinition, isUserEditable);
            return builder;
        }

        public static Instance<TComponent> WithValueType<TComponent>(this Instance<TComponent> builder, string valueKey, ITypeDefinition typeDefinition, bool isUserEditable = false) where TComponent : INodeField
        {
            builder.Build.AddValue(valueKey, ManualTypeDefinitionManager().AddAcceptedDefinition(typeDefinition).Build, isUserEditable);
            return builder;
        }

        public static Instance<TComponent> WithInput<TComponent, TValue>(this Instance<TComponent> builder, TValue defaultValue) where TComponent : INodeField
            => builder.WithValue<TComponent, TValue>(INodeField.InputKey, defaultValue, true);

        public static Instance<TComponent> WithInputTypeProvider<TComponent>(this Instance<TComponent> builder, ITypeDefinitionManager typeDefinition) where TComponent : INodeField
            => builder.WithValueTypeProvider<TComponent>(INodeField.InputKey, typeDefinition, true);

        public static Instance<TComponent> WithInputType<TComponent>(this Instance<TComponent> builder, ITypeDefinition typeDefinition) where TComponent : INodeField
    => builder.WithValueType<TComponent>(INodeField.InputKey, typeDefinition, true);

        public static Instance<TComponent> WithOutput<TComponent, TValue>(this Instance<TComponent> builder, TValue defaultValue) where TComponent : INodeField
            => builder.WithValue<TComponent, TValue>(INodeField.OutputKey, defaultValue, false);

        public static Instance<TComponent> WithOutputTypeProvider<TComponent>(this Instance<TComponent> builder, ITypeDefinitionManager typeDefinition) where TComponent : INodeField
            => builder.WithValueTypeProvider<TComponent>(INodeField.OutputKey, typeDefinition, false);

        public static Instance<TComponent> WithOutputType<TComponent>(this Instance<TComponent> builder, ITypeDefinition typeDefinition) where TComponent : INodeField
            => builder.WithValueType<TComponent>(INodeField.OutputKey, typeDefinition, true);

        public static Instance<TComponent> Add<TComponent>(this Instance<TComponent> builder, object key, INodeComponent value) where TComponent : INodeComponentDictionary
        {
            builder.Build.Add(key, value);
            return builder;
        }

        public static Instance<IManualTypeDefinitionManager> AddAcceptedDefinition<T>(this Instance<IManualTypeDefinitionManager> builder, T defaultValue, string editorName = null, string displayName = null, ValueConstraintChain<T> constraints = null)
        {
            builder.Build.RegisterTypeDefinition(defaultValue, editorName, displayName, constraints);
            return builder;
        }

        public static Instance<IManualTypeDefinitionManager> AddAcceptedDefinition(this Instance<IManualTypeDefinitionManager> builder, ITypeDefinition typeDefinition)
        {
            builder.Build.RegisterTypeDefinition(typeDefinition);
            return builder;
        }

        public class Instance<T>
        {
            public Instance(IObjectFactory factory)
            {
                Build = factory.GetImplementation<T>();
            }

            public T Build { get; }
        }
    }
}

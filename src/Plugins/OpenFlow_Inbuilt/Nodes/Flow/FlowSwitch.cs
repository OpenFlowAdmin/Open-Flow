namespace OpenFlow_Inbuilt.Nodes.Flow
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Collections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;
    using OpenFlow_PluginFramework.Primitives.TypeDefinition;

    public class FlowSwitch : IFlowNode
    {
        private readonly INodeLabel flowInput = NodeComponentBuilder.NodeLabel("Flow Input").WithFlowInput().Build;

        private readonly INodeField valueInput = NodeComponentBuilder.NodeField("Switch Value").WithInputTypeProvider(NodeComponentBuilder.TypeDefinitionManager().Build).Build;

        private readonly INodeLabel OutputsLabel = NodeComponentBuilder.NodeLabel("Possible Values").Build;

        private readonly INodeLabel defaultOutput = NodeComponentBuilder.NodeLabel("Default").WithFlowOutput().Build;

        private readonly INodeComponentDictionary flowOutputs = NodeComponentBuilder.NodeComponentDictionary().Add(
                typeof(bool),
                NodeComponentBuilder.NodeComponentList(
                    NodeComponentBuilder.NodeField("True").WithValueTypeProvider("Displayed", NodeComponentBuilder.ManualTypeDefinitionManager().AddAcceptedDefinition<bool>(true, "DefaultDisplay").Build, false).WithFlowOutput().Build,
                    NodeComponentBuilder.NodeField("False").WithValueTypeProvider("Displayed", NodeComponentBuilder.ManualTypeDefinitionManager().AddAcceptedDefinition<bool>(false, "DefaultDisplay").Build, false).WithFlowOutput().Build).Build).Build;

        public FlowSwitch()
        {
            valueInput.GetDisplayValue(INodeField.InputKey).PropertyChanged += FlowSwitch_OnTypeDefinitionChanged;
        }

        public string NodeName => "Switch";

        public IEnumerable<INodeComponent> Fields
        {
            get
            {
                yield return flowInput;
                yield return valueInput;
                yield return NodeComponentBuilder.NodeDecorator(NodeDecoratorType.MajorSeparator).Build;
                yield return OutputsLabel;
                yield return NodeComponentBuilder.NodeDecorator(NodeDecoratorType.MajorSeparator).Build;
                yield return flowOutputs;
            }
        }

        public IVisualNodeComponent FlowOutField { get; private set; }

        public void Evaluate()
        {
            foreach (IVisualNodeComponent field in flowOutputs.VisualComponentList)
            {
                if (field is INodeField valueField && valueField.DisplayedValue != null && valueField.DisplayedValue.Value.Equals(valueInput.Input))
                {
                    FlowOutField = valueField;
                    return;
                }
            }

            FlowOutField = defaultOutput;
        }

        private void FlowSwitch_OnTypeDefinitionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ILaminarValue.TypeDefinition))
            {
                ChangeSwitchTypeTo((sender as ILaminarValue).TypeDefinition);
            }
        }

        private void ChangeSwitchTypeTo(ITypeDefinition typeDef)
        {
            flowOutputs.HideAllComponents();
            if (!flowOutputs.ContainsKey(typeDef.ValueType))
            {
                flowOutputs.Add(typeDef.ValueType, GenerateSwitchesFor(typeDef));
            }

            flowOutputs.ShowSectionByKey(typeDef.ValueType);
        }

        private INodeComponent GenerateSwitchesFor(ITypeDefinition typeDef)
        {
            if (typeDef.ValueType.IsEnum)
            {
                return NodeComponentBuilder.NodeComponentList(Enum.GetNames(typeDef.ValueType).Select(x => ValueDisplay(typeDef, Enum.Parse(typeDef.ValueType, x)))).Build;
            }

            return NodeComponentBuilder.NodeComponentList(
                NodeComponentBuilder.NodeComponentAutoCloner(NodeComponentBuilder.NodeField("Case").WithInputType(typeDef).WithFlowOutput().Build, 0, (x) => $"Case {x + 1}").Build,
                defaultOutput
            ).Build;
        }

        private static INodeField ValueDisplay(ITypeDefinition typeDef, object x)
        {
            INodeField output = NodeComponentBuilder.NodeField(x.ToString()).WithValueTypeProvider("Displayed", NodeComponentBuilder.RigidTypeDefinitionManager(typeDef.DefaultValue, null, "DefaultDisplay").Build, false).Build;
            output.DisplayedValue.Value = x;
            output.SetFlowOutput();
            return output;
        }
    }
}

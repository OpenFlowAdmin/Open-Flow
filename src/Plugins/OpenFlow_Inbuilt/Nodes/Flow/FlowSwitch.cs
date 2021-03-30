namespace OpenFlow_Inbuilt.Nodes.Flow
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;
    using OpenFlow_PluginFramework.Primitives.TypeDefinition;
    using OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider;

    public class FlowSwitch : IFlowNode
    {
        private readonly INodeLabel flowInput = NodeComponentBuilder.NodeLabel("Flow Input").WithFlowInput().Build;

        private readonly INodeField valueInput = NodeComponentBuilder.NodeField("Switch Value").WithInputTypeProvider(new AcceptsAllTypeDefinitionProvider()).Build;

        private readonly INodeLabel OutputsLabel = NodeComponentBuilder.NodeLabel("Possible Values").Build;

        private readonly INodeLabel defaultOutput = NodeComponentBuilder.NodeLabel("Default").WithFlowOutput().Build;

        private readonly NodeComponentDictionary flowOutputs = new()
        {
            {
                typeof(bool),
                    new NodeComponentCollection(new[] {
                        NodeComponentBuilder.NodeField("True").WithValueTypeProvider("Displayed", new TypeDefinition<double>() { DisplayName = "DefaultDisplay", DefaultValue = true}, false).WithFlowOutput().Build,
                        NodeComponentBuilder.NodeField("False").WithValueTypeProvider("Displayed", new TypeDefinition<double>() { DisplayName = "DefaultDisplay", DefaultValue = false}, false).WithFlowOutput().Build,
                    })
            },
        };

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
                yield return NodeDecorators.MajorSeparator;
                yield return OutputsLabel;
                yield return NodeDecorators.MajorSeparator;
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
            if (e.PropertyName == nameof(LaminarValue.TypeDefinition))
            {
                ChangeSwitchTypeTo((sender as LaminarValue).TypeDefinition);
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

        private NodeComponent GenerateSwitchesFor(ITypeDefinition typeDef)
        {
            if (typeDef.ValueType.IsEnum)
            {
                return new NodeComponentCollection(Enum.GetNames(typeDef.ValueType).Select(x => ValueDisplay(typeDef, Enum.Parse(typeDef.ValueType, x)).WithFlowOutput()));
            }

            return new NodeComponentCollection(
                new NodeComponentAutoCloner(NodeComponentBuilder.NodeField("Case").WithInputTypeProvider(typeDef).WithFlowOutput().Build, 0, (x) => $"Case {x + 1}"),
                defaultOutput
            );
        }

        private static INodeField ValueDisplay(ITypeDefinition typeDef, object x)
        {
            INodeField output = NodeComponentBuilder.NodeField(x.ToString()).WithValueTypeProvider("Displayed", new CopiedTypeDefinition(typeDef) { DisplayName = "DefaultDisplay" }, false).Build;
            output.DisplayedValue.Value = x;
            return output;
        }
    }
}

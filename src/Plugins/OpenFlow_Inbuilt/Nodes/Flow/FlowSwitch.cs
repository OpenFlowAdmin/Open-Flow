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
        private readonly NodeLabel flowInput = new NodeLabel("Flow Input").WithFlowInput(true);

        private readonly NodeField valueInput = new NodeField() { Name = "Switch Value" }.WithInputTypeProvider(new AcceptsAllTypeDefinitionProvider());

        private readonly NodeLabel OutputsLabel = new("Possible Values");

        private readonly NodeLabel defaultOutput = new NodeLabel("Default").WithFlowOutput();

        private readonly NodeComponentDictionary flowOutputs = new()
        {
            {
                typeof(bool),
                    new NodeComponentCollection(new[] {
                        new NodeField() { Name = "True" }.WithTypeProvider("Displayed", new TypeDefinition<double>() { DisplayName = "DefaultDisplay", DefaultValue = true}, false).WithFlowOutput(),
                        new NodeField() { Name = "False" }.WithTypeProvider("Displayed", new TypeDefinition<double>() { DisplayName = "DefaultDisplay", DefaultValue = false}, false).WithFlowOutput(),
                    })
            },
        };

        public FlowSwitch()
        {
            valueInput.InputDisplayValue.PropertyChanged += FlowSwitch_OnTypeDefinitionChanged;
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

        public VisualNodeComponent FlowOutField { get; private set; }

        public void Evaluate()
        {
            foreach (VisualNodeComponent field in flowOutputs.VisualComponentList)
            {
                if (field is NodeField valueField && valueField.DisplayedValue != null && valueField.DisplayedValue.Value.Equals(valueInput.Input))
                {
                    FlowOutField = valueField;
                    // this.SetSpecialField(SpecialFieldFlags.FlowOutput, valueField);
                    return;
                }
            }

            FlowOutField = defaultOutput;
            // this.SetSpecialField(SpecialFieldFlags.FlowOutput, defaultOutput);
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
                new NodeComponentAutoCloner(new NodeField() { Name = "Case" }.WithInputTypeProvider(typeDef).WithFlowOutput(), 0, (x) => $"Case {x + 1}"),
                defaultOutput
            );
        }

        private static NodeField ValueDisplay(ITypeDefinition typeDef, object x)
        {
            NodeField output = new NodeField() { Name = x.ToString() }.WithTypeProvider("Displayed", new CopiedTypeDefinition(typeDef) { DisplayName = "DefaultDisplay" }, false);
            output.DisplayedValue.Value = x;
            return output;
        }
    }
}

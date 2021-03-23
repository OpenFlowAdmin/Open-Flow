namespace OpenFlow_Inbuilt.Nodes.Flow
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;
    using OpenFlow_PluginFramework.Primitives.TypeDefinition;
    using OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider;

    public class FlowSwitch : INode
    {
        private readonly NodeField flowInput = new NodeField("Flow Input").WithFlowInput(true);

        private readonly ValueField valueInput = new ValueField("Switch Value").WithInputTypeProvider(new AcceptsAllTypeDefinitionProvider());

        private readonly NodeField OutputsLabel = new NodeField("Possible Values");

        private readonly NodeField defaultOutput = new NodeField("Default").WithFlowOutput();

        private readonly NodeComponentDictionary flowOutputs = new()
        {
            {
                typeof(bool),
                    new NodeComponentCollection(new[] {
                        new ValueField("True").WithTypeProvider("Displayed", new TypeDefinition<double>() { DisplayName = "DefaultDisplay", DefaultValue = true}, false).WithFlowOutput(),
                        new ValueField("False").WithTypeProvider("Displayed", new TypeDefinition<double>() { DisplayName = "DefaultDisplay", DefaultValue = false}, false).WithFlowOutput(),
                    })
            },
        };

        public FlowSwitch()
        {
            valueInput.InputDisplayValue.PropertyChanged += FlowSwitch_OnTypeDefinitionChanged;
        }

        public string NodeName => "Switch";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return flowInput;
                yield return valueInput;
                yield return OutputsLabel;
                yield return flowOutputs;
            }
        }

        public void Evaluate()
        {
            foreach (NodeField field in flowOutputs.AllFields)
            {
                if (field is ValueField valueField && valueField.DisplayedValue.Value.Equals(valueInput.Input))
                {
                    this.SetSpecialField(SpecialFieldFlags.FlowOutput, field);
                    return;
                }
            }

            this.SetSpecialField(SpecialFieldFlags.FlowOutput, defaultOutput);
        }

        private void FlowSwitch_OnTypeDefinitionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OpenFlowValue.TypeDefinition))
            {
                ChangeSwitchTypeTo((sender as OpenFlowValue).TypeDefinition);
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

            return new NodeComponentList() {
                new ValueFieldGenerator(new ValueField("Case").WithInputTypeProvider(typeDef).WithFlowOutput() as ValueField, 0, (x) => $"Case {x + 1}"),
                defaultOutput,
            };
        }

        private static ValueField ValueDisplay(ITypeDefinition typeDef, object x)
        {
            ValueField output = new ValueField(x.ToString()).WithTypeProvider("Displayed", new CopiedTypeDefinition(typeDef) { DisplayName = "DefaultDisplay" }, false);
            output.DisplayedValue.Value = x;
            return output;
        }
    }
}

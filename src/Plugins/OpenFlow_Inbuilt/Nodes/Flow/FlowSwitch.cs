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

        private readonly NodeComponentDictionary flowOutputs = new()
        {
            {
                typeof(bool),
                    new NodeComponentCollection(new[] {
                        new ValueField("True").WithTypeProvider("Displayed", new TypeDefinition<double>() { DisplayName = "DefaultDisplay", DefaultValue = true}).WithFlowOutput(),
                        new ValueField("False").WithTypeProvider("Displayed", new TypeDefinition<double>() { DisplayName = "DefaultDisplay", DefaultValue = false}).WithFlowOutput(),
                    })
            },
            {
                typeof(double) , new NodeField("A double was put in idk what to do with that")
            }
        }
            ;

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
                yield return flowOutputs;
            }
        }

        public void Evaluate()
        {
            foreach (ValueField field in flowOutputs.AllFields)
            {
                if (field.DisplayedValue.Value.Equals(valueInput.Input))
                {
                    this.SetSpecialField(SpecialFieldFlags.FlowOutput, field);
                    return;
                }
            }
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
            Debug.WriteLine("Type definition changed");
            flowOutputs.HideAllComponents();
            if (!flowOutputs.ContainsKey(typeDef.ValueType))
            {
                if (typeDef.ValueType.IsEnum)
                {
                    flowOutputs.Add(typeDef.ValueType,
                        new NodeComponentCollection(typeDef.ValueType.GetEnumNames().Select(x => {
                            ValueField newField = new ValueField(x).WithTypeProvider("Displayed", new CopiedTypeDefinition(typeDef) { DisplayName = "DefaultDisplay" }).WithFlowOutput() as ValueField;
                            newField.DisplayedValue.Value = Enum.Parse(typeDef.ValueType, x);
                            return newField;
                        })));
                }
                else
                {
                    flowOutputs.Add(typeDef.ValueType, new ValueFieldGenerator(
                        new ValueField("Case").WithInputTypeProvider(typeDef).WithFlowOutput() as ValueField,
                        0,
                        (x) => $"Case {x + 1}"
));
                }
            }

            flowOutputs.ShowSectionByKey(typeDef.ValueType);
        }
    }
}

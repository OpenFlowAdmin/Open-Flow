namespace OpenFlow_Inbuilt.Nodes.Flow
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives.TypeDefinition;

    public class FlowSwitch : INode
    {
        private readonly NodeField flowInput = new NodeField("Flow Input").WithFlowInput(true);

        private readonly ValueField valueInput = new ValueField("Value").WithInput(new ITypeDefinition[]
        {
            new TypeDefinition<bool>(),
        });

        private readonly ValueFieldGenerator possibleOutputs = new ValueFieldGenerator(new ValueField("Outputs"), 0, null);

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
                yield return possibleOutputs;
            }
        }

        public void Evaluate()
        {
            if (valueInput[ValueField.InputKey] is bool boolInput)
            {
                if (boolInput == true)
                {
                    this.SetSpecialField(SpecialFieldFlags.FlowOutput, (NodeField)possibleOutputs.AllFields[0]);
                }
                else
                {
                    this.SetSpecialField(SpecialFieldFlags.FlowOutput, (NodeField)possibleOutputs.AllFields[1]);
                }
            }
            else
            {
                foreach (ValueField field in possibleOutputs)
                {
                    if (field.Input != null && field[ValueField.InputKey].Equals(valueInput[ValueField.InputKey]))
                    {
                        this.SetSpecialField(SpecialFieldFlags.FlowOutput, field);
                    }
                }
            }
        }

        private void FlowSwitch_OnTypeDefinitionChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}

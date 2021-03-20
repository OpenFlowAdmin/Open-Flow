namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeMultiply : INode
    {
        private readonly ValueFieldGenerator multiplyFields = new(new ValueField("Number").WithInput(1.0), 1, index => $"Number {index + 1}");
        private readonly ValueField outputField = new ValueField("Product").WithOutput<double>();

        public string NodeName => "Multiply";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return multiplyFields;
                yield return outputField;
            }
        }

        public void Evaluate()
        {
            double output = 1.0;

            foreach (ValueField field in multiplyFields)
            {
                output *= (double)field.Input;
            }

            outputField.Output = output;
        }
    }
}

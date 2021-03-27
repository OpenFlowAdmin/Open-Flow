namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeMultiply : INode
    {
        private readonly NodeComponentAutoCloner multiplyFields = new(new NodeField() { Name = "Number" }.WithInput(1.0), 1, index => $"Number {index + 1}");
        private readonly NodeField outputField = new NodeField() { Name = "Product" }.WithOutput<double>();

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

            foreach (NodeField field in multiplyFields)
            {
                output *= (double)field.Input;
            }

            outputField.Output = output;
        }
    }
}

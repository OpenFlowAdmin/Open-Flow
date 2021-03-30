namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeMultiply : INode
    {
        private readonly NodeComponentAutoCloner multiplyFields = new(NodeComponentBuilder.NodeField("Number").WithInput(0.0).Build, 1, index => $"Number {index + 1}");
        private readonly INodeField outputField = NodeComponentBuilder.NodeField("Product").WithOutput(0.0).Build;

        public string NodeName => "Multiply";

        public IEnumerable<INodeComponent> Fields
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

            foreach (INodeField field in multiplyFields)
            {
                output *= (double)field.Input;
            }

            outputField.Output = output;
        }
    }
}

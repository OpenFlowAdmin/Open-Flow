namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeDifference : INode
    {
        private readonly INodeField firstNumber = NodeComponentBuilder.NodeField("Number 1").WithInput(0.0).Build;
        private readonly INodeField secondNumber = NodeComponentBuilder.NodeField("Number 2").WithInput(0.0).Build;
        private readonly INodeField outputField = NodeComponentBuilder.NodeField("Difference").WithOutput(0.0).Build;

        public string NodeName => "Difference";

        public IEnumerable<INodeComponent> Fields
        {
            get
            {
                yield return firstNumber;
                yield return secondNumber;
                yield return outputField;
            }
        }

        public void Evaluate()
        {
            outputField.Output = (double)firstNumber.Input - (double)secondNumber.Input;
        }
    }
}

namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeDivide : INode
    {
        private readonly NodeField firstNumber = new NodeField() { Name = "Number 1" }.WithInput<double>();
        private readonly NodeField secondNumber = new NodeField() { Name = "Number 2" }.WithInput(1.0);
        private readonly NodeField outputField = new NodeField() { Name = "Difference" }.WithOutput<double>();

        public string NodeName => "Divide";

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
            outputField.Output = (double)firstNumber.Input / (double)secondNumber.Input;
        }
    }
}

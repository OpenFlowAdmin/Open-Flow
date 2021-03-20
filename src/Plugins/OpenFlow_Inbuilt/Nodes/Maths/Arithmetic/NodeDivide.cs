namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeDivide : INode
    {
        private readonly ValueField firstNumber = new ValueField("Number 1").WithInput<double>(0);
        private readonly ValueField secondNumber = new ValueField("Number 2").WithInput<double>(1);
        private readonly ValueField outputField = new ValueField("Difference").WithOutput<double>(0);

        public string NodeName => "Divide";

        public IEnumerable<NodeComponent> Fields
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

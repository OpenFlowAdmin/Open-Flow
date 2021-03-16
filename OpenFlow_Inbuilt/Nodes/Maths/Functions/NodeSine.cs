namespace OpenFlow_Inbuilt.Nodes.Maths.Functions
{
    using System;
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeSine : INode
    {
        private readonly ValueField inputField = new ValueField("x").WithInput<double>(0);
        private readonly ValueField outputField = new ValueField("sin(x)").WithOutput<double>();

        public string NodeName => "Sine";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return inputField;
                yield return outputField;
            }
        }

        public void Evaluate()
        {
            outputField.Output = Math.Sin((double)inputField.Input);
        }
    }
}

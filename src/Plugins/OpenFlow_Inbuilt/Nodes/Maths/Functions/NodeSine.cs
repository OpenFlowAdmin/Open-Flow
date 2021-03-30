namespace OpenFlow_Inbuilt.Nodes.Maths.Functions
{
    using System;
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeSine : INode
    {
        private readonly NodeField inputField = new NodeField() { Name = "x" }.WithInput<double>(0);
        private readonly NodeField outputField = new NodeField() { Name = "sin(x)" }.WithOutput<double>();

        public string NodeName => "Sine";

        public IEnumerable<INodeComponent> Fields
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

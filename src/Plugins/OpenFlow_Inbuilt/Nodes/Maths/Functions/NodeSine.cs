namespace OpenFlow_Inbuilt.Nodes.Maths.Functions
{
    using System;
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeSine : INode
    {
        private readonly INodeField inputField = NodeComponentBuilder.NodeField("x").WithInput(0.0).Build;
        private readonly INodeField outputField = NodeComponentBuilder.NodeField("sin(x)").WithOutput(0.0).Build;

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

namespace OpenFlow_Inbuilt.Nodes.StringOperations
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class Node_Convert_To_String : INode
    {
        private readonly INodeField converterField = NodeComponentBuilder.NodeField("Text")
            .WithInputTypeProvider(NodeComponentBuilder.TypeDefinitionManager().Build)
            .WithOutput("").Build;

        public string NodeName => "Convert to Text";

        public IEnumerable<INodeComponent> Fields
        {
            get
            {
                yield return converterField;
            }
        }

        public void Evaluate()
        {
            converterField.Output = converterField.Input == null ? string.Empty : converterField.Input.ToString();
        }
    }
}

namespace OpenFlow_Inbuilt.Nodes.StringOperations
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider;

    public class Node_Convert_To_String : INode
    {
        private readonly NodeField converterField = new NodeField("Text")
            .WithInputTypeProvider(new AcceptsAllTypeDefinitionProvider())
            .WithOutput<string>();

        public string NodeName => "Convert to Text";

        public IEnumerable<NodeComponent> Fields
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

namespace OpenFlow_Inbuilt.Nodes.StringOperations
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class Node_Join_Strings : INode
    {
        private readonly NodeComponentAutoCloner combineStrings = new(new NodeField() { Name = "Text" }.WithInput<string>(), 2, (x) => $"Text {x}");
        private readonly NodeField combinedString = new NodeField() { Name = "Combined String" }.WithOutput<string>();

        public string NodeName => "Join Strings";

        public IEnumerable<INodeComponent> Fields
        {
            get
            {
                yield return combineStrings;
                yield return combinedString;
            }
        }

        public void Evaluate()
        {
            string output = string.Empty;
            foreach (NodeField field in combineStrings)
            {
                output += (string)field.Input;
            }

            combinedString.Output = output;
        }
    }
}

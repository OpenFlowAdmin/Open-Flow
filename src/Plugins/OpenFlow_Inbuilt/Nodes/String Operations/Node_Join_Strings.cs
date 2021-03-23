namespace OpenFlow_Inbuilt.Nodes.StringOperations
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class Node_Join_Strings : INode
    {
        private readonly NodeComponentAutoCloner combineStrings = new(new ValueField("Text").WithInput<string>(), 2);
        private readonly ValueField combinedString = new ValueField("Combined String").WithOutput<string>();

        public string NodeName => "Join Strings";

        public IEnumerable<NodeComponent> Fields
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
            foreach (ValueField field in combineStrings)
            {
                output += (string)field.Input;
            }

            combinedString.Output = output;
        }
    }
}

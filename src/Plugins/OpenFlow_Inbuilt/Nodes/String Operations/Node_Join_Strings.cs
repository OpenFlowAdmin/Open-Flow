﻿namespace OpenFlow_Inbuilt.Nodes.StringOperations
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Collections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class Node_Join_Strings : INode
    {
        private readonly INodeComponentAutoCloner combineStrings = NodeComponentBuilder.NodeComponentAutoCloner(NodeComponentBuilder.NodeField("Text").WithInput("").Build, 2, (x) => $"Text {x}").Build;
        private readonly INodeField combinedString = NodeComponentBuilder.NodeField("Combined").WithOutput("").Build;

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
            foreach (INodeField field in combineStrings)
            {
                output += (string)field.Input;
            }

            combinedString.Output = output;
        }
    }
}

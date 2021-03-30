﻿namespace OpenFlow_Core.Nodes
{
    using System.Collections.Generic;
    using System.Linq;
    using OpenFlow_Core.Nodes.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class LoadedNodeManager
    {
        public LoadedNodeManager()
        {
            NodeComponentBuilder.Factory = new NodeComponentFactory()
                .RegisterImplementation<INodeField, NodeField>()
                .RegisterImplementation<INodeLabel, NodeLabel>();
        }

        public NodeCatagories LoadedNodes { get; } = new();

        public void AddNodeToCatagory<TNode>(string catagoryName, string subCatagoryName = null)
            where TNode : INode, new()
        {
            LoadedNodes.PlaceNode(new string[] { catagoryName, subCatagoryName }, new NodeBase(new TNode()));
        }

        public class NodeCatagories
        {
            public Dictionary<string, NodeCatagories> SubCatagories { get; } = new Dictionary<string, NodeCatagories>();

            public List<NodeBase> Nodes { get; } = new List<NodeBase>();

            public List<NodeBase> FirstGroup()
            {
                if (Nodes.Count > 0)
                {
                    return Nodes;
                }
                else
                {
                    return SubCatagories.First().Value.FirstGroup();
                }
            }

            public void PlaceNode(IEnumerable<string> catagoryPath, NodeBase node)
            {
                if (!catagoryPath.Any() || catagoryPath.First() == null || catagoryPath.First() == "")
                {
                    Nodes.Add(node);
                }
                else
                {
                    if (!SubCatagories.ContainsKey(catagoryPath.First()))
                    {
                        SubCatagories.Add(catagoryPath.First(), new NodeCatagories());
                    }

                    SubCatagories[catagoryPath.First()].PlaceNode(catagoryPath.Skip(1), node);
                }
            }
        }
    }
}

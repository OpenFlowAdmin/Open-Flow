namespace OpenFlow_PluginFramework.NodeSystem.Nodes
{
    using System;
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;

    public static class NodeExtensions
    {
        private static readonly Dictionary<INode, NodeField[]> FlaggedNodeFields = new Dictionary<INode, NodeField[]>();
        private static readonly Dictionary<INode, Action> TriggerNodeEvaluate = new Dictionary<INode, Action>();

        public static void SetSpecialField(this INode node, SpecialFieldFlags flag, NodeField field)
        {
            if (!FlaggedNodeFields.ContainsKey(node))
            {
                FlaggedNodeFields.Add(node, new NodeField[Enum.GetNames(typeof(SpecialFieldFlags)).Length]);
            }

            FlaggedNodeFields[node][(int)flag] = field;
        }

        public static NodeField GetSpecialField(this INode node, SpecialFieldFlags flag)
        {
            return node.TryGetSpecialField(flag, out NodeField field) ? field : null;
        }

        public static bool TryGetSpecialField(this INode node, SpecialFieldFlags flag, out NodeField field)
        {
            if (FlaggedNodeFields.TryGetValue(node, out NodeField[] specialFields) && specialFields[(int)flag] != null)
            {
                field = specialFields[(int)flag];
                return true;
            }
            else
            {
                field = default;
                return false;
            }
        }

        public static void SubscribeToEvaluate(this INode node, Action onEvaluate)
        {
            if (!TriggerNodeEvaluate.ContainsKey(node))
            {
                TriggerNodeEvaluate.Add(node, new Action(() => { }));
            }

            TriggerNodeEvaluate[node] += onEvaluate;
        }

        public static void TriggerEvaluate(this INode node)
        {
            if (TriggerNodeEvaluate.TryGetValue(node, out Action trigger))
            {
                trigger?.Invoke();
            }
        }
    }
}

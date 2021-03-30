namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;
    using System;
    using System.Collections.Generic;

    public static class ComponentFlowHandler
    {
        private static readonly Dictionary<IVisualNodeComponent, FlowState> FlowStates = new();
        private static readonly Dictionary<INode, IVisualNodeComponent> NodeFlowOuts = new();

        public static void SetFlowOutput(this INode node, IVisualNodeComponent nodeComponent) => NodeFlowOuts[node] = nodeComponent;

        public static IVisualNodeComponent GetFlowOutput(this INode node) => NodeFlowOuts[node];

        public static bool HasFlowOutput(this INode node) => NodeFlowOuts.ContainsKey(node);

        public static T WithFlowInput<T>(this T field, bool inputState = true) where T : IVisualNodeComponent
        {
            if (FlowStates.TryGetValue(field, out FlowState _))
            {
                FlowStates[field].Input.Value = inputState;
            }
            else
            {
                FlowStates[field] = new FlowState(new ObservableValue<bool>(inputState), new ObservableValue<bool>(false));
            }

            return field;
        }

        public static T WithFlowOutput<T>(this T field, bool outputState = true) where T : IVisualNodeComponent
        {
            if (FlowStates.TryGetValue(field, out FlowState _))
            {
                FlowStates[field].Output.Value = outputState;
            }
            else
            {
                FlowStates[field] = new FlowState(new ObservableValue<bool>(false), new ObservableValue<bool>(outputState));
            }

            return field;
        }

        public static ObservableValue<bool> GetFlowInput(this IVisualNodeComponent field)
        {
            if (!FlowStates.ContainsKey(field))
            {
                FlowStates[field] = new FlowState(new ObservableValue<bool>(false), new ObservableValue<bool>(false));
            }

            return FlowStates[field].Input;
        }

        public static ObservableValue<bool> GetFlowOutput(this IVisualNodeComponent field)
        {
            if (!FlowStates.ContainsKey(field))
            {
                FlowStates[field] = new FlowState(new ObservableValue<bool>(false), new ObservableValue<bool>(false));
            }

            return FlowStates[field].Output;
        }

        private record FlowState(ObservableValue<bool> Input, ObservableValue<bool> Output);
    }
}

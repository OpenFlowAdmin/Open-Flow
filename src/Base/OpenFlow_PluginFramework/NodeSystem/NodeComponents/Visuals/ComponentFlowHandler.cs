namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;
    using System;
    using System.Collections.Generic;

    public static class ComponentFlowHandler
    {
        private static readonly Dictionary<VisualNodeComponent, FlowState> FlowStates = new();
        private static readonly Dictionary<INode, VisualNodeComponent> NodeFlowOuts = new();

        public static void SetFlowOutput(this INode node, VisualNodeComponent nodeComponent) => NodeFlowOuts[node] = nodeComponent;

        public static VisualNodeComponent GetFlowOutput(this INode node) => NodeFlowOuts[node];

        public static bool HasFlowOutput(this INode node) => NodeFlowOuts.ContainsKey(node);

        public static T WithFlowInput<T>(this T field, bool inputState = true) where T : VisualNodeComponent
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

        public static T WithFlowOutput<T>(this T field, bool outputState = true) where T : VisualNodeComponent
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

        public static ObservableValue<bool> GetFlowInput(this VisualNodeComponent field)
        {
            if (!FlowStates.ContainsKey(field))
            {
                FlowStates[field] = new FlowState(new ObservableValue<bool>(false), new ObservableValue<bool>(false));
            }

            return FlowStates[field].Input;
        }

        public static ObservableValue<bool> GetFlowOutput(this VisualNodeComponent field)
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

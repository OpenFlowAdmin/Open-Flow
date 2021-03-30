namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;
    using System;
    using System.Collections.Generic;

    public static class ComponentFlowHandler
    {
        private static readonly Dictionary<IVisualNodeComponent, FlowState> FlowStates = new();

        public static void SetFlowInput(this IVisualNodeComponent field, bool inputState = true)
        {
            if (FlowStates.TryGetValue(field, out FlowState _))
            {
                FlowStates[field].Input.Value = inputState;
            }
            else
            {
                FlowStates[field] = new FlowState(new ObservableValue<bool>(inputState), new ObservableValue<bool>(false));
            }
        }

        public static void SetFlowOutput(this IVisualNodeComponent field, bool outputState = true)
        {
            if (FlowStates.TryGetValue(field, out FlowState _))
            {
                FlowStates[field].Output.Value = outputState;
            }
            else
            {
                FlowStates[field] = new FlowState(new ObservableValue<bool>(false), new ObservableValue<bool>(outputState));
            }
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

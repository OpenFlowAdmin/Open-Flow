namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    using System;
    using System.Collections.Generic;

    public static class FieldFlowHandler
    {
        private static readonly Dictionary<VisualNodeComponent, FlowState> FlowStates = new();

        public static NodeField WithFlowInput(this NodeField field, bool inputState = true)
        {
            if (FlowStates.TryGetValue(field, out FlowState _))
            {
                FlowStates[field].Input.Val = inputState;
            }
            else
            {
                FlowStates[field] = new FlowState(inputState, false);
            }

            return field;
        }

        public static NodeField WithFlowOutput(this NodeField field, bool outputState = true)
        {
            if (FlowStates.TryGetValue(field, out FlowState _))
            {
                FlowStates[field].Output.Val = outputState;
            }
            else
            {
                FlowStates[field] = new FlowState(false, outputState);
            }

            return field;
        }

        public static NotifyChange<bool> GetFlowInput(this NodeField field)
        {
            if (!FlowStates.ContainsKey(field))
            {
                FlowStates[field] = new FlowState(false, false);
            }

            return FlowStates[field].Input;
        }

        public static NotifyChange<bool> GetFlowOutput(this NodeField field)
        {
            if (!FlowStates.ContainsKey(field))
            {
                FlowStates[field] = new FlowState(false, false);
            }

            return FlowStates[field].Output;
        }

        private record FlowState(NotifyChange<bool> Input, NotifyChange<bool> Output);

        public class NotifyChange<T>
        {
            private T val;
            private Action<T> update;

            public NotifyChange(T val)
            {
                Val = val;
            }

            public T Val
            {
                get => val;
                set
                {
                    val = value;
                    update?.Invoke(val);
                }
            }

            public static implicit operator NotifyChange<T>(T val) => new(val);

            public static implicit operator T(NotifyChange<T> notChange) => notChange.Val;

            public void SubscribeToChange(Action<T> action) => update += action;
        }
    }
}

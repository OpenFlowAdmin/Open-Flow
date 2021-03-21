namespace OpenFlow_Core.Nodes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class FlowSourceNode : INode
    {
        private readonly NodeField sourceField = new ValueField("Manual Trigger").WithValue<Action>("Displayed", () => { Debug.WriteLine("Button pressy pressy"); }).WithFlowOutput();

        public FlowSourceNode()
        {
            Debug.WriteLine((sourceField as ValueField)?.GetDisplayValue("Displayed").IsUserEditable);
            Debug.WriteLine((sourceField as ValueField)?.GetDisplayValue("Displayed").TypeDefinition.DisplayName);
            // sourceField["Trigger Button"] = new Action(() => { });
        }

        public NodeField FlowOutField => sourceField;

        public string NodeName => "Flow Source";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return sourceField;
            }
        }

        public void Evaluate()
        {
        }
    }
}

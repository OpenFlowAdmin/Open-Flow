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
        private readonly NodeField _sourceField = new ValueField("Manual Trigger").WithValue<Action>("Displayed", false, () => { Debug.WriteLine("Button pressy pressy"); }).WithFlowOutput();

        public FlowSourceNode()
        {
            Debug.WriteLine((_sourceField as ValueField)?.GetDisplayValue("Displayed").IsUserEditable);
            Debug.WriteLine((_sourceField as ValueField)?.GetDisplayValue("Displayed").TypeDefinition.DisplayName);
            // sourceField["Trigger Button"] = new Action(() => { });
        }

        public NodeField FlowOutField => _sourceField;

        public string NodeName => "Flow Source";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return _sourceField;
            }
        }

        public void Evaluate()
        {
        }
    }
}

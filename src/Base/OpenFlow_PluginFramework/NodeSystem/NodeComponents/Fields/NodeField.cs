namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeField : NodeComponent
    {
        private string _name;

        public NodeField(string name)
        {
            Name = name;
            NodeFields = new List<NodeField>() { this };
        }

        public virtual string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public override IList NodeFields { get; }

        public override NodeComponent Clone() => CloneTo(new NodeField(Name));

        protected override NodeField CloneTo(NodeComponent nodeField)
        {
            base.CloneTo(nodeField);
            (nodeField as NodeField).Name = Name;
            return (nodeField as NodeField).WithFlowInput(this.GetFlowInput()).WithFlowOutput(this.GetFlowOutput());
        }
    }
}

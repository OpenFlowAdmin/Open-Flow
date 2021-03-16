namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeField : NodeComponent, ICloneable
    {
        private string name;

        public NodeField(string name)
        {
            Name = name;
        }

        public virtual string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public override int FieldCount { get; } = 1;

        public object Clone() => CloneTo(new NodeField(Name));

        public virtual NodeField CloneTo(NodeField nodeField)
        {
            nodeField.Name = Name;
            nodeField.Opacity.Value= Opacity.Value;
            return nodeField.WithFlowInput(this.GetFlowInput().Val).WithFlowOutput(this.GetFlowOutput());
        }
    }
}

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;

    public class VisualNodeComponent : NodeComponent, IVisualNodeComponent
    {
        private string _name;

        public VisualNodeComponent()
        {
            VisualComponentList = new List<VisualNodeComponent>() { this };
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

        public override IList VisualComponentList { get; }

        public override NodeComponent Clone() => CloneTo(new VisualNodeComponent());

        protected override VisualNodeComponent CloneTo(NodeComponent nodeField)
        {
            base.CloneTo(nodeField);
            (nodeField as VisualNodeComponent).Name = Name;
            return (nodeField as VisualNodeComponent).WithFlowInput(this.GetFlowInput()).WithFlowOutput(this.GetFlowOutput());
        }
    }
}

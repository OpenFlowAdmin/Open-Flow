﻿namespace OpenFlow_Core.Nodes.NodeComponents.Visuals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;

    public class VisualNodeComponent : NodeComponent, IVisualNodeComponent
    {
        private string _name;

        public VisualNodeComponent(IOpacity opacity) : base(opacity)
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

        public override INodeComponent Clone() => CloneTo(NodeComponentBuilder.GetInstance<IVisualNodeComponent>().Build);

        protected override IVisualNodeComponent CloneTo(INodeComponent nodeField)
        {
            base.CloneTo(nodeField);
            (nodeField as VisualNodeComponent).Name = Name;
            (nodeField as VisualNodeComponent).SetFlowOutput(this.GetFlowOutput());
            (nodeField as VisualNodeComponent).SetFlowInput(this.GetFlowInput());
            return nodeField as VisualNodeComponent;
        }
    }
}

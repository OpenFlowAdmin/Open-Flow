namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;

    public abstract class  NodeComponent : INotifyPropertyChanged
    {
        private bool _isVisible = true;
        private Action<NodeComponent> _removeAction;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<bool> VisibilityChanged;

        public virtual INode ParentNode { get; set; }

        public abstract IList VisualComponentList { get; }

        public Action RemoveSelf { get; private set; }

        public Opacity Opacity { get; } = new();

        public virtual bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    VisibilityChanged?.Invoke(this, _isVisible);
                }
            }
        }

        public void SetRemoveAction(Action<NodeComponent> removeAction)
        {
            _removeAction = removeAction;
            RemoveSelf = () =>
            {
                removeAction(this);
                ParentNode.TriggerEvaluate();
            };
        }

        public abstract NodeComponent Clone();

        protected virtual NodeComponent CloneTo(NodeComponent component)
        {
            component.ParentNode = ParentNode;
            component.SetRemoveAction(_removeAction);
            component.Opacity.Value = Opacity.Value;

            return component;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

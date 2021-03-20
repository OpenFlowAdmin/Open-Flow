namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents
{
    using System;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<bool> VisibilityChanged;

        public virtual INode ParentNode { get; set; }

        public Action<NodeComponent> RemoveAction 
        { 
            init
            {
                RemoveSelf = () => 
                {
                    value(this);
                    ParentNode.TriggerEvaluate();
                };
            }
        }

        public Action RemoveSelf { get; private set; }

        public Opacity Opacity { get; } = new();

        public abstract int FieldCount { get; }

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

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

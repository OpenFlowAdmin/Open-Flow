using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Nodes.VisualNodeComponentDisplays
{
    public class NodeLabelDisplay : IVisualNodeComponentDisplay, INotifyPropertyChanged
    {
        private readonly NodeLabel _child;

        public NodeLabelDisplay(NodeLabel child)
        {
            _child = child;
            child.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName is nameof(NodeLabel.Name))
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
                }
            };
        }

        public string LabelText => _child.Name;

        public Opacity Opacity => _child.Opacity;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

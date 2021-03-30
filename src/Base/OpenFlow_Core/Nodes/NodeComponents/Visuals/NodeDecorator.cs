using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Nodes.NodeComponents.Visuals
{
    class NodeDecorator : VisualNodeComponent, INodeDecorator
    {
        public NodeDecoratorType DecoratorType { get; set; }
    }
}

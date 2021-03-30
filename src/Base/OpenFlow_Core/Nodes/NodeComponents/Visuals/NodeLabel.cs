using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Nodes.NodeComponents.Visuals
{
    public class NodeLabel : VisualNodeComponent, INodeLabel
    {
        public NodeLabel(IOpacity opacity) : base(opacity) { }
    }
}

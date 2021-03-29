using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Nodes.VisualNodeComponentDisplays
{
    public class MinorSeperator : VisualNodeComponentDisplay<NodeDecorators>
    {
        public MinorSeperator(NodeBase parent) : base(parent, NodeDecorators.MinorSeparator) { }
    }
}

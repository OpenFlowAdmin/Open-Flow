using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Nodes
{
    class NodeFieldMapper : ITypeMapper<NodeField, DisplayNodeField>
    {
        public DisplayNodeField MapType(NodeField toMap)
        {
            return new DisplayNodeField(toMap);
        }
    }
}

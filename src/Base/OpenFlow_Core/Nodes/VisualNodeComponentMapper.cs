using OpenFlow_Core.Nodes.VisualNodeComponentDisplays;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Nodes
{
    class VisualNodeComponentMapper : ITypeMapper<VisualNodeComponent, IVisualNodeComponentDisplay>
    {
        private NodeBase _parentNode;

        public VisualNodeComponentMapper(NodeBase parentNode)
        {
            _parentNode = parentNode;
        }

        public IVisualNodeComponentDisplay MapType(VisualNodeComponent toMap)
        {
            if (typeof(NodeField).IsAssignableFrom(toMap.GetType()))
            {
                return new NodeFieldDisplay((NodeField)toMap, _parentNode);
            }

            if (typeof(NodeLabel).IsAssignableFrom(toMap.GetType()))
            {
                return new NodeLabelDisplay((NodeLabel)toMap);
            }

            if (toMap is Decorators decorator)
            {
                switch (decorator.Type)
                {
                    case Decorators.DecoratorType.MajorSeperator:
                        return new MajorSeperator();
                    case Decorators.DecoratorType.MinorSeperator:
                        return new MinorSeperator();
                }
            }

            return null;
        }
    }
}

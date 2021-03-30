﻿using OpenFlow_Core.Nodes.VisualNodeComponentDisplays;
using OpenFlow_Core.Nodes.NodeComponents.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;

namespace OpenFlow_Core.Nodes
{
    class VisualNodeComponentMapper : ITypeMapper<IVisualNodeComponent, IVisualNodeComponentDisplay>
    {
        private readonly NodeBase _parentNode;

        public VisualNodeComponentMapper(NodeBase parentNode)
        {
            _parentNode = parentNode;
        }

        public IVisualNodeComponentDisplay MapType(IVisualNodeComponent toMap)
        {
            if (typeof(NodeField).IsAssignableFrom(toMap.GetType()))
            {
                return new NodeFieldDisplay((NodeField)toMap, _parentNode);
            }

            if (typeof(NodeLabel).IsAssignableFrom(toMap.GetType()))
            {
                return new NodeLabelDisplay(_parentNode, (NodeLabel)toMap);
            }

            if (toMap is NodeDecorators decorator)
            {
                switch (decorator.Type)
                {
                    case NodeDecorators.DecoratorType.MajorSeparator:
                        return new MajorSeperator(_parentNode);
                    case NodeDecorators.DecoratorType.MinorSeparator:
                        return new MinorSeperator(_parentNode);
                }
            }

            return null;
        }
    }
}

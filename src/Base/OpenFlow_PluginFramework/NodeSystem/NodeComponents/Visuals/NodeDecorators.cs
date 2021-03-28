using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    public class NodeDecorators : VisualNodeComponent
    {
        private NodeDecorators(DecoratorType type)
        {
            Type = type;
        }

        public DecoratorType Type { get; }

        public static NodeDecorators MajorSeperator { get; } = new NodeDecorators(DecoratorType.MajorSeperator);

        public static NodeDecorators MinorSeperator { get; } = new NodeDecorators(DecoratorType.MinorSeperator);

        public enum DecoratorType
        {
            MajorSeperator,
            MinorSeperator,
        }
    }
}

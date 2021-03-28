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

        public static NodeDecorators MajorSeparator { get; } = new NodeDecorators(DecoratorType.MajorSeparator);

        public static NodeDecorators MinorSeparator { get; } = new NodeDecorators(DecoratorType.MinorSeparator);

        public enum DecoratorType
        {
            MajorSeparator,
            MinorSeparator,
        }
    }
}

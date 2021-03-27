using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    public class Decorators : VisualNodeComponent
    {
        private Decorators(DecoratorType type)
        {
            Type = type;
        }

        public DecoratorType Type { get; }

        public static Decorators MajorSeperator { get; } = new Decorators(DecoratorType.MajorSeperator);

        public static Decorators MinorSeperator { get; } = new Decorators(DecoratorType.MinorSeperator);

        public enum DecoratorType
        {
            MajorSeperator,
            MinorSeperator,
        }
    }
}

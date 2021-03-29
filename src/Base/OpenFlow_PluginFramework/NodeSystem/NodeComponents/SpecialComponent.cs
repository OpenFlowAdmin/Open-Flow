using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents
{
    public class SpecialComponent<T> : ISpecialComponentContainer
        where T : NodeComponent
    {
        private SpecialComponent() { }

        public static SpecialComponent<NodeField> ConvertInput { get; } = new();

        public static SpecialComponent<NodeField> ConvertOutput { get; } = new();

        public static SpecialComponent<VisualNodeComponent> FlowOutput { get; } = new();
    }
}

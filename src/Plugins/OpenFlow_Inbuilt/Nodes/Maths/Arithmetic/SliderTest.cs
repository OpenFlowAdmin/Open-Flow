using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.NodeSystem.Nodes;
using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    public class SliderTest : INode
    {
        private readonly INodeField _sliderTest = NodeComponentBuilder.NodeField("Slider Test").WithValueTypeProvider("Display", new TypeDefinition<double>() { DefaultValue = 50.0, EditorName = "SliderEditor" }, true).Build;

        public string NodeName { get; } = "Slider Test";

        public IEnumerable<INodeComponent> Fields
        {
            get
            {
                yield return _sliderTest;
            }
        }

        public void Evaluate()
        {
        }
    }
}

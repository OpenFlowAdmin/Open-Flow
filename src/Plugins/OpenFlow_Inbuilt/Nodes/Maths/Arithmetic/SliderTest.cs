using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.NodeSystem.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    public class SliderTest : INode
    {
        private readonly NodeField _sliderTest = new NodeField() { Name = "Slider Test" }.WithValue("Display", true, 50.0, "SliderEditor");

        public string NodeName { get; } = "Slider Test";

        public IEnumerable<NodeComponent> Fields
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

using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
using OpenFlow_PluginFramework.NodeSystem.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Inbuilt.Nodes.Input.MouseInput
{
    public class MouseButton : INode
    {
        private readonly ValueField mouseButtonOutput = new ValueField("Mouse Button").WithOutput<MouseButtonEnum>();

        public string NodeName => "Mouse Button";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return mouseButtonOutput;
            }
        }

        public void Evaluate()
        {
        }
    }
}

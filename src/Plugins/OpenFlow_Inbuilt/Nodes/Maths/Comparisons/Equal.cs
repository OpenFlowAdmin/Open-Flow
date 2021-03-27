using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.NodeSystem.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Inbuilt.Nodes.Maths.Comparisons
{
    public class Equal : INode
    {
        private readonly NodeField inputOne = new NodeField() { Name = "First Input" }.WithInput(0.0);
        private readonly NodeField inputTwo = new NodeField() { Name = "Second Input" }.WithInput(0.0);
        private readonly NodeField outputField = new NodeField() { Name = "Equal" }.WithOutput<bool>();

        public string NodeName => "Equal";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return inputOne;
                yield return inputTwo;
                yield return outputField;
            }
        }

        public void Evaluate()
        {
            outputField.Output = inputOne.Input.Equals(inputTwo.Input);
        }
    }
}

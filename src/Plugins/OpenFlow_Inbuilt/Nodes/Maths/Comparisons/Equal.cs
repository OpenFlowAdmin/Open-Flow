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
        private readonly INodeField inputOne = NodeComponentBuilder.NodeField("First Input").WithInput(0.0).Build;
        private readonly INodeField inputTwo = NodeComponentBuilder.NodeField("Second Input").WithInput(0.0).Build;
        private readonly INodeField outputField = NodeComponentBuilder.NodeField("Equal").WithInput(false).Build;

        public string NodeName => "Equal";

        public IEnumerable<INodeComponent> Fields
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

using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
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
        private readonly ValueField inputOne = new ValueField("First Input").WithInput(0.0);
        private readonly ValueField inputTwo = new ValueField("Second Input").WithInput(0.0);
        private readonly ValueField outputField = new ValueField("Equal").WithOutput<bool>();

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

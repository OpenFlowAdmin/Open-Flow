namespace OpenFlow_Inbuilt.Nodes.Maths.Arithmetic
{
    using System.Collections.Generic;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeAdd : INode
    {
        private readonly NodeComponentAutoCloner addFields = new(new ValueField("Number").WithInput(0.0), 1, index => $"Number {index + 1}");
        private readonly ValueField totalField = new ValueField("Total").WithOutput<double>();

        public string NodeName { get; } = "Add";

        public IEnumerable<NodeComponent> Fields
        {
            get
            {
                yield return addFields;
                yield return totalField;
            }
        }

        public void Evaluate()
        {
            double total = 0;

            foreach (ValueField field in addFields)
            {
                total += (double)field.Input;
            }

            totalField.Output = total;
        }
    }
}

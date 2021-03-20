namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    using System;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class ValueFieldGenerator : NodeComponentCollection
    {
        private readonly ValueField originalClone;
        private readonly Func<int, string> nameRule;
        private readonly int minimumFieldCount;

        public ValueFieldGenerator(ValueField clone, int minimumFieldCount, Func<int, string> nameRule = null)
            : base()
        {
            Reset();
            this.nameRule = nameRule;
            originalClone = clone;
            this.minimumFieldCount = minimumFieldCount;

            originalClone.ParentNode = ParentNode;
            originalClone.Opacity.Value = 0.5;

            for (int i = 0; i < minimumFieldCount + 1; i++)
            {
                CloneField();
            }

            (this[^1] as ValueField).AnyValueChanged += LastField_AnyValueChanged;
        }

        public override INode ParentNode
        {
            set
            {
                base.ParentNode = value;
                originalClone.ParentNode = value;
            }
        }

        private void LastField_AnyValueChanged(object sender, object value)
        {
            (this[^1] as ValueField).AnyValueChanged -= LastField_AnyValueChanged;
            CloneField();
            (this[^1] as ValueField).AnyValueChanged += LastField_AnyValueChanged;
        }

        private void CloneField()
        {
            if (nameRule != null)
            {
                originalClone.Name = nameRule(FieldCount);
            }

            Add(originalClone.CloneTo(new ValueField("") { RemoveAction = (component) => { Remove(component); } }));

            if (FieldCount > 1)
            {
                this[^2].Opacity.Value = 1.0;
            }
        }

        protected override bool Remove(NodeComponent component)
        {
            if (FieldCount <= minimumFieldCount + 1 || component == this[^1])
            {
                return false;
            }
            
            if (base.Remove(component))
            {
                int index = 0;
                foreach (NodeField field in AllFields)
                {
                    field.Name = nameRule(index);
                    index++;
                }
                return true;
            }

            return false;
        }
    }
}

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    using System;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeComponentAutoCloner : NodeComponentCollection
    {
        private readonly NodeComponent originalClone;
        private readonly Func<int, string> nameRule;
        private readonly int minimumFieldCount;

        public NodeComponentAutoCloner(NodeComponent clone, int minimumFieldCount, Func<int, string> nameRule = null)
            : base()
        {
            this.nameRule = nameRule;
            originalClone = clone;
            this.minimumFieldCount = minimumFieldCount;

            originalClone.ParentNode = ParentNode;
            originalClone.Opacity.Value = 0.5;
            originalClone.SetRemoveAction((component) => 
            {
                ProtectedRemove(component);
                UpdateNames();
            });

            for (int i = 0; i < minimumFieldCount + 1; i++)
            {
                CloneComponent();
            }

            this[^1].PropertyChanged += LastField_PropertyChanged;
        }

        public override INode ParentNode
        {
            set
            {
                base.ParentNode = value;
                originalClone.ParentNode = value;
            }
        }

        private void LastField_PropertyChanged(object sender, object value)
        {
            this[^1].PropertyChanged -= LastField_PropertyChanged;
            CloneComponent();
            this[^1].PropertyChanged += LastField_PropertyChanged;
        }

        private void CloneComponent()
        {
            if (nameRule != null)
            {
                int index = 0;
                foreach (NodeField field in originalClone.NodeFields)
                {
                    field.Name = nameRule(NodeFields.Count + index);
                    index++;
                }
            }

            ProtectedAdd(originalClone.Clone());//To(new ValueField("") { RemoveAction = (component) => { ProtectedRemove(component); } }));

            if (NodeFields.Count > 1)
            {
                this[^2].Opacity.Value = 1.0;
            }
        }

        private void RemoveComponent(NodeComponent component) => ProtectedRemove(component);

        protected override bool ProtectedRemove(NodeComponent component)
        {
            if (NodeFields.Count <= minimumFieldCount + 1 || component == this[^1])
            {
                return false;
            }
            
            if (base.ProtectedRemove(component))
            {
                /*
                int index = 0;
                foreach (NodeField field in NodeFields)
                {
                    field.Name = nameRule(index);
                    index++;
                }
                */
                return true;
            }

            return false;
        }

        private void UpdateNames()
        {
            int index = 0;
            foreach (NodeField field in NodeFields)
            {
                field.Name = nameRule(index);
                index++;
            }
        }
    }
}

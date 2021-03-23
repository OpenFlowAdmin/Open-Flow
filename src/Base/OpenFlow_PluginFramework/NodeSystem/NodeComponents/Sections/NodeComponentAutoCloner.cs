namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    using System;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeComponentAutoCloner : NodeComponentCollection
    {
        private readonly NodeComponent _originalClone;
        private readonly Func<int, string> _nameRule;
        private readonly int _minimumFieldCount;

        public NodeComponentAutoCloner(NodeComponent clone, int minimumFieldCount, Func<int, string> nameRule = null)
            : base()
        {
            this._nameRule = nameRule;
            _originalClone = clone;
            this._minimumFieldCount = minimumFieldCount;

            _originalClone.ParentNode = ParentNode;
            _originalClone.Opacity.Value = 0.5;
            _originalClone.SetRemoveAction((component) => 
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
                _originalClone.ParentNode = value;
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
            if (_nameRule != null)
            {
                int index = 0;
                foreach (NodeField field in _originalClone.NodeFields)
                {
                    field.Name = _nameRule(NodeFields.Count + index);
                    index++;
                }
            }

            ProtectedAdd(_originalClone.Clone());//To(new ValueField("") { RemoveAction = (component) => { ProtectedRemove(component); } }));

            if (NodeFields.Count > 1)
            {
                this[^2].Opacity.Value = 1.0;
            }
        }

        private void RemoveComponent(NodeComponent component) => ProtectedRemove(component);

        protected override bool ProtectedRemove(NodeComponent component)
        {
            if (NodeFields.Count <= _minimumFieldCount + 1 || component == this[^1])
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
                field.Name = _nameRule(index);
                index++;
            }
        }
    }
}

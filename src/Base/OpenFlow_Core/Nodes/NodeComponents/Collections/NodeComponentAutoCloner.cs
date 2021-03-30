﻿namespace OpenFlow_Core.Nodes.NodeComponents.Collections
{
    using System;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Collections;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;

    public class NodeComponentAutoCloner : NodeComponentCollection, INodeComponentAutoCloner
    {
        private INodeComponent _originalClone;
        private Func<int, string> _nameRule;
        private int _minimumFieldCount;

        public override INode ParentNode
        {
            set
            {
                base.ParentNode = value;
                _originalClone.ParentNode = value;
            }
        }

        public void ResetWith(INodeComponent originalClone, int minimumFieldCount, Func<int, string> nameRule)
        {
            _originalClone = originalClone;
            _minimumFieldCount = minimumFieldCount;
            _nameRule = nameRule;

            _originalClone.ParentNode = ParentNode;
            _originalClone.Opacity.Value = 0.5;
            _originalClone.SetRemoveAction((component) =>
            {
                ProtectedRemove(component);
                //UpdateNames();
            });

            ProtectedReset();
        }

        protected override void ProtectedReset()
        {
            base.ProtectedReset();

            for (int i = 0; i < _minimumFieldCount + 1; i++)
            {
                CloneComponent();
            }

            this[^1].PropertyChanged += LastField_PropertyChanged;
        }

        protected override bool ProtectedRemove(INodeComponent component)
        {
            if (VisualComponentList.Count <= _minimumFieldCount + 1 || component == this[^1])
            {
                return false;
            }

            if (base.ProtectedRemove(component))
            {
                return true;
            }

            return false;
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
                foreach (IVisualNodeComponent field in _originalClone.VisualComponentList)
                {
                    field.Name = _nameRule(VisualComponentList.Count + index);
                    index++;
                }
            }

            ProtectedAdd(_originalClone.Clone());

            if (VisualComponentList.Count > 1)
            {
                this[^2].Opacity.Value = 1.0;
            }
        }

        private void RemoveComponent(INodeComponent component) => ProtectedRemove(component);

        private void UpdateNames()
        {
            int index = 0;
            foreach (IVisualNodeComponent field in VisualComponentList)
            {
                field.Name = _nameRule(index);
                index++;
            }
        }
    }
}

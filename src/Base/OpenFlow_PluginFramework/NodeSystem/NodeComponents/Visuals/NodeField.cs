namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;
    using OpenFlow_PluginFramework.Primitives.TypeDefinition;
    using OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider;

    public partial class NodeField : VisualNodeComponent, INodeField
    {
        private readonly Dictionary<object, LaminarValue> _valueStore = new();

        public event EventHandler<object> ValueStoreChanged;

        public event EventHandler<object> AnyValueChanged;

        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                foreach (LaminarValue storedValue in _valueStore.Values)
                {
                    storedValue.Name = base.Name;
                }
            }
        }

        public object this[object key]
        {
            get => GetDisplayValue(key)?.Value;
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (value == null)
                {
                    RemoveValue(key);
                }
                else if (_valueStore.TryGetValue(key, out LaminarValue OFVal))
                {
                    if (OFVal.CanSetValue(value))
                    {
                        OFVal.Value = value;
                    }
                    else
                    {
                        RemoveValue(key);
                        AddValue(key, new AutoTypeDefinition(value), true);
                    }
                }
                else
                {
                    AddValue(key, new AutoTypeDefinition(value), true);
                }
            }
        }

        public LaminarValue DisplayedValue { get; private set; }

        private void SetDisplayedValue(object displayValueKey)
        {
            DisplayedValue = GetDisplayValue(displayValueKey);
            NotifyPropertyChanged(nameof(DisplayedValue));
        }

        public void AddValue(object key, ITypeDefinitionProvider typeDefs, bool isUserEditable)
        {
            AddValue(key, new LaminarValue(typeDefs) { IsUserEditable = isUserEditable });
        }

        public void AddValue(object key, LaminarValue newVal)
        {
            _valueStore.Add(key, newVal);
            newVal.PropertyChanged += ChildValue_PropertyChanged;
            if (_valueStore.Count == 1)
            {
                SetDisplayedValue(key);
            }
            ValueStoreChanged?.Invoke(this, key);
        }

        private bool RemoveValue(object key)
        {
            if (_valueStore.TryGetValue(key, out LaminarValue val))
            {
                val.PropertyChanged -= ChildValue_PropertyChanged;
                _valueStore.Remove(key);
                ValueStoreChanged?.Invoke(this, key);
                return true;
            }
            return false;
        }

        private void ChildValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LaminarValue.Value))
            {
                ParentNode.TriggerEvaluate();
                AnyValueChanged?.Invoke(this, (sender as LaminarValue)?.Value);
                NotifyPropertyChanged("Child Value");
            }
        }

        public override NodeComponent Clone() => CloneTo(new NodeField());

        protected override NodeField CloneTo(NodeComponent nodeField)
        {
            base.CloneTo(nodeField);
            foreach (KeyValuePair<object, LaminarValue> kvp in _valueStore)
            {
                (nodeField as NodeField).AddValue(kvp.Key, kvp.Value.Clone());
            }

            return (nodeField as NodeField);
        }

        public LaminarValue GetDisplayValue(object key) => key != null && _valueStore.TryGetValue(key, out LaminarValue value) ? value : null;
    }

    public partial class NodeField
    {
        public LaminarValue InputDisplayValue => GetDisplayValue(INodeField.InputKey);

        public LaminarValue OutputDisplayValue => GetDisplayValue(INodeField.OutputKey);

        public object Input
        {
            get => InputDisplayValue?.Value;
            set => this[INodeField.InputKey] = value;
        }

        public object Output
        {
            get => OutputDisplayValue?.Value;
            set => this[INodeField.OutputKey] = value;
        }

        public NodeField WithValue<T>(object key, bool isUserEditable, T value = default, string editorName = null, string displayName = null)
        {
            return WithTypeProvider(key, new TypeDefinition<T>() { DefaultValue = value, EditorName = editorName, DisplayName = displayName }, isUserEditable);
        }

        public NodeField WithTypeProvider(object key, ITypeDefinitionProvider typeDefinitions, bool isUserEditable)
        {
            AddValue(key, typeDefinitions, isUserEditable);
            return this;
        }

        public NodeField WithInput<T>(T initialValue = default, string editorName = null, string displayName = null) => WithValue(INodeField.InputKey, true, initialValue, editorName, displayName);

        public NodeField WithInputTypeProvider(ITypeDefinitionProvider typeDefinition) => WithTypeProvider(INodeField.InputKey, typeDefinition, true);

        public NodeField WithOutput<T>(T initialValue = default, string editorName = null, string displayName = null) => WithValue(INodeField.OutputKey, false, initialValue, editorName, displayName);

        public NodeField WithOutputTypeProvider(ITypeDefinitionProvider typeDefinition) => WithTypeProvider(INodeField.OutputKey, typeDefinition, false);
    }
}

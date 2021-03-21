﻿namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using OpenFlow_PluginFramework.NodeSystem.Nodes;
    using OpenFlow_PluginFramework.Primitives;
    using OpenFlow_PluginFramework.Primitives.TypeDefinition;
    using OpenFlow_PluginFramework.Primitives.TypeDefinitionProvider;

    public partial class ValueField : NodeField
    {
        public const string InputKey = "Input";
        public const string OutputKey = "Output";

        private readonly Dictionary<object, OpenFlowValue> valueStore = new();

        public ValueField(string name, object displayValueKey = null) : base(name)
        {
            SetDisplayedValue(displayValueKey);
        }

        public event EventHandler<object> ValueStoreChanged;

        public event EventHandler<object> AnyValueChanged;

        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                foreach (OpenFlowValue storedValue in valueStore.Values)
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
                    throw new ArgumentNullException("Key for ValueField cannot be null");
                }

                if (value == null)
                {
                    RemoveValue(key);
                }
                else if (valueStore.TryGetValue(key, out OpenFlowValue OFVal) && OFVal.CanSetValue(value))
                {
                    OFVal.Value = value;
                }
                else
                {
                    AddValue(key, new AutoTypeDefinition(value));
                }
            }
        }

        public OpenFlowValue DisplayedValue { get; private set; }

        private void SetDisplayedValue(object displayValueKey)
        {
            DisplayedValue = GetDisplayValue(displayValueKey);
            NotifyPropertyChanged(nameof(DisplayedValue));
        }

        private void AddValue(object key, ITypeDefinitionProvider typeDefs)
        {
            AddValue(key, new OpenFlowValue(typeDefs));
        }

        public void AddValue(object key, OpenFlowValue newVal)
        {
            valueStore.Add(key, newVal);
            newVal.PropertyChanged += ChildValue_PropertyChanged;
            if (valueStore.Count == 1)
            {
                SetDisplayedValue(key);
            }
            ValueStoreChanged?.Invoke(this, key);
        }

        private bool RemoveValue(object key)
        {
            if (valueStore.TryGetValue(key, out OpenFlowValue val))
            {
                val.PropertyChanged -= ChildValue_PropertyChanged;
                valueStore.Remove(key);
                ValueStoreChanged?.Invoke(this, key);
                return true;
            }
            return false;
        }

        private void ChildValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OpenFlowValue.Value))
            {
                ParentNode.TriggerEvaluate();
                AnyValueChanged?.Invoke(this, (sender as OpenFlowValue)?.Value);
            }
        }

        public override NodeField CloneTo(NodeField nodeField)
        {
            base.CloneTo(nodeField);
            foreach (KeyValuePair<object, OpenFlowValue> kvp in valueStore)
            {
                (nodeField as ValueField)?.AddValue(kvp.Key, kvp.Value.Clone());
            }

            return nodeField;
        }

        public OpenFlowValue GetDisplayValue(object key) => key != null && valueStore.TryGetValue(key, out OpenFlowValue value) ? value : null;
    }

    public partial class ValueField
    {
        public OpenFlowValue InputDisplayValue => GetDisplayValue(InputKey);

        public OpenFlowValue OutputDisplayValue => GetDisplayValue(OutputKey);

        public object Input
        {
            get => InputDisplayValue?.Value;
            set => this[InputKey] = value;
        }

        public object Output
        {
            get => OutputDisplayValue?.Value;
            set => this[OutputKey] = value;
        }

        public ValueField WithValue<T>(object key, T value = default, string editorName = null, string displayName = null) => WithTypeProvider(key, new TypeDefinition<T>() { DefaultValue = value, EditorName = editorName, DisplayName = displayName });

        public ValueField WithTypeProvider(object key, ITypeDefinitionProvider typeDefinitions)
        {
            AddValue(key, typeDefinitions);
            return this;
        }

        public ValueField WithInput<T>(T initialValue = default, string editorName = null, string displayName = null) => WithValue(InputKey, initialValue, editorName, displayName);

        public ValueField WithInputTypeProvider(ITypeDefinitionProvider typeDefinition) => WithTypeProvider(InputKey, typeDefinition);

        public ValueField WithOutput<T>(T initialValue = default, string editorName = null, string displayName = null) => WithValue(OutputKey, initialValue, editorName, displayName);

        public ValueField WithOutputTypeProvider(ITypeDefinitionProvider typeDefinition) => WithTypeProvider(OutputKey, typeDefinition);
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    public class NodeComponentDictionary : NodeComponentCollection, IDictionary<object, NodeComponent>
    {
        private readonly Dictionary<object, NodeComponent> _subComponents = new();

        public ICollection<object> Keys => _subComponents.Keys;

        public ICollection<NodeComponent> Values => _subComponents.Values;

        public int Count => _subComponents.Count;

        public bool IsReadOnly => false;

        public NodeComponent this[object key] { get => _subComponents[key]; set => _subComponents[key] = value; }

        public bool ShowSectionByKey(object key)
        {
            if (_subComponents.TryGetValue(key, out NodeComponent component) && !Contains(component))
            {
                ProtectedAdd(component);
                return true;
            }

            return false;
        }

        public bool HideComponentByKey(object key)
        {
            if (_subComponents.TryGetValue(key, out NodeComponent component) && Contains(component))
            {
                ProtectedRemove(component);
                return true;
            }

            return false;
        }

        public void HideAllComponents()
        {
            ProtectedReset();
        }

        public void Add(object key, NodeComponent value) => _subComponents.Add(key, value);

        public bool ContainsKey(object key) => _subComponents.ContainsKey(key);

        public bool Remove(object key) => _subComponents.Remove(key);

        public bool TryGetValue(object key, [MaybeNullWhen(false)] out NodeComponent value) => _subComponents.TryGetValue(key, out value);

        public void Add(KeyValuePair<object, NodeComponent> item) => _subComponents.Add(item.Key, item.Value);

        public void Clear() => _subComponents.Clear();

        public bool Contains(KeyValuePair<object, NodeComponent> item) => _subComponents.ContainsKey(item.Key) && _subComponents[item.Key] == item.Value;

        public void CopyTo(KeyValuePair<object, NodeComponent>[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<object, NodeComponent> item) => _subComponents.Remove(item.Key);

        IEnumerator<KeyValuePair<object, NodeComponent>> IEnumerable<KeyValuePair<object, NodeComponent>>.GetEnumerator() => _subComponents.GetEnumerator();
    }
}

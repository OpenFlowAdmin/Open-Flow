using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    public class NodeComponentDictionary : NodeComponentCollection, IDictionary<object, NodeComponent>
    {
        private readonly Dictionary<object, NodeComponent> subComponents = new();

        public ICollection<object> Keys => subComponents.Keys;

        public ICollection<NodeComponent> Values => subComponents.Values;

        public int Count => subComponents.Count;

        public bool IsReadOnly => false;

        public NodeComponent this[object key] { get => subComponents[key]; set => subComponents[key] = value; }

        public bool ShowSectionByKey(object key)
        {
            if (subComponents.TryGetValue(key, out NodeComponent component) && !Contains(component))
            {
                ProtectedAdd(component);
                return true;
            }

            return false;
        }

        public bool HideComponentByKey(object key)
        {
            if (subComponents.TryGetValue(key, out NodeComponent component) && Contains(component))
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

        public void Add(object key, NodeComponent value) => subComponents.Add(key, value);

        public bool ContainsKey(object key) => subComponents.ContainsKey(key);

        public bool Remove(object key) => subComponents.Remove(key);

        public bool TryGetValue(object key, [MaybeNullWhen(false)] out NodeComponent value) => subComponents.TryGetValue(key, out value);

        public void Add(KeyValuePair<object, NodeComponent> item) => subComponents.Add(item.Key, item.Value);

        public void Clear() => subComponents.Clear();

        public bool Contains(KeyValuePair<object, NodeComponent> item) => subComponents.ContainsKey(item.Key) && subComponents[item.Key] == item.Value;

        public void CopyTo(KeyValuePair<object, NodeComponent>[] array, int arrayIndex) => throw new NotImplementedException();

        public bool Remove(KeyValuePair<object, NodeComponent> item) => subComponents.Remove(item.Key);

        IEnumerator<KeyValuePair<object, NodeComponent>> IEnumerable<KeyValuePair<object, NodeComponent>>.GetEnumerator() => subComponents.GetEnumerator();
    }
}

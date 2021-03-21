using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    public class NodeComponentList : NodeComponentCollection
    {
        public NodeComponent this[int index] { get => base[index]; set => base[index] = value; }

        public bool IsReadOnly => false;

        public int Count => ComponentCount;

        public void Clear() => ProtectedClear();

        public void CopyTo(NodeComponent[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void Add(NodeComponent item) => ProtectedAdd(item);

        public void Insert(int index, NodeComponent item) => ProtectedInsert(index, item);

        public bool Remove(NodeComponent item) => ProtectedRemove(item);

        public void RemoveAt(int index) => ProtectedRemoveAt(index);
    }
}

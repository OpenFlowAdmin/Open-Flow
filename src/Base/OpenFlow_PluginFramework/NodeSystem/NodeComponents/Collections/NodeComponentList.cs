using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents.Sections
{
    public class NodeComponentList : NodeComponentCollection, IList<INodeComponent>
    {
        public INodeComponent this[int index] { get => base[index]; set => base[index] = value; }

        public bool IsReadOnly => false;

        public int Count => base.ComponentCount;

        public void Clear() => ProtectedReset();

        public void CopyTo(INodeComponent[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void Add(INodeComponent item) => ProtectedAdd(item);

        public void Insert(int index, INodeComponent item) => ProtectedInsert(index, item);

        public bool Remove(INodeComponent item) => ProtectedRemove(item);

        public void RemoveAt(int index) => ProtectedRemoveAt(index);
    }
}

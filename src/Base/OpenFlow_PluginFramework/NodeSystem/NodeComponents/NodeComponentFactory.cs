using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents
{
    public class NodeComponentFactory
    {
        private readonly Dictionary<Type, Type> interfaceImplementations = new();

        public T GetImplementation<T>() where T : INodeComponent
        {
            return (T)Activator.CreateInstance(interfaceImplementations[typeof(T)]);
        }

        public NodeComponentFactory RegisterImplementation<TInterface, TImplementation>() 
            where TInterface : INodeComponent
            where TImplementation : class, TInterface
        {
            interfaceImplementations.Add(typeof(TInterface), typeof(TImplementation));
            return this;
        }
    }
}

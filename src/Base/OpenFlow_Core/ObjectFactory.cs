using OpenFlow_Core.Nodes.NodeComponents.Collections;
using OpenFlow_Core.Nodes.NodeComponents.Visuals;
using OpenFlow_Core.Primitives;
using OpenFlow_Core.Primitives.TypeDefinitionManagers;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Collections;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.Primitives;
using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core
{
    public class ObjectFactory : IObjectFactory
    {
        private readonly Dictionary<Type, Type> interfaceImplementations = new();

        public ObjectFactory()
        {
            RegisterImplementation<IOpacity, Opacity>();
            RegisterImplementation<INodeField, NodeField>();
            RegisterImplementation<INodeLabel, NodeLabel>();
            RegisterImplementation<INodeDecorator, NodeDecorator>();
            RegisterImplementation<INodeComponentList, NodeComponentList>();
            RegisterImplementation<INodeComponentAutoCloner, NodeComponentAutoCloner>();
            RegisterImplementation<INodeComponentDictionary, NodeComponentDictionary>();
            RegisterImplementation<INodeComponentCollection, NodeComponentCollection>();
            RegisterImplementation<ITypeDefinitionManager, TypeDefinitionManager>();
            RegisterImplementation<IRigidTypeDefinitionManager, RigidTypeDefinitionManager>();
            RegisterImplementation<IManualTypeDefinitionManager, ManualTypeDefinitionManager>();
            RegisterImplementation<ILaminarValue, LaminarValue>();
        }

        public T GetImplementation<T>()
        {
            return (T)GetLooseTypedImplementation(typeof(T));
        }

        public IObjectFactory RegisterImplementation<TInterface, TImplementation>()
            where TImplementation : class, TInterface
        {
            interfaceImplementations.Add(typeof(TInterface), typeof(TImplementation));
            return this;
        }

        private object GetLooseTypedImplementation(Type typeToGet)
        {
            Type targetType = interfaceImplementations[typeToGet];
            if (targetType.GetConstructor(Type.EmptyTypes) != null)
            {
                return Activator.CreateInstance(targetType);
            }

            ConstructorInfo info = targetType.GetConstructors()[0];
            ParameterInfo[] parameters = info.GetParameters();
            object[] parameterObjects = new object[parameters.Length];
            int i = 0;
            foreach (ParameterInfo parameter in info.GetParameters())
            {
                parameterObjects[i] = GetLooseTypedImplementation(parameter.ParameterType);
                i++;
            }

            return Activator.CreateInstance(targetType, parameterObjects);
        }
    }
}

using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.NodeSystem.NodeComponents
{
    public class NodeComponentFactory
    {
        private readonly Dictionary<Type, Type> interfaceImplementations = new();

        public T GetImplementation<T>()
        {
            return (T)GetLooseTypedImplementation(typeof(T));
        }

        public NodeComponentFactory RegisterImplementation<TInterface, TImplementation>() 
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

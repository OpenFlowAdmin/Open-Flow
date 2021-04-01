﻿using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Collections;
using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Visuals;
using OpenFlow_PluginFramework.Primitives;
using OpenFlow_PluginFramework.Primitives.TypeDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework
{
    public class Constructor
    {
        public static INodeField NodeField(string fieldName)
        {
            INodeField output = Laminar.New<INodeField>();

            output.Name = fieldName;

            return output;
        }

        public static INodeLabel NodeLabel(string labelText)
        {
            INodeLabel output = Laminar.New<INodeLabel>();

            output.Name = labelText;

            return output;
        }
        public static INodeComponentAutoCloner NodeComponentAutoCloner(INodeComponent originalClone, int minimumFieldCount, Func<int, string> nameRule)
        {
            INodeComponentAutoCloner output = Laminar.New<INodeComponentAutoCloner>();

            output.ResetWith(originalClone, minimumFieldCount, nameRule);

            return output;
        }

        public static INodeComponentList NodeComponentList(params INodeComponent[] components) => NodeComponentList(components.AsEnumerable());

        public static INodeComponentList NodeComponentList(IEnumerable<INodeComponent> components)
        {
            INodeComponentList output = Laminar.New<INodeComponentList>();

            foreach (INodeComponent component in components)
            {
                output.Add(component);
            }

            return output;
        }

        public static IRigidTypeDefinitionManager RigidTypeDefinitionManager(object value, string editorName = null, string displayName = null)
        {
            IRigidTypeDefinitionManager output = Laminar.New<IRigidTypeDefinitionManager>();

            output.RegisterTypeDefinition(value, editorName, displayName);

            return output;
        }

        public static INodeDecorator NodeDecorator(NodeDecoratorType DecoratorType)
        {
            INodeDecorator output = Laminar.New<INodeDecorator>();

            output.DecoratorType = DecoratorType;

            return output;
        }

        public static ITypeDefinitionConstructor<T> TypeDefinition<T>(T defaultValue, string editorName = null, string displayName = null)
        {
            ITypeDefinitionConstructor<T> output = Laminar.New<ITypeDefinitionConstructor<T>>();

            output.DefaultValue = defaultValue;
            output.EditorName = editorName;
            output.DisplayName = displayName;

            return output;
        }

        public static IManualTypeDefinitionManager ManualTypeDefinitionManager() => Laminar.New<IManualTypeDefinitionManager>();

        public static ITypeDefinitionManager TypeDefinitionManager() => Laminar.New<ITypeDefinitionManager>();

        public static INodeComponentDictionary NodeComponentDictionary() => Laminar.New<INodeComponentDictionary>();

        public static IValueConstraint<T> ValueConstraint<T>(Func<T, T> constraintFunction)
        {
            IValueConstraint<T> output = Laminar.New<IValueConstraint<T>>();

            output.MyFunc = constraintFunction;

            return output;
        }
    }
}

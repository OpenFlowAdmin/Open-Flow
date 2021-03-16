namespace OpenFlow_Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using OpenFlow_Core.Management;
    using OpenFlow_Core.Nodes;

    public class Instance
    {
        private readonly PluginManager pluginManager;
        private readonly Configs configs = new Configs(Path.Combine(Directory.GetCurrentDirectory(), @"Configs.xml"));

        public Instance()
        {
            Current = this;
            pluginManager = new PluginManager();
            if (configs.Valid && configs.PluginPaths != null)
            {
                pluginManager.LoadFromFolders(configs.PluginPaths);
            }
        }

        public static Instance Current { get; private set; }

        public Dictionary<Type, TypeInfoRecord> TypeInfo { get; } = new ();

        public LoadedNodeManager LoadedNodeManager { get; } = new LoadedNodeManager();

        public RegisteredUIs RegisteredEditors { get; } = new RegisteredUIs();

        public RegisteredUIs RegisteredDisplays { get; } = new RegisteredUIs();

        public TypeInfoRecord GetTypeInfo(Type type)
        {
            if (TypeInfo.TryGetValue(type, out TypeInfoRecord info))
            {
                return info;
            }

            throw new NotSupportedException($"The type {type} is not registered, and cannot be displayed");
        }

        public record TypeInfoRecord(object DefaultValue, string HexColour, string DefaultDisplay, string DefaultEditor, string UserFriendlyName);
    }
}

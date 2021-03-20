﻿namespace OpenFlow_PluginFramework.Registration
{
    /// <summary>
    /// Defines the class whcih registers a plugin with the <see cref="IPluginHost"/>
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Registers the plugin with the host
        /// </summary>
        /// <param name="host">The plugin host this plugin will be registered with</param>
        void Register(IPluginHost host);
    }
}

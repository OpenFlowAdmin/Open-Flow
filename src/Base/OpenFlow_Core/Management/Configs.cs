namespace OpenFlow_Core.Management
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;

    public class Configs
    {
        private readonly string xmlDocPath;
        private State state;
        private XmlNode configs;

        public Configs(string path)
        {
            xmlDocPath = path;
            Reload();
        }

        private enum State
        {
            InvalidPath,
            InvalidFile,
            Unknown,
            Working,
        }

        public string ErrorState => state switch
        {
            State.InvalidPath => "The config file could not be found in " + xmlDocPath,
            State.InvalidFile => "The config file was found but is not valid",
            State.Unknown => "An unknown error occured loading the config file",
            State.Working => "There were no errors, the config file is valid",
            _ => "wtf bro",
        };

        public bool Valid => state == State.Working;

        public ReadOnlyCollection<string> PluginPaths { get; private set; }

        public void Reload()
        {
            state = State.Unknown;
            try
            {
                if (!File.Exists(xmlDocPath))
                {
                    state = State.InvalidPath;
                }

                XmlDocument configsDocu = new();
                configsDocu.Load(xmlDocPath);
                if (configsDocu["Configs"] == null)
                {
                    state = State.InvalidFile;
                }

                configs = configsDocu["Configs"];
                state = State.Working;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception {e}; Configs not found! Invalid Config File");
            }

            LoadPluginDirectories();
        }

        public void LoadPluginDirectories()
        {
            if (Valid)
            {
                List<string> output = new();
                foreach (XmlNode path in configs["PluginDirectories"])
                {
                    Debug.WriteLine(path.InnerText);
                    if (Directory.Exists(path.InnerText))
                    {
                        output.AddRange(Directory.GetDirectories(path.InnerText));
                    }
                }

                PluginPaths = new ReadOnlyCollection<string>(output);
            }
            else
            {
                PluginPaths = null;
            }
        }
    }
}

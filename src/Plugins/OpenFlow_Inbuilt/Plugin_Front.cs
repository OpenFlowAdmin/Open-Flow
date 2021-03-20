namespace OpenFlow_Inbuilt
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Avalonia.Controls.Templates;
    using Avalonia.Markup.Xaml;
    using Avalonia.Styling;
    using OpenFlow_Inbuilt.Nodes.Flow;
    using OpenFlow_Inbuilt.Nodes.Maths.Arithmetic;
    using OpenFlow_Inbuilt.Nodes.Maths.Functions;
    using OpenFlow_Inbuilt.Nodes.StringOperations;
    using OpenFlow_PluginFramework.Registration;
    using Avalonia.Markup.Xaml.XamlIl.Runtime;
    using System.Collections.Generic;
    using System.Linq;

    public class Plugin_Front : IPlugin
    {
        private IEnumerable<KeyValuePair<string, IDataTemplate>> editors = new Dictionary<string, IDataTemplate>();
        private IEnumerable<KeyValuePair<string, IDataTemplate>> displays = new Dictionary<string, IDataTemplate>();

        public Plugin_Front()
        {
            Debug.WriteLine("Plugin Front Initialized");
            string projectFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, @"Plugins\OpenFlow_Inbuilt\");
            Debug.WriteLine(projectFolder);
            editors = DataTemplatesFromFolder(Path.Combine(projectFolder, @"Editors"));
            displays = DataTemplatesFromFolder(Path.Combine(projectFolder, @"Displays"));
        }

        public void Register(IPluginHost host)
        {
            host.RegisterType("#FF0000", "Number", 0.0, "NumberEnterDisplay", "StringDisplay");
            host.RegisterType("#0000FF", "Text", "", "StringEditor", "StringDisplay");
            host.RegisterType("#0000FF", "Condition", false, "StringDisplay", "StringDisplay");

            //host.TryAddTypeConverter<double, string, Node_Convert_To_String>();

            foreach (KeyValuePair<string, IDataTemplate> kvp in editors)
            {
                host.RegisterEditor(kvp.Key, kvp.Value);
            }
            foreach (KeyValuePair<string, IDataTemplate> kvp in displays)
            {
                host.RegisterDisplay(kvp.Key, kvp.Value);
            }

            host.AddNodeToMenu<NodeAdd, NodeDifference, NodeMultiply, NodeDivide>("Number", "Arithmetic");
            host.AddNodeToMenu<NodeSine>("Number", "Functions");
            host.AddNodeToMenu<Node_Join_Strings>("Text");
            host.AddNodeToMenu<FlowSwitch>("Flow Control");
        }

        private static IEnumerable<KeyValuePair<string, IDataTemplate>> DataTemplatesFromFolder(string folderPath)
        {
            Debug.WriteLine(folderPath);
            IEnumerable<KeyValuePair<string, IDataTemplate>> dataTemplates = new Dictionary<string, IDataTemplate> ();
            foreach (Styles fileStyles in Directory.EnumerateFiles(folderPath)
                .Select(x => AvaloniaRuntimeXamlLoader.Parse(File.ReadAllText(x)) as Styles)
                .Where(x => x != null))
            {
                dataTemplates = dataTemplates.Concat(fileStyles.Resources
                    .Where(x => x.Key is string && x.Value is IDataTemplate)
                    .Select(x => new KeyValuePair<string, IDataTemplate>(x.Key as string, x.Value as IDataTemplate)));
            }
            Debug.WriteLine(dataTemplates.Count());
            return dataTemplates;
        }
    }
}

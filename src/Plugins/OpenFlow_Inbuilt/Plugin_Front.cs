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
    using OpenFlow_Inbuilt.Nodes.Maths.Comparisons;
    using OpenFlow_Inbuilt.Nodes.Input.MouseInput;
    using Avalonia.Controls;

    public class Plugin_Front : IPlugin
    {
        private readonly IEnumerable<KeyValuePair<string, IDataTemplate>> editors = new Dictionary<string, IDataTemplate>();
        private readonly IEnumerable<KeyValuePair<string, IDataTemplate>> displays = new Dictionary<string, IDataTemplate>();

        public Plugin_Front()
        {
            string projectFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, @"Plugins\OpenFlow_Inbuilt\");
            editors = DataTemplatesFromFolder(Path.Combine(projectFolder, @"Editors"));
            displays = DataTemplatesFromFolder(Path.Combine(projectFolder, @"Displays"));
        }

        public void Register(IPluginHost host)
        {
            host.RegisterType<double>("#FF0000", "Number", 0.0, "NumberEnterDisplay", "StringDisplay");
            host.RegisterType<string>("#0000FF", "Text", "", "StringEditor", "StringDisplay");
            host.RegisterType<bool>("#00FFFF", "Condition", false, "StringDisplay", "StringDisplay");
            host.RegisterType<Action>("00FF00", "Button", null, "DefaultDisplay", "ActionDisplay");
            host.RegisterType<MouseButtonEnum>("#FFFF00", "Mouse Button", MouseButtonEnum.LeftButton, "EnumEditor", "StringDisplay");

            //host.TryAddTypeConverter<double, string, Node_Convert_To_String>();

            foreach (KeyValuePair<string, IDataTemplate> kvp in editors)
            {
                host.RegisterEditor(kvp.Key, kvp.Value);
            }
            foreach (KeyValuePair<string, IDataTemplate> kvp in displays)
            {
                host.RegisterDisplay(kvp.Key, kvp.Value);
            }

            host.RegisterDisplay<IControl>("DefaultDisplay", typeof(UserControls.DefaultDisplay));

            host.AddNodeToMenu<NodeAdd, NodeDifference, NodeMultiply, NodeDivide>("Number", "Arithmetic");
            host.AddNodeToMenu<NodeSine>("Number", "Functions");
            host.AddNodeToMenu<Equal>("Number", "Comparisons");
            host.AddNodeToMenu<Node_Join_Strings>("Text");
            host.AddNodeToMenu<FlowSwitch>("Flow Control");
            host.AddNodeToMenu<MouseButton>("Input", "Mouse");
        }

        private static IEnumerable<KeyValuePair<string, IDataTemplate>> DataTemplatesFromFolder(string folderPath)
        {
            IEnumerable<KeyValuePair<string, IDataTemplate>> dataTemplates = new Dictionary<string, IDataTemplate> ();
            foreach (Styles fileStyles in Directory.EnumerateFiles(folderPath)
                .Select(x => AvaloniaRuntimeXamlLoader.Parse(File.ReadAllText(x)) as Styles)
                .Where(x => x != null))
            {
                dataTemplates = dataTemplates.Concat(fileStyles.Resources
                    .Where(x => x.Key is string && x.Value is IDataTemplate)
                    .Select(x => new KeyValuePair<string, IDataTemplate>(x.Key as string, x.Value as IDataTemplate)));
            }
            return dataTemplates;
        }
    }
}

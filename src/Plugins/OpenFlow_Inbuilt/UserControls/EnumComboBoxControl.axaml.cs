using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OpenFlow_Inbuilt.UserControls
{
    public class EnumComboBoxControl : UserControl
    {
        public EnumComboBoxControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

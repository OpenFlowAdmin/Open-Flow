using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OpenFlow_Inbuilt.UserControls
{
    public class SliderEditor : UserControl
    {
        public SliderEditor()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

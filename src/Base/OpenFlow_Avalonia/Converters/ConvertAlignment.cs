namespace OpenFlow_Avalonia.Converters
{
    using System;
    using System.Globalization;
    using Avalonia.Data.Converters;
    using Avalonia.Layout;
    using OpenFlow_Core.Nodes;

    public class ConvertAlignment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value) switch
            {
                OpenFlow_Core.HorizontalAlignment.Left => HorizontalAlignment.Left,
                OpenFlow_Core.HorizontalAlignment.Right => HorizontalAlignment.Right,
                _ => HorizontalAlignment.Stretch,
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

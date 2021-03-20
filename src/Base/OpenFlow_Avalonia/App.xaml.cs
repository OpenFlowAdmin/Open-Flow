﻿namespace OpenFlow_Avalonia
{
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;
    using OpenFlow_Avalonia.Views;

    public class App : Application
    {
        public static DragDropHandler DragDropHandler { get; private set; }

        public static OpenFlow_Core.Instance Instance { get; } = new OpenFlow_Core.Instance();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}

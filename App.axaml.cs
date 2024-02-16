using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using GetnMethods.ViewModels;
using GetnMethods.Views;

namespace GetnMethods;
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    //private void SetApplicationIcon(Window window)
    //{
    //    var iconPath = "Assets/report-repo.ico";
        
        
    //    var icon = new Bitmap($"avares://GetNMethods/{iconPath}");
    //    window.Icon = new WindowIcon(icon);
    //}


    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            //SetApplicationIcon(desktop.MainWindow);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
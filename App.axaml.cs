using System;
using System.Threading;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using FluentAvalonia.UI.Windowing;

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
            var splashScreen = new MainAppSplashScreen();
            ShowSplashScreen(splashScreen);

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            desktop.MainWindow.Show();

            //SetApplicationIcon(desktop.MainWindow);
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async void ShowSplashScreen(MainAppSplashScreen splashScreen)
    {
        // Create a window to host the splash screen content
        var splashWindow = new Window
        {
            // Set basic properties for the splash screen window
            Width = 600, // Adjust size as needed
            Height = 400, // Adjust size as needed
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ShowInTaskbar = false // Optional: depending on whether you want it to appear in the taskbar
        };

        // Assuming SplashScreenContent is a UserControl or a visual element
        if (splashScreen.SplashScreenContent is Control content)
        {
            splashWindow.Content = content;
        }

        // Optionally, set the window's icon if AppIcon is provided
        if (splashScreen.AppIcon is Bitmap appIconBitmap)
        {
            splashWindow.Icon = new WindowIcon(appIconBitmap);
        }

        // Show the splash screen window
        splashWindow.Show();

        // Ensure the splash screen is shown for a minimum amount of time
        var minimumShowTimeTask = Task.Delay(splashScreen.MinimumShowTime);

        // Run any background tasks needed before the application starts
        var runTasksTask = splashScreen.RunTasks(CancellationToken.None);

        // Wait for both the minimum show time and the background tasks to complete
        await Task.WhenAll(minimumShowTimeTask,runTasksTask);

        // Close the splash screen once tasks are complete
        splashWindow.Close();
    }

}



/// <summary>
/// This class is used to display the splash screen for the application.
/// </summary>
public class MainAppSplashScreen : IApplicationSplashScreen
{

    public static bool IsInitialized = false;

    public MainWindowViewModel? ViewModel { get; }

    public string? AppName { get; }

    public IImage? AppIcon { get; }
    public object SplashScreenContent => new MainAppSplashContent();
    public int MinimumShowTime => 3000;

    public async Task RunTasks(CancellationToken cancellationToken)
    {
        // Simulate or run actual background tasks here
        await Task.Delay(20000,cancellationToken); // Simulate a task with 2 seconds delay
    }

}


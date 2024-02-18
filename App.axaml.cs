
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

using FluentAvalonia.UI.Windowing;

using GetnMethods.Products.ProductViewModels;
using GetnMethods.ViewModels;

using GetnMethods.Views;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace GetnMethods;
public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {

            try
            {
                var splashScreen = new MainAppSplashScreen();
                if (!MainAppSplashScreen.IsInitialized)
                {
                    MainAppSplashScreen.IsInitialized = true;
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        await ShowSplashScreen(splashScreen,desktop);
                    });
                }
            }
            catch (Exception ex)
            {
                var warningDialog = new WarningDialogProduct("Warning","Warning!",ex.Message);
                warningDialog.ShowDialog();
            }

        }

        base.OnFrameworkInitializationCompleted();
    }

    private async Task ShowSplashScreen(MainAppSplashScreen splashScreen,IClassicDesktopStyleApplicationLifetime desktop)
    {
        // Create a window to host the splash screen content
        var splashWindow = new Window
        {
            // Set basic properties for the splash screen window
            SystemDecorations = SystemDecorations.None,
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

        var minimumShowTimeTask = Task.Delay(splashScreen.MinimumShowTime);
        var runTasks = splashScreen.RunTasks(CancellationToken.None);

        await Task.WhenAll(minimumShowTimeTask,runTasks);


        var mainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel()
        };

        desktop.MainWindow = mainWindow;
        desktop.MainWindow.Show();
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
        await Task.Delay(2000,cancellationToken); // Simulate a task with 2 seconds delay
    }

}


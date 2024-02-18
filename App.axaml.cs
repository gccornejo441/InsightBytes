
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
            if (ApplicationSplashScreen.IsInitialized)
            {
                base.OnFrameworkInitializationCompleted();
                return;
            }

            ApplicationSplashScreen.IsInitialized = true;

            Dispatcher.UIThread.InvokeAsync(() => ShowSplashScreenAsync(desktop));
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async Task ShowSplashScreenAsync(IClassicDesktopStyleApplicationLifetime desktop)
    {
        try
        {
            throw new InvalidOperationException("This is a test exception");
            throw new Exception("This is a test exception");
            var splashScreen = new ApplicationSplashScreen();
            await ShowSplashScreen(splashScreen,desktop);
        }
        catch (Exception ex)
        {
            
                var warningDialog = new WarningDialogProduct("Failed","Application start up has failed",ex.Message);
                warningDialog.ShowDialog();
        }
    }

    /// <summary>
    /// Shows the splash screen for the application.
    /// </summary>
    /// <param name="splashScreen"></param>
    /// <param name="desktop"></param>
    /// <returns>
    /// Returns a new instance of the <see cref="Task"/> class which must be awaited.
    /// </returns>
    private async Task ShowSplashScreen(ApplicationSplashScreen splashScreen,IClassicDesktopStyleApplicationLifetime desktop)
    {
        var splashModel = CreateSplashWindow(splashScreen);

        if (splashScreen.SplashScreenContent is Control content)
            splashModel.Content = content;
        
        if (splashScreen.AppIcon is Bitmap appIconBitmap)
            splashModel.Icon = new WindowIcon(appIconBitmap);
        
        splashModel.Show();

        var minimumShowTimeTask = Task.Delay(splashScreen.MinimumShowTime);
        var runTasks = splashScreen.RunTasks(CancellationToken.None);

        await Task.WhenAll(minimumShowTimeTask,runTasks);


        var mainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel()
        };

        desktop.MainWindow = mainWindow;
        desktop.MainWindow.Show();
        splashModel.Close();

    }

    /// <summary>
    /// Creates the splash window for the application.
    /// </summary>
    /// <param name="splashScreen"></param>
    /// <returns>
    /// Returns a new instance of the <see cref="Window"/> class.
    /// </returns>
    private Window CreateSplashWindow(ApplicationSplashScreen splashScreen)
    {
        var splashWindow = new Window
        {
            Width = 600,
            Height = 400,
            CanResize = false,
            SystemDecorations = SystemDecorations.None,
            ExtendClientAreaToDecorationsHint = true,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ShowInTaskbar = false 
        };

        if (splashScreen.SplashScreenContent is Control content)
        {
            splashWindow.Content = content;
        }

        if (splashScreen.AppIcon is Bitmap appIconBitmap)
        {
            splashWindow.Icon = new WindowIcon(appIconBitmap);
        }

        return splashWindow;
    }

}


/// <summary>
/// This class is used to display the splash screen for the application.
/// </summary>
public class ApplicationSplashScreen : IApplicationSplashScreen
{

    public static bool IsInitialized = false;

    public MainWindowViewModel? ViewModel { get; }

    public string? AppName { get; }

    public IImage? AppIcon { get; }
    public object SplashScreenContent => new MainAppSplashContent();
    public int MinimumShowTime => 3000;

    public async Task RunTasks(CancellationToken cancellationToken)
    {
        await Task.Delay(2000,cancellationToken);
    }

}


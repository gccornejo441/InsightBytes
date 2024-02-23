using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;

using InsightBytes.Services.Units;
using InsightBytes.Services.UnitViewModels;
using InsightBytes.ViewModels;

using FluentAvalonia.UI.Windowing;

using ReactiveUI;
using InsightBytes.Services.Factory;

namespace InsightBytes;

public partial class HomeControl : ReactiveUserControl<HomeControlViewModel>
{
    public HomeControl()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            ViewModel?.SelectAllTextInteraction.RegisterHandler(async interaction =>
            {
                LogViewer.SelectAll();
                interaction.SetOutput(Unit.Default);
            }).DisposeWith(disposables);

            ViewModel?.ShowNotificationDialog.RegisterHandler(HandleNotificationDialog);
        });

    }

    private async Task HandleNotificationDialog(InteractionContext<IDialogUnit, bool> interaction)
    {
        var (dialogType, windowTitle, dialogTitle, dialogSubTitle) = ViewModel?.uiTitles;

        var window = this.FindAncestorOfType<AppWindow>();

        try
        {
            if (dialogType == "Warning")
            {
                var warningDialog = new WarningDialogProduct(windowTitle,dialogTitle,dialogSubTitle);
                warningDialog.DataContext = interaction.Input;
                var result = await warningDialog.ShowDialog<bool>(window);
                interaction.SetOutput(result);

            }
            else if (dialogType == "Download")
            {

                var downloadDialog = new DownloadDialogViewModel(windowTitle,dialogTitle,dialogSubTitle);
                downloadDialog.DataContext = interaction.Input;

                var result = await downloadDialog.ShowDialog<bool>(window);
                interaction.SetOutput(result);

            }

        }
        catch (Exception ex)
        {
            LogError(ex); // TODO: LogError is a helper method that needs to be moved to a helper class.

            interaction.SetOutput(false);
        }
    }

    /// <summary>
    /// LogError displays a dialog with the error message.
    /// </summary>
    /// <param name="ex"></param>
    /// <remarks>LogError is a helper method that needs to be moved to a helper class.</remarks>
    private void LogError(Exception ex)
    {
        var warningDialog = new WarningDialogProduct("Error","Error!",ex.Message);
        warningDialog.DataContext = warningDialog;

        warningDialog.Show();
    }
    private void Control_OnSizeChanged(object sender,SizeChangedEventArgs e)
    {
        if (e.HeightChanged)
        {
            ScrollOutputViewer.Height = e.NewSize.Height;
        }
    }
}
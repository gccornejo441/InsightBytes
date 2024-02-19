using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.Threading;

using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;

using GetnMethods.Products;
using GetnMethods.Products.ProductViewModels;


using GetnMethods.ViewModels;

using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GetnMethods.Views;
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{

    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(
            d =>
            {
                ViewModel?.ShowNotificationDialog.RegisterHandler(HandleNotificationDialog);
            });
    }

    private async Task HandleNotificationDialog(InteractionContext<IDialogProduct, bool> interaction)
    {
        var currentDialog = ViewModel?.DialogCalled;
        var notificationMessage = ViewModel?.NotificationMessage;

        try
        {
            if (currentDialog == "Warning")
            {
                var warningDialog = new WarningDialogProduct("Warning", "Warning!", "You cannot return from this action are you sure you would like to continue?");
                warningDialog.DataContext = interaction.Input;
                var result = await warningDialog.ShowDialog<bool>(this);
                interaction.SetOutput(result);

            } else if (currentDialog == "Download")
            {

                var downloadDialog = new DownloadDialogViewModel("Download", "Downloading",notificationMessage);
                downloadDialog.DataContext = interaction.Input;

                var result = await downloadDialog.ShowDialog<bool>(this);
                interaction.SetOutput(result);

            }


        }
        catch (Exception ex)
        {
            LogError(ex);

            interaction.SetOutput(false);
        }
    }

    private void LogError(Exception ex)
    {
        var warningDialog = new WarningDialogProduct("Error", "Error!", ex.Message);
        warningDialog.DataContext = warningDialog;

        warningDialog.Show();
    }
}

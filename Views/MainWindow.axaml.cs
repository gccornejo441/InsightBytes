using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.ReactiveUI;

using GetnMethods.Products;
using GetnMethods.ViewModels;

using ReactiveUI;

namespace GetnMethods.Views;
public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(
            d =>
            {
                ViewModel?.ShowDownloadDialog.RegisterHandler(HandleDownloadDialog);
                ViewModel?.ShowNotificationDialog.RegisterHandler(HandleNotificationDialog);
            });
    }


    private async Task HandleNotificationDialog(InteractionContext<IDialogProduct, bool> interaction)
    {
        var currentDialog = ViewModel?.DialogCalled;

        try
        {
            if (currentDialog == "Warning")
            {
                var warningDialog = new WarningDialogProduct();
                warningDialog.DataContext = interaction.Input;
                var result = await warningDialog.ShowDialog<bool>(this);
                interaction.SetOutput(result);

            } else if (currentDialog == "Download")
            {
                var downloadDialog = new DownloadDialogProduct();
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
        Console.WriteLine($"An error occurred: {ex.Message}");
        Debugger.Break();
    }

    private async Task HandleDownloadDialog(InteractionContext<DownloadDialogViewModel, bool> interaction)
    {
        var downloadDialog = new DownloadViewDialog(); // Create the dialog
        downloadDialog.DataContext = interaction.Input; // Sets the data context of the dialog to the input of the interaction.  The input interaction is the view model that is passed to the dialog.

        var result = await downloadDialog.ShowDialog<bool>(this); // Show the dialog and wait for the result.
        interaction.SetOutput(result); // Set the output of the interaction to the result of the dialog.  // https://www.reactiveui.net/docs/handbook/interactions/

    }

}
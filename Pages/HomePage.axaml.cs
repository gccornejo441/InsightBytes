using System;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;

using GetnMethods.Products.ProductViewModels;
using GetnMethods.Products;
using GetnMethods.ViewModels;

using ReactiveUI;
using Avalonia.ReactiveUI;

namespace GetnMethods.Pages;
public partial class HomePage : ReactiveUserControl<HomePageViewModel>
{
    public HomePage()
    {
        InitializeComponent();

        this.WhenActivated(
            d =>
            {
                ViewModel?.ShowNotificationDialog.RegisterHandler(HandleNotificationDialog);
            });
    }

    private void Control_OnSizeChanged(object sender,SizeChangedEventArgs e)
    {
        if (e.HeightChanged)
        {
            ScrollOutputViewer.Height = e.NewSize.Height;
        }
    }

    private async Task HandleNotificationDialog(InteractionContext<IDialogProduct,bool> interaction)
    {
        var currentDialog = ViewModel?.DialogCalled;
        var notificationMessage = ViewModel?.NotificationMessage;

        try
        {
            if (currentDialog == "Warning")
            {
                var warningDialog = new WarningDialogProduct("Warning","Warning!","You cannot return from this action are you sure you would like to continue?");
                warningDialog.DataContext = interaction.Input;
                var result = await warningDialog.ShowDialog<bool>(warningDialog);
                interaction.SetOutput(result);

            }
            else if (currentDialog == "Download")
            {

                var downloadDialog = new DownloadDialogViewModel("Download","Downloading",notificationMessage);
                downloadDialog.DataContext = interaction.Input;

                var result = await downloadDialog.ShowDialog<bool>(downloadDialog);
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
        var warningDialog = new WarningDialogProduct("Error","Error!",ex.Message);
        warningDialog.DataContext = warningDialog;

        warningDialog.Show();
    }
}

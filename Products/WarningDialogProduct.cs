namespace GetnMethods.Products;
public class WarningDialogProduct : BaseDialogProduct, IDialogProduct
{
    public WarningDialogProduct()
    {
        var baseDialogViewModel = new BaseDialogProductViewModel();
        this.Title = "Warning";
        baseDialogViewModel.DialogTitle = "Warning!";
        baseDialogViewModel.DialogSubTitle = "You are about to clear your screen, are you sure you want to proceed?";
        var isConfirmed = baseDialogViewModel.IsConfirmed;


        this.DataContext = baseDialogViewModel;

        CloseDialog(baseDialogViewModel);
    }

    /// <summary>
    /// Closes the dialog by invoking the RequestClose event from the BaseDialogProductViewModel
    /// </summary>
    /// <param name="baseDialogProductViewModel"></param>
    public void CloseDialog(BaseDialogProductViewModel baseDialogProductViewModel)
    {
        baseDialogProductViewModel.RequestClose += (dialogResult) =>
        {
            this.Close(dialogResult);
        };
    }


    public void ShowDialog()
    {
        this.Show();
    }
}

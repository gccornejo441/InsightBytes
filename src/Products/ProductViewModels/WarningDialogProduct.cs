namespace GetnMethods.Products.ProductViewModels;

public class WarningDialogProduct : BaseDialog, IDialogProduct
{
    /// <summary>
    /// Constructor for the WarningDialogProduct
    /// </summary>
    /// <param name="dialogTitle"></param>
    /// <param name="dialogSubTitle"></param>
    public WarningDialogProduct(string windowTitle, string dialogTitle, string dialogSubTitle)
    {
        var baseDialogViewModel = new BaseDialogViewModel();

        baseDialogViewModel.WindowTitle = windowTitle;
        baseDialogViewModel.DialogTitle = dialogTitle;
        baseDialogViewModel.DialogSubTitle = dialogSubTitle;


        DataContext = baseDialogViewModel;

        CloseDialog(baseDialogViewModel);
    }
    /// <summary>
    /// Default constructor for the WarningDialogProduct
    /// </summary>
    public WarningDialogProduct() { }

    /// <summary>
    /// Closes the dialog by invoking the RequestClose event from the BaseDialogProductViewModel
    /// </summary>
    /// <param name="baseDialogProductViewModel"></param>
    public void CloseDialog(BaseDialogViewModel baseDialogProductViewModel)
    {
        baseDialogProductViewModel.RequestClose += (dialogResult) =>
        {
            Close(dialogResult);
        };
    }


    public void ShowDialog() { Show(); }

    public void SetMessage(string message)
    {
        
    }
}

namespace GetnMethods.Products.ProductViewModels;

public class DownloadDialogViewModel : BaseDialog, IDialogProduct
{
    private string _message;
    /// <summary>
    /// Constructor for the DownloadDialogViewModel
    /// </summary>
    /// <param name="dialogTitle"></param>
    /// <param name="dialogSubTitle"></param>
    public DownloadDialogViewModel(string windowTitle,string dialogTitle,string dialogSubTitle)
    {
        var baseDialogViewModel = new BaseDialogViewModel();

        baseDialogViewModel.WindowTitle = windowTitle;
        baseDialogViewModel.DialogTitle = dialogTitle;
        baseDialogViewModel.DialogSubTitle = dialogSubTitle;


        DataContext = baseDialogViewModel;

        CloseDialog(baseDialogViewModel);
    }
    /// <summary>
    /// Default constructor for the DownloadDialogViewModel
    /// </summary>
    public DownloadDialogViewModel() { }

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

    public void ShowDialog()
    {
        throw new System.NotImplementedException();
    }

    public void SetMessage(string message)
    {
    }
}

namespace GetnMethods.Products;
public class WarningDialogProduct : BaseDialogProduct, IDialogProduct
{
    public WarningDialogProduct()
    {
        var baseDialogViewModel = new BaseDialogProductViewModel();
        this.Title = "Warning";
        baseDialogViewModel.DialogTitle = "Warning!";
        baseDialogViewModel.DialogSubTitle = "You are about to clear your screen, are you sure you want to proceed?";

        this.DataContext = baseDialogViewModel;

    }


    public void CloseDialog()
    {
        this.Close();
    }

    public void ShowDialog()
    {
        this.Show();
    }
}

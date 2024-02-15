using Avalonia.Controls;

namespace GetnMethods.Products;
public class ConfirmDialogProduct : Window, IDialogProduct
{
    public ConfirmDialogProduct()
    {
        var baseDialogViewModel = new BaseDialogProductViewModel();
        this.Title = "Confirmation";
        baseDialogViewModel.DialogTitle = "Success";
        baseDialogViewModel.DialogSubTitle = "Your actions has executed successfully";
        this.DataContext = baseDialogViewModel;

    }
    

    public void CloseDialog(BaseDialogProductViewModel baseDialogProductViewModel)
    {
        throw new System.NotImplementedException();
    }

    public void ShowDialog()
    {
        this.Show();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Controls;

using GetnMethods.ViewModels;
using ReactiveUI;

namespace GetnMethods.Products;
public class DownloadDialogProduct : BaseDialogProduct, IDialogProduct
{
    public DownloadDialogProduct()
    {
        var baseDialogViewModel = new BaseDialogProductViewModel();
        this.Title = "Download";
        baseDialogViewModel.DialogTitle = "Downloading";
        baseDialogViewModel.DialogSubTitle = "Downloading";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Controls;

using GetnMethods.ViewModels;
using ReactiveUI;

namespace GetnMethods.Products;
public class DownloadDialogProduct : Window, IDialogProduct
{
    public DownloadDialogProduct()
    {
        var baseDialogViewModel = new BaseDialogProductViewModel();
        this.Title = "Download";

        baseDialogViewModel.DialogTitle = "Downloading";
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

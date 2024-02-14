﻿using Avalonia.Controls;

namespace GetnMethods.Products;
public class ConfirmationDialogProduct : Window, IDialogProduct
{
    public ConfirmationDialogProduct()
    {
        var baseDialogViewModel = new BaseDialogProductViewModel();
        this.Title = "Confirmation";
        baseDialogViewModel.DialogTitle = "Confirmation!";
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

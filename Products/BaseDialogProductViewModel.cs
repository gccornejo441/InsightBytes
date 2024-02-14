using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

using Avalonia.Controls;

using GetnMethods.ViewModels;
using GetnMethods.Views;

using ReactiveUI;

namespace GetnMethods.Products;
public class BaseDialogProductViewModel : ViewModelBase
{
    private string _dialogTitle;
    public string DialogTitle
    {
        get => _dialogTitle;
        set => this.RaiseAndSetIfChanged(ref _dialogTitle,value);
    }

    private string _dialogSubTitle;
    public string DialogSubTitle
    {
        get => _dialogSubTitle;
        set => this.RaiseAndSetIfChanged(ref _dialogSubTitle,value);
    }

    private bool _isConfirmed;
    public bool IsConfirmed
    {
        get => _isConfirmed;
        set => this.RaiseAndSetIfChanged(ref _isConfirmed,value);
    }

    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    public BaseDialogProductViewModel()
    {
        ConfirmCommand = ReactiveCommand.Create(() =>
        {
            IsConfirmed = true;
            CloseDialog();
        });

        CancelCommand = ReactiveCommand.Create(() =>
        {
            IsConfirmed = false;
            CloseDialog();
        });
    }


    private async void CloseDialog()
    {
    }

}

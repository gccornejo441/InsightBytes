using System;
using System.Windows.Input;

using GetnMethods.ViewModels;

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

    public event Action<bool> RequestClose;
    public BaseDialogProductViewModel()
    {
        ConfirmCommand = ReactiveCommand.Create(() =>
        {
            IsConfirmed = true;
            RequestClose?.Invoke(false);
        });

        CancelCommand = ReactiveCommand.Create(() =>
        {
            IsConfirmed = true;
            RequestClose?.Invoke(true);
        });

    }

}

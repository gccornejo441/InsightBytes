using System;
using System.Windows.Controls;
using System.Windows.Input;
using GetnMethods.ViewModels;

using ReactiveUI;

namespace GetnMethods.Products.ProductViewModels;
public partial class BaseDialogViewModel : ViewModelBase
{
    private string _windowTitle;
    public string WindowTitle
    {
        get => _windowTitle;
        set => this.RaiseAndSetIfChanged(ref _windowTitle, value);
    }
    private string _dialogTitle;
    public string DialogTitle
    {
        get => _dialogTitle;
        set => this.RaiseAndSetIfChanged(ref _dialogTitle, value);
    }

    private string _dialogSubTitle;
    public string DialogSubTitle
    {
        get => _dialogSubTitle;
        set => this.RaiseAndSetIfChanged(ref _dialogSubTitle, value);
    }

    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    public object? DataContext
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public event Action<bool> RequestClose;
    public BaseDialogViewModel()
    {
        ConfirmCommand = ReactiveCommand.Create(() =>
        {
            RequestClose?.Invoke(false);
        });

        CancelCommand = ReactiveCommand.Create(() =>
        {
            RequestClose?.Invoke(true);
        });

    }

}

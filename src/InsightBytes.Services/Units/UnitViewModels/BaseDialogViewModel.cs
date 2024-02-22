using System.Windows.Input;

using ReactiveUI;

namespace InsightBytes.Services.Units.UnitViewModels;
public partial class BaseDialogViewModel : ReactiveObject
{
    private string _windowTitle;
    public string WindowTitle
    {
        get => _windowTitle;
        set => this.RaiseAndSetIfChanged(ref _windowTitle,value);
    }
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
 
    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

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

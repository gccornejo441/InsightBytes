using System.Windows.Input;

using FluentAvalonia.UI.Controls;

using InsightBytes.Services.Units;

using ReactiveUI;

namespace InsightBytes.ViewModels;

/// <summary>
/// Base class for all view models that inherit from <see cref="ReactiveObject"/>.
/// </summary>
/// <remarks>
/// Refrain from directly inheriting this class, instead inherit from <see cref="MainViewModelBase"/>.
/// </remarks>
public class ViewModelBase : ReactiveObject
{
    public Interaction<IDialogUnit,bool> ShowNotificationDialog { get; set; }

    private string _dialogCalled;
    public string DialogCalled
    {
        get => _dialogCalled;
        set => this.RaiseAndSetIfChanged(ref _dialogCalled,value);
    }

    private string _notificationMessage;
    public string NotificationMessage
    {
        get => _notificationMessage;
        set => this.RaiseAndSetIfChanged(ref _notificationMessage,value);
    }
    
}

public class MainViewModelBase : ViewModelBase
{
    public string NavHeader { get; set; }

    public Symbol IconKey { get; set; }

    public bool ShowsInFooter { get; set; }
}

using System;
using System.Linq;

namespace GetnMethods.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

using FluentAvalonia.UI.Windowing;

using ReactiveUI;

using System;
using System.Reactive.Linq;


/// <summary>
/// A ReactiveUI <see cref="Window"/> that implements the <see cref="IViewFor{TViewModel}"/> interface and will
/// activate your ViewModel automatically if the view model implements <see cref="IActivatableViewModel"/>. When
/// the DataContext property changes, this class will update the ViewModel property with the new DataContext value,
/// and vice versa.
/// </summary>
/// <typeparam name="TViewModel">ViewModel type.</typeparam>
public class ReactiveAppWindow<TViewModel> : AppWindow, IViewFor<TViewModel>, IViewFor, IActivatableView where TViewModel : class
{
    public static readonly StyledProperty<TViewModel?> ViewModelProperty = ReactiveWindow<TViewModel>.ViewModelProperty.AddOwner<ReactiveAppWindow<TViewModel>>();

    public static readonly StyledProperty<bool> IsSaveWindowSizeProperty = AvaloniaProperty
        .Register<ReactiveAppWindow<TViewModel>,bool>(nameof(IsSaveWindowSize),true);

    public bool IsSaveWindowSize
    {
        get => GetValue(IsSaveWindowSizeProperty);
        set => SetValue(IsSaveWindowSizeProperty,value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveWindow{TViewModel}"/> class.
    /// </summary>
    public ReactiveAppWindow()
    {
        // This WhenActivated block calls ViewModel's WhenActivated
        // block if the ViewModel implements IActivatableViewModel.
        this.WhenActivated(disposables => { });
        this.GetObservable(DataContextProperty).Subscribe(OnDataContextChanged);
        this.GetObservable(ViewModelProperty).Subscribe(OnViewModelChanged);
    }

    /// <summary>
    /// The ViewModel.
    /// </summary>
    public TViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty,value);
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel?)value;
    }

    private void OnDataContextChanged(object? value)
    {
        if (value is TViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        else
        {
            ViewModel = null;
        }
    }

    private void OnViewModelChanged(object? value)
    {
        if (value == null)
        {
            ClearValue(DataContextProperty);
        }
        else if (DataContext != value)
        {
            DataContext = value;
        }
    }
}


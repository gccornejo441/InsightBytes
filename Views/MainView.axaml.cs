using System.Reactive;
using System.Reactive.Disposables;

using Avalonia.Controls;
using Avalonia.ReactiveUI;

using GetnMethods.ViewModels;

using ReactiveUI;

namespace GetnMethods;

public partial class MainView : ReactiveUserControl<MainWindowViewModel>
{
    public MainView()
    {
        InitializeComponent();
        this.WhenActivated(disposables =>
        {
            ViewModel?.SelectAllTextInteraction.RegisterHandler(async interaction =>
            {
                LogViewer.SelectAll();
                interaction.SetOutput(Unit.Default);
            }).DisposeWith(disposables);
        });

    }

    private void Control_OnSizeChanged(object sender,SizeChangedEventArgs e)
    {
        if (e.HeightChanged)
        {
            ScrollOutputViewer.Height = e.NewSize.Height;
        }
    }

}
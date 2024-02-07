using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using FluentAvalonia.UI.Controls;

namespace GetnMethods;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void NavPanelOnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is NavigationViewItem navItem && navItem.Tag is not null)
        {

        }
    }   

    private void Control_OnSizeChanged(object sender,SizeChangedEventArgs e)
    {
        if (e.HeightChanged)
            ScrollOutputViewer.Height = e.NewSize.Height;
    }
}
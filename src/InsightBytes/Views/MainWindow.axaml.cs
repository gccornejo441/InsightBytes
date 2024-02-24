using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;

using FluentAvalonia.Styling;
using FluentAvalonia.UI.Windowing;

namespace InsightBytes.Views;
public partial class MainWindow : Window 
{ 
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.ExtendClientAreaToDecorationsHint = true;
     
        Application.Current.ActualThemeVariantChanged += OnActualThemeVariantChanged;
    }

   

    private void OnActualThemeVariantChanged(object? sender, EventArgs e)
    {
        //if(IsWindows11)
        //{
        //    if(ActualThemeVariant != FluentAvaloniaTheme.HighContrastTheme)
        //    {

        //    }
        //}
    }

    private void Panel_PointerPressed(object? sender,Avalonia.Input.PointerPressedEventArgs e)
    {
        this.BeginMoveDrag(e);
    }
}
using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;

using FluentAvalonia.Styling;
using FluentAvalonia.UI.Windowing;

namespace InsightBytes.Views;
public partial class MainWindow : AppWindow 
{ 
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }
}
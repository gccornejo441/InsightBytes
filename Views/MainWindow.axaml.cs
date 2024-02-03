using Avalonia.Controls;

namespace GetAllMethods.Views;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Control_OnSizeChanged(object sender,SizeChangedEventArgs e)
    {
        if (e.HeightChanged)
            ScrollOutputViewer.Height = e.NewSize.Height;
    }
}
using ReactiveUI;

namespace GetnMethods.ViewModels;
public class ViewModelBase : ReactiveObject
{
    public string NavHeader { get; set; }

    public string IconKey { get; set; }

    public bool ShowsInFooter { get; set; }
}


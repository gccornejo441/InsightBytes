using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InsightBytes.Factory;

using FluentAvalonia.UI.Controls;

namespace InsightBytes.ViewModels;
public class MainViewViewModel : MainViewModelBase
{
    public NavigationPageFactory NavigationPageFactory { get; }
    public MainViewViewModel()
    {
        NavigationPageFactory = new NavigationPageFactory(this);
    }


}

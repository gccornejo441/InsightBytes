using System;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

using GetnMethods.Pages;
using GetnMethods.ViewModels;

namespace GetnMethods.ViewModels;

public class MainViewViewModel : SuperViewModelBase
{
    public MainViewViewModel()
    {
        NavigationFactory = new NavigationFactory(this);
    }

    public NavigationFactory NavigationFactory { get; }

    public AvaloniaList<MainAppSearchItem> SearchTerms { get; } = new AvaloniaList<MainAppSearchItem>();

    public void BuildSearchTerms(MainPageViewModelBase pageItem)
    {
        if (pageItem is HomePageViewModel || pageItem is SettingsPageViewModel)
            return;

        void Add(PageBaseViewModel item)
        {
            if (item.SearchKeywords is null)
                return;

            string ctrlNamespace = "Avalonia.UI.Controls";

            for (int i = 0; i < item.SearchKeywords.Length; i++)
            {
                SearchTerms.Add(new MainAppSearchItem
                {
                    Header = item.SearchKeywords[i],
                    ViewModel = item,
                    Namespace = ctrlNamespace
                });
            }            
        }
    }
}

public class NavigationFactory : INavigationPageFactory
{
    public NavigationFactory(MainViewViewModel owner)
    {
        Owner = owner;
    }

    public MainViewViewModel Owner { get; }

    public Control GetPage(Type srcType)
    {
        return null;
    }

    public Control GetPageFromObject(object target)
    {
        if (target is HomePageViewModel)
        {
            return new HomePage
            {
                DataContext = target,
            };
        }
        else if (target is SettingsPageViewModel)
        {
            return new SettingsPage
            {
                DataContext = target
            };
        }
        else
        {
            return ResolvePage(target as PageBaseViewModel);
        }
    }

    private Control ResolvePage(PageBaseViewModel pbvm)
    {
        if (pbvm is null)
            return null;

        Control page = null;
        var key = pbvm.PageKey;
        return page;
    }
    //if (CorePages.TryGetValue(key, out var func))
    //{
    //    page = func();

    //    const string faPageGithub =
    //       "https://github.com/amwx/FluentAvalonia/tree/master/samples/FAControlsGallery/Pages/CoreControlPages";

    //    (page as ControlsPageBase).GithubPrefixString = faPageGithub;
    //    (page as ControlsPageBase).CreationContext = pbvm;
    //}
    //else if (FAPages.TryGetValue(key, out func))
    //{
    //    var pg = (ControlsPageBase)func();
    //    var dc = (FAControlsPageItem)pbvm;
    //    const string faPageGithub =
    //       "https://github.com/amwx/FluentAvalonia/tree/master/samples/FAControlsGallery/Pages/FAControlsPages";

    //    pg.GithubPrefixString = faPageGithub;
    //    pg.PreviewImage = Application.Current.FindResource(dc.IconResourceKey) as IconSource;
    //    pg.ControlName = dc.Header;
    //    pg.ControlNamespace = dc.Namespace;
    //    pg.Description = dc.Description;
    //    pg.CreationContext = pbvm;

    //    if (dc.WinUIDocsLink is not null)
    //        pg.WinUIDocsLink = new Uri(dc.WinUIDocsLink);

    //    pg.WinUINamespace = dc.WinUINamespace;

    //    if (dc.WinUIGuidelinesLink is not null)
    //        pg.WinUIGuidelinesLink = new Uri(dc.WinUIGuidelinesLink);

    //    page = pg;
    //}



}

//public class MainAppSearchItem
//{
//    public MainAppSearchItem() { }

//    public MainAppSearchItem(string pageHeader, Type pageType)
//    {
//        Header = pageHeader;
//        PageType = pageType;
//    }

//    public string Header { get; set; }

//    public PageBaseViewModel ViewModel { get; set; }

//    public string Namespace { get; set; }

//    public Type PageType { get; set; }
//}

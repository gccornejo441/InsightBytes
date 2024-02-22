
using Avalonia;
using Avalonia.Controls;

using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

using InsightBytes.Services;
using InsightBytes.Services.UnitViewModels;
using InsightBytes.ViewModels;

using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using FluentAvalonia.UI.Windowing;

using System;
using System.Collections.Generic;
using System.Linq;

namespace InsightBytes;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void OnFrameViewNavigated(object sender,NavigationEventArgs e)
    {
        var page = e.Content as Control;
        var dataContext = page?.DataContext;

    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var vm = new MainViewViewModel();
        DataContext = vm;
        FrameView.NavigationPageFactory = vm.NavigationPageFactory;
        NavigationService.Instance.SetFrame(FrameView);

        if (e.Root is AppWindow window)
        {
            InitializeNavigationPages();
        }
        else
        {
            InitializeNavigationPages();
        }

        FrameView.Navigated += OnFrameViewNavigated;
        NavView.ItemInvoked += OnNavigationViewItemInvoked;
    }

    private void OnNavigationViewItemInvoked(object sender,NavigationViewItemInvokedEventArgs e)
    {
        
        if (e.InvokedItemContainer is NavigationViewItem nvi)
        {
            NavigationTransitionInfo info;

            info = e.RecommendedNavigationTransitionInfo;

            NavigationService.Instance.NavigateFromContext(nvi.Tag,info);
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (VisualRoot is AppWindow aw)
        {
          
            TitleBarHost.ColumnDefinitions[3].Width = new GridLength(aw.TitleBar.RightInset,GridUnitType.Pixel);
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        var pt = e.GetCurrentPoint(this);
        if (pt.Properties.PointerUpdateKind == PointerUpdateKind.XButton2Released)
        {
            if (FrameView.CanGoForward)
            {
                FrameView.GoForward();
                e.Handled = true;
            }
        }

        base.OnPointerReleased(e);
    }

    public void InitializeNavigationPages()
    {

        try
        {
            var mainPages = new MainViewModelBase[]
            {
                new SettingsControlViewModel { NavHeader = "Setting", IconKey = "SettingsIcon", ShowsInFooter = true },
                new HomeControlViewModel
                {
                    NavHeader = "Home",
                    IconKey = "HomeIcon",
                }
            };
            var menuItems = new List<NavigationViewItemBase>(4);
            var footerItems = new List<NavigationViewItemBase>(2);

            Dispatcher.UIThread.Post(() =>
            {
                for (int i = 0; i < mainPages.Length; i++)
                {
                    var pg = mainPages[i];
                    var nvi = new NavigationViewItem
                    {
                        Content = pg.NavHeader,
                        Tag = pg,
                        IconSource = this.FindResource(pg.IconKey) as IconSource
                    };

                    if (pg.ShowsInFooter)
                        footerItems.Add(nvi);
                    else
                        menuItems.Add(nvi);

                }

                NavView.MenuItemsSource = menuItems;
                NavView.FooterMenuItemsSource = footerItems;
                
                NavView.PaneDisplayMode = NavigationViewPaneDisplayMode.LeftMinimal;
                

                FrameView.NavigateFromObject((NavView.MenuItemsSource.ElementAt(0) as Control).Tag);
            });
        }
        catch (Exception ex)
        {
            var warningDialog = new WarningDialogProduct("Warning","Warning!",$"Exception: {ex.Message}");

            warningDialog.ShowDialog();
        }
    }
}

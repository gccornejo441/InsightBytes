using System;
using System.Reflection;

using Avalonia.Controls;

using InsightBytes.Services;
using InsightBytes.ViewModels;

using FluentAvalonia.UI.Controls;

namespace InsightBytes.Factory;
public class NavigationPageFactory : INavigationPageFactory
{
    public NavigationPageFactory(MainViewViewModel mainViewViewModel)
    {
        MainViewViewModel = mainViewViewModel;
    }

    public MainViewViewModel MainViewViewModel { get; }

    /// <summary>
    /// Get a page from a type.
    /// </summary>
    /// <param name="srcType"></param>
    /// <returns></returns>
    public Control GetPage(Type srcType)
    {
        return null;
    }

    /// <summary>
    /// Get a page from an object.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Control GetPageFromObject(object target)
    {
        var viewModelName = target.GetType().Name;
        var controlName = viewModelName.Replace("ControlViewModel","Control");

        var namespacePrefix = "InsightBytes";

        try
        {
            var controlType = Assembly.GetExecutingAssembly().GetType($"{namespacePrefix}.{controlName}");
            if (controlType != null && Activator.CreateInstance(controlType) is Control control)
            {
                control.DataContext = target;
                return control;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating control instance: {ex.Message}");
        }


        return null;
    }
}
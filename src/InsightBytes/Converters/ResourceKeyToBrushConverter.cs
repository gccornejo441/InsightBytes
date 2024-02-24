using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

using Avalonia.Media;

namespace InsightBytes.Converters;
public class ResourceKeyToBrushConverter : IValueConverter
{
    public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
    {
        if (value is string resourceKey)
        {
            // Attempt to find the resource in the current application context
            var resource = Avalonia.Application.Current.FindResource(resourceKey);
            if (resource is SolidColorBrush brush)
            {
                return brush;
            }
            else
            {
                // Log or handle the case where the resource is not found or not a SolidColorBrush
                Console.WriteLine($"Resource with key '{resourceKey}' not found or is not a SolidColorBrush.");
            }
        }

        // Return a default brush or null if preferred
        return SolidColorBrush.Parse("Transparent");
    }

    public object ConvertBack(object value,Type targetType,object parameter,CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
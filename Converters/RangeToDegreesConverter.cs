using System;
using System.Collections.Generic;
using System.Globalization;

using Avalonia.Data.Converters;

namespace GetnMethods.Converters;
public class RangeToDegreesConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values,Type targetType,object? parameter,CultureInfo culture)
    {
        double min = 0d, max = 100d, val = 0d;
        if (values.Count > 0 && values[0] is double value)
        {
            val = value;
        }
        if (values.Count > 1 && values[1] is double minimum)
        {
            min = minimum;
        }
        if (values.Count > 2 && values[2] is double maximum)
        {
            max = maximum;
        }

        double range = max - min;
        double proportionOfCircle = (val - min) / range;
        return proportionOfCircle * 360d;
    }

}

using System;
using System.Globalization;
using Avalonia.Data.Converters;

using FluentAvalonia.UI.Controls;

namespace InsightBytes.Converters
{
    public class IntToSymbolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return (Symbol)intValue;
            }

            return Symbol.AlertFilled;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Symbol symbol)
            {
                return (int)symbol;
            }

            return 0;
        }
    }
}

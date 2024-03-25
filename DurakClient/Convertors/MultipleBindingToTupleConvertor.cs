using Avalonia.Data;
using Avalonia.Data.Converters;

using System;
using System.Collections.Generic;
using System.Globalization;

namespace DurakClient.Convertors
{
    public class MultipleBindingToTupleConvertor : IMultiValueConverter
    {
        public object? Convert(IList<object?> value, Type targetType, object? parameter, CultureInfo culture)
        {
            Guid guid = Guid.Empty;
            if (value[0] is not null)
                guid = Guid.Parse(value[0]!.ToString()!);

            return (guid, value[1]?.ToString());
        }
        public object ConvertBack(IList<object?> value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : BindingOperations.DoNothing;
        }
    }
}
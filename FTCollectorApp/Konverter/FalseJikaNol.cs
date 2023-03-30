using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FTCollectorApp.Konverter
{
    public class FalseJikaNol : IValueConverter
    {
        public object Convert(object? value, Type? targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                int num = (int)value;
                Console.WriteLine();
                return num != 0;
            }

            throw new ArgumentException("Value is not a valid integer", "value");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine();
            throw new NotImplementedException();
        }
    }
}

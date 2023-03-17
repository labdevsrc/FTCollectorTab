using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FTCollectorApp.Konverter
{
    public class CheckTimeFormatValidity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan dummyOutput;
            var result = false;
            Console.WriteLine(value.ToString());
            try
            {
                result = !TimeSpan.TryParse(value.ToString(), out dummyOutput);
                Console.WriteLine(result);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine();
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine();
            throw new NotImplementedException();
        }
    }
}

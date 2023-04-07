using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace FTCollectorApp.Konverter
{
    public class CheckTimeFormatValidity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan dummyOutput;
            var displayInvalidtime = false;
            if (value != null)
            {
                Console.WriteLine(value.ToString());
                if (value.ToString().Length > 1)
                {
                    //    displayInvalidtime = !TimeSpan.TryParse(value.ToString(), out dummyOutput);
                    string pattern = "\\d{1,2}:\\d{2}";
                    displayInvalidtime = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                }
            }
            Console.WriteLine();
            return displayInvalidtime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine();
            throw new NotImplementedException();
        }
    }
}

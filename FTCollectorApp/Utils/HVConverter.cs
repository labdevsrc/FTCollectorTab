using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FTCollectorApp.Utils
{
    public class HVConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Horizontal")
                return (string) "H";
            else if ((string)value == "Vertical")
                return (string) "V";
            return string.Empty;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "H")
                return (string)"Horizontal";
            else if ((string)value == "V")
                return (string)"Vertical";
            return string.Empty;
        }

    }
}

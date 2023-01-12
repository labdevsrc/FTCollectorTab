using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace FTCollectorApp.Utils
{
    public class FBConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Front")
                return (string)"F";
            else if ((string)value == "Back")
                return (string)"B";
            return string.Empty;
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "F")
                return (string)"Front";
            else if ((string)value == "B")
                return (string)"Back";
            return string.Empty;
        }


    }
}

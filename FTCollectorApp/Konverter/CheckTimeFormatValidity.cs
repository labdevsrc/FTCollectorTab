﻿using System;
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
            var displayInvalidtime = false;
            if (value != null)
            {
                Console.WriteLine(value.ToString());
                displayInvalidtime = !TimeSpan.TryParse(value.ToString(), out dummyOutput);
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

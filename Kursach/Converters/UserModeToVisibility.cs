﻿using Kursach.DataBase;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kursach.Converters
{
    [ValueConversion(typeof(UserMode), typeof(Visibility))]
    class UserModeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UserMode mode = (UserMode)value;

            return mode == UserMode.Admin ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

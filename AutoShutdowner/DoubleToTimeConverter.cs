using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoShutdowner
{
    public class DoubleToTimeConverter : IValueConverter
    {
        static double minuteSize = 60;
        static double hourSize = 60;
        static double minute = minuteSize * 1000;
        static double hour = hourSize * minute;
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (Double) value;
            var minutes = ((int)(source / minute)) % minuteSize;
            var hours = ((int)(source / hour)) % hourSize;
            var minutesString = minutes.ToString();
            if (minutesString.Length < 2)
                minutesString = "0"+ minutesString;
            return hours + ":" + minutesString;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

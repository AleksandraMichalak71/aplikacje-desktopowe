using System;
using System.Globalization;
using System.Windows.Data;

namespace aplikacje_desktopowe_zad3
{
    public class DateTimeToStringConverter : IValueConverter
    {
        private const string Format = "dd.MM.yyyy";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dataUrodzin)
            {
                return dataUrodzin.ToString(Format, culture);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string tekst &&
                DateTime.TryParseExact(tekst, Format, culture, DateTimeStyles.None, out var data))
            {
                return data;
            }

            return Binding.DoNothing;
        }
    }
}

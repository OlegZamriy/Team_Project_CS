using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Team_Project_CS
{
    public class GradesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var grades = value as ObservableCollection<int>;
            if (grades != null)
            {
                return string.Join(", ", grades);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gradesString = value as string;
            if (gradesString != null)
            {
                var grades = gradesString.Split(new[] { ", " }, StringSplitOptions.None);
                return new ObservableCollection<int>(Array.ConvertAll(grades, int.Parse));
            }
            return new ObservableCollection<int>();
        }
    }
}
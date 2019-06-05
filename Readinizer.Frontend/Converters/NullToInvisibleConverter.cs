using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Frontend.Converters
{
    public class NullToInvisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ObservableCollection<ADDomain>)
            {
                var observableCollection = (ObservableCollection<ADDomain>) value;
                return observableCollection.Count <= 0 ? Visibility.Hidden : Visibility.Visible;
            }
            if (value is ObservableCollection<OrganisationalUnit>)
            {
                var observableCollection = (ObservableCollection<OrganisationalUnit>)value;
                return observableCollection.Count <= 0 ? Visibility.Hidden : Visibility.Visible;
            }
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

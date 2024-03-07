using SOSTeam.TravelAgency.WPF.ViewModels.Guest2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SOSTeam.TravelAgency.Converter
{
    public class BorderConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)values[0];
            int count = (int)values[1];

            if (index == 0)
            {
                return new Thickness(0, 2, 0, 2);
            }
            else if (index == count - 1)
            {
                return new Thickness(0, 0, 0, 2);
            }
            else
            {
                return new Thickness(0, 0, 0, 2);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

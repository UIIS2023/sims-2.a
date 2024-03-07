using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.Behaviors
{
    public static class DatePickerBehavior
    {
        public static readonly DependencyProperty DatesToBlackoutProperty =
            DependencyProperty.RegisterAttached(
                "DatesToBlackout",
                typeof(IEnumerable<DateTime>),
                typeof(DatePickerBehavior),
                new PropertyMetadata(null, DatesToBlackoutChanged));

        public static IEnumerable<DateTime> GetDatesToBlackout(DependencyObject obj)
        {
            return (IEnumerable<DateTime>)obj.GetValue(DatesToBlackoutProperty);
        }

        public static void SetDatesToBlackout(DependencyObject obj, IEnumerable<DateTime> value)
        {
            obj.SetValue(DatesToBlackoutProperty, value);
        }

        private static void DatesToBlackoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DatePicker datePicker)
            {
                if (e.NewValue is IEnumerable<DateTime> blackoutDates)
                {
                    datePicker.BlackoutDates.Clear();

                    foreach (DateTime date in blackoutDates)
                    {
                        datePicker.BlackoutDates.Add(new CalendarDateRange(date));
                    }
                }
            }
        }
    }
}

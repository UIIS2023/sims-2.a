using System;
using System.Globalization;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ValidationRules.TourGuide
{
    class DateTimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || !(value is DateTime))
            {
                return new ValidationResult(false, "This field cannot be empty.");
            }

            DateTime fieldValue = (DateTime)value;


            if (fieldValue < DateTime.Now)
            {
                return new ValidationResult(false, "Selected date and time cannot be less than the current\ndate and time.");
            }

            return ValidationResult.ValidResult;
        }
    }
}

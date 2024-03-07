using System.Globalization;
using System.Windows.Controls;

namespace SOSTeam.TravelAgency.WPF.ValidationRules.TourGuide
{
    public class EmptyFieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string fieldValue = (string)value;

            if (string.IsNullOrEmpty(fieldValue))
            {
                return new ValidationResult(false, "This field cannot be empty.");
            }

            return ValidationResult.ValidResult;
        }
    }
}

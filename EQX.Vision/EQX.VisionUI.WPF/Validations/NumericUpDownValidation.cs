using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EQX.VisionUI.WPF.Validations
{
    public class NumberUpDownValidationRule : ValidationRule
    {
        public bool AllowDecimal { get; set; } = true;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "Value cannot be empty");
            }

            if (AllowDecimal)
            {
                if (double.TryParse(value.ToString(), out _))
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, "Value must be a valid number");
                }
            }
            else
            {
                if (int.TryParse(value.ToString(), out _))
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, "Value must be a valid integer");
                }
            }
        }
    }
}

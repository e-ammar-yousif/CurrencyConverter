using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CurrencyConverter.Attributes
{

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("The date value could not be null");
            }

            var currentValue = DateOnly.Parse(value.ToString()!); ;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
            {
                throw new ArgumentException("Property with this name not found");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue == null)
            {
                return new ValidationResult($"The value of {_comparisonProperty} could not be null.");
            }

            var parsedComparisonValue = DateOnly.Parse(comparisonValue.ToString()!);

            if (currentValue > parsedComparisonValue)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "The end date must be greater than the start date.");
        }
    }
}

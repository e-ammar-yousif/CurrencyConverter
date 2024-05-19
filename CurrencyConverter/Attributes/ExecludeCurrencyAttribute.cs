using System.ComponentModel.DataAnnotations;

namespace CurrencyConverter.Attributes
{
    public class ExecludeCurrencyAttribute : ValidationAttribute
    {
        private readonly string[] _currencies;

        public ExecludeCurrencyAttribute(params string[] currencies)
        {
            _currencies = currencies;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currency = value as string;

            if (currency != null && _currencies.Contains(currency, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult("Currency conversion not supported.");
            }

            return ValidationResult.Success!;
        }
    }
}

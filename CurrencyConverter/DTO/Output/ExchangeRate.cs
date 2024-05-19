using Newtonsoft.Json;

namespace CurrencyConverter.DTO.Output
{
    public class ExchangeRate
    {
        public string Base { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public Dictionary<string, decimal> Rates { get; set; } = new Dictionary<string, decimal>();
    }
}

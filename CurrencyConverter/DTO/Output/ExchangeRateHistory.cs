namespace CurrencyConverter.DTO.Output
{
    public class ExchangeRateHistory
    {
        public string Base { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public Dictionary<DateOnly, Dictionary<string, decimal>> Rates { get; set; } = new Dictionary<DateOnly, Dictionary<string, decimal>>();
    }
}

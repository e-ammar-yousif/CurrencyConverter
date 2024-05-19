using CurrencyConverter.DTO.Output;

namespace CurrencyConverter.Contract
{
    public interface IExchangeRateService
    {
        public Task<ExchangeRate> GetLatestAsync(string baseCurrency = "EUR");

        public Task<decimal> ConvertAsync(string baseCurrency, string quoteCurrency, decimal amount);

        public Task<ExchangeRateHistory> GetHistoryAsync(string baseCurrency, string fromDate, string toDate);
    }
}

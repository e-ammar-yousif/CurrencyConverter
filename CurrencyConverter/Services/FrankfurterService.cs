using CurrencyConverter.Attributes;
using CurrencyConverter.Contract;
using CurrencyConverter.DTO.Output;
using CurrencyConverter.Exceptions;
using CurrencyConverter.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CurrencyConverter.Services
{
    public class FrankfurterService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiBaseUrl = "https://api.frankfurter.app";

        private const int MaxRetryAttempts = 3;

        private const int DelayBetweenRetriesMilliseconds = 1000;

        private readonly IMemoryCache _cache;

        public FrankfurterService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(_apiBaseUrl);

            _cache = cache;
        }

        public async Task<ExchangeRate> GetLatestAsync(string baseCurrency = "EUR")
        {
            var cacheKey = $"Latest-{baseCurrency}";

            if (_cache.TryGetValue(cacheKey, out ExchangeRate? cachedRates))
                return cachedRates!;

            try
            {
                return await RetryHelper.Retry(async () =>
                {
                    var response = await _httpClient.GetAsync($"/latest?base={baseCurrency}");

                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new NotFoundException("Currency not found.");

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    var exchangeRate = JsonConvert.DeserializeObject<ExchangeRate>(content);

                    _cache.Set(cacheKey, exchangeRate, TimeSpan.FromHours(1));

                    return exchangeRate!;

                }, MaxRetryAttempts, DelayBetweenRetriesMilliseconds);
            }
            catch (BaseException) { throw; }

            catch (Exception ex)
            {
                throw new BaseException("An error occurred while fetching the latest exchange rates.", ex);
            }
        }

        public async Task<decimal> ConvertAsync(string baseCurrency, string quoteCurrency, decimal amount)
        {
            var cacheKey = $"Latest-{baseCurrency}";

            if (_cache.TryGetValue(cacheKey, out ExchangeRate? cachedRates) && cachedRates!.Rates.ContainsKey(quoteCurrency))
                return cachedRates!.Rates[quoteCurrency] * amount;

            try
            {
                return await RetryHelper.Retry(async () =>
                {
                    var response = await _httpClient.GetAsync($"/latest?amount={amount}&from={baseCurrency}&to={quoteCurrency}");

                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new NotFoundException("Currency not found.");

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    var currency = JsonConvert.DeserializeObject<ExchangeRate>(content);

                    return currency == null ? throw new Exception("Currency conversion not supported.") : currency.Rates[quoteCurrency];

                }, MaxRetryAttempts, DelayBetweenRetriesMilliseconds);
            }
            catch (BaseException) { throw; }

            catch (Exception ex)
            {
                throw new BaseException("An error occurred while fetching the latest exchange rates.", ex);
            }
        }

        public async Task<ExchangeRateHistory> GetHistoryAsync(string baseCurrency, string fromDate, string toDate)
        {
            var cacheKey = $"History-{baseCurrency}-{fromDate}-{toDate}";

            if (_cache.TryGetValue(cacheKey, out ExchangeRateHistory? cachedHistory))
                return cachedHistory!;

            try
            {
                return await RetryHelper.Retry(async () =>
                {
                    var response = await _httpClient.GetAsync($"/{fromDate}..{toDate}?from={baseCurrency}");

                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new NotFoundException("Currency history not found.");

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    var history = JsonConvert.DeserializeObject<ExchangeRateHistory>(content);

                    _cache.Set(cacheKey, history, TimeSpan.FromHours(1));

                    return history!;

                }, MaxRetryAttempts, DelayBetweenRetriesMilliseconds);
            }
            catch (BaseException) { throw; }

            catch (Exception ex)
            {
                throw new BaseException("An error occurred while fetching the latest exchange rates.", ex);
            }
        }
    }
}

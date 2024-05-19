using CurrencyConverter.DTO.Output;
using CurrencyConverter.Exceptions;
using CurrencyConverter.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Tests.Services
{
    [TestFixture]
    public class FrankfurterServiceTests
    {
        private FrankfurterService _frankfurterService;

        private Mock<HttpClient> _mockHttpClient;

        private Mock<IMemoryCache> _mockMemoryCache;

        [SetUp]
        public void Setup()
        {
            _mockHttpClient = new Mock<HttpClient>();

            _mockMemoryCache = new Mock<IMemoryCache>();

            _frankfurterService = new FrankfurterService(_mockHttpClient.Object, _mockMemoryCache.Object);
        }

        [Test]
        public async Task GetLatest_ValidBaseCurrency_ReturnsExchangeRates()
        {
            const string baseCurrency = "EUR";

            var exchangeRate = await _frankfurterService.GetLatestAsync(baseCurrency);
            
            Assert.That(exchangeRate, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(exchangeRate!.Rates, Is.Not.Null);

                Assert.That(exchangeRate.Rates.ContainsKey("USD"), Is.True);

                Assert.That(exchangeRate.Rates.ContainsKey("GBP"), Is.True);
            });
        }

        [Test]
        public async Task GetLatest_InvalidBaseCurrency_ThrowsException()
        {
            const string baseCurrency = "KKK";

            var exchangeRate = await _frankfurterService.GetLatestAsync(baseCurrency);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _frankfurterService.GetLatestAsync(baseCurrency));

            Assert.That(ex.Code, Is.EqualTo((int) HttpStatusCode.NotFound));
        }


        [Test]
        public async Task Convert_ValidInput_ReturnsConvertedAmount()
        {
            var exchangeRate = await _frankfurterService.ConvertAsync("EUR", "USD", 2);

            Assert.That(exchangeRate, Is.Positive);
        }

        [Test]
        public async Task GetHistory_ValidInput_ReturnsHistory()
        {
            var history = await _frankfurterService.GetHistoryAsync("USD", "2022-10-04", "2022-10-09");

            Assert.That(history, Is.Not.Null);
        }
    }
}

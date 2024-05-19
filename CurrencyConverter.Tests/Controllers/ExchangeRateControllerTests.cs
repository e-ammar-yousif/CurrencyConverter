using CurrencyConverter.Contract;
using CurrencyConverter.Controllers;
using CurrencyConverter.DTO.Input;
using CurrencyConverter.DTO.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace CurrencyConverter.Tests.Controllers
{
    [TestFixture]
    public class ExchangeRateControllerTests
    {
        private ExchangeRateController _controller;

        private Mock<IExchangeRateService> _mockExchangeRateService;

        private Mock<ILogger<ExchangeRateController>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockExchangeRateService = new Mock<IExchangeRateService>();

            _mockLogger = new Mock<ILogger<ExchangeRateController>>();

            _controller = new ExchangeRateController(_mockLogger.Object, _mockExchangeRateService.Object);
        }

        [Test]
        public async Task GetLatest_ValidBaseCurrency_ReturnsOkResult()
        {
            var result = await _controller.GetLatest("USD");

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetLatest_InvalidLengthBaseCurrency_ReturnsBadResult()
        {
            var result = await _controller.GetLatest("FIJSfljjnd");

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetLatest_NotExistedBaseCurrency_ReturnsNotFoundResult()
        {
            var result = await _controller.GetLatest("KKK");

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Convert_ValidInput_ReturnsOkResult()
        {
            var model = new ExchangeRateDto { BaseCurrency = "USD", QuoteCurrency = "EUR", Amount = 1 };

            var result = await _controller.Convert(model);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Convert_ExcludedBaseCurrencyInput_ReturnsBadResult()
        {
            var model = new ExchangeRateDto { BaseCurrency = "TRY", QuoteCurrency = "EUR", Amount = 1 };

            var result = await _controller.Convert(model);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Convert_InvalidLengthBaseCurrency_ReturnsNotFoundResult()
        {
            var model = new ExchangeRateDto { BaseCurrency = "TfsdojhfjdRY", QuoteCurrency = "EUR", Amount = 1 };

            var result = await _controller.Convert(model);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Convert_NotExistedBaseCurrency_ReturnsNotFoundResult()
        {
            var model = new ExchangeRateDto { BaseCurrency = "KKK", QuoteCurrency = "EUR", Amount = 1 };

            var result = await _controller.Convert(model);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetHistory_ValidInput_ReturnsOkResult()
        {
            var model = new HistoricalRatesDto { BaseCurrency = "USD", From = "2022-01-01", To = "2022-01-02"};

            var result = await _controller.GetHistory(model);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetHistory_StartDateGreaterThanEndDate_ReturnsBadResult()
        {
            var model = new HistoricalRatesDto { BaseCurrency = "USD", From = "2022-01-10", To = "2021-01-02" };

            var result = await _controller.GetHistory(model);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetHistory_InvalidLengthBaseCurrency_ReturnsNotFoundResult()
        {
            var model = new HistoricalRatesDto { BaseCurrency = "fgpdsjkfg", From = "2022-01-01", To = "2021-01-02" };

            var result = await _controller.GetHistory(model);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}

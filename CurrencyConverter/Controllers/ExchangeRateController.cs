using CurrencyConverter.Attributes;
using CurrencyConverter.Contract;
using CurrencyConverter.DTO.Input;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("api/exchangerates")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ILogger<ExchangeRateController> _logger;

        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController(ILogger<ExchangeRateController> logger, IExchangeRateService exchangeRateService)
        {
            _logger = logger;

            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest(string baseCurrency = "EUR")
        {
            var exchangeRate = await _exchangeRateService.GetLatestAsync(baseCurrency);

            return Ok(new { Data = exchangeRate });
        }

        [HttpGet("convert")]
        public async Task<IActionResult> Convert([FromQuery] ExchangeRateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var convertedAmount = await _exchangeRateService.ConvertAsync(model.BaseCurrency, model.QuoteCurrency, model.Amount);

            return Ok(new { Data = convertedAmount });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory([FromQuery] HistoricalRatesDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var historicalRates = await _exchangeRateService.GetHistoryAsync(model.BaseCurrency, model.From, model.To);

            return Ok(new { Data = historicalRates });
        }
    }   
}

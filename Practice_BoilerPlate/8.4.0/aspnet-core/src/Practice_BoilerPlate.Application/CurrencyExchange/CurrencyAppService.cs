using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.CurrencyExchange
{
    internal class CurrencyAppService:ApplicationService
    {
        private readonly ICurrencyExchangeService _currencyExchangeService;

        public CurrencyAppService(ICurrencyExchangeService currencyExchangeService)
        {
            _currencyExchangeService = currencyExchangeService;
        }

        public async Task<decimal> GetExchangeRate(string from, string to)
        {
            return await _currencyExchangeService.GetExchangeRateAsync(from, to);
        }
    }
}

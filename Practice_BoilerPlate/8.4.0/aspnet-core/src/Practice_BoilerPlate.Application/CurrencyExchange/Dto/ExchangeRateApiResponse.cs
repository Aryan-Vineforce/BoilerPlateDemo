using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.CurrencyExchange.Dto
{
    public class ExchangeRateApiResponse
    {
        public string result { get; set; }
        public string base_code { get; set; }
        public Dictionary<string, decimal> conversion_rates { get; set; }
    }
}

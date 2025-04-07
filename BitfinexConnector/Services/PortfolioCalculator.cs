using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitfinexConnector.Services
{
    public class BalanceEntry
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
    }

    public class PortfolioCalculator
    {
        private readonly BitfinexRestClient _client;

        public PortfolioCalculator(BitfinexRestClient client)
        {
            _client = client;
        }

        public async Task<List<BalanceEntry>> CalculateAsync(Dictionary<string, decimal> portfolio)
        {
            var result = new List<BalanceEntry>();

            foreach (var from in portfolio)
            {
                foreach (var to in portfolio.Keys)
                {
                    decimal converted = from.Value;

                    if (from.Key != to)
                    {
                        try
                        {
                            var ticker = await _client.GetTickerAsync($"{from.Key}{to}");
                            converted *= ticker.LastPrice;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    result.Add(new BalanceEntry
                    {
                        From = from.Key,
                        To = to,
                        Amount = Math.Round(converted, 4)
                    });
                }
            }

            return result;
        }
    }

}

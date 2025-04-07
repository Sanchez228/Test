using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using BitfinexConnector.Models;

namespace BitfinexConnector
{
    public class BitfinexRestClient
    {
        private readonly HttpClient _http = new HttpClient { BaseAddress = new Uri("https://api-pub.bitfinex.com") };


        public async Task<List<Trade>> GetTradesAsync(string symbol)
        {
            var url = $"/v2/trades/t{symbol}/hist?limit=5";
            var json = await _http.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<List<List<object>>>(json);

            var trades = new List<Trade>();
            foreach (var t in data)
            {
                trades.Add(new Trade
                {
                    Timestamp = (long)t[1],
                    Amount = Convert.ToDecimal(t[2]),
                    Price = Convert.ToDecimal(t[3])
                });
            }
            return trades;
        }

        public async Task<List<Candle>> GetCandlesAsync(string symbol, string timeframe = "1m")
        {
            var url = $"/v2/candles/trade:{timeframe}:t{symbol}/hist?limit=5";
            var json = await _http.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<List<List<object>>>(json);

            var candles = new List<Candle>();
            foreach (var c in data)
            {
                candles.Add(new Candle
                {
                    Timestamp = (long)c[0],
                    Open = Convert.ToDecimal(c[1]),
                    Close = Convert.ToDecimal(c[2]),
                    High = Convert.ToDecimal(c[3]),
                    Low = Convert.ToDecimal(c[4]),
                    Volume = Convert.ToDecimal(c[5])
                });
            }
            return candles;
        }

        public async Task<Ticker> GetTickerAsync(string symbol)
        {
            var json = await _http.GetStringAsync($"/v2/ticker/t{symbol}");
            var arr = JsonConvert.DeserializeObject<List<object>>(json);

            return new Ticker
            {
                Bid = Convert.ToDecimal(arr[0]),
                Ask = Convert.ToDecimal(arr[2]),
                LastPrice = Convert.ToDecimal(arr[6])
            };
        }
    }
}

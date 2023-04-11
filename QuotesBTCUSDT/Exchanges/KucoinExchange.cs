using Kucoin.Net.Clients;
using System;
using System.Threading.Tasks;

namespace Quotes.Exchanges
{
    internal class KucoinExchange : IExchange
    {
        private readonly KucoinClient _client;
        public KucoinExchange()
        {
            _client = new KucoinClient();
        }
        public async Task<QuotesModel> GetQuotesBTCUSDT()
        {
            var ticker = await _client.SpotApi.ExchangeData.GetTickerAsync(Symbols.KucoinSymbolBTCUSDT);
            QuotesModel model = new QuotesModel()
            {
                Exchange = GetType().Name,
                Price = Math.Round((decimal)ticker.Data.LastPrice, 3),
                Symbol = Symbols.KucoinSymbolBTCUSDT
            };
            return model;
        }
    }
}

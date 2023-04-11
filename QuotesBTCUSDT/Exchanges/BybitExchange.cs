using Bybit.Net.Clients;
using System;
using System.Threading.Tasks;

namespace Quotes.Exchanges
{
    internal class BybitExchange : IExchange
    {
        private readonly BybitClient _client;
        public BybitExchange()
        {
            _client = new BybitClient();
        }
        public async Task<QuotesModel> GetQuotesBTCUSDT()
        {
            var ticker = await _client.SpotApiV1.ExchangeData.GetPriceAsync(Symbols.BybitSymbolBTCUSDT);
            QuotesModel model = new QuotesModel()
            {
                Exchange = GetType().Name,
                Price = Math.Round(ticker.Data.Price, 3),
                Symbol = Symbols.BybitSymbolBTCUSDT
            };
            return model;
        }
    }
}

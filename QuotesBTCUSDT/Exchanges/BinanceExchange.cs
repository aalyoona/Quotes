using Binance.Net.Clients;
using System;
using System.Threading.Tasks;

namespace Quotes.Exchanges
{
    internal class BinanceExchange : IExchange
    {
        private readonly BinanceClient _client;
        public BinanceExchange()
        {
            _client = new BinanceClient();
        }
        public async Task<QuotesModel> GetQuotesBTCUSDT()
        {
            var ticker = await _client.SpotApi.ExchangeData.GetPriceAsync(Symbols.BinanceSymbolBTCUSDT);
            QuotesModel model = new QuotesModel()
            {
                Exchange = GetType().Name,
                Price = Math.Round(ticker.Data.Price, 3),
                Symbol = Symbols.BinanceSymbolBTCUSDT
            };
            return model;
        }
    }
}

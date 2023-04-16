using Binance.Net.Clients;
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

        public async Task<QuotesModel> GetQuotes(string firstSymbol, string secondSymbol)
        {
            var ticker = await _client.SpotApi.ExchangeData.GetPriceAsync($"{firstSymbol}{secondSymbol}");
            if (ticker.Data == null)
            {
                return null;
            }

            QuotesModel model = new QuotesModel()
            {
                Exchange = GetType().Name,
                Price = ticker.Data.Price,
                Symbol = $"{firstSymbol}{secondSymbol}"
            };
            return model;
        }
    }
}

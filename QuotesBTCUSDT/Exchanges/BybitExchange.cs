using Bybit.Net.Clients;
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

        public async Task<QuotesModel> GetQuotes(string firstSymbol, string secondSymbol)
        {
            var ticker = await _client.SpotApiV1.ExchangeData.GetPriceAsync($"{firstSymbol}{secondSymbol}");
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
